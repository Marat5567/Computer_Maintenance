namespace Computer_Maintenance.Model.Models
{

    public class SystemCleaningControlModel
    {
        public static class SystemPaths
        {
            // Пути, которые не зависят от конкретного диска
            public static readonly string UserTemp = Path.GetTempPath();
            public static readonly string WindowsTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            public static readonly string WindowsUpdateCache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download");
            public static readonly string CBSLogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs", "CBS");
            public static readonly string MiniDump = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Minidump");
            public static readonly string OldInstallPackages = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Installer");
            public static readonly string ReportArchive = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "WER", "ReportArchive");
            public static readonly string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        //МЕТОДЫ ДЛЯ ДИНАМИЧЕСКИХ ПУТЕЙ

        public string GetRecycleBinPath(DriveInfoModel drive)
        {
            return $@"{drive.Name}\$Recycle.Bin";
        }

        public string GetMemoryDumpsPath(DriveInfoModel drive)
        {
            return $@"{drive.Name}Windows\MEMORY.DMP";
        }

        public string GetRestorePointsPath(DriveInfoModel drive)
        {
            return $@"{drive.Name}System Volume Information";
        }

        public List<string> GetBrowserCachePaths(string profile = "Default")
        {
            var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return new List<string>
        {
            Path.Combine(local, "Google", "Chrome", "User Data", profile, "Cache"),
            Path.Combine(local, "Google", "Chrome", "User Data", profile, "Code Cache"),
            Path.Combine(local, "Yandex", "YandexBrowser", "User Data", profile, "Cache"),
            Path.Combine(local, "Yandex", "YandexBrowser", "User Data", profile, "Code Cache"),
            Path.Combine(local, "Microsoft", "Edge", "User Data", profile, "Cache"),
            Path.Combine(local, "Microsoft", "Edge", "User Data", profile, "Code Cache"),
            Path.Combine(local, "Opera Software", "Opera Stable", "Cache"),
            Path.Combine(local, "Opera Software", "Opera Stable", "Code Cache"),
        };
        }

        public List<DriveInfo> GetDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfo> readyDrives = new List<DriveInfo>();

            foreach (DriveInfo dInfo in allDrives)
            {
                if (dInfo.IsReady)
                {
                    if(dInfo.DriveType == DriveType.Network) //Пропускаем сетвые диски
                    {
                        continue;
                    }
                    readyDrives.Add(dInfo);
                }
            }

            return readyDrives;
        }

        public string GetSystemDriveName()
        {
            return Path.GetPathRoot(Environment.SystemDirectory);

        }
        public double BytesToGB(long bytes) => bytes / (1024.0 * 1024 * 1024);

        public DiskType GetDiskType(DriveInfo drive)
        {
            if (drive.Name.Equals(GetSystemDriveName(), StringComparison.OrdinalIgnoreCase))
            {
                return DiskType.System;
            }

            return drive.DriveType == DriveType.Removable ? DiskType.UsbFlash : DiskType.Regular;
        }

        public void PerformCleanup(DriveInfoModel dInfo, OptionInfo oInfo)
        {
            switch (oInfo.Option)
            {
                case CleanOption.TempUserFiles:
                    //ClearUserTempFiles();
                    break;

                case CleanOption.BrowserCache:
                    //ClearBrowserCache();
                    break;

                case CleanOption.RecycleBin:
                    //ClearRecycleBin(dInfo);
                    break;

                case CleanOption.SystemTemp:
                    //ClearSystemTemp();
                    break;

                case CleanOption.WindowsUpdateCache:
                    //ClearWindowsUpdateCache();
                    break;

                    //case CleanOption.SystemLogs:
                    //    Globals.Message.ShowMessage(null, $"Очистка системных логов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //case CleanOption.MemoryDumps:
                    //    Globals.Message.ShowMessage(null, $"Очистка дампов памяти на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //case CleanOption.MiniDumps:
                    //    Globals.Message.ShowMessage(null, $"Очистка минидампов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //case CleanOption.RestorePoints:
                    //    Globals.Message.ShowMessage(null, $"Удаление старых точек восстановления системы на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //case CleanOption.OldInstallPackages:
                    //    Globals.Message.ShowMessage(null, $"Удаление старых установочных пакетов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //case CleanOption.CBSLogs:
                    //    Globals.Message.ShowMessage(null, $"Очистка журналов ошибок CBS на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    //default:
                    //    Globals.Message.ShowMessage(null, $"Неизвестная опция очистки для диска {dInfo.Name}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    break;
            }

        }
        // === ОБНОВЛЕННЫЙ GetOptionSize ===
        public long GetOptionSize(DriveInfoModel drive, OptionInfo option)
        {
            long size = 0;

            switch (option.Option)
            {
                case CleanOption.TempUserFiles:
                    size += GetDirectorySize(SystemPaths.UserTemp);
                    break;

                case CleanOption.BrowserCache:
                    foreach (string path in GetBrowserCachePaths())
                    {
                        size += GetDirectorySize(path);
                    }
                    break;

                case CleanOption.RecycleBin:
                    size += GetDirectorySize(GetRecycleBinPath(drive));
                    break;

                case CleanOption.SystemTemp:
                    size += GetDirectorySize(SystemPaths.WindowsTemp);
                    size += GetDirectorySize(SystemPaths.ReportArchive);
                    break;

                case CleanOption.WindowsUpdateCache:
                    size += GetDirectorySize(SystemPaths.WindowsUpdateCache);
                    break;

                case CleanOption.SystemLogs:
                    size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "LogFiles"));
                    size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs"));
                    break;

                case CleanOption.MemoryDumps:
                    string memoryDumpPath = GetMemoryDumpsPath(drive);
                    if (File.Exists(memoryDumpPath))
                        size += new FileInfo(memoryDumpPath).Length;
                    size += GetDirectorySize(SystemPaths.MiniDump);
                    break;

                case CleanOption.MiniDumps:
                    size += GetDirectorySize(SystemPaths.MiniDump);
                    break;

                case CleanOption.CBSLogs:
                    size += GetDirectorySize(SystemPaths.CBSLogs);
                    break;

                case CleanOption.OldInstallPackages:
                    size += GetDirectorySize(SystemPaths.OldInstallPackages);
                    break;

                case CleanOption.RestorePoints:
                    size += GetDirectorySize(GetRestorePointsPath(drive));
                    break;
            }

            return size;
        }
        private long GetDirectorySize(string path)
        {
            if (!Directory.Exists(path))
            {
                return 0;
            }

            long size = 0;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                FileInfo[] files;
                try
                {
                    files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                }
                catch (UnauthorizedAccessException) { return 0; }
                catch (IOException) { return 0; }

                foreach (FileInfo file in files)
                {
                    try
                    {
                        size += file.Length;
                    }
                    catch { continue; }
                }

                DirectoryInfo[] subDirs;
                try
                {
                    subDirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly);
                }
                catch (UnauthorizedAccessException) { return size; }
                catch (IOException) { return size; }

                foreach (DirectoryInfo subDir in subDirs)
                {
                    try
                    {
                        size += GetDirectorySize(subDir.FullName);
                    }
                    catch { continue; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка доступа к {path}: {ex.Message}");
                return 0;
            }

            return size;
        }
        public void ClearDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

         
                FileInfo[] files;
                try
                {
                    files = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                }
                catch (UnauthorizedAccessException)
                {
                    files = Array.Empty<FileInfo>();
                }
                catch (IOException)
                {
                    files = Array.Empty<FileInfo>();
                }

                foreach (FileInfo file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch { continue; }
                }

                DirectoryInfo[] subDirs;
                try
                {
                    subDirs = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
                }
                catch (UnauthorizedAccessException)
                {
                    subDirs = Array.Empty<DirectoryInfo>();
                }
                catch (IOException)
                {
                    subDirs = Array.Empty<DirectoryInfo>();
                }

                foreach (DirectoryInfo subDir in subDirs)
                {
                    try
                    {
                        ClearDirectory(subDir.FullName);
                        subDir.Delete(false);
                    }
                    catch { continue; }
                }
            }
            catch (Exception ex)
            {
                //($"Ошибка очистки директории {directoryPath}: {ex.Message}");
            }
        }

        private void ClearUserTempFiles()
        {
            ClearDirectory(SystemPaths.UserTemp);
        }
        private void ClearBrowserCache()
        {
            foreach (string path in GetBrowserCachePaths())
            {
                ClearDirectory(path);
            }
        }
        private void ClearRecycleBin(DriveInfoModel dInfo)
        {
            ClearDirectory(GetRecycleBinPath(dInfo));
        }
        private void ClearSystemTemp()
        {
            ClearDirectory(SystemPaths.WindowsTemp);
            ClearDirectory(SystemPaths.ReportArchive);
        }
        private void ClearWindowsUpdateCache()
        {
            ClearDirectory(SystemPaths.WindowsUpdateCache);
        }

    }
    public enum DiskType
    {
        System,
        Regular,
        UsbFlash,
        Removable
    }
    public enum CleanOption
    {
        TempUserFiles,           // Временные файлы пользователя
        BrowserCache,            // Кэш браузеров
        RecycleBin,              // Корзина
        SystemTemp,              // Системные временные файлы
        WindowsUpdateCache,      // Кэш обновлений Windows
        SystemLogs,              // Системные логи
        MemoryDumps,             // Файлы дампов памяти
        MiniDumps,               // Минидампы
        RestorePoints,           // Старые точки восстановления системы
        OldInstallPackages,      // Старые установочные пакеты
        PrefetchFiles,           // Prefetch файлы
        CBSLogs                  // Журналы ошибок CBS
    }

    public enum TypeSize
    {
        Terabyte,
        Gigabyte,
        Megabyte,
        kilobyte,
        bytes,
        TerabyteAndGigabyte,
        GigabyteAndMegabyte,
        MegabyteAndKilobyte,
        KilobyteAndByte
    }
    public class DriveInfoModel
    {
        public string Name { get; set; }
        public DiskType DiskType { get; set; }
        public bool IsSystem { get; set; }
        public double TotalGB { get; set; }
        public double FreeGB { get; set; }
        public double UsedGB => TotalGB - FreeGB;
        public int PercentFree => (int)(FreeGB / TotalGB * 100);
        public int PercentUsed => 100 - PercentFree;

    }

    public class OptionInfo
    {
        public CleanOption Option { get; set; }
        public string Name { get; set; }         // Текст для UI
        public bool RequiresAdmin { get; set; }
    }


    public static class CleaningRules
    {
        public static readonly Dictionary<DiskType, List<OptionInfo>> Rules = new Dictionary<DiskType, List<OptionInfo>>
        {
            // Системный диск
            {
                DiskType.System,
                new List<OptionInfo>
                {
                    new OptionInfo { Option = CleanOption.TempUserFiles, Name = "Временные файлы пользователя", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.BrowserCache, Name = "Кэш браузеров", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.RecycleBin, Name = "Корзина", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.SystemTemp, Name = "Системные временные файлы", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.WindowsUpdateCache, Name = "Кэш обновлений Windows", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.SystemLogs, Name = "Системные логи", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.MemoryDumps, Name = "Файлы дампов памяти", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.MiniDumps, Name = "Минидампы", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.RestorePoints, Name = "Старые точки восстановления системы", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.OldInstallPackages, Name = "Старые установочные пакеты", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.CBSLogs, Name = "Журналы ошибок CBS", RequiresAdmin = true },
                }
            },

            // Обычные локальные диски
            {
                DiskType.Regular,
                new List<OptionInfo>
                {
                    new OptionInfo { Option = CleanOption.TempUserFiles, Name = "Файлы пользователя", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.RecycleBin, Name = "Корзина", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.SystemTemp, Name = "Временные файлы приложений", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.MemoryDumps, Name = "Локальные дампы приложений", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.BrowserCache, Name = "Кэш браузеров", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.OldInstallPackages, Name = "Файлы автосохранения Office", RequiresAdmin = false }
                }
            },

            // USB флешки
            {
                DiskType.UsbFlash,
                new List<OptionInfo>
                {
                    new OptionInfo { Option = CleanOption.TempUserFiles, Name = "Пользовательские файлы", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.SystemTemp, Name = "Временные файлы", RequiresAdmin = false }
                }
            },

            // Другие съемные носители
            {
                DiskType.Removable,
                new List<OptionInfo>
                {
                    new OptionInfo { Option = CleanOption.TempUserFiles, Name = "Пользовательские файлы", RequiresAdmin = false },
                    new OptionInfo { Option = CleanOption.SystemTemp, Name = "Временные файлы", RequiresAdmin = false }
                }
            }
        };
    }

}
