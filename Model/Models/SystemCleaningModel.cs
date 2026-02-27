using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.SystemCleaning;
using Computer_Maintenance.Model.Services.SystemCleaning;
using Computer_Maintenance.Model.Structs.SystemCleaning;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Computer_Maintenance.Model.Models
{

    public class SystemCleaningModel
    {
        private readonly object _regexCacheLock = new object();
        private Dictionary<string, Regex> _regexCache = new Dictionary<string, Regex>();
        public List<string> _successfulDeletedFiles = new List<string>();
        public Dictionary<string, int> _failedDeletedFiles = new Dictionary<string, int>();

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
        ///

        public string SaveFileDeleteFailLogs()
        {
            if (_failedDeletedFiles.Count > 0)
            {
                try
                {
                    string pathDirectorySave = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                    string fullPath = Path.Combine(pathDirectorySave, "Логи_ошибок_уадаления_файлов.txt");

                    if (!Directory.Exists(pathDirectorySave))
                    {
                        Directory.CreateDirectory(pathDirectorySave);
                    }

                    StreamWriter sw = new StreamWriter(fullPath);

                    foreach (KeyValuePair<string, int> keyValuePair in _failedDeletedFiles)
                    {
                        switch (keyValuePair.Value)
                        {
                            case FileApi.ERROR_ACCESS_DENIED:
                                sw.WriteLine($"ФАЙЛ/ПАПКА: {keyValuePair.Key}   ОШИБКА: Нет доступа");
                                break;
                            case FileApi.ERROR_SHARING_VIOLATION:
                                sw.WriteLine($"ФАЙЛ/ПАПКА: {keyValuePair.Key}   ОШИБКА: Файл занят процессом");
                                break;
                            default:
                                sw.WriteLine($"ФАЙЛ/ПАПКА: {keyValuePair.Key}   ОШИБКА: Код {keyValuePair.Value}");
                                break;
                        }
                    }
                    sw.Close();
                    return fullPath;
                }
                catch { }
            }
            return String.Empty;
        }

        public unsafe List<DirectoryContents> GetDirectoryContents(string directoryPath)
        {
            if (directoryPath == null || !Directory.Exists(directoryPath)) { return new List<DirectoryContents>(); }

            List<DirectoryContents> directoryContents = new List<DirectoryContents>();

            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) { return new List<DirectoryContents>(); }

            try
            {
                do
                {

                    string name = findData.GetFileName();
                    if (name == "." || name == "..") { continue; }

                    string fullPath = Path.Combine(directoryPath, name);

                    DirectoryContents contents = new DirectoryContents
                    {
                        FullPath = fullPath,
                        Name = name,
                        Size = findData.GetFileSize(),
                        IsDirectory = findData.IsDirectory()
                    };

                    if (!contents.IsDirectory)
                    {
                        contents.Icon = Icon.ExtractAssociatedIcon(fullPath);
                    }
                    else
                    {
                        contents.Icon = Resource.FolderIcon;
                    }

                    directoryContents.Add(contents);

                }
                while (FileApi.FindNextFileW(hFind, &findData));
            }
            finally
            {
                FileApi.FindClose(hFind);
            }

            return directoryContents;
        }

        public StorageSize GetSizeSubSection(SubCleaningInformation subCleaningInformation)
        {
            ulong totalSizeBytes = GetSizeWinApi(subCleaningInformation.SearchConfig.BasePath,
                subCleaningInformation.SearchConfig.SearchTarget,
                subCleaningInformation.SearchConfig.SearchScope,
                subCleaningInformation.SearchConfig.DeleteScope,
                subCleaningInformation.SearchConfig.IncludePatterns,
                subCleaningInformation.SearchConfig.ExcludePatterns);

            return ConvertSizeService.ConvertSize(totalSizeBytes);
        }
        public void StartDelete(SubCleaningInformation subCleaningInformation)
        {

            // при первоначальном запуске — это корень секции, не удаляем его как папку
            StartDeleteWinApi(subCleaningInformation.SearchConfig.BasePath,
                              subCleaningInformation.SearchConfig.SearchTarget,
                              subCleaningInformation.SearchConfig.SearchScope,
                              subCleaningInformation.SearchConfig.DeleteScope,
                              subCleaningInformation.SearchConfig.IncludePatterns,
                              subCleaningInformation.SearchConfig.ExcludePatterns,
                              isRoot: true);
        }

        // Добавлен флаг isRoot — защищает текущую папку от удаления, если true.
        private void StartDeleteWinApi(
                    string path,
                    SearchTarget searchTarget,
                    SearchScope searchScope,
                    DeleteScope deleteScope,
                    List<SearchPattern> includePatterns,
                    List<SearchPattern> excludePatterns,
                    bool isRoot)
        {
            if (!Directory.Exists(path)) { return; }

            bool recursive = (searchScope & SearchScope.Recursive) != 0;

            if ((searchTarget & SearchTarget.Files) != 0)
            {
                DeleteFilesWinApi(path, recursive, includePatterns, excludePatterns);
            }
            if ((searchTarget & SearchTarget.Directories) != 0)
            {
                DeleteFoldersWinApi(path, recursive, deleteScope, includePatterns, excludePatterns, isRoot);
            }
        }
        private unsafe void DeleteFoldersWinApi(
    string directoryPath,
    bool recursive,
    DeleteScope deleteScope,
    List<SearchPattern> includePatterns,
    List<SearchPattern> excludePatterns,
    bool isRoot)
        {
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) return;

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) continue;

                    string name = findData.GetFileName();
                    if (name == "." || name == "..") continue;

                    string childDir = Path.Combine(directoryPath, name);

                    if (!findData.IsDirectory()) continue;

                    bool includeMatched = MatchInclude(name, includePatterns);
                    bool handledByChild = false;

                    // Обработка ChildConfiguration
                    if (includePatterns != null)
                    {
                        foreach (var pattern in includePatterns)
                        {
                            if (!pattern.IsActive) continue;

                            if (IsMatch(name, pattern) && pattern.ChildConfiguration != null)
                            {
                                bool excludedGlobally = MatchExclude(name, excludePatterns);
                                if (excludedGlobally) break;

                                // child — запускаем как корень для child-конфигурации,
                                // чтобы сам matched каталог не удалялся (он должен оставаться).
                                StartDeleteWinApi(
                                    childDir,
                                    pattern.ChildConfiguration.SearchTarget,
                                    pattern.ChildConfiguration.SearchScope,
                                    pattern.ChildConfiguration.DeleteScope,
                                    pattern.ChildConfiguration.IncludePatterns ?? new(),
                                    pattern.ChildConfiguration.ExcludePatterns ?? new(),
                                    isRoot: true);

                                handledByChild = true;
                                break;
                            }
                        }
                    }

                    // Рекурсивный вызов для подпапок (обычная рекурсия — это уже не корень)
                    if (recursive && !handledByChild && !(includeMatched && MatchExclude(name, excludePatterns)))
                    {
                        DeleteFoldersWinApi(childDir, recursive, deleteScope, includePatterns, excludePatterns, isRoot: false);
                    }

                } while (FileApi.FindNextFileW(hFind, &findData));
            }
            finally
            {
                FileApi.FindClose(hFind);
            }

            // Удаляем текущую папку после обработки всех вложенных,
            // но не удаляем корневую директорию вызова (isRoot == true).
            // Удаляем только если в deleteScope явно указано AllContents.
            if (isRoot) return;

            if ((deleteScope & DeleteScope.AllContents) == 0) return;

            // Проверяем, остались ли видимые (не пропускаемые) записи в каталоге
            bool isEmpty = true;
            string checkSearch = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";
            FileApi.WIN32_FIND_DATA checkData;
            IntPtr hCheck = FileApi.FindFirstFileExW(
                checkSearch,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &checkData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hCheck.ToInt64() != FileApi.INVALID_HANDLE_VALUE)
            {
                try
                {
                    do
                    {
                        if ((checkData.dwFileAttributes & (int)skipMask) != 0) continue;

                        string n = checkData.GetFileName();
                        if (n == "." || n == "..") continue;

                        // найден видимый элемент — не пустая
                        isEmpty = false;
                        break;
                    } while (FileApi.FindNextFileW(hCheck, &checkData));
                }
                finally
                {
                    FileApi.FindClose(hCheck);
                }
            }
            else
            {
                // Не удалось перечислить — не пытаемся удалять
                return;
            }

            if (isEmpty && Directory.Exists(directoryPath))
            {
                try
                {
                    var attrs = File.GetAttributes(directoryPath);
                    if ((attrs & FileAttributes.ReadOnly) != 0)
                        File.SetAttributes(directoryPath, attrs & ~FileAttributes.ReadOnly);
                }
                catch
                {
                    // если не удалось снять атрибут — всё равно попробуем удалить
                }

                bool removed = FileApi.RemoveDirectoryW(directoryPath);
            }

        }



        private unsafe void DeleteFilesWinApi(string directoryPath,
            bool recursive,
            List<SearchPattern> includePatterns,
            List<SearchPattern> excludePatterns)
        {
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) { return; }

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) { continue; }

                    string name = findData.GetFileName();
                    if (name == "." || name == "..") { continue; }

                    bool isDirectory = findData.IsDirectory();

                    if (!isDirectory)
                    {
                        if (MatchInclude(name, includePatterns) && !MatchExclude(name, excludePatterns))
                        {
                            string fullPath = Path.Combine(directoryPath, name);

                            if ((findData.dwFileAttributes & (int)FileAttributes.ReadOnly) != 0)
                            {
                                FileApi.SetFileAttributesW(fullPath, (uint)(findData.dwFileAttributes & ~(int)FileAttributes.ReadOnly));
                            }

                            bool deleted = FileApi.DeleteFileW(fullPath);

                            if (deleted)
                            {
                                _successfulDeletedFiles.Add(fullPath);
                            }
                            else
                            {
                                try { _failedDeletedFiles.Add(fullPath, Marshal.GetLastWin32Error()); } catch { }
                            }
                        }
                    }
                    else if (recursive)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        DeleteFilesWinApi(childDir, recursive, includePatterns, excludePatterns);
                    }
                }
                while (FileApi.FindNextFileW(hFind, &findData));
            }
            finally
            {
                FileApi.FindClose(hFind);
            }
        }


        /// <summary>
        ///Центральный метод расчёта размера через WinAPI
        /// </summary>
        private ulong GetSizeWinApi(
                   string path,
                   SearchTarget searchTarget,
                   SearchScope searchScope,
                   DeleteScope deleteScope,
                   List<SearchPattern> includePatterns,
                   List<SearchPattern> excludePatterns)
        {
            if (!Directory.Exists(path)) return 0;

            ulong totalSize = 0;
            bool recursive = (searchScope & SearchScope.Recursive) != 0;

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
        private unsafe ulong GetSizeFoldersWinApi(
            string directoryPath,
            bool recursive,
            List<SearchPattern> includePatterns,
    List<SearchPattern> excludePatterns)
        {
            ulong totalSize = 0;
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) { return 0; }

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) { continue; }

                    string name = findData.GetFileName();
                    if (name == "." || name == "..") { continue; }

                    bool isDirectory = findData.IsDirectory();
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
                while (FileApi.FindNextFileW(hFind, &findData));
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
        private unsafe ulong GetSizeFilesWinApi(
            string directoryPath,
            bool recursive,
            List<SearchPattern> includePatterns,
            List<SearchPattern> excludePatterns)
        {
            ulong totalSize = 0;
            string searchPath = directoryPath.EndsWith("\\") ? directoryPath + "*" : directoryPath + "\\*";

            FileApi.WIN32_FIND_DATA findData;
            IntPtr hFind = FileApi.FindFirstFileExW(
                searchPath,
                FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                &findData,
                FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) { return 0; }

            try
            {
                do
                {
                    if ((findData.dwFileAttributes & (int)skipMask) != 0) { continue; }

                    string name = findData.GetFileName();
                    if (name == "." || name == "..") { continue; }

                    bool isDirectory = findData.IsDirectory();

                    if (!isDirectory)
                    {

                        if (MatchInclude(name, includePatterns) && !MatchExclude(name, excludePatterns))
                        {
                            totalSize += findData.GetFileSize();
                        }
                    }
                    else if (recursive)
                    {
                        string childDir = Path.Combine(directoryPath, name);
                        totalSize += GetSizeFilesWinApi(childDir, recursive, includePatterns, excludePatterns);
                    }
                }
                while (FileApi.FindNextFileW(hFind, &findData));
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