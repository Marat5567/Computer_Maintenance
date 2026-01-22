using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Microsoft.Win32;

namespace Computer_Maintenance.Model.Models
{
    public class StartupManagementModel
    {
        private const string REGISTRY_CURRENT_USER = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        public List<StartupItem> RegistryStartupItems_CurrentUser { get; set; } = new List<StartupItem>();

        public void LoadAllStartupItems()
        {
            LoadRegistryStartupItems();
        }
        private void LoadRegistryStartupItems()
        {
            RegistryStartupItems_CurrentUser.Clear();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_CURRENT_USER))
            {
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();

                    foreach (string valueName in valueNames)
                    {
                        object value = key.GetValue(valueName);
                        string extractedPath = ExtractPath(value?.ToString());

                        StartupItem item = new StartupItem
                        {
                            Name = GetNameExe(extractedPath),
                            OriginalRegistryName = valueName,
                            Type = StartupType.RegistryCurrentUser,
                            Path = extractedPath,
                            OriginalRegistryValue = value?.ToString()
                        };

                        RegistryStartupItems_CurrentUser.Add(item);
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
                    default:
                        return false;
                }


                if (key != null)
                {
                    try
                    {
                        key.DeleteValue(registryKeyName);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
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

        /// <summary>
        /// Извлекаем чистый путь из реестра без всяких аргументов
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
                        return rawStr.Substring(0, index_EmptyChar - 1);
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


        public List<StartupItem> GetStartupItems()
        {
            return RegistryStartupItems_CurrentUser;
        }

    }
}
