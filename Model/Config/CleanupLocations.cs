using Computer_Maintenance.Model.Enums.SystemCleaning;
using Computer_Maintenance.Model.Structs;
using System.Security.Principal;

namespace Computer_Maintenance.Model.Config
{
    public static class CleanupLocations
    {
        private readonly static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static List<CleaningInformation> GetLocationsByDriveType(DriveInfo dInfo, string systemDrive)
        {
            switch (dInfo.DriveType)
            {
                case DriveType.Fixed:
                    if (string.Equals(dInfo.Name, systemDrive, StringComparison.OrdinalIgnoreCase))
                    {
                        return GetLocationsFor_FixedDrive(dInfo);
                    }
                    else
                    {
                        return GetLocationsFor_OtherFixedDrive(dInfo);
                    }
                default:
                    return new List<CleaningInformation>();
            }
        }
        private static List<CleaningInformation> GetLocationsFor_OtherFixedDrive(DriveInfo dInfo)
        {
            List<CleaningInformation> locations = new List<CleaningInformation>();
            locations.Add(GetRecycleBin(dInfo));
            return locations;
        }

        private static List<CleaningInformation> GetLocationsFor_FixedDrive(DriveInfo dInfo)
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
                // ================= FIREFOX =================
                if (browser == "Mozilla Firefox")
                {
                    string profilesRoot = Path.Combine(appData, "Mozilla", "Firefox", "Profiles");
                    if (!Directory.Exists(profilesRoot))
                        continue;

                    List<SubCleaningInformation> subItems = new List<SubCleaningInformation>();
                    string[] subFolders =
                    {
                            "cache2",
                            "cache2\\entries",
                            "cache2\\doomed",
                            "thumbnails",
                            "jumpListCache",
                            "startupCache"
                        };

                    foreach (string profileDir in Directory.GetDirectories(profilesRoot))
                    {
                        foreach (string folder in subFolders)
                        {
                            string fullPath = Path.Combine(profileDir, folder);
                            if (!Directory.Exists(fullPath))
                                continue;

                            // назначаем детализированный TypeCleaning для каждого подпункта Firefox
                            TypeCleaning type = folder switch
                            {
                                "cache2" => TypeCleaning.Firefox_Cache2,
                                "cache2\\entries" => TypeCleaning.Firefox_Cache2_Entries,
                                "cache2\\doomed" => TypeCleaning.Firefox_Cache2_Doomed,
                                "thumbnails" => TypeCleaning.Firefox_Thumbnails,
                                "jumpListCache" => TypeCleaning.Firefox_JumpListCache,
                                "startupCache" => TypeCleaning.Firefox_StartupCache,
                                _ => TypeCleaning.UserBrowserCache
                            };

                            subItems.Add(new SubCleaningInformation
                            {
                                SectionName = Path.GetFileName(profileDir) + "\\" + folder,
                                TypeCleaning = type,
                                SearchConfig = new SearchConfiguration
                                {
                                    BasePath = fullPath,
                                    SearchTarget = SearchTarget.Files,
                                    SearchScope = SearchScope.Recursive,
                                    DeleteScope = DeleteScope.OnlyFiles,
                                    IncludePatterns = new List<SearchPattern>
                                        {
                                            new SearchPattern { IsActive = false }
                                        }
                                }
                            });
                        }
                    }

                    if (subItems.Count > 0)
                    {
                        locations.Add(new CleaningInformation
                        {
                            SectionName = browser,
                            IsSingleItem = false,
                            SubItems = subItems
                        });
                    }

                    continue;
                }

                // ================= CHROMIUM =================
                string basePath = null;

                if (browser == "Google Chrome")
                    basePath = Path.Combine(localAppData, "Google", "Chrome", "User Data", "Default");
                else if (browser == "Yandex Browser")
                    basePath = Path.Combine(localAppData, "Yandex", "YandexBrowser", "User Data", "Default");
                else if (browser == "Microsoft Edge")
                    basePath = Path.Combine(localAppData, "Microsoft", "Edge", "User Data", "Default");

                if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
                    continue;

                List<SubCleaningInformation> browserSubItems = new List<SubCleaningInformation>();
                var folders = new Dictionary<string, TypeCleaning>
                    {
                        { "Cache", TypeCleaning.BrowserCache_Cache },
                        { "Code Cache", TypeCleaning.BrowserCache_CodeCache },
                        { "GPUCache", TypeCleaning.BrowserCache_GPUCache },
                        { "Service Worker\\CacheStorage", TypeCleaning.BrowserCache_ServiceWorker_CacheStorage },
                        { "Sessions", TypeCleaning.BrowserCache_Sessions },
                        { "Storage\\ext", TypeCleaning.BrowserCache_StorageExt }
                    };

                foreach (var kv in folders)
                {
                    string folder = kv.Key;
                    TypeCleaning type = kv.Value;
                    string fullPath = Path.Combine(basePath, folder);
                    if (!Directory.Exists(fullPath))
                        continue;

                    browserSubItems.Add(new SubCleaningInformation
                    {
                        SectionName = folder,
                        TypeCleaning = type,
                        SearchConfig = new SearchConfiguration
                        {
                            BasePath = fullPath,
                            SearchTarget = SearchTarget.Files,
                            SearchScope = SearchScope.Recursive,
                            DeleteScope = DeleteScope.OnlyFiles,
                            IncludePatterns = new List<SearchPattern> { new SearchPattern { IsActive = false } }
                        }
                    });
                }

                if (browserSubItems.Count > 0)
                {
                    locations.Add(new CleaningInformation
                    {
                        SectionName = browser,
                        IsSingleItem = false,
                        SubItems = browserSubItems
                    });
                }
            }

            return locations;
        }

        private static List<string> GetInstalledBrowserCaches(DriveInfo dInfo)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            List<string> caches = new List<string>();

            if (Directory.Exists(Path.Combine(localAppData, "Google", "Chrome")))
                caches.Add("Google Chrome");

            if (Directory.Exists(Path.Combine(localAppData, "Yandex", "YandexBrowser")))
                caches.Add("Yandex Browser");

            if (Directory.Exists(Path.Combine(localAppData, "Microsoft", "Edge")))
                caches.Add("Microsoft Edge");

            if (Directory.Exists(Path.Combine(appData, "Mozilla", "Firefox")))
                caches.Add("Mozilla Firefox");

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
            if (dInfo.DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
                return Path.Combine(dInfo.RootDirectory.FullName, "$Recycle.Bin");

            if (dInfo.DriveFormat.Equals("FAT32", StringComparison.OrdinalIgnoreCase) ||
                dInfo.DriveFormat.Equals("FAT", StringComparison.OrdinalIgnoreCase))
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