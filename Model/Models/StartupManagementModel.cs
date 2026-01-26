using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System.Diagnostics;

namespace Computer_Maintenance.Model.Models
{
    public class StartupManagementModel
    {
        private readonly string[] EXECUTABLE_EXTENSIONS = new string[5] { ".lnk", ".exe", ".bat", ".cmd", ".msc" };

        private const string REGISTRY_CURRENT_USER = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string REGISTRY_ALL_USERS = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string REGISTRY_WOW6432Node = "Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";

        private readonly string FOLDER_CURRENT_USER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");
        private readonly string FOLDER_All_USERS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");


        // Пути где храниться состояния автозагрузок вкл или выкл
        private const string REGISTRY_CURRENT_USER_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run";
        private const string REGISTRY_ALL_USERS_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run";
        private const string REGISTRY_WOW6432Node_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run32";

        private const string STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder";
        private const string STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder";

        private const byte ENABLED_FLAG = 0x02;
        private const byte ENABLED_FLAG_SYSTEM = 0x06;
        private const byte DISABLED_FLAG = 0x03 ;

        private List<StartupItemRegistry> RegistryStartupItems_CurrentUser { get; set; } = new List<StartupItemRegistry>();
        private List<StartupItemRegistry> RegistryStartupItems_AllUsers { get; set; } = new List<StartupItemRegistry>();
        private List<StartupItemFolder> FolderStartupItems_CurrentUser { get; set; } = new List<StartupItemFolder>();
        private List<StartupItemFolder> FolderStartupItems_AllUsers { get; set; } = new List<StartupItemFolder>();
        private List<TaskSchedulerItem> TaskSchedulerItems { get; set; } = new List<TaskSchedulerItem>();

        public void LoadStartupItems(StartupType type)
        {
            switch (type)
            {
                case StartupType.All:
                    RegistryStartupItems_CurrentUser.Clear();
                    LoadRegistryStartupItems(registryRoot: Registry.CurrentUser, startupType: StartupType.RegistryCurrentUser, collectionToAdd: RegistryStartupItems_CurrentUser, path: REGISTRY_CURRENT_USER);

                    RegistryStartupItems_AllUsers.Clear();
                    LoadRegistryStartupItems(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_ALL_USERS);
                    LoadWOW6432Node(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_WOW6432Node);

                    FolderStartupItems_CurrentUser.Clear();
                    LoadFolderStartupItems(startupType: StartupType.StartupFolderCurrentUser, collectionToAdd: FolderStartupItems_CurrentUser, FOLDER_CURRENT_USER);

                    FolderStartupItems_AllUsers.Clear();
                    LoadFolderStartupItems(startupType: StartupType.StartupFolderAllUsers, collectionToAdd: FolderStartupItems_AllUsers, FOLDER_All_USERS);

                    TaskSchedulerItems.Clear();
                    LoadTaskSchedulerItems(startupType: StartupType.TaskScheduler, TaskSchedulerItems);
                    break;
                case StartupType.RegistryCurrentUser:
                    RegistryStartupItems_CurrentUser.Clear();
                    LoadRegistryStartupItems(registryRoot: Registry.CurrentUser, startupType: StartupType.RegistryCurrentUser, collectionToAdd: RegistryStartupItems_CurrentUser, path: REGISTRY_CURRENT_USER);
                    break;
                case StartupType.RegistryLocalMachine:
                    RegistryStartupItems_AllUsers.Clear();
                    LoadRegistryStartupItems(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_ALL_USERS);
                    LoadWOW6432Node(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_WOW6432Node);
                    break;
                case StartupType.StartupFolderCurrentUser:
                    FolderStartupItems_CurrentUser.Clear();
                    LoadFolderStartupItems(startupType: StartupType.StartupFolderCurrentUser, collectionToAdd: FolderStartupItems_CurrentUser, FOLDER_CURRENT_USER);
                    break;
                case StartupType.StartupFolderAllUsers:
                    FolderStartupItems_AllUsers.Clear();
                    LoadFolderStartupItems(startupType: StartupType.StartupFolderAllUsers, collectionToAdd: FolderStartupItems_AllUsers, FOLDER_All_USERS);
                    break;
                case StartupType.TaskScheduler:
                    TaskSchedulerItems.Clear();
                    LoadTaskSchedulerItems(startupType: StartupType.TaskScheduler, TaskSchedulerItems);
                    break;
            }
            if ((type & StartupType.RegistryCurrentUser) != 0)
            {
                RegistryStartupItems_CurrentUser.Clear();
                LoadRegistryStartupItems(registryRoot: Registry.CurrentUser, startupType: StartupType.RegistryCurrentUser, collectionToAdd: RegistryStartupItems_CurrentUser, path: REGISTRY_CURRENT_USER);
            }
            if ((type & StartupType.RegistryLocalMachine) != 0)
            {
                RegistryStartupItems_AllUsers.Clear();

                LoadRegistryStartupItems(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_ALL_USERS);
                LoadWOW6432Node(registryRoot: Registry.LocalMachine, startupType: StartupType.RegistryLocalMachine, collectionToAdd: RegistryStartupItems_AllUsers, path: REGISTRY_WOW6432Node);
            }
            if ((type & StartupType.StartupFolderCurrentUser) != 0)
            {
                FolderStartupItems_CurrentUser.Clear();
                LoadFolderStartupItems(startupType: StartupType.StartupFolderCurrentUser, collectionToAdd: FolderStartupItems_CurrentUser, FOLDER_CURRENT_USER);
            }
            if ((type & StartupType.StartupFolderAllUsers) != 0)
            {
                FolderStartupItems_AllUsers.Clear();
                LoadFolderStartupItems(startupType: StartupType.StartupFolderAllUsers, collectionToAdd: FolderStartupItems_AllUsers, FOLDER_All_USERS);
            }
        }

