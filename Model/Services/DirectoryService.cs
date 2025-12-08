namespace Computer_Maintenance.Model.Services
{
    public static class DirectoryService
    {
        public static string[] GetFiles(string path, string pattern = "*", EnumerationOptions enumerationOptions = null)
        {
            try
            {
                if (enumerationOptions == null)
                {
                    enumerationOptions = new EnumerationOptions 
                    {
                        RecurseSubdirectories = false,
                        IgnoreInaccessible = true, 
                        ReturnSpecialDirectories = false,
                    };
                }
                return Directory.GetFiles(path, pattern, enumerationOptions);
            }
            catch (ArgumentNullException)
            {

            }
            catch (IOException)
            {

            }
            catch (UnauthorizedAccessException)
            {

            }
            catch
            {

            }

            return Array.Empty<string>();
        }
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
