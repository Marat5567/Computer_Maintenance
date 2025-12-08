using System.Security.AccessControl;
using System.Security.Principal;

namespace Computer_Maintenance.Model.Services
{
    public static class FileService
    {
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
        public static bool HasAccess(string filePath)
        {
            try
            {
                if ((File.GetAttributes(filePath) & (FileAttributes.System | 
                                                     FileAttributes.Hidden |
                                                     FileAttributes.ReparsePoint | 
                                                     FileAttributes.Encrypted | FileAttributes.Device | 
                                                     FileAttributes.SparseFile | FileAttributes.Offline)) == 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
    }
}