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

        public static List<CleaningInformation> GetLocationsByDriveType(DriveInfo dInfo, string systemDrive)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();

            switch (dInfo.DriveType)
            {
                case DriveType.Fixed:
                    if (string.Equals(dInfo.Name, systemDrive, StringComparison.OrdinalIgnoreCase))
                    {
                        locations.AddRange(GetLocationsFor_FixedDrive(dInfo, systemDrive));
                    }
                    else
                        locations.AddRange(GetLocationsFor_OtherFixedDrive(dInfo));
                    break;
            }


            return locations;
        }
        private static List<CleaningInformation> GetLocationsFor_OtherFixedDrive(DriveInfo dInfo)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();
            locations.Add(GetRecycleBin(dInfo));
            return locations;
        }


        private static List<CleaningInformation> GetLocationsFor_FixedDrive(DriveInfo dInfo, string systemDrive)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();

            locations.AddRange(GetBrowserCaches(dInfo));
            locations.Add(GetRecycleBin(dInfo));
            locations.Add(GetUserTemp());
            locations.Add(GetWindowsTemp());
            locations.Add(GetApplicationsCrashDump());

            return locations;
        }

        private static List<CleaningInformation> GetBrowserCaches(DriveInfo dInfo)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            List<CleaningInformation> locations = new List<CleaningInformation>();
            List<string> browsers = GetInstalledBrowserCaches(dInfo);

            foreach (string browser in browsers)
            {
                if (browser == "Mozilla Firefox")
                {
                    string profilesPath = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");

                    List<SubCleaningInformation> subItems = new List<SubCleaningInformation>();
                    string[] subFolders = new string[] { "cache2", "cache2/entries", "cache2/doomed", "thumbnails", "jumpListCache", "startupCache" };

                    foreach (string folder in subFolders)
                    {
                        subItems.Add(new SubCleaningInformation
                        {
                            SectionName = folder,
                            TypeCleaning = TypeCleaning.UserBrowserCache,
                            SearchConfig = new SearchConfiguration
                            {
                                BasePath = Path.Combine(profilesPath, folder),
                                SearchTarget = SearchTarget.Files,
                                SearchScope = SearchScope.Recursive,
                                DeleteScope = DeleteScope.OnlyFiles,
                                IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                            }
                        });
                    }

                    locations.Add(new CleaningInformation
                    {
                        SectionName = browser,
                        IsSingleItem = false,
                        SubItems = subItems
                    });

                    continue;
                }

                string basePath = null;
                if (browser == "Google Chrome") basePath = Path.Combine(localAppData, "Google", "Chrome", "User Data", "Default");
                else if (browser == "Yandex Browser") basePath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default");
                else if (browser == "Microsoft Edge") basePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default");

                List<SubCleaningInformation> browserSubItems = new List<SubCleaningInformation>();
                string[] folders = new string[] { "Cache", "Code Cache", "GPUCache", "Service Worker\\CacheStorage", "Sessions", "Storage\\ext" };

                foreach (string folder in folders)
                {
                    browserSubItems.Add(new SubCleaningInformation
                    {
                        SectionName = folder,
                        TypeCleaning = TypeCleaning.UserBrowserCache,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(basePath, folder),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.OnlyFiles,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    });
                }

                locations.Add(new CleaningInformation
                {
                    SectionName = browser,
                    IsSingleItem = false,
                    SubItems = browserSubItems
                });
            }

            return locations;
        }
        private static List<string> GetInstalledBrowserCaches(DriveInfo dInfo)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            List<string> caches = new List<string>
            {
                "Google Chrome",
                "Yandex Browser",
                "Microsoft Edge",
                "Mozilla Firefox"
            };

            return caches;
        }

        private static CleaningInformation GetRecycleBin(DriveInfo dInfo)
        {
            string recycleBinPath = GetRecycleBinPath(dInfo);
            string userSid = WindowsIdentity.GetCurrent().User?.Value ?? "";
            string userRecyclePath = Path.Combine(recycleBinPath, userSid);

            return new CleaningInformation
            {
                SectionName = "Корзина " + dInfo.Name.Replace("\\", ""),
                IsSingleItem = true,
                SingleItem = new SubCleaningInformation
                {
                    TypeCleaning = TypeCleaning.UserRecycleBin,
                    SectionName = "Корзина " + dInfo.Name,
                    SearchConfig = new SearchConfiguration
                    {
                        BasePath = userRecyclePath,
                        SearchTarget = SearchTarget.Files,
                        SearchScope = SearchScope.Recursive,
                        DeleteScope = DeleteScope.AllContents,
                        IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                    }
                }
            };
        }
        private static string GetRecycleBinPath(DriveInfo dInfo)
        {
            if (dInfo.DriveFormat.Equals("NTFS", System.StringComparison.OrdinalIgnoreCase))
                return Path.Combine(dInfo.RootDirectory.FullName, "$Recycle.Bin");

            if (dInfo.DriveFormat.Equals("FAT32", System.StringComparison.OrdinalIgnoreCase) ||
                dInfo.DriveFormat.Equals("FAT", System.StringComparison.OrdinalIgnoreCase))
                return Path.Combine(dInfo.RootDirectory.FullName, "Recycled");

            return Path.Combine(dInfo.RootDirectory.FullName, "$Recycle.Bin");
        }
        private static CleaningInformation GetUserTemp()
        {
            string tempPath = Path.GetTempPath();

            return new CleaningInformation
            {
                SectionName = "Временные файлы текущего пользователя",
                IsSingleItem = true,
                SingleItem = new SubCleaningInformation
                {
                    SectionName = "UserTemp",
                    TypeCleaning = TypeCleaning.UserTemp,
                    SearchConfig = new SearchConfiguration
                    {
                        BasePath = tempPath,
                        SearchTarget = SearchTarget.Files,
                        SearchScope = SearchScope.Recursive,
                        DeleteScope = DeleteScope.AllContents,
                        IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                    }
                }
            };
        }

        private static CleaningInformation GetWindowsTemp()
        {
            string windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            return new CleaningInformation
            {
                SectionName = "Временные файлы, создаваемые Windows",
                IsSingleItem = false,
                SubItems = new List<SubCleaningInformation>
                {
                    new SubCleaningInformation
                    {
                        SectionName = "Временные файлы Windows",
                        TypeCleaning = TypeCleaning.WindowsTempFiles,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(windowsFolder, "Temp"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Кэш эскизов изображений",
                        TypeCleaning = TypeCleaning.ThumbnailCache,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(localAppData, "Microsoft", "Windows", "Explorer"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.CurrentDirectory,
                            DeleteScope = DeleteScope.OnlyFiles,
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern { IsActive = true, Pattern = "iconcache*.db", PatternMatchType = PatternMatchType.Simple },
                                new SearchPattern { IsActive = true, Pattern = "thumbcache*.db", PatternMatchType = PatternMatchType.Simple }
                            }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Кэш DirectX",
                        TypeCleaning = TypeCleaning.D3DSCache,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(localAppData, "D3DSCache"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Кэш установленных обновлений Windows",
                        TypeCleaning = TypeCleaning.WindowsUpdateCache,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(windowsFolder, "SoftwareDistribution", "Download"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Логи обновлений Windows",
                        TypeCleaning = TypeCleaning.WindowsUpdateLogs,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(windowsFolder, "SoftwareDistribution", "DataStore", "Logs"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Отчёты об ошибках Windows",
                        TypeCleaning = TypeCleaning.WindowsErrorReporting,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(localAppData, "Microsoft", "Windows", "WER"),
                            SearchTarget = SearchTarget.Directories,
                            SearchScope = SearchScope.Recursive,
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "ReportArchive",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "ReportQueue",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "Temp",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                }
                            }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Системные отчёты об ошибках Windows",
                        TypeCleaning = TypeCleaning.SystemWindowsErrorReporting,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "WER"),
                            SearchTarget = SearchTarget.Directories,
                            SearchScope = SearchScope.Recursive,
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "ReportArchive",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,

                                        DeleteScope = DeleteScope.AllContents
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "ReportQueue",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "Temp",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
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
                            BasePath = Path.Combine(localAppData, "Packages"),
                            SearchTarget = SearchTarget.Directories,
                            SearchScope = SearchScope.Recursive,
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "Temp",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "TempState",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.AllContents
                                    }
                                }
                            }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Системные логи",
                        TypeCleaning = TypeCleaning.SystemLogs,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(windowsFolder, "Logs"),
                            SearchTarget = SearchTarget.Directories,
                            SearchScope = SearchScope.CurrentDirectory,
                            
                            IncludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "CBS",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.CurrentDirectory,
                                        DeleteScope = DeleteScope.OnlyFiles,
                                        IncludePatterns = new List<SearchPattern>
                                        {
                                            new SearchPattern
                                            {
                                                IsActive = true,
                                                Pattern = "*.log",
                                                PatternMatchType = PatternMatchType.Simple,
                                            }
                                        }
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "DISM",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.Recursive,
                                        DeleteScope = DeleteScope.OnlyFiles,
                                        IncludePatterns = new List<SearchPattern>
                                        {
                                            new SearchPattern
                                            {
                                                IsActive = true,
                                                Pattern = "*.log",
                                                PatternMatchType = PatternMatchType.Simple,
                                            }
                                        }
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "WindowsUpdate",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.CurrentDirectory,
                                        DeleteScope = DeleteScope.OnlyFiles,
                                        IncludePatterns = new List<SearchPattern>
                                        {
                                            new SearchPattern
                                            {
                                                IsActive = true,
                                                Pattern = "*.log",
                                                PatternMatchType = PatternMatchType.Simple,
                                            }
                                        }
                                    }
                                },
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "DPX",
                                    PatternMatchType = PatternMatchType.Exact,
                                    ChildConfiguration = new SearchConfiguration
                                    {
                                        SearchTarget = SearchTarget.Files,
                                        SearchScope = SearchScope.CurrentDirectory,
                                        DeleteScope = DeleteScope.OnlyFiles,
                                        IncludePatterns = new List<SearchPattern>
                                        {
                                            new SearchPattern
                                            {
                                                IsActive = true,
                                                Pattern = "*.log",
                                                PatternMatchType = PatternMatchType.Simple,
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    },
                    new SubCleaningInformation
                    {
                        SectionName = "Файлы для ускорения запуска программ (Prefetch)",
                        TypeCleaning = TypeCleaning.Prefetch,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"),
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.AllContents,
                            ExcludePatterns = new List<SearchPattern>
                            {
                                new SearchPattern
                                {
                                    IsActive = true,
                                    Pattern = "*.mkd",
                                    PatternMatchType = PatternMatchType.Simple
                                }
                            }
                        }
                    }

                }
            };
        }

        private static CleaningInformation GetApplicationsCrashDump()
        {
            string crashDumpsPath = Path.Combine(localAppData, "CrashDumps");

            return new CleaningInformation
            {
                SectionName = "Дампы ошибок приложений",
                IsSingleItem = true,
                SingleItem = new SubCleaningInformation
                {
                    SectionName = "CrashDumps",
                    TypeCleaning = TypeCleaning.CrashDumps,
                    SearchConfig = new SearchConfiguration
                    {
                        BasePath = crashDumpsPath,
                        SearchTarget = SearchTarget.Files,
                        SearchScope = SearchScope.CurrentDirectory,
                        DeleteScope = DeleteScope.OnlyFiles,
                        IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                    }
                }
            };
        }
    }
}