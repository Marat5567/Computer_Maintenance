namespace Computer_Maintenance.Models
{

    public class SystemCleaningControlModel
    {
        // Основные системные пути
        public readonly string TempWindowsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
        public readonly string UserTempPath = Path.GetTempPath();
        public readonly string RecycleBinPath = @"C:\$Recycle.Bin"; // Для всех пользователей
        public readonly string PrefetchPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch");
        public readonly string WindowsUpdateCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download");
        public readonly string CBSLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs", "CBS");
        public readonly string MiniDumpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Minidump");
        public readonly string MemoryDumpsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "MEMORY.DMP");
        public readonly string RestorePointsPath = @"C:\System Volume Information";
        public readonly string OldInstallPackagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Installer");
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
                    CleanTempUserFiles(dInfo);
                    Globals.Message.ShowMessage(null, $"Очистка временных файлов пользователя на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.BrowserCache:
                    Globals.Message.ShowMessage(null, $"Очистка кэша браузеров на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.RecycleBin:
                    Globals.Message.ShowMessage(null, $"Очистка корзины на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.SystemTemp:
                    Globals.Message.ShowMessage(null, $"Очистка системных временных файлов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.WindowsUpdateCache:
                    Globals.Message.ShowMessage(null, $"Очистка кеша обновлений Windows на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.RegistryCleanup:
                    Globals.Message.ShowMessage(null, $"Очистка реестра на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.SystemLogs:
                    Globals.Message.ShowMessage(null, $"Очистка системных логов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.MemoryDumps:
                    Globals.Message.ShowMessage(null, $"Очистка дампов памяти на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.MiniDumps:
                    Globals.Message.ShowMessage(null, $"Очистка минидампов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.RestorePoints:
                    Globals.Message.ShowMessage(null, $"Удаление старых точек восстановления системы на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.OldInstallPackages:
                    Globals.Message.ShowMessage(null, $"Удаление старых установочных пакетов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.PrefetchFiles:
                    Globals.Message.ShowMessage(null, $"Очистка Prefetch файлов на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case CleanOption.CBSLogs:
                    Globals.Message.ShowMessage(null, $"Очистка журналов ошибок CBS на диске {dInfo.Name}", "Очистка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    Globals.Message.ShowMessage(null, $"Неизвестная опция очистки для диска {dInfo.Name}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

        }
        public long GetOptionSize(DriveInfoModel dInfo, OptionInfo oInfo)
        {
            long size = 0;
            try
            {
                switch (oInfo.Option)
                {
                    case CleanOption.TempUserFiles:
                        size += GetDirectorySize(Path.GetTempPath()); // TEMP текущего процесса
        
                        break;

                    case CleanOption.BrowserCache:
                        size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data", "Default", "Cache"));
                        size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Edge", "User Data", "Default", "Cache"));
                        break;

                    case CleanOption.RecycleBin:
                        // Можно использовать COM API для Recycle Bin, пока пример через путь
                        size += GetDirectorySize(RecycleBinPath);
                        break;

                    case CleanOption.SystemTemp:
                        size += GetDirectorySize(TempWindowsPath);
                        break;

                    case CleanOption.WindowsUpdateCache:
                        size += GetDirectorySize(WindowsUpdateCachePath);
                        break;

                    case CleanOption.SystemLogs:
                        size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "LogFiles"));
                        size += GetDirectorySize(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs"));
                        break;

                    case CleanOption.MemoryDumps:
                        if (File.Exists(MemoryDumpsPath)) size += new FileInfo(MemoryDumpsPath).Length;
                        size += GetDirectorySize(MiniDumpPath);
                        break;

                    case CleanOption.MiniDumps:
                        size += GetDirectorySize(MiniDumpPath);
                        break;

                    case CleanOption.PrefetchFiles:
                        size += GetDirectorySize(PrefetchPath);
                        break;

                    case CleanOption.CBSLogs:
                        size += GetDirectorySize(CBSLogsPath);
                        break;

                    case CleanOption.OldInstallPackages:
                        size += GetDirectorySize(OldInstallPackagesPath);
                        break;

                        // Остальные опции: RestorePoints, RegistryCleanup и т.д. сложно посчитать размер напрямую
                }
            }
            catch { /* Игнорируем ошибки доступа */ }

            return size; // Возвращаем в байтах
        }

        private long GetDirectorySize(string path)
        {
            if (!Directory.Exists(path)) return 0;

            long size = 0;
            var dir = new DirectoryInfo(path);

            try
            {
                // Файлы в текущей директории
                foreach (var file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        size += file.Length;
                    }
                    catch (UnauthorizedAccessException) { continue; }
                    catch (IOException) { continue; }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }

            // Рекурсивно обрабатываем поддиректории
            foreach (var subDir in dir.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    size += GetDirectorySize(subDir.FullName);
                }
                catch (UnauthorizedAccessException) { continue; }
                catch (IOException) { continue; }
            }

            return size;
        }



        private void CleanTempUserFiles(DriveInfoModel dInfo)
        {

        }
        private string GetUserTempPath(string userName, string driveLetter)
        {
            return Path.Combine(driveLetter, "Users", userName, "AppData", "Local", "Temp");
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
        RegistryCleanup,         // Очистка реестра
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
        public int PercentFree => (int)((FreeGB / TotalGB) * 100);
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
                    new OptionInfo { Option = CleanOption.RegistryCleanup, Name = "Очистка реестра", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.SystemLogs, Name = "Системные логи", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.MemoryDumps, Name = "Файлы дампов памяти", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.MiniDumps, Name = "Минидампы", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.RestorePoints, Name = "Старые точки восстановления системы", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.OldInstallPackages, Name = "Старые установочные пакеты", RequiresAdmin = true },
                    new OptionInfo { Option = CleanOption.PrefetchFiles, Name = "Prefetch файлы", RequiresAdmin = true },
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
                    new OptionInfo { Option = CleanOption.PrefetchFiles, Name = "Временные файлы Office", RequiresAdmin = false },
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
