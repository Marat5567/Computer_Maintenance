using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.SystemCleaning;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using System.Text.RegularExpressions;

namespace Computer_Maintenance.Model.Models
{

    public class SystemCleaningModel
    {
        private readonly object _regexCacheLock = new object(); 
        private Dictionary<string, Regex> _regexCache = new();

        //Маска атрибутов для пропуска при сканировании
        private readonly FileAttributes skipMask =
            FileAttributes.ReparsePoint     // Символические ссылки, junction’ы, точки монтирования (риск зацикливания и выхода за пределы каталога)
          | FileAttributes.Device           // Псевдофайлы устройств (CON, NUL и т.п.), не имеют реального размера
          | FileAttributes.Offline          // Файлы, отсутствующие локально (OneDrive / Remote Storage), реального размера на диске нет
          | FileAttributes.IntegrityStream; // Файлы с контрольными суммами (ReFS / Storage Spaces), возможны ограничения доступа и некорректный подсчёт

        ///<summary>
        ///Получение списка доступных (готовых) локальных накопителей
        ///<summary>
        public List<DriveInfo> GetDrives()
        {
            List<DriveInfo> readyDrives = new List<DriveInfo>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo dInfo in allDrives)
            {
                if (dInfo.IsReady && dInfo.DriveType != DriveType.Network)
                {
                    readyDrives.Add(dInfo);
                }
            }
            return readyDrives;
        }

        ///<summary>
        ///Получение корня системного диска
        ///<summary>
        public string GetSystemDrive()
        {
            return Path.GetPathRoot(Environment.SystemDirectory)!;
        }

        ///<summary>
        ///Получение конфигураций очистки для выбранного диска
        ///<summary>
        public List<CleaningInformation> GetLocationsForDrive(DriveInfo dInfo)
        {
            return CleanupLocations.GetLocationsByDriveType(dInfo, GetSystemDrive());
        }
        ///<summary>
        ///Получение размера секции очистки
        ///<summary>
        public StorageSize GetSizeSubSection(SubCleaningInformation subCleaningInformation)
        {
            long totalSizeBytes = GetSizeWinApi(subCleaningInformation.SearchConfig.BasePath, 
                subCleaningInformation.SearchConfig.SearchTarget,
                subCleaningInformation.SearchConfig.SearchScope, 
                subCleaningInformation.SearchConfig.DeleteScope, 
                subCleaningInformation.SearchConfig.IncludePatterns,
                subCleaningInformation.SearchConfig.ExcludePatterns);

            return ConvertSizeService.ConvertSize(totalSizeBytes);
        }
        /// <summary>
        ///Центральный метод расчёта размера через WinAPI
        /// </summary>
        private long GetSizeWinApi(
                   string path,
                   SearchTarget searchTarget,
                   SearchScope searchScope,
                   DeleteScope deleteScope,
                   List<SearchPattern> includePatterns,
                   List<SearchPattern> excludePatterns)
        {
            if (!Directory.Exists(path)) return 0;

            long totalSize = 0;
            bool recursive = ((searchScope & SearchScope.Recursive) != 0);

            if ((searchTarget & SearchTarget.Files) != 0)
            {
                totalSize += GetSizeFilesWinApi(path, recursive, includePatterns, excludePatterns);
            }
            if ((searchTarget & SearchTarget.Directories) != 0)
            {
                totalSize += GetSizeFoldersWinApi(path, recursive, includePatterns, excludePatterns);
            }
            return totalSize;
        }

