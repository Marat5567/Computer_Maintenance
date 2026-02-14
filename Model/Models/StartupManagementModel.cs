using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Core.WinApi;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Forms;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Computer_Maintenance.Model.Models
{
    public class StartupManagementModel
    {
        private const string REGISTRY_CURRENT_USER = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string REGISTRY_ALL_USERS = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string REGISTRY_WOW6432Node = "Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";

        // Пути где храниться состояния автозагрузок вкл или выкл
        private const string REGISTRY_CURRENT_USER_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run";
        private const string REGISTRY_ALL_USERS_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run";
        private const string REGISTRY_WOW6432Node_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run32";

        private const string STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder";
        private const string STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder";

        private const byte ENABLED_FLAG = 0x02;
        private const byte ENABLED_FLAG_SYSTEM = 0x06;
        private const byte DISABLED_FLAG = 0x03;

        private readonly string[] EXECUTABLE_EXTENSIONS = { ".exe", ".com", ".scr", ".msc", ".bat", ".cmd", ".ps1", ".vbs", ".js", ".wsf", ".ws", ".hta", ".lnk", ".msi", ".msp", ".msix", ".appx", ".appxbundle", ".msixbundle" };
        private readonly string FOLDER_CURRENT_USER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");
        private readonly string FOLDER_All_USERS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "Startup");

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
                case StartupType.None:
                    return;
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
                            string extractedPath = ExtractRegistryPath(value?.ToString());

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
                            string extractedPath = ExtractRegistryPath(value?.ToString());

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
        private void LoadTaskSchedulerItems(StartupType startupType, List<TaskSchedulerItem> collectionToAdd)
        {
            using (TaskService ts = new TaskService())
            {
                TaskCollection tasks = ts.RootFolder.GetTasks();
                foreach (Microsoft.Win32.TaskScheduler.Task task in tasks)
                {
                    string executablePathFromTask = GetExecutablePathFromTask(task);
                    string pathExtracted = ExtractTaskSchedulerPath(executablePathFromTask);

                    
                    string author = task.Definition?.RegistrationInfo?.Author ?? "Неизвестно";
                    string description = task.Definition?.RegistrationInfo?.Description ?? String.Empty;
                    DateTime created = task?.Definition?.RegistrationInfo?.Date ?? DateTime.MinValue;
                    TaskSchedulerItem item = new TaskSchedulerItem
                    {
                        File = task.Name,
                        Name = GetNameExe(pathExtracted),
                        Path = pathExtracted,
                        State = task.State,
                        Type = startupType,
                        Author = author,
                        Description = description,
                        Created = created,
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
                catch {}
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


        public void ViewDetailTaskSchedulerItem(string author, string description, DateTime created)
        {
            ViewDetailTaskSchedulerItem_Form form = new ViewDetailTaskSchedulerItem_Form(author, description, created);
            form.StartPosition = FormStartPosition.WindowsDefaultLocation;
            form.Show();
        }

        public void OpenPathToExplorer(bool isFile, string path, StartupType startupType)
        {
            if (isFile)
            {
                if (File.Exists(path))
                {
                    Process.Start("explorer.exe", $"/select,\"{path}\"");
                }
                else
                {
                    ShowInfo("Файл не существует, можете удалить запись из реестра");
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
                        break;
                    default:
                        directoryPath = Path.GetDirectoryName(path);
                        break;
                }

                if (string.IsNullOrEmpty(directoryPath)) { return; }
                
                Process.Start("explorer.exe", $"\"{directoryPath}\"");
            }
        }

        public void CopyToClipboard(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return; }

            Clipboard.SetText(path);
        }

        public List<object> GetStartupItems(StartupType type)
        {
            switch (type)
            {
                case StartupType.RegistryCurrentUser:
                    return RegistryStartupItems_CurrentUser.Cast<object>().ToList();
                case StartupType.RegistryLocalMachine:
                    return RegistryStartupItems_AllUsers.Cast<object>().ToList();
                case StartupType.StartupFolderCurrentUser:
                    return FolderStartupItems_CurrentUser.Cast<object>().ToList();
                case StartupType .StartupFolderAllUsers:
                    return FolderStartupItems_AllUsers.Cast<object>().ToList();
                case StartupType.TaskScheduler:
                    return TaskSchedulerItems.Cast<object>().ToList();
                default:
                    return new List<object>();
            }
        }
        public bool RunTask(string name)
        {
            using (TaskService ts = new TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task? task = ts.GetTask(name);
                if (task == null) { return false; }

                try
                {
                    task.Run();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool CompleteTask(string name)
        {
            using (TaskService ts = new TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task? task = ts.GetTask(name);
                if (task == null) { return false; }

                task.Stop();
                return true;
            }
        }
        public bool ChangeStateStartup(string name, string path, StartupType startupType, bool is32Bit = false)
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
                    case StartupType.TaskScheduler:
                        using (TaskService ts = new TaskService())
                        {
                            Microsoft.Win32.TaskScheduler.Task? task = ts.GetTask(name);

                            if (task == null || task.State == TaskState.Running) { return false; }

                            task.Enabled = !task.Enabled;
                            return true;

                        }
                }

                if (key == null) { return false; }

                object? existingValue = key?.GetValue(name);

                if (existingValue != null)
                {
                    if (key.GetValueKind(name) == RegistryValueKind.Binary && existingValue is byte[] data)
                    {
                        byte[] newData = new byte[data.Length];
                        Array.Copy(data, newData, data.Length);

                        newData[0] = (newData[0] == ENABLED_FLAG) ? DISABLED_FLAG : ENABLED_FLAG;

                        key.SetValue(name, newData, RegistryValueKind.Binary);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        byte[] newEntry = new byte[] { DISABLED_FLAG, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        key?.SetValue(name, newEntry, RegistryValueKind.Binary);
                    }
                    return true;
                }
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



        public void DeleteUnusedRecords_Click(StartupType startupType, bool is32Bit = false)
        {
            RegistryKey? key = null;

            try
            {
                switch (startupType)
                {
                    case StartupType.RegistryCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER_APPROWED, true);
                        if (key == null) break;

                        string[] valueNames = key.GetValueNames();

                        HashSet<string> validNamesSet = new HashSet<string>(
                            RegistryStartupItems_CurrentUser.Select(r => r.RegistryName),
                            StringComparer.OrdinalIgnoreCase
                        );

                        foreach (string valueName in valueNames)
                        {
                            if (!validNamesSet.Contains(valueName))
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
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

                        if (key == null) break;

                        string[] lmValueNames = key.GetValueNames();

                        HashSet<string> lmValidNamesSet = new HashSet<string>(
                            RegistryStartupItems_AllUsers.Select(r => r.RegistryName),
                            StringComparer.OrdinalIgnoreCase
                        );

                        foreach (string valueName in lmValueNames)
                        {
                            if (!lmValidNamesSet.Contains(valueName))
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        break;

                    case StartupType.StartupFolderCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED, true);
                        if (key == null) break;

                        string[] folderCuValueNames = key.GetValueNames();

                        HashSet<string> folderCuValidNamesSet = new HashSet<string>(
                            FolderStartupItems_CurrentUser
                                .Where(f => f.State != StartupState.None)
                                .Select(f => f.NameExtracted),
                            StringComparer.OrdinalIgnoreCase
                        );

                        foreach (string valueName in folderCuValueNames)
                        {
                            if (!folderCuValidNamesSet.Contains(valueName))
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        break;

                    case StartupType.StartupFolderAllUsers:
                        key = Registry.LocalMachine.OpenSubKey(STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED, true);
                        if (key == null) break;

                        string[] folderAuValueNames = key.GetValueNames();

                        HashSet<string> folderAuValidNamesSet = new HashSet<string>(
                            FolderStartupItems_AllUsers
                                .Where(f => f.State != StartupState.None)
                                .Select(f => f.NameExtracted),
                            StringComparer.OrdinalIgnoreCase
                        );

                        foreach (string valueName in folderAuValueNames)
                        {
                            if (!folderAuValidNamesSet.Contains(valueName))
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                key?.Dispose();
            }
        }


        public void DeleteRegistryRecord(string registryName, string nameFile, StartupType startupType, bool is32Bit = false)
        {
            if (string.IsNullOrWhiteSpace(registryName)) { throw new Exception("Имя записи реестра не может быть пустым"); }
            RegistryKey? key = null;
            RegistryKey? keyApprowed = null;

            try
            {
                switch (startupType)
                {
                    case StartupType.RegistryCurrentUser:
                        key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER, true);
                        keyApprowed = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER_APPROWED, true);
                        break;
                    case StartupType.RegistryLocalMachine:
                        if (is32Bit)
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_WOW6432Node, true);
                            keyApprowed = Registry.LocalMachine.OpenSubKey(REGISTRY_WOW6432Node_APPROWED, true);
                        }
                        else
                        {
                            key = Registry.LocalMachine.OpenSubKey(REGISTRY_ALL_USERS, true);
                            keyApprowed = Registry.LocalMachine.OpenSubKey(REGISTRY_ALL_USERS_APPROWED, true);
                        }
                        break;
                }

                if (keyApprowed != null)
                {
                    keyApprowed.DeleteValue(registryName, false);
                }
                key?.DeleteValue(registryName, true);
            }
            catch
            {
                string msg = string.IsNullOrWhiteSpace(nameFile) ? registryName : nameFile;
                throw new Exception($"Ну удалось удалить значение: {msg} из рестра");
            }
            finally
            {
                key?.Dispose();
                keyApprowed?.Dispose();
            }
        }

        //public void CreateRegistryRecord(StartupType startupType)
        //{

        //}

        //public void DeleteFolderRecord(string nameFile, string folderPath, StartupType startupType)
        //{
        //    if (string.IsNullOrWhiteSpace(folderPath)) { throw new Exception("Путь не может быть пустым"); }
        //    RegistryKey? keyApprowed = null;

        //    try
        //    {
        //        switch (startupType)
        //        {
        //            case StartupType.StartupFolderCurrentUser:
        //                keyApprowed = Registry.CurrentUser.OpenSubKey(STARTUP_FOLDER_CURRENT_USER_REGISTRY_PATH_APPROWED, true);
        //                break;
        //            case StartupType.StartupFolderAllUsers:
        //                keyApprowed = Registry.CurrentUser.OpenSubKey(STARTUP_FOLDER_ALL_USERS_REGISTRY_PATH_APPROWED, true);
        //                break;
        //        }

        //        if (keyApprowed != null)
        //        {
        //            keyApprowed?.DeleteValue(nameFile, false);
        //        }

        //        bool deleted = FileApi.DeleteFileW(folderPath);

        //        if (!deleted)
        //        {
        //            int errorCode = Marshal.GetLastWin32Error();
        //            string errorMessage = new Win32Exception(errorCode).Message;

        //            string itemName = string.IsNullOrWhiteSpace(nameFile) ? Path.GetFileName(folderPath) : nameFile;
        //            throw new IOException($"Не удалось удалить файл '{itemName}'. Код ошибки: {errorCode} - {errorMessage}");
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        keyApprowed?.Dispose(); 
        //    }
        //}

        private StartupState GetStartupState(string registryName, StartupType startupType, bool is32Bit = false)
        {
            if (string.IsNullOrWhiteSpace(registryName)) { return StartupState.None; }
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
       

        private string ExtractTaskSchedulerPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return string.Empty; }

            string trimmedPath = path.Trim();

            // Если путь начинается с двойных кавычек ""
            if (trimmedPath.StartsWith("\"\"") && trimmedPath.Length > 2)
            {
                int endQuoteIndex = trimmedPath.IndexOf('"', 2);
                if (endQuoteIndex == -1) { return path; }

                trimmedPath = trimmedPath.Substring(2, endQuoteIndex - 2);
            }
            // Если путь начинается с одной кавычки "
            else if (trimmedPath.StartsWith("\"") && trimmedPath.Length > 1)
            {
                int endQuoteIndex = trimmedPath.IndexOf('"', 1);
                if (endQuoteIndex == -1) { return path; }

                trimmedPath = trimmedPath.Substring(1, endQuoteIndex - 1);
            }

            return CheckExistsPath(trimmedPath);
        }
        private string ExtractRegistryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return string.Empty; }

            string trimmedPath = path.Trim();

            string expandedPath = Environment.ExpandEnvironmentVariables(trimmedPath);
            if (File.Exists(expandedPath) || Directory.Exists(expandedPath))
            {
                return expandedPath;
            }
            // Убираем кавычки в начале и конце
            if (trimmedPath.StartsWith("\"") && trimmedPath.Length > 1)
            {
                int endQuoteIndex = trimmedPath.IndexOf('"', 1);
                if (endQuoteIndex == -1) { return path; }

                trimmedPath = trimmedPath.Substring(1, endQuoteIndex - 1);
            }
            else if (trimmedPath.Length > 1)
            {
                int endEmptyIndex = trimmedPath.IndexOf(' ', 1);
                if (endEmptyIndex == -1) { return path; }

                trimmedPath = trimmedPath.Substring(0, endEmptyIndex);
            }

            return CheckExistsPath(trimmedPath);
        }
        private string CheckExistsPath(string path)
        {
            string expandedPath = Environment.ExpandEnvironmentVariables(path);
            if (File.Exists(expandedPath) || Directory.Exists(expandedPath))
            {
                return expandedPath;
            }
            return path;
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
