using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Microsoft.Win32;

namespace Computer_Maintenance.Model.Models
{
    public class StartupManagementModel
    {
        private readonly string[] EXECUTABLE_EXTENSIONS = new string[5] { ".lnk", ".exe", ".bat", ".cmd", ".msc" };

        private const string REGISTRY_CURRENT_USER = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string REGISTRY_ALL_USERS = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string WOW6432Node = "Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static readonly string FOLDER_CURRENT_USER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");
        private static readonly string FOLDER_All_USERS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");

        public List<StartupItemRegistry> RegistryStartupItems_CurrentUser { get; set; } = new List<StartupItemRegistry>();
        public List<StartupItemRegistry> RegistryStartupItems_AllUsers { get; set; } = new List<StartupItemRegistry>();
        public List<StartupItemFolder> FolderStartupItems_CurrentUser { get; set; } = new List<StartupItemFolder>();
        public List<StartupItemFolder> FolderStartupItems_AllUsers { get; set; } = new List<StartupItemFolder>();

        public void LoadAllStartupItems()
        {
            LoadRegistryStartupItems(registryRoot: Registry.CurrentUser, startupType: StartupType.RegistryCurrentUser, collectionToAdd: RegistryStartupItems_CurrentUser, path: REGISTRY_CURRENT_USER);
            LoadRegistryStartupItems(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_ALL_USERS);
            LoadWOW6432Node(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: WOW6432Node);
            LoadFolderStartupItems(startupType: StartupType.StartupFolderCurrentUser, collectionToAdd: FolderStartupItems_CurrentUser, FOLDER_CURRENT_USER);
            LoadFolderStartupItems(startupType: StartupType.StartupFolderAllUsers, collectionToAdd: FolderStartupItems_AllUsers, FOLDER_All_USERS);
        }

