using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;

using System.Text.RegularExpressions;


namespace Computer_Maintenance.Model.Models
{

    public class SystemCleaningModel
    {
        private readonly object _regexCacheLock = new object();
        private Dictionary<string, Regex> _regexCache = new();

        ///<summary>
        ///Метод для получения доступных накопителей
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
        ///Метод для получения имени системного диска
        ///<summary>
        public string GetSystemDrive()
        {
            return Path.GetPathRoot(Environment.SystemDirectory)!;
        }

        ///<summary>
        ///Метод для получения информации о чистки для пользователя
        ///<summary>
        public List<CleaningInformation> GetLocationsForDrive(DriveInfo dInfo)
        {
            return CleanupLocations.GetLocationsByDriveType(dInfo, GetSystemDrive());
        }

        ///<summary>
        ///Метод для получения размера секции
        ///<summary>
        public StorageSize GetSizeSubSection(SubCleaningInformation subCleaningInformation)
        {
            long totalSizeBytes = 0;
            string path = subCleaningInformation.SearchConfig.BasePath;
            FilesAndDirectories.SearchTarget searchTarget = subCleaningInformation.SearchConfig.SearchTarget;
            FilesAndDirectories.SearchScope searchScope = subCleaningInformation.SearchConfig.SearchScope;
            FilesAndDirectories.DeleteScope deleteScope = subCleaningInformation.SearchConfig.DeleteScope;

            List<SearchPattern> includePatterns = subCleaningInformation.SearchConfig.IncludePatterns;
            List<SearchPattern> excludePatterns = subCleaningInformation.SearchConfig.ExcludePatterns;

            int bytesReaded = 0;
            totalSizeBytes = GetSizeWinApi(path, ref searchTarget, ref searchScope, ref deleteScope, ref includePatterns, ref excludePatterns);
            return ConvertSizeService.ConvertSize(totalSizeBytes);
        }
        private unsafe long GetSizeFoldersWinApi(string directoryPath, bool recursive, ref List<SearchPattern> includePatterns, ref List<SearchPattern> excludePatterns)
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

            if (hFind == FileApi.INVALID_HANDLE_VALUE) return 0;

