using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using System.Security.Principal;
namespace Computer_Maintenance.Model.Config
{
    public static class CleanupLocations
    {

        private readonly static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private readonly static string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        ///summary>
        ///Метод для получения информации о чистки по доступу
        ///<summary>
        public static List<CleaningInformation> GetLocationsByDriveType(DriveInfo dInfo, string systemDrive)
        {
            List <CleaningInformation> locations = new List<CleaningInformation>();
            switch (dInfo.DriveType)
            {
                case DriveType.Fixed:
                 locations.AddRange(GetLocationsFor_FixedDrive(dInfo, systemDrive));
                    break;
                case DriveType.Removable:
                    break;
            }
            return locations;
        }

        private static List<CleaningInformation> GetBrowserCaches(DriveInfo dInfo)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            List<CleaningInformation> locations = new();
            List<string> browsers = GetInstalledBrowserCaches(dInfo);

            foreach (string browser in browsers)
            {
                if (browser == "Mozilla Firefox")
                {
                    // Firefox: ищем профиль
                    string firefoxProfiles = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
                    foreach (string profileDir in DirectoryService.GetDirectories(firefoxProfiles))
                    {
                        string cache2 = Path.Combine(profileDir, "cache2");
                        if (!DirectoryService.Exists(cache2))
                            continue;

                        locations.Add(new CleaningInformation
                        {
                            SectionName = browser,
                            SubItems = new List<SubCleaningInformation>
                            {
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "cache2",
                                    Path = cache2,
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = true
                                },
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "cache2/entries",
                                    Path = Path.Combine(cache2, "entries"),
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = false
                                },
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "cache2/doomed",
                                    Path = Path.Combine(cache2, "doomed"),
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = false
                                },
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "thumbnails",
                                    Path = Path.Combine(profileDir, "thumbnails"),
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = false
                                },
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "jumpListCache",
                                    Path = Path.Combine(profileDir, "jumpListCache"),
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = false
                                },
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserBrowserCache,
                                    SectionName = "startupCache",
                                    Path = Path.Combine(profileDir, "startupCache"),
                                    Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                                    RecursiveSearch = false
                                }
                            }
                        });
                    }

                    continue;
                }

                string basePath = null;

                if (browser == "Google Chrome")
                {
                    basePath = Path.Combine(
                        localAppData,
                        "Google",
                        "Chrome",
                        "User Data",
                        "Default");
                }
                else if (browser == "Yandex Browser")
                {
                    basePath = Path.Combine(
                        localAppData,
                        "Yandex",
                        "YandexBrowser",
                        "User Data",
                        "Default");
                }
                else if (browser == "Microsoft Edge")
                {
                    basePath = Path.Combine(
                        localAppData,
                        "Microsoft",
                        "Edge",
                        "User Data",
                        "Default");
                }
                else
                {
                    basePath = null;
                }
                if (basePath == null)
                    continue;


                locations.Add(new CleaningInformation
                {
                    SectionName = browser,
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Основной кэш браузера",
                            Path = Path.Combine(basePath, "Cache"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш скомпилированного кода (JS/WASM)",
                            Path = Path.Combine(basePath, "Code Cache"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш графического ускорителя (GPUCache)",
                            Path = Path.Combine(basePath, "GPUCache"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш сервис-воркеров (офлайн-данные сайтов)",
                            Path = Path.Combine(basePath, "Service Worker", "CacheStorage"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false,
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Файлы восстановления сессии",
                            Path = Path.Combine(basePath, "Sessions"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Временные данные расширений",
                            Path = Path.Combine(basePath, "Storage", "ext"),
                            Pattern = new CleaningInformation_Pattern { Type = PatternType.All, RecursiveDelete = true },
                            RecursiveSearch = false
                        }

                    }
                });
            }
            return locations;
        }



        private static CleaningInformation GetRecycleBin(DriveInfo dInfo)
        {
            string recycleBinPath = GetRecycleBinPath(dInfo);
            if (DirectoryService.Exists(recycleBinPath))
            {
                // Корзина текущего пользователя (только его файлы)
                string userSid = WindowsIdentity.GetCurrent().User?.Value ?? "";
                if (!string.IsNullOrEmpty(userSid))
                {
                    string userRecyclePath = Path.Combine(recycleBinPath, userSid);
                    if (Directory.Exists(userRecyclePath))
                    {
                        return new CleaningInformation
                        {
                            OnlyOnePoint = true,
                            SectionName = $"Корзина ({dInfo.Name.Replace("\\", "")}) - мои файлы",
                            SubItems = new List<SubCleaningInformation>
                            {
                                new SubCleaningInformation
                                {
                                    TypeCleaning = TypeCleaning.UserRecycleBin,
                                    Path = userRecyclePath,
                                    Pattern = new CleaningInformation_Pattern
                                    {
                                        Type = PatternType.All,
                                        RecursiveDelete = true,
                                    },
                                    RecursiveSearch = false,
                                },
                            }

                        };
                    }
                }
            }
            return null;
        }

        private static CleaningInformation GetUserTemp()
        {
            string tempPath = Path.GetTempPath();
            if (DirectoryService.Exists(tempPath))
            {
                return new CleaningInformation
                {
                    SectionName = "Временные файлы текущего пользователя",
                    OnlyOnePoint = true,
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserTemp,
                            Path = tempPath,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = false
                        }
                    }
                };
            }
            return null;
        }

        private static CleaningInformation GetWindowsCache()
        {
            string thumbnailCachePath = Path.Combine(localAppData, "Microsoft", "Windows", "Explorer");
            string D3DSCache = Path.Combine(localAppData, "D3DSCache");
            string updateCache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download");
            string dataStoreLogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "DataStore", "Logs");

            if (Directory.Exists(thumbnailCachePath))
            {
                return new CleaningInformation
                {
                    SectionName = "Кэш создаваемой Windows",
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
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
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.D3DSCache,
                            SectionName = "Кэш DirectX",
                            Path = D3DSCache,
                            Pattern = new
                            CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true,
                            },
                            RecursiveSearch = false
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.WindowsUpdateCache,
                            SectionName = "Кэш центра обновления Windows",
                            Path = updateCache,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true
                            },
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.WindowsUpdateLogs,
                            SectionName = "Логи центра обновления Windows",
                            Path = dataStoreLogs,
                            Pattern = new CleaningInformation_Pattern
                            {
                                Type = PatternType.All,
                                RecursiveDelete = true
                            },
                            RecursiveSearch = false
                        }
                    }
                };
            }
            return null;
        }

        private static CleaningInformation GetWindowsErrorReporting()
        {
            string WindowsErrorReporting = Path.Combine(localAppData, "Microsoft", "Windows", "WER");

            if (Directory.Exists(WindowsErrorReporting))
            {
                return new CleaningInformation
                {
                    SectionName = "Отчеты об ошибках Windows",
                    OnlyOnePoint = true,
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.WindowsErrorReporting,
                            Path = WindowsErrorReporting,
                            Pattern = new CleaningInformation_Pattern{
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
                    }
                };
            }
            return null;
        }
            
        private static CleaningInformation GetWindowsTemp()
        {
            string packagesPath = Path.Combine(localAppData, "Packages");
            if (DirectoryService.Exists(packagesPath))
            {
                return new CleaningInformation
                {
                    SectionName = "Временные файлы создаваемой Windows",
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
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
                    }
                };
            }
            return null;
        }

        private static CleaningInformation GetApllicationsCrashDump()
        {
            string crashDumpsPath = Path.Combine(localAppData, "CrashDumps");
            if (Directory.Exists(crashDumpsPath))
            {
                return new CleaningInformation
                {
                    SectionName = "Дампы ошибок приложений",
                    OnlyOnePoint = true,
                    SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
                        {
                             TypeCleaning = TypeCleaning.CrashDumps,
                             Path = crashDumpsPath,
                             Pattern = new
                             CleaningInformation_Pattern
                             {
                                 Type = PatternType.All,
                                 RecursiveDelete = false,
                             },
                             RecursiveSearch = false
                        }
                    }
                };
            }
            return null;
        }
        

        ///<summary>
        ///Метод для получения информации о чистки
        ///<summary>
        private static List<CleaningInformation> GetLocationsFor_FixedDrive(DriveInfo dInfo, string systemDrive)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();

            if (string.Equals(dInfo.Name, systemDrive, StringComparison.OrdinalIgnoreCase))
            {
                locations.AddRange(GetBrowserCaches(dInfo));
                locations.Add(GetRecycleBin(dInfo));
                locations.Add(GetUserTemp());
                locations.Add(GetWindowsCache());
                locations.Add(GetWindowsTemp());
                locations.Add(GetWindowsErrorReporting());
                locations.Add(GetApllicationsCrashDump());
            }
            return locations;
        }


        ///<summary>
        ///Метод для получения информации о пути кэша браузеров
        ///<summary>

        private static List<string> GetInstalledBrowserCaches(DriveInfo dInfo)
        {
            string profileDrive = Path.GetPathRoot(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))!;

            // На другом диске браузеров нет
            if (!string.Equals(dInfo.Name, profileDrive, StringComparison.OrdinalIgnoreCase))
                return new List<string>();

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            List<string> caches = new();

            // Chrome
            string chromePath = Path.Combine(localAppData, "Google", "Chrome", "User Data", "Default", "Cache");
            if (DirectoryService.Exists(chromePath))
                caches.Add("Google Chrome");

            // Yandex
            string yandexPath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default", "Cache");
            if (DirectoryService.Exists(yandexPath))
                caches.Add("Yandex Browser");

            // Edge
            string edgePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default", "Cache");
            if (DirectoryService.Exists(edgePath))
                caches.Add("Microsoft Edge");

            // Firefox (несколько профилей → достаточно наличия хотя бы одного)
            string firefoxProfilesPath = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (DirectoryService.Exists(firefoxProfilesPath))
            {
                foreach (var profileDir in DirectoryService.GetDirectories(firefoxProfilesPath))
                {
                    string cacheDir = Path.Combine(profileDir, "cache2");
                    if (DirectoryService.Exists(cacheDir))
                    {
                        caches.Add("Mozilla Firefox");
                        break;
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
