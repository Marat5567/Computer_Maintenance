using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

            totalSizeBytes = GetSize(cleaningInformation.Path, cleaningInformation.ClearRecursive, cleaningInformation.Pattern);

            return ConvertSizeService.ConvertSize(totalSizeBytes);
        }

        /// <summary>
        /// Метод для получения размера файлов
        /// </summary>

        public long GetSize(string? path, bool? recursive, CleaningInformation_Pattern pattern)
        {
            long size = 0;

            if (pattern.Type == PatternType.All || pattern.Type == PatternType.File)
            {
                if (!DirectoryService.Exists(path))
                {
                    return 0;
                }

                string[] files = DirectoryService.GetFiles(path);

                foreach (string file in files)
                {
                    if (FileService.HasAccess(file))
                    {
                        size += file.Length;
                    }
                }
            }

            return size;
        }

      

        private bool MathPattern<T>(T path, CleaningInformation_Pattern pattern)
        {
            if (path != null)
            {
                string name = String.Empty;

                if (path is FileInfo fileInfo)
                {
                    name = fileInfo.Name;
                }
                if (path is DirectoryInfo dirInfo)
                {
                    name = dirInfo.Name;
                }

                switch (pattern.Type)
                {
                    case PatternType.All:
                        return true;

                    case PatternType.File:
                    case PatternType.Folder:
                        return CheckPattern(name, pattern.IncludePattern);

                    default:
                        return false;
                }
            }
            return false;
        }
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