        private unsafe void LoadFolderStartupItems(StartupType startupType, List<StartupItemFolder> collectionToAdd, string path)
        {
            if (!Directory.Exists(path)) return;

            foreach (string extensions in EXECUTABLE_EXTENSIONS)
            {
                string searchPath = path.EndsWith("\\") ? path + "*" : path + $"\\*{extensions}";

                FileApi.WIN32_FIND_DATA findData;
                IntPtr hFind = FileApi.FindFirstFileExW(
                    searchPath,
                    FileApi.FINDEX_INFO_LEVELS.FindExInfoBasic,
                    &findData,
                    FileApi.FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                    IntPtr.Zero,
                    FileApi.FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

                if (hFind.ToInt64() == FileApi.INVALID_HANDLE_VALUE) { continue; }

                try
                {
                    do
                    {
                        string name = findData.GetFileName();
                        if (name == "." || name == "..") { continue; }

                        string fullPath = Path.Combine(path, name);

                        StartupItemFolder item = new StartupItemFolder
                        {
                            NameExtracted = name,
                            PathExtracted = fullPath,
                            Type = startupType
                        };
                        collectionToAdd.Add(item);

                    }
                    while (FileApi.FindNextFileW(hFind, &findData));
                }
                finally
                {
                    FileApi.FindClose(hFind);
                }
            }

        }
        private void LoadRegistryStartupItems(RegistryKey registryRoot, StartupType startupType, List<StartupItemRegistry> collectionToAdd, string path)
        {
            using (RegistryKey key = registryRoot.OpenSubKey(path))
            {
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();

                    foreach (string valueName in valueNames)
                    {
                        object value = key.GetValue(valueName);
                        string extractedPath = ExtractPath(value?.ToString());

                        StartupItemRegistry item = new StartupItemRegistry
                        {
                            NameExtracted = GetNameExe(extractedPath),
                            RegistryName = valueName,
                            PathExtracted = extractedPath,
                            Type = startupType
                        };

                        collectionToAdd.Add(item);
                    }
                }
            }
        }
        private void LoadWOW6432Node(RegistryKey registryRoot, StartupType startupType, List<StartupItemRegistry> collectionToAdd, string path)
        {
            using (RegistryKey key = registryRoot.OpenSubKey(path))
            {
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();

                    foreach (string valueName in valueNames)
                    {
                        object value = key.GetValue(valueName);
                        string extractedPath = ExtractPath(value?.ToString());

                        StartupItemRegistry item = new StartupItemRegistry
                        {
                            NameExtracted = GetNameExe(extractedPath),
                            RegistryName = valueName,
                            PathExtracted = extractedPath,
                            Bit = "32-разрядная",
                            Type = startupType
                        };

                        collectionToAdd.Add(item);
                    }
                }
            }
        }
        public bool DeleteRegistryRecord(StartupType startupType, string registryKeyName)
        {
            if (string.IsNullOrEmpty(registryKeyName)) { return false; }

            RegistryKey? key = null;

            try
            {
                switch (startupType)
                {
                    case StartupType.RegistryCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER, true);
                        break;
                    case StartupType.RegistryLocalMachine:
                        key = Registry.LocalMachine.OpenSubKey(REGISTRY_ALL_USERS, true);
                        break;
                    default:
                        return false;
                }

                if (key == null) { return false; }

                //key.DeleteValue(registryKeyName);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                key?.Dispose();
            }

        }

        public bool DeleteFolderStartupRecord(string fullPath)
        {
            if (!File.Exists(fullPath)) { return false; }

            string extension = Path.GetExtension(fullPath).ToLower();

            if (EXECUTABLE_EXTENSIONS.Contains(extension))
            {

                if (extension == ".lnk")
                {
                    try
                    {
                        FileApi.DeleteFileW(fullPath);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Извлекаем чистый путь из реестра без аргументов
        /// </summary>
        /// <returns></returns>
        /// 

        private string ExtractPath(string rawStr)
        {
            if (string.IsNullOrEmpty(rawStr)) { return String.Empty; }

            rawStr = rawStr.Trim();

            if (rawStr.Length > 0)
            {
                if (rawStr[0] == '"')
                {
                    int index_TwoQuotes = rawStr.IndexOf('"', 1);
                    if (index_TwoQuotes == -1) //Если не найдено второй кавычки, ищем пустой символ
                    {
                        return rawStr;
                    }
                    else
                    {
                        return rawStr.Substring(1, index_TwoQuotes - 1);
                    }
                }
                else
                {
                    int index_EmptyChar = rawStr.IndexOf(' ');
                    if (index_EmptyChar == -1)
                    {
                        return rawStr;
                    }
                    else
                    {
                        return rawStr.Substring(0, index_EmptyChar);
                    }
                }
            }

            return rawStr;
        }

        private string GetNameExe(string extractedPath)
        {
            if (string.IsNullOrEmpty(extractedPath)) { return String.Empty; }

            extractedPath.Trim();
            return Path.GetFileName(extractedPath);
        }

        public List<StartupItemRegistry> GetRegistryStartupItems()
        {
            List<StartupItemRegistry> allItems = new List<StartupItemRegistry>();
            allItems.AddRange(RegistryStartupItems_CurrentUser);
            allItems.AddRange(RegistryStartupItems_AllUsers);

            return allItems;
        }
        public List<StartupItemFolder> GetFolderStartupItems(StartupType type)
        {
            if ((type & StartupType.StartupFolderCurrentUser) != 0 && (type & StartupType.StartupFolderAllUsers) != 0)
            {
                return FolderStartupItems_CurrentUser
                    .Concat(FolderStartupItems_AllUsers)
                    .ToList();
            }

            if ((type & StartupType.StartupFolderCurrentUser) != 0)
            {
                return FolderStartupItems_CurrentUser;
            }
            else if ((type & StartupType.StartupFolderAllUsers) != 0)
            {
                return FolderStartupItems_AllUsers;
            }

            return new List<StartupItemFolder>();
        }

        public void ShowInfo(string message)
        {
            MessageService.ShowMessage(null, message, "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowError(string message)
        {
            MessageService.ShowMessage(null, message, "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
