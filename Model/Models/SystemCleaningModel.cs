using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using System;
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
            return CleanupLocations.GetLocationsByDriveType(dInfo, GetSystemDrive());
        }

        ///<summary>
        ///Метод для получения размера секции
        ///<summary>
        public StorageSize GetSizeSection(SubCleaningInformation subCleaningInformation)
        {
            long totalSizeBytes = 0;

            PatternType type = subCleaningInformation.Pattern.Type;
            string path = subCleaningInformation.Path;
            bool recursiveSearch = subCleaningInformation.RecursiveSearch;
            bool recursiveDelete = subCleaningInformation.Pattern.RecursiveDelete;
            List<string> patternInclude = subCleaningInformation.Pattern.IncludePattern;

            switch (type)
            {
                case PatternType.All:
                    totalSizeBytes = GetSizeAllFiles(path, recursiveSearch || recursiveDelete);
                    break;
                case PatternType.File:
                    totalSizeBytes = GetSizeMatchingFiles(path, recursiveSearch, ref patternInclude);
                    break;
                case PatternType.Folder:
                    totalSizeBytes = GetSizeFilesInMatchingFolders(path, recursiveSearch, recursiveDelete, ref patternInclude);
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
                size += FileService.GetSize(filePath);
            }

            if (recursiveSerch)
            {
                foreach (string dirPath in DirectoryService.GetDirectories(path))
                {
                    size += GetSizeAllFiles(dirPath, recursiveSerch);
                }
            }
            return size;
        }

        ///<summary>
        ///Метод для получения размера всех файлов по паттерну для файлов
        ///<summary>
        private long GetSizeMatchingFiles(string path, bool recursiveSearch, ref List<string>patterns) 
        {
            if (patterns == null || patterns.Count == 0) return 0;
            long size = 0;

            foreach (string filePath in DirectoryService.GetFiles(path))
            {
                if (MathPatternFiles(filePath, ref patterns))
                {
                    size += FileService.GetSize(filePath);
                }
            }

            if (recursiveSearch)
            {
                foreach (string dirPath in DirectoryService.GetDirectories(path))
                {
                    size += GetSizeMatchingFiles(dirPath, recursiveSearch, ref patterns);
                }
            }
            return size;
        }

        ///<summary>
        ///Метод для получения размера всех файлов в папках по паттерну для папок
        ///<summary>
        private long GetSizeFilesInMatchingFolders(string path, bool recursiveSearch, bool recursiveDelete,  ref List<string> patterns)
        {
            if (patterns == null || patterns.Count == 0) return 0;

            long size = 0;


            foreach (string dirPath in DirectoryService.GetDirectories(path))
            {
                if (DirecoriyMathPattern(dirPath, ref patterns))
                {
                    size += GetSizeAllFiles(dirPath, recursiveDelete);
                }

                if (recursiveSearch)
                {
                    size += GetSizeFilesInMatchingFolders(dirPath, recursiveSearch, recursiveDelete, ref patterns);
                }

            }

            return size;
        }

        ///<summary>
        ///Метод для получения сравнения имени папок 
        ///<summary>
        private bool DirecoriyMathPattern(string path, ref List<string> patterns)
        {
            if (patterns.Count > 0)
            {
                foreach (string pattern in patterns)
                {
                    if(string.Equals(DirectoryService.GetName(path), pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        ///<summary>
        ///Метод для получения сравнения имени файлов
        ///<summary>
        private bool MathPatternFiles(string path, ref List<string> patterns)
        {
            if (patterns.Count <= 0)
            {
                return false;
            }

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

            return false;
        }
    }
}
