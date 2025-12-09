using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Structs;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;
namespace Computer_Maintenance.Model.Config
{
    public static class CleanupLocations
    {
        ///summary>
        ///Метод для получения информации о чистки по доступу
        ///<summary>
        public static List<CleaningInformation> GetLocationsByAccess(DriveInfo dInfo, string systemDrive)
        {
            List <CleaningInformation> locations = new List<CleaningInformation>();
            switch (dInfo.DriveType)
            {
                case DriveType.Fixed:
                 locations.AddRange(GetLocationFor_Fixed(dInfo, systemDrive));
                    break;
                case DriveType.Removable:
                    break;
            }
            return locations;
        }


        ///<summary>
        ///Метод для получения информации о чистки для user
        ///<summary>
        private static List<CleaningInformation> GetLocationFor_Fixed(DriveInfo dInfo, string systemDrive)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();

            foreach ((string BrowserName, string CachePath) browser in GetInstalledBrowserCaches(dInfo))
            {
                locations.Add(
                    new CleaningInformation
                    {
                        TypeCleaning = TypeCleaning.UserBrowserCache,
                        SectionName = $"Кэш {browser.BrowserName}",
                        Path = browser.CachePath,
                        Pattern = new CleaningInformation_Pattern
                        {
                            Type = PatternType.All,
                            RecursiveDelete = true,
                        },
                        RecursiveSearch = false
                    }
                );
            }

            string recycleBinPath = GetRecycleBinPath(dInfo);
            if (Directory.Exists(recycleBinPath))
            {
                // Корзина текущего пользователя (только его файлы)
                string userSid = WindowsIdentity.GetCurrent().User?.Value ?? "";
                if (!string.IsNullOrEmpty(userSid))
                {
                    string userRecyclePath = Path.Combine(recycleBinPath, userSid);
                    if (Directory.Exists(userRecyclePath))
                    {
                        locations.Add(
                            new CleaningInformation
                            {
                                TypeCleaning = TypeCleaning.UserRecycleBin,
                                SectionName = $"Корзина ({dInfo.Name.Replace("\\", "")}) - мои файлы",
                                Path = userRecyclePath,
                                Pattern = new CleaningInformation_Pattern
                                {
                                    Type = PatternType.All,
                                    RecursiveDelete = false,
                                },
                                RecursiveSearch = false
                            }
                        );
                    }
                }
            }


            if (string.Equals(dInfo.Name, systemDrive, StringComparison.OrdinalIgnoreCase))
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                string tempPath = Path.GetTempPath();
                if (Directory.Exists(tempPath))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserTemp,
                            SectionName = "Временные файлы текущего пользователя",
                            Path = tempPath,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = false
                        }
                    );
                }


                string thumbnailCachePath = Path.Combine(localAppData, "Microsoft", "Windows", "Explorer");
                if (Directory.Exists(thumbnailCachePath))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.ThumbnailCache,
                            SectionName = "Кэш эскизов изображений",
                            Path = thumbnailCachePath,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.File,
                                IncludePattern = new List<string>
                                {
                                    "thumbcache_*.db",
                                    "iconcache_*.db",
                                },
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = true
                        }
                    );
                }

                string crashDumpsPath = Path.Combine(localAppData, "CrashDumps");
                if (Directory.Exists(crashDumpsPath))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.CrashDumps,
                            SectionName = "Дампы ошибок приложений",
                            Path = crashDumpsPath,
                            Pattern = new 
                            CleaningInformation_Pattern 
                            { 
                                Type = PatternType.All,
                                RecursiveDelete = false,
                            },
                            RecursiveSearch = false
                        }
                    );
                }

                string D3DSCache = Path.Combine(localAppData, "D3DSCache");
                if (Directory.Exists(D3DSCache))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.D3DSCache,
                            SectionName = "Временные файлы DirectX",
                            Path = D3DSCache,
                            Pattern = new
                            CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = false
                        }
                    );
                }

                string WindowsErrorReporting = Path.Combine(localAppData, "Microsoft", "Windows", "WER");
                if (Directory.Exists(WindowsErrorReporting))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.WindowsErrorReporting,
                            SectionName = "Отчеты об ошибках Windows",
                            Path = WindowsErrorReporting,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.Folder,
                                IncludePattern = new List<string>()
                                {
                                    "ReportArchive", 
                                    "ReportQueue",
                                    "Temp"
                                },
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = true,
                        }
                    );
                }

                string packagesPath = Path.Combine(localAppData, "Packages");
                if (Directory.Exists(packagesPath))
                {
                    locations.Add(
                        new CleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UWPTempFiles,
                            SectionName = "Временные файлы приложений Microsoft Store",
                            Path = packagesPath,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.Folder,
                                IncludePattern = new List<string>()
                                {
                                    "Temp", 
                                    "TempState"
                                },
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = true
                        }
                    );
                }
            }

            return locations;
        }

        ///<summary>
        ///Метод для получения информации о пути кэша браузеров
        ///<summary>

        public static List<(string BrowserName, string CachePath)> GetInstalledBrowserCaches(DriveInfo dInfo)
        {
            string profileDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))!;
            
            // Если диск не тот, где находится профиль — браузеров там быть не может
            if (!string.Equals(dInfo.Name, profileDrive, StringComparison.OrdinalIgnoreCase))
            {
                return new List<(string, string)>();
            }

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            List<(string BrowserName, string CachePath)> caches = new List<(string, string)>();

            string chromePath = Path.Combine(localAppData, "Google", "Chrome", "User Data", "Default", "Cache");
            if (Directory.Exists(chromePath))
            {
                caches.Add(("Google Chrome", chromePath));
            }

            string yandexPath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default", "Cache");
            if (Directory.Exists(yandexPath))
            {
                caches.Add(("Yandex Browser", yandexPath));
            }

            string edgePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default", "Cache");
            if (Directory.Exists(edgePath))
            {
                caches.Add(("Microsoft Edge", edgePath));
            }

            string firefoxProfilesPath = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (Directory.Exists(firefoxProfilesPath))
            {
                foreach (var profileDir in Directory.GetDirectories(firefoxProfilesPath))
                {
                    string cacheDir = Path.Combine(profileDir, "cache2");
                    if (Directory.Exists(cacheDir))
                    {
                        caches.Add(("Mozilla Firefox", cacheDir));
                    }
                }
            }

            return caches;
        }


        ///<summary>
        /// Вспомогательный метод для получения пути к корзине
        ///<summary>
        private static string GetRecycleBinPath(DriveInfo dInfo)
        {
            // Для разных файловых систем
            if (dInfo.DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
            {
                return Path.Combine(dInfo.RootDirectory.FullName, "$Recycle.Bin");
            }
            else if (dInfo.DriveFormat.Equals("FAT32", StringComparison.OrdinalIgnoreCase) ||
                     dInfo.DriveFormat.Equals("FAT", StringComparison.OrdinalIgnoreCase))
            {
                // В FAT корзина называется Recycled
                return Path.Combine(dInfo.RootDirectory.FullName, "Recycled");
            }

            // По умолчанию для NTFS
            return Path.Combine(dInfo.RootDirectory.FullName, "$Recycle.Bin");
        }
    }
}