        private void LoadTaskSchedulerItems(StartupType startupType, List<TaskSchedulerItem> collectionToAdd)
        {
            using (TaskService ts = new TaskService())
            {
                TaskCollection tasks = ts.RootFolder.GetTasks();
                foreach (Microsoft.Win32.TaskScheduler.Task task in tasks)
                {
                    string pathExtracted = ExtractPath(GetExecutablePathFromTask(task));
                    TaskSchedulerItem item = new TaskSchedulerItem
                    {
                        File = task.Name,
                        Name = GetNameExe(pathExtracted),
                        Path = pathExtracted,
                        State = task.State,
                        Type = startupType,
                    };
                    TaskSchedulerItems.Add(item);
                }
            }
            string GetExecutablePathFromTask(Microsoft.Win32.TaskScheduler.Task task)
            {
                try
                {
                    foreach (Microsoft.Win32.TaskScheduler.Action? action in task.Definition.Actions)
                    {
                        if (action is ExecAction execAction)
                        {
                            return execAction.Path;
                        }
                    }
                }
                catch
                {
                }
                return String.Empty;
            }
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

                        StartupState state;

                        StartupItemFolder item = new StartupItemFolder
                        {
                            NameExtracted = name,
                            PathExtracted = fullPath,
                            Type = startupType,
                            State = GetStartupState(name, startupType)
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

                    if (valueNames.Length > 0)
                    {
                        foreach (string valueName in valueNames)
                        {
                            object value = key.GetValue(valueName);
                            string extractedPath = ExtractPath(value?.ToString());

                            StartupItemRegistry item = new StartupItemRegistry
                            {
                                NameExtracted = GetNameExe(extractedPath),
                                RegistryName = valueName,
                                PathExtracted = extractedPath,
                                Type = startupType,
                                State = GetStartupState(valueName, startupType)
                            };

                            collectionToAdd.Add(item);
                        }
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

                    if (valueNames.Length > 0)
                    {
                        foreach (string valueName in valueNames)
                        {
                            object value = key.GetValue(valueName);
                            string extractedPath = ExtractPath(value?.ToString());

                            StartupItemRegistry item = new StartupItemRegistry
                            {
                                NameExtracted = GetNameExe(extractedPath),
                                RegistryName = valueName,
                                PathExtracted = extractedPath,
                                Is32Bit = true,
                                Type = startupType,
                                State = GetStartupState(valueName, startupType, is32Bit: true)
                            };

                            collectionToAdd.Add(item);
                        }
                    }
                }
            }
        }