            try
            {
                do
                {
                    string name = GetFileNamePtr(&findData);
                    if (name == "." || name == "..") continue;

                    bool isDirectory = (findData.dwFileAttributes & (int)FileAttributes.Directory) != 0;
                    if (!isDirectory)
                    {
                        // Нас интересуют только папки в этой функции
                        continue;
                    }

                    // Если нет активных include-шаблонов — считать все папки как совпадающие
                    bool hasActiveInclude = includePatterns != null && includePatterns.Count > 0 && includePatterns.Exists(p => p.IsActive);
                    bool includeMatched = !hasActiveInclude;

                    if (hasActiveInclude)
                    {
                        for (int i = 0; i < includePatterns.Count; i++)
                        {
                            SearchPattern p = includePatterns[i];
                            if (!p.IsActive) continue;
                            if (IsMatch(name, ref p))
                            {
                                includeMatched = true;

                                // Обработать child-конфигурацию (вызов даже при пустом child.IncludePatterns)
                                if (p.ChildConfiguration != null)
                                {
                                    // проверить, не исключена ли папка по глобальным excludePatterns
                                    bool excludedGlobally = false;
                                    if (excludePatterns != null && excludePatterns.Count > 0)
                                    {
                                        for (int exI = 0; exI < excludePatterns.Count; exI++)
                                        {
                                            var ex = excludePatterns[exI];
                                            if (!ex.IsActive) continue;
                                            if (IsMatch(name, ref ex))
                                            {
                                                excludedGlobally = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!excludedGlobally)
                                    {
                                        string childPath = Path.Combine(directoryPath, name);
                                        bool recursiveChild = ((p.ChildConfiguration.SearchScope & FilesAndDirectories.SearchScope.Recursive) != 0);
                                        List<SearchPattern> includePatternsChild = p.ChildConfiguration.IncludePatterns ?? new List<SearchPattern>();
                                        List<SearchPattern> excludePatternsChild = p.ChildConfiguration.ExcludePatterns ?? new List<SearchPattern>();

                                        totalSize += GetSizeFilesWinApi(childPath, recursiveChild, ref includePatternsChild, ref excludePatternsChild);
                                    }
                                }

                                break;
                            }
                        }
                    }

                    // Проверить глобальные exclude и при совпадении пропустить дальнейшую обработку (включая рекурсию)
                    bool excluded = false;
                    if (includeMatched && excludePatterns != null && excludePatterns.Count > 0)
                    {
                        for (int i = 0; i < excludePatterns.Count; i++)
                        {
                            SearchPattern ex = excludePatterns[i];
                            if (!ex.IsActive) continue;
                            if (IsMatch(name, ref ex))
                            {
                                excluded = true;
                                break;
                            }
                        }
                    }

                    // Рекурсивно спускаемся в поддиректории для поиска других совпадающих папок
                    if (recursive && !excluded)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        totalSize += GetSizeFoldersWinApi(childDir, recursive, ref includePatterns, ref excludePatterns);
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

        private unsafe long GetSizeFilesWinApi(string directoryPath, bool recursive, ref List<SearchPattern> includePatterns, ref List<SearchPattern> excludePatterns)
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


            if (hFind == FileApi.INVALID_HANDLE_VALUE) return 0;

            try
            {
                do
                {
                    string name = GetFileNamePtr(&findData);
                    if (name == "." || name == "..") continue;

                    bool isDirectory = (findData.dwFileAttributes & (int)FileAttributes.Directory) != 0;
                    long fileSize = ((long)findData.nFileSizeHigh << 32) | (long)findData.nFileSizeLow;

                    if (!isDirectory)
                    {
                        // Если нет активных include-шаблонов — считаем все файлы.
                        bool hasActiveInclude = includePatterns != null && includePatterns.Count > 0 && includePatterns.Exists(p => p.IsActive);
                        bool includeMatched = !hasActiveInclude;

                        if (hasActiveInclude)
                        {
                            for (int i = 0; i < includePatterns.Count; i++)
                            {
                                SearchPattern p = includePatterns[i];
                                if (!p.IsActive) continue;
                                if (IsMatch(name, ref p))
                                {
                                    includeMatched = true;
                                    break;
                                }
                            }
                        }

                        if (includeMatched)
                        {
                            // проверка exclude (только активные исключения)
                            bool excluded = false;
                            if (excludePatterns != null && excludePatterns.Count > 0)
                            {
                                for (int j = 0; j < excludePatterns.Count; j++)
                                {
                                    SearchPattern ex = excludePatterns[j];
                                    if (!ex.IsActive) continue;
                                    if (IsMatch(name, ref ex))
                                    {
                                        excluded = true;
                                        break;
                                    }
                                }
                            }

                            if (!excluded)
                            {
                                totalSize += fileSize;
                            }
                        }
                    }
                    else if (recursive)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        totalSize += GetSizeFilesWinApi(childDir, recursive, ref includePatterns, ref excludePatterns);
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

        private unsafe long GetSizeWinApi(
                    string path,
                    ref FilesAndDirectories.SearchTarget searchTarget,
                    ref FilesAndDirectories.SearchScope searchScope,
                    ref FilesAndDirectories.DeleteScope deleteScope,
                    ref List<SearchPattern> includePatterns,
                    ref List<SearchPattern> excludePatterns)
        {
            if (!Directory.Exists(path)) return 0;
            long totalSize = 0;

            if ((searchTarget & FilesAndDirectories.SearchTarget.Files) != 0)
            {
                bool recursive = ((searchScope & FilesAndDirectories.SearchScope.Recursive) != 0);
                // Всегда запускаем обход — пустой include трактуем как "включать все файлы".
                totalSize += GetSizeFilesWinApi(path, recursive, ref includePatterns, ref excludePatterns);
            }
            if ((searchTarget & FilesAndDirectories.SearchTarget.Directories) != 0)
            {
                bool recursive = ((searchScope & FilesAndDirectories.SearchScope.Recursive) != 0);
                totalSize += GetSizeFoldersWinApi(path, recursive, ref includePatterns, ref excludePatterns);
            }

            return totalSize;
        }

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

        private unsafe bool IsMatch(string name, ref SearchPattern pattern)
        {
            switch (pattern.PatternMatchType)
            {
                case FilesAndDirectories.PatternMatchType.Exact:
                    return string.Equals(
                        name,
                        pattern.Pattern,
                        StringComparison.OrdinalIgnoreCase);

                case FilesAndDirectories.PatternMatchType.Contains:
                    return name.Contains(
                        pattern.Pattern,
                        StringComparison.OrdinalIgnoreCase);

                case FilesAndDirectories.PatternMatchType.Simple:
                    return MatchWildcard(name, pattern.Pattern);

                case FilesAndDirectories.PatternMatchType.Regex:
                    return GetRegex(pattern.Pattern).IsMatch(name);

                default:
                    return false;
            }
        }

        private bool MatchWildcard(string input, string wildcard)
        {
            lock (_regexCacheLock)
            {
                if (!_regexCache.TryGetValue(wildcard, out var regex))
                {
                    string pattern =
                        "^" + Regex.Escape(wildcard)
                            .Replace("\\*", ".*")
                            .Replace("\\?", ".") + "$";

                    regex = new Regex(
                        pattern,
                        RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    _regexCache[wildcard] = regex;
                }

                return regex.IsMatch(input);
            }
        }

        private Regex GetRegex(string pattern)
        {
            lock (_regexCacheLock)
            {
                if (!_regexCache.TryGetValue(pattern, out var regex))
                {
                    regex = new Regex(
                        pattern,
                        RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    _regexCache[pattern] = regex;
                }

                return regex;
            }
        }
    }
}