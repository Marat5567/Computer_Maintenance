namespace Computer_Maintenance.Model.Services
{
    public static class DirectoryService
    {

        /// <summary>
        /// Метод возврощающий все файлы в каталоге и подкаталогах включая текущий каталог
        /// </summary>
        public static string[] GetFilesRecursiveIncludeCurrentCatalog(string path, string pattern = "*")
        {
            try
            {

                EnumerationOptions options = new EnumerationOptions
                {
                    AttributesToSkip = FileAttributes.System | FileAttributes.Directory | FileAttributes.Device | FileAttributes.ReparsePoint | FileAttributes.Encrypted,
                    IgnoreInaccessible = true,
                    MatchCasing = MatchCasing.PlatformDefault,
                    MatchType = MatchType.Win32,
                    RecurseSubdirectories = true,
                    ReturnSpecialDirectories = false,
                };

                return Directory.GetFiles(path, pattern, options);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
        public static string[] GetFiles(string path, string pattern = "*", EnumerationOptions enumerationOptions = null)
        {
            try
            {
                if (enumerationOptions == null)
                {
                    enumerationOptions = new EnumerationOptions
                    {
                        RecurseSubdirectories = true,
                        IgnoreInaccessible = true,
                        ReturnSpecialDirectories = false,
                        MatchType = MatchType.Win32
                    };
                }
                return Directory.GetFiles(path, pattern, enumerationOptions);
            }
            catch
            {
                return Array.Empty<string>();
            }     
        }
        public static string[] GetDirectories(string path, string pattern = "*")
        {
            try
            {
                return Directory.GetDirectories(path, pattern);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }
        //public static bool HasAccess(string directoryPath)
        //{
        //    try
        //    {
        //        FileAttributes attr = File.GetAttributes(directoryPath);
        //        if (attr.HasFlag(FileAttributes.Directory) && (attr & (FileAttributes.Device | 
        //                                                 FileAttributes.ReparsePoint | 
        //                                                 FileAttributes.Encrypted)) == 0)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public static string GetName(string path)
        {
            try
            {
                return new DirectoryInfo(path).Name;
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
