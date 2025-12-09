using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Computer_Maintenance.Model.Models
{

    public class SystemCleaningModel
    {
        private Dictionary<string, Regex> _regexCache = new();

        public List<DriveInfo> GetDrives()
        {
            List<DriveInfo> readyDrives = new List<DriveInfo>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo dInfo in allDrives)
            {
                if(dInfo.IsReady && dInfo.DriveType != DriveType.Network)
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
        public List<CleaningInformation> GetLocationsByAccessForDrive(DriveInfo dInfo)
        {
            return CleanupLocations.GetLocationsByAccess(dInfo, GetSystemDrive());
        }

        ///<summary>
        ///Метод для получения размера папки
        ///<summary>
        public StorageSize GetSizeSection(CleaningInformation cleaningInformation)
        {
            long totalSizeBytes = 0;

            PatternType type = cleaningInformation.Pattern.Type;
            string path = cleaningInformation.Path;
            bool recursiveSearch = cleaningInformation.RecursiveSearch;
            bool recursiveDelete = cleaningInformation.Pattern.RecursiveDelete;
            List<string> patternInclude = cleaningInformation.Pattern.IncludePattern;

            switch (type)
            {
                case PatternType.All:
                    totalSizeBytes = GetAllFiles(path, recursiveSearch || recursiveDelete);
                    break;
                case PatternType.File:
                    totalSizeBytes = GetMatchingFiles(path, recursiveSearch, recursiveDelete, patternInclude);
                    break;
                case PatternType.Folder:
                    totalSizeBytes = GetFilesInMatchingFolders(path, recursiveSearch, recursiveDelete, patternInclude);
                    break;
            }

            return ConvertSizeService.ConvertSize(totalSizeBytes);

        }



        private long GetAllFiles(string path, bool recursiveSerch)
        {
            long size = 0;

            foreach (string filePath in DirectoryService.GetFiles(path))
            {
                if (FileService.HasAccess(filePath))
                {
                    size += FileService.GetSize(filePath);
                }
            }

            if (recursiveSerch)
            {
                foreach (string dirPath in DirectoryService.GetDirectories(path))
                {
                    if (DirectoryService.HasAccess(dirPath))
                    {
                        size += GetAllFiles(dirPath, true);
                    }
                }
            }
            return size;
        }

        private long GetMatchingFiles(string path, bool recursiveSearch, bool recursiveDelete, List<string>patterns) 
        {
            long size = 0;

            foreach (string filePath in DirectoryService.GetFiles(path))
            {
                if (FileService.HasAccess(filePath) && MathPattern(filePath, patterns, false))
                {
                    size += FileService.GetSize(filePath);
                }
            }

            if (recursiveSearch)
            {
                foreach (string dirPath in DirectoryService.GetDirectories(path))
                {
                    if (DirectoryService.HasAccess(dirPath))
                    {
                        size += GetMatchingFiles(dirPath, recursiveSearch, recursiveDelete, patterns);
                    }
                }
            }
            return size;
        }

        private long GetFilesInMatchingFolders(string path, bool recursiveSearch, bool recursiveDelete, List<string>patterns)
        {
            long size = 0;

            foreach (string dirPath in DirectoryService.GetDirectories(path))
            {
                if (DirectoryService.HasAccess(path))
                {
                    if (MathPattern(dirPath, patterns, true))
                    {
                        size += GetAllFiles(dirPath, recursiveDelete);
                    }
                    if (recursiveSearch)
                    {
                        size += GetFilesInMatchingFolders(dirPath, recursiveSearch, recursiveDelete, patterns);
                    }
                }
            }
            return size;
        }

        // 1. Метод для PatternType.All - считает ВСЕ файлы
        private long CalculateAllFilesSize(string folderPath, bool recursive)
        {
            long totalSize = 0;

            if (!DirectoryService.Exists(folderPath))
                return 0;

            try
            {
                // Считаем файлы в текущей папке
                string[] files = DirectoryService.GetFiles(folderPath);
                foreach (string filePath in files)
                {
                    if (FileService.HasAccess(filePath))
                    {
                        totalSize += FileService.GetSize(filePath);
                    }
                }

                // Если нужна рекурсия - обрабатываем подпапки
                if (recursive)
                {
                    string[] subfolders = DirectoryService.GetDirectories(folderPath);
                    foreach (string subfolderPath in subfolders)
                    {
                        if (DirectoryService.HasAccess(subfolderPath))
                        {
                            totalSize += CalculateAllFilesSize(subfolderPath, true);
                        }
                    }
                }
            }
            catch
            {
                // Логирование ошибок
            }

            return totalSize;
        }

        // 2. Метод для PatternType.File - считает файлы по паттерну
        private long CalculateFilesByPatternSize(string folderPath, bool recursive, CleaningInformation_Pattern pattern)
        {
            long totalSize = 0;

            try
            {
                // Проверяем файлы в текущей папке
                string[] files = DirectoryService.GetFiles(folderPath);
                foreach (string filePath in files)
                {
                    if (FileService.HasAccess(filePath) &&
                        MathPattern(filePath, pattern.IncludePattern, false))
                    {
                        totalSize += FileService.GetSize(filePath);
                    }
                }

                // Если нужна рекурсия - обрабатываем подпапки
                if (recursive)
                {
                    string[] subfolders = DirectoryService.GetDirectories(folderPath);
                    foreach (string subfolderPath in subfolders)
                    {
                        if (DirectoryService.HasAccess(subfolderPath))
                        {
                            totalSize += CalculateFilesByPatternSize(subfolderPath, true, pattern);
                        }
                    }
                }
            }
            catch
            {
                // Логирование ошибок
            }

            return totalSize;
        }

        // 3. Метод для PatternType.Folder - ищет папки по паттерну и считает ВСЕ в них
        private long CalculateFoldersByPatternSize(string folderPath, bool recursive, CleaningInformation_Pattern pattern)
        {
            long totalSize = 0;

            if (!DirectoryService.Exists(folderPath))
                return 0;

            try
            {
                // Проверяем ВСЕ подпапки на совпадение с паттерном
                string[] subfolders = DirectoryService.GetDirectories(folderPath);

                foreach (string subfolderPath in subfolders)
                {
                    if (!DirectoryService.HasAccess(subfolderPath))
                        continue;

                    // Проверяем, совпадает ли папка с паттерном
                    if (MathPattern(subfolderPath, pattern.IncludePattern, true))
                    {
                        // Если совпала - считаем ВСЕ файлы в ней (рекурсивно)
                        totalSize += CalculateAllFilesSize(subfolderPath, true);
                    }

                    // Если нужна рекурсия - продолжаем поиск в подпапках
                    // (даже если текущая папка совпала, нужно искать другие совпадения вглубь)
                    if (recursive)
                    {
                        totalSize += CalculateFoldersByPatternSize(subfolderPath, true, pattern);
                    }
                }
            }
            catch
            {
                // Логирование ошибок
            }

            return totalSize;
        }


        //private long GetSize(string path, bool recursive, CleaningInformation_Pattern pattern)
        //{
        //    long size = 0;

        //    if (!DirectoryService.Exists(path))
        //    {
        //        return 0;
        //    }
        //    if ((pattern.Type & (PatternType.File | PatternType.All)) != 0)
        //    {
        //        try
        //        {
        //            string[] files = DirectoryService.GetFiles(path);

        //            foreach (string filePath in files)
        //            {
        //                if (FileService.HasAccess(filePath))
        //                {
        //                    bool includeFile = false;

        //                    if ((pattern.Type & PatternType.File) != 0)
        //                    {
        //                        includeFile = MathPattern(filePath, pattern.IncludePattern, false);
        //                    }
        //                    else if ((pattern.Type & PatternType.All) != 0)
        //                    {
        //                        includeFile = true;
        //                    }
        //                    if (includeFile)
        //                    {
        //                        size += FileService.GetSize(filePath);
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {

        //        }
        //    }
        //    if ((pattern.Type & (PatternType.Folder | PatternType.File | PatternType.All)) != 0 && recursive)
        //    {

        //        try
        //        {
        //            string[] directories = DirectoryService.GetDirectories(path);

        //            for (int i = 0; i < directories.Length; i++)
        //            {
        //                if (DirectoryService.HasAccess(directories[i]))
        //                {
        //                    bool folderMatched = false;

        //                    if ((pattern.Type & PatternType.Folder) != 0)
        //                    {

        //                        folderMatched = MathPattern(directories[i], pattern.IncludePattern, true);
        //                    }

        //                    if (folderMatched)
        //                    {
        //                        CleaningInformation_Pattern newPattern = new CleaningInformation_Pattern
        //                        {
        //                            Type = PatternType.All,
        //                            IncludePattern = pattern.IncludePattern.ToList() ?? new List<string>(),
        //                            ExcludePattern = pattern.ExcludePattern.ToList() ?? new List<string>(),
        //                        };
        //                        newPattern.IncludePattern.Remove(directories[i].ToString());


        //                        size += GetSize(directories[i], recursive, newPattern);
        //                    }


        //                    if ((pattern.Type & (PatternType.File | PatternType.All)) != 0 && folderMatched)
        //                    {
        //                        size += GetSize(directories[i], recursive, pattern);
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {

        //        }

        //    }
        //    return size;
        //}

        private bool MathPattern(string path, List<string> patterns, bool IsFolder = false)
        {
            if (patterns.Count <= 0)
            {
                return false;
            }

            if (IsFolder)
            {
                string name = DirectoryService.GetName(path);
      

                foreach (string pattern in patterns)
                {
                    if (String.Equals(name, pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            else
            {

                string name = FileService.GetName(path);

                foreach (string pattern in patterns)
                {
                    if (String.Equals(name, pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    try
                    {
                        if (!_regexCache.TryGetValue(pattern, out var regex))
                        {
                            string regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                            regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            _regexCache[pattern] = regex;
                        }

                        if (regex.IsMatch(name))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
            }
            return false;

        }

        //private bool MathPattern<T>(T path, CleaningInformation_Pattern pattern)
        //{
        //    if (path != null)
        //    {
        //        string name = String.Empty;

        //        if (path is FileInfo fileInfo)
        //        {
        //            name = fileInfo.Name;
        //        }
        //        if (path is DirectoryInfo dirInfo)
        //        {
        //            name = dirInfo.Name;
        //        }

        //        switch (pattern.Type)
        //        {
        //            case PatternType.All:
        //                return true;

        //            case PatternType.File:
        //            case PatternType.Folder:
        //                return CheckPattern(name, pattern.IncludePattern);

        //            default:
        //                return false;
        //        }
        //    }
        //    return false;
        //}
        Stopwatch timer = new Stopwatch();
        private bool CheckPattern(string name, List<string> patterns)
        {
            if (patterns == null || patterns.Count == 0)
            {
                return false;
            }

            foreach (string pattern in patterns)
            {
                if (pattern == "*")
                {
                    return true;
                }
                // 1. Проверка точного совпадения (для простых имен: "Temp", "ReportArchive")
                if (string.Equals(name, pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                try
                {
                    //timer.Restart();
                    // 2. Проверка wildcard (для паттернов: "thumbcache_*.db", "iconcache_*")
                    if (pattern.Contains("*") || pattern.Contains("?"))
                    {
                        if(!_regexCache.TryGetValue(pattern, out var regex))
                        {
                            string regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                            regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            _regexCache[pattern] = regex;
                        }

                        if (regex.IsMatch(name))
                        {
                            return true;
                        }
                    }
                    //timer.Stop();
                    //MessageBox.Show(timer.ElapsedMilliseconds.ToString());
                }
                catch {}
            }

            return false;
        }


        //private long GetFilesSizeRecursive(string folderPath, bool recursive, CleaningInformation_Pattern includePattern, AccessLevel accessLevel, bool isFolderPattern = false)
        //{
        //    long size = 0;

        //    try
        //    {
        //         === 1.Сканируем файлы ===
        //        string[] files = Directory.GetFiles(folderPath);

        //        foreach (string filePath in files)
        //        {
        //            FileInfo file = new FileInfo(filePath);

        //            если папка совпала с паттерном → считаем все файлы в ней
        //            if (isFolderPattern)
        //            {
        //                if (HasAccessToFile(file, ref accessLevel))
        //                    size += file.Length;

        //                continue;
        //            }

        //            иначе — считаем только файлы по паттерну
        //            if (IsFileToPattern(file.Name, ref includePatterns) &&
        //                HasAccessToFile(file, ref accessLevel))
        //            {
        //                size += file.Length;
        //            }
        //        }

        //         === 2.Рекурсивный обход подпапок ===
        //        if (!recursive)
        //            return size;

        //        string[] subdirectories = Directory.GetDirectories(folderPath);

        //        foreach (string subdirectory in subdirectories)
        //        {
        //            if (!HasAccessToFolder(subdirectory, ref accessLevel))
        //                continue;

        //            новая папка подходит по паттерну ?
        //           bool matchedFolder = IsFolderToPattern(subdirectory, ref includePatterns);

        //            рекурсия идет ВСЕГДА,
        //             но флаг isFolderPattern активируется только при совпадении папки
        //            size += GetFilesSizeRecursive(
        //                subdirectory,
        //                true,
        //                includePatterns,
        //                accessLevel,
        //                matchedFolder || isFolderPattern
        //            );
        //        }
        //    }
        //    catch
        //    {
        //        пропускаем недоступные каталоги
        //    }

        //    return size;
        //}




        /// <summary>
        /// Проверяет, совпадает ли файл по паттерну
        /// </summary>
        private bool IsFileToPattern(string fileName, ref List<string> patterns)
        {
            foreach (string pattern in patterns)
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    continue;
                }

                // Если паттерн "*" - совпадает с любым файлом
                if (pattern == "*")
                {
                    return true;
                }

                // Проверяем, содержит ли паттерн wildcards (*)
                if (pattern.Contains('*'))
                {
                    // Используем метод для проверки wildcards
                    if (IsWildcardMatch(fileName, pattern))
                    {
                        return true;
                    }
                }
                else
                {
                    // Простая проверка на вхождение
                    if (fileName.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет соответствие файла паттерну с wildcards (*)
        /// </summary>
        private bool IsWildcardMatch(string fileName, string pattern)
        {
            try
            {
                // Конвертируем паттерн в регулярное выражение
                string regexPattern = "^" + Regex.Escape(pattern)
                    .Replace("\\*", ".*")      // * → любое количество символов
                    + "$";                     // ? не обрабатываем, так как нет в паттернах

                return Regex.IsMatch(fileName, regexPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                // Fallback: убираем звездочки и проверяем вхождение
                return fileName.Contains(pattern.Replace("*", ""),
                    StringComparison.OrdinalIgnoreCase);
            }
        }


        /// <summary>
        /// Проверяет, совпадает ли папка по паттерну
        /// </summary>
        private bool IsFolderToPattern(string folderPath, ref List<string> patterns)
        {
            string name = Path.GetFileName(folderPath);

            foreach (string pattern in patterns)
            {
                if (string.IsNullOrEmpty(pattern))
                    continue;

                if (pattern == "*")
                    return true;

                if (IsWildcardMatch(name, pattern))
                    return true;
            }

            return false;
        }

    }
}