        public void OpenPathToExplorer(bool isFile, string path, StartupType startupType)
        {
            if (isFile)
            {
                if (File.Exists(path))
                {
                    Process.Start("explorer.exe", $"/select,\"{path}\"");
                }
            }
            else
            {
                string directoryPath = String.Empty;
                switch (startupType)
                {
                    case StartupType.StartupFolderCurrentUser:
                        directoryPath = FOLDER_CURRENT_USER;
                        break;
                    case StartupType.StartupFolderAllUsers:
                        directoryPath = FOLDER_All_USERS;
                        break;
                    case StartupType.None:
                        return;
                    default:
                        directoryPath = Path.GetDirectoryName(path);
                        break;
                }

                
                if (Directory.Exists(directoryPath))
                {
                    Process.Start("explorer.exe", $"\"{directoryPath}\"");
                }
            }
        }
        public List<StartupItemRegistry> GetRegistryStartupItems(StartupType type)
        {
            if ((type & StartupType.RegistryCurrentUser) != 0 && (type & StartupType.RegistryLocalMachine) != 0)
            {
                return RegistryStartupItems_CurrentUser
                    .Concat(RegistryStartupItems_AllUsers)
                    .ToList();
            }
            if ((type & StartupType.RegistryCurrentUser) != 0)
            {
                return RegistryStartupItems_CurrentUser;
            }
            else if ((type & StartupType.RegistryLocalMachine) != 0)
            {
                return RegistryStartupItems_AllUsers;
            }

            return new List<StartupItemRegistry>();
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

        public List<TaskSchedulerItem> GetTaskSchedulerItems()
        {
            return TaskSchedulerItems;
        }

        private StartupState GetStartupState(string registryName, StartupType startupType, bool is32Bit = false)
        {
            if (string.IsNullOrEmpty(registryName)) { return StartupState.None; }

            RegistryKey? key = null;

            try
            {
                switch (startupType)
                {
                    case StartupType.RegistryCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER_APPROWED, true);
                        break;
                    case StartupType.RegistryLocalMachine:

                        if (is32Bit)
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_WOW6432Node_APPROWED, true);
                        }
                        else
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_ALL_USERS_APPROWED, true);
                        }
                        break;
                    case StartupType.StartupFolderCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED, true);
                        break;
                    case StartupType.StartupFolderAllUsers:
                        key = Registry.LocalMachine.OpenSubKey(STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED, true);
     
                        break;
                }

                if (key == null) { return StartupState.None; }

                string[] valueNames = key.GetValueNames();
                if (valueNames.Length > 0)
                {
                    foreach (string valueName in valueNames)
                    {
                        if (valueName == registryName)
                        {
                            if (key.GetValueKind(valueName) != RegistryValueKind.Binary) { continue; }

                            object valueData = key.GetValue(valueName);

                            if (valueData != null)
                            {
                                if (valueData is byte[] data)
                                {
                                    if ((byte)data[0] == ENABLED_FLAG || (byte)data[0] == ENABLED_FLAG_SYSTEM)
                                    {
                                        return StartupState.Enabled;
                                    }
                                    else
                                    {
                                        return StartupState.Disabled;
                                    }

                                }
                            }
                        }
                    }
                }

                return StartupState.Enabled;
            }
            catch
            {
                return StartupState.None;
            }
            finally
            {
                key?.Dispose();
            }
        }

        public bool ChangeStateStartup(string name, StartupType startupType, bool is32Bit = false)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            RegistryKey? key = null;

            try
            {
                switch (startupType)
                {
                    case StartupType.RegistryCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER_APPROWED, true);
                        break;
                    case StartupType.RegistryLocalMachine:

                        if (is32Bit)
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_WOW6432Node_APPROWED, true);
                        }
                        else
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_ALL_USERS_APPROWED, true);
                        }
                        break;
                    case StartupType.StartupFolderCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED, true);
                        break;
                    case StartupType.StartupFolderAllUsers:
                        key = Registry.LocalMachine.OpenSubKey(STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED, true);
                        break;

                }

                if (key == null) { return false; }

                string[] valueNames = key.GetValueNames();

                if (valueNames.Length > 0)
                {
                    foreach (string valueName in valueNames)
                    {
                        if (valueName == name)
                        {
                            if (key.GetValueKind(valueName) != RegistryValueKind.Binary) { continue; }

                            object valueData = key.GetValue(valueName);

                            if (valueData != null)
                            {
                                if (valueData is byte[] data)
                                {
                                    if ((byte)data[0] == ENABLED_FLAG)
                                    {
                                        data[0] = DISABLED_FLAG;
                                    }
                                    else
                                    {
                                        data[0] = ENABLED_FLAG;
                                    }

                                    key.SetValue(valueName, data, RegistryValueKind.Binary);
                                    return true;
                                }
                            }            
                        }
                    }
                }

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
                if (rawStr[0] == '"' && rawStr[1] != '"')
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
                else if (rawStr[0] == '"' && rawStr[1] == '"')
                {
                    int index = rawStr.IndexOf('"', 2);

                    if (index == -1)
                    {
                        return rawStr.Substring(2);
                    }
                    else
                    {
                        return rawStr.Substring(2, index - 2);
                    }
                }
                else if (rawStr[0] != '"')
                {
                    return rawStr;
                }
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

            return Path.GetFileName(extractedPath);

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
