using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Structs;
using System.Security.Principal;
using static Computer_Maintenance.Model.Enums.FilesAndDirectories;
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
                    if (Directory.Exists(firefoxProfiles))
                    {
                        foreach (string profileDir in Directory.GetDirectories(firefoxProfiles))
                        {
                            string cache2 = Path.Combine(profileDir, "cache2");
                            if (!Directory.Exists(cache2))
                                continue;

                            locations.Add(new CleaningInformation
                            {
                                SectionName = browser,
                                IsSingleItem = false,
                                SubItems = new List<SubCleaningInformation>
                                {
                                    new SubCleaningInformation
                                    {
                                        SectionName = "cache2",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = cache2,
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false
                                                }
                                            }
                                        },
                                    },
                                    new SubCleaningInformation
                                    {
                                        SectionName = "cache2/entries",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = Path.Combine(cache2, "entries"),
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false
                                                }
                                            }
                                        },
                                    },
                                    new SubCleaningInformation
                                    {
                                        SectionName = "cache2/doomed",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = Path.Combine(cache2, "doomed"),
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false
                                                }
                                            }
                                        },
                                    },
                                    new SubCleaningInformation
                                    {
                                        SectionName = "thumbnails",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = Path.Combine(profileDir, "thumbnails"),
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        },
                                    },
                                    new SubCleaningInformation
                                    {
                                        SectionName = "jumpListCache",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = Path.Combine(profileDir, "jumpListCache"),
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false
                                                }
                                            }
                                        },
                                    },
                                    new SubCleaningInformation
                                    {
                                        SectionName = "startupCache",
                                        TypeCleaning = TypeCleaning.UserBrowserCache,
                                        SearchConfig = new SearchConfiguration
                                        {
                                            BasePath = Path.Combine(profileDir, "startupCache"),
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.OnlyFiles,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false
                                                }
                                            }
                                        },
                                    },
                                }
                            });
                        }
                    }
                    continue;
                }

                string basePath = null;

                if (browser == "Google Chrome")
                {
                    basePath = Path.Combine(localAppData, "Google", "Chrome", "User Data", "Default");
                }
                else if (browser == "Yandex Browser")
                {
                    basePath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default");
                }
                else if (browser == "Microsoft Edge")
                {
                    basePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default");
                }

                if (basePath == null || !Directory.Exists(basePath))
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
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "Cache"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш скомпилированного кода",
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "Code Cache"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш графического ускорителя",
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "GPUCache"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Кэш сервис-воркеров",
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "Service Worker", "CacheStorage"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Файлы восстановления сессии",
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "Sessions"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SectionName = "Временные данные расширений",
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(basePath, "Storage", "ext"),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false
                                    }
                                }
                            }
                        }
                    }
                });
            }

            return locations;
        }



        private static CleaningInformation GetRecycleBin(DriveInfo dInfo)
        {
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
                        return new CleaningInformation
                        {
                            SectionName = $"Корзина {dInfo.Name.Replace("\\", "")}",
                            IsSingleItem = true,
                            SingleItem = new SubCleaningInformation
                            {
                                TypeCleaning = TypeCleaning.UserRecycleBin,
                                SectionName = $"Корзина {dInfo.Name}",
                                SearchConfig = new SearchConfiguration
                                {
                                    BasePath = userRecyclePath,
                                    SearchTarget = SearchTarget.All,
                                    SearchScope = SearchScope.Recursive,
                                    DeleteScope = DeleteScope.AllContents,
                                    IncludePatterns = new List<SearchPattern>
                                    {
                                        new SearchPattern
                                        {
                                            IsActive = false
                                        }
                                    }
                                }
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
            if (!Directory.Exists(tempPath)) { return null; }

            return new CleaningInformation
            {
                SectionName = "Временные файлы текущего пользователя",
                IsSingleItem = true,
                SingleItem = new SubCleaningInformation
                {
                    TypeCleaning = TypeCleaning.UserTemp,
                    SearchConfig = new SearchConfiguration
                    {
                        BasePath = tempPath,
                        SearchTarget = SearchTarget.Files,
                        SearchScope = SearchScope.Recursive,
                        DeleteScope = DeleteScope.OnlyFiles,
                        IncludePatterns = new List<SearchPattern>
                        {
                            new SearchPattern
                            {
                                IsActive = false
                            }
                        }
                    }
                }
            };
        }

        private static CleaningInformation GetWindowsTemp()
        {
            string thumbnailCachePath = Path.Combine(localAppData, "Microsoft", "Windows", "Explorer");
            string D3DSCache = Path.Combine(localAppData, "D3DSCache");
            string updateCache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download");
            string dataStoreLogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "DataStore", "Logs");
            string WindowsErrorReporting = Path.Combine(localAppData, "Microsoft", "Windows", "WER");
            string packagesPath = Path.Combine(localAppData, "Packages");

            return new CleaningInformation
            {
                SectionName = "Временные файлы создаваемой Windows",
                IsSingleItem = false,
                SubItems = new List<SubCleaningInformation>
                    {
                        new SubCleaningInformation
                        {
                            SectionName = "Кэш эскизов изображений",
                            TypeCleaning = TypeCleaning.ThumbnailCache,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = thumbnailCachePath,
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "thumbcache_*.db",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Simple,
                                    },
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "iconcache_*.db",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Simple,
                                    }
                                },
                            },

                        },
                        new SubCleaningInformation
                        {
                            SectionName = "Кэш DirectX",
                            TypeCleaning = TypeCleaning.D3DSCache,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = D3DSCache,
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.AllContents,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false,
                                    }
                                }
                            }
                        },

                        new SubCleaningInformation
                        {
                            SectionName = "Кэш центра обновления Windows",
                            TypeCleaning = TypeCleaning.WindowsUpdateCache,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = updateCache,
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.AllContents,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false,
                                    }
                                }

                            }
                        },
                        new SubCleaningInformation
                        {
                            SectionName = "Логи центра обновления Windows",
                            TypeCleaning = TypeCleaning.WindowsUpdateLogs,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = dataStoreLogs,
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.AllContents,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = false,
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            SectionName = "Отчеты об ошибках Windows",
                            TypeCleaning = TypeCleaning.WindowsErrorReporting,
                            SearchConfig = new SearchConfiguration

                            {
                                BasePath = WindowsErrorReporting,
                                SearchTarget = SearchTarget.Directories,
                                SearchScope = SearchScope.Recursive,

                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "ReportArchive",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Exact,
                                        ChildConfiguration = new SearchConfiguration
                                        {
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.AllContents,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        }
                                    },
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "ReportQueue",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Exact,
                                        ChildConfiguration = new SearchConfiguration
                                        {
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.AllContents,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        }
                                    },
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "Temp",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Exact,
                                        ChildConfiguration = new SearchConfiguration
                                        {
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.AllContents,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new SubCleaningInformation
                        {
                            SectionName = "Временные файлы приложений Microsoft Store",
                            TypeCleaning = TypeCleaning.UWPTempFiles,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = packagesPath,
                                SearchTarget = SearchTarget.Directories,
                                SearchScope = SearchScope.Recursive,
                                IncludePatterns = new List<SearchPattern>
                                {
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "Temp",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Exact,
                                        ChildConfiguration = new SearchConfiguration
                                        {
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.AllContents,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        }
                                    },
                                    new SearchPattern
                                    {
                                        IsActive = true,
                                        Pattern = "TempState",
                                        PatternMatchType = FilesAndDirectories.PatternMatchType.Exact,
                                        ChildConfiguration = new SearchConfiguration
                                        {
                                            SearchTarget = SearchTarget.Files,
                                            SearchScope = SearchScope.Recursive,
                                            DeleteScope = DeleteScope.AllContents,
                                            IncludePatterns = new List<SearchPattern>
                                            {
                                                new SearchPattern
                                                {
                                                    IsActive = false,
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };  
        }

        private static CleaningInformation GetApllicationsCrashDump()
        {
            string crashDumpsPath = Path.Combine(localAppData, "CrashDumps");
            if (Directory.Exists(crashDumpsPath))
            {
                return new CleaningInformation
                {
                    SectionName = "Дампы ошибок приложений",
                    IsSingleItem = true,
                    SingleItem = new SubCleaningInformation
                    {
                        TypeCleaning = TypeCleaning.CrashDumps,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = crashDumpsPath,
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = false,
                                }
                            }
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
                locations.Add(GetWindowsTemp());
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
            if (Directory.Exists(chromePath))
                caches.Add("Google Chrome");

            // Yandex
            string yandexPath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default", "Cache");
            if (Directory.Exists(yandexPath))
                caches.Add("Yandex Browser");

            // Edge
            string edgePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default", "Cache");
            if (Directory.Exists(edgePath))
                caches.Add("Microsoft Edge");

            // Firefox (несколько профилей → достаточно наличия хотя бы одного)
            string firefoxProfilesPath = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
            if (Directory.Exists(firefoxProfilesPath))
            {
                foreach (var profileDir in Directory.GetDirectories(firefoxProfilesPath))
                {
                    string cacheDir = Path.Combine(profileDir, "cache2");
                    if (Directory.Exists(cacheDir))
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
