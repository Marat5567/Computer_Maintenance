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

        ///<summary>
        ///Метод для получения доступных накопителей
        ///<summary>
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
        ///Метод для получения размера секции
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
                    totalSizeBytes = GetSizeAllFiles(path, recursiveSearch || recursiveDelete);
                    break;
                case PatternType.File:
                    totalSizeBytes = GetSizeMatchingFiles(path, recursiveSearch, patternInclude);
                    break;
                case PatternType.Folder:
                    totalSizeBytes = GetSizeFilesInMatchingFolders(path, recursiveSearch, recursiveDelete, patternInclude);
                    break;
            }
            return ConvertSizeService.ConvertSize(totalSizeBytes);
        }

        ///<summary>
        ///Метод для получения размера всех файлов 
        ///<summary>
        private long GetSizeAllFiles(string path, bool recursiveSerch)
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
                        size += GetSizeAllFiles(dirPath, true);
                    }
                }
            }
            return size;
        }

        ///<summary>
        ///Метод для получения размера всех файлов по паттерну для файлов
        ///<summary>
        private long GetSizeMatchingFiles(string path, bool recursiveSearch, List<string>patterns) 
        {
            if (patterns == null || patterns.Count == 0) return 0;
            long size = 0;

            foreach (string filePath in DirectoryService.GetFiles(path))
            {

                if (FileService.HasAccess(filePath) && MathPattern(filePath, ref patterns, false))
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
                        size += GetSizeMatchingFiles(dirPath, recursiveSearch, patterns);
                    }
                }
            }
            return size;
        }

        ///<summary>
        ///Метод для получения размера всех файлов в папках по паттерну для папок
        ///<summary>
        private long GetSizeFilesInMatchingFolders(string path, bool recursiveSearch, bool recursiveDelete, List<string>patterns)
        {
            if (patterns == null || patterns.Count == 0) return 0;

            long size = 0;

            foreach (string dirPath in DirectoryService.GetDirectories(path))
            {
                if (DirectoryService.HasAccess(dirPath))
                {
                    if (MathPattern(dirPath, ref patterns, true))
                    {
                        size += GetSizeAllFiles(dirPath, recursiveDelete);
                    }
                    if (recursiveSearch)
                    {
                        size += GetSizeFilesInMatchingFolders(dirPath, recursiveSearch, recursiveDelete, patterns);
                    }
                }
            }
            return size;
        }

        private bool MathPattern(string path, ref List<string> patterns, bool IsFolder = false)
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
    }
}