        /// <summary>
        ///Расчёт размера папок (каталогов)
        /// </summary>
        private unsafe long GetSizeFoldersWinApi(
            string directoryPath,
            bool recursive,
            List<SearchPattern> includePatterns,
    List<SearchPattern> excludePatterns)
        {
            long totalSize = 0;
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                out findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind == FileApi.INVALID_HANDLE_VALUE) { return 0; }

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) { continue; }

                    string name = GetFileNamePtr(&findData);
                    if (name == "." || name == "..") { continue; }

                    bool isDirectory = (findData.dwFileAttributes & (int)FileAttributes.Directory) != 0;
                    if (!isDirectory) { continue; }

                    bool includeMatched = MatchInclude(name, includePatterns);
                    bool handledByChild = false;

                    if (includePatterns != null)
                    {
                        for (int i = 0; i < includePatterns.Count; i++)
                        {
                            SearchPattern inPattern = includePatterns[i];
                            if (!inPattern.IsActive) { continue; }

                            if (IsMatch(name, inPattern) && inPattern.ChildConfiguration != null)
                            {
                                // exclude НЕ должен блокировать child
                                bool excludedGlobally = MatchExclude(name, excludePatterns);
                                if (excludedGlobally) { break; }

                                string childPath = Path.Combine(directoryPath, name);

                                totalSize += GetSizeWinApi(
                                    childPath,
                                    inPattern.ChildConfiguration.SearchTarget,
                                    inPattern.ChildConfiguration.SearchScope,
                                    inPattern.ChildConfiguration.DeleteScope,
                                    inPattern.ChildConfiguration.IncludePatterns ?? new(),
                                    inPattern.ChildConfiguration.ExcludePatterns ?? new());

                                handledByChild = true;
                                break;
                            }
                        }
                    }

                    bool excluded = includeMatched && MatchExclude(name, excludePatterns);

                    if (recursive && !excluded && !handledByChild)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        totalSize += GetSizeFoldersWinApi(childDir, recursive, includePatterns, excludePatterns);
                    }
                }
                while (FileApi.FindNextFileW(hFind, out findData));
            }
            finally
            {
                FileApi.FindClose(hFind);
            }

            return totalSize;
        }

        /// <summary>
        /// Расчёт размера файлов
        /// </summary>
        private unsafe long GetSizeFilesWinApi(
            string directoryPath,
            bool recursive,
            List<SearchPattern> includePatterns,
            List<SearchPattern> excludePatterns)
        {
            long totalSize = 0;
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                out findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind == FileApi.INVALID_HANDLE_VALUE) { return 0; }

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) { continue; }

                    string name = GetFileNamePtr(&findData);
                    if (name == "." || name == "..") { continue; }

                    bool isDirectory = (findData.dwFileAttributes & (int)FileAttributes.Directory) != 0;

                    if (!isDirectory)
                    {
                        long fileSize =
                            ((long)findData.nFileSizeHigh << 32) |
                            (long)findData.nFileSizeLow;

                        if (MatchInclude(name, includePatterns) && !MatchExclude(name, excludePatterns))
                        {
                            totalSize += fileSize;
                        }
                    }
                    else if (recursive)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        totalSize += GetSizeFilesWinApi(childDir, recursive, includePatterns, excludePatterns);
                    }
                }
                while (FileApi.FindNextFileW(hFind, out findData));
            }
            finally
            {
                FileApi.FindClose(hFind);
            }

            return totalSize;
        }

        /// <summary>
        /// Проверка, подходит ли имя под include-паттерны
        /// </summary>
        private bool MatchInclude(string name, List<SearchPattern> patterns)
        {
            if (patterns == null || patterns.Count == 0)
            {
                return true;
            }

            bool hasActiveInclude = false;

            for (int i = 0; i < patterns.Count; i++)
            {
                if (patterns[i].IsActive)
                {
                    hasActiveInclude = true;
                    break;
                }
            }

            if (!hasActiveInclude)
            {
                return true;
            }

            for (int i = 0; i < patterns.Count; i++)
            {
                SearchPattern pattern = patterns[i];
                if (!pattern.IsActive) { continue; }

                if (IsMatch(name, pattern))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Проверка, подпадает ли имя под exclude-паттерны
        /// </summary>
        private bool MatchExclude(string name, List<SearchPattern> patterns)
        {
            if (patterns == null || patterns.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < patterns.Count; i++)
            {
                SearchPattern pattern = patterns[i];
                if (!pattern.IsActive) { continue; }

                if (IsMatch(name, pattern))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Получение имени файла/каталога из WIN32_FIND_DATA
        /// </summary>
        private unsafe string GetFileNamePtr(FileApi.WIN32_FIND_DATA* findData)
        {
            char* ptr = findData->cFileName;
            int length = 0;
            while (length < 260 && ptr[length] != '\0')
            {
                length++;
            }
            return new string(ptr, 0, length);
        }

        /// <summary>
        /// Универсальная проверка совпадения имени с паттерном
        /// </summary>
        private bool IsMatch(string name, SearchPattern pattern)
        {
            switch (pattern.PatternMatchType)
            {
                case PatternMatchType.Exact:
                    return string.Equals(name, pattern.Pattern, StringComparison.OrdinalIgnoreCase);

                case PatternMatchType.Contains:
                    return name.Contains(pattern.Pattern, StringComparison.OrdinalIgnoreCase);

                case PatternMatchType.Simple:
                    return MatchWildcard(name, pattern.Pattern);

                case PatternMatchType.Regex:
                    return GetRegex(pattern.Pattern).IsMatch(name);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверка wildcard-маски (*, ?) через кешируемый Regex
        /// </summary>
        private bool MatchWildcard(string input, string wildcard)
        {
            lock (_regexCacheLock)
            {
                if (!_regexCache.TryGetValue(wildcard, out var regex))
                {
                    string pattern = "^" + Regex.Escape(wildcard).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                    regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    _regexCache[wildcard] = regex;
                }
                return regex.IsMatch(input);
            }
        }

        /// <summary>
        /// Получение (или создание) Regex из кеша
        /// </summary>
        private Regex GetRegex(string pattern)
        {
            lock (_regexCacheLock)
            {
                if (!_regexCache.TryGetValue(pattern, out var regex))
                {
                    regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    _regexCache[pattern] = regex;
                }
                return regex;
            }
        }
    }
}