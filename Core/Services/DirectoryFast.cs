using System.Runtime.InteropServices;

namespace Computer_Maintenance.Core.Services
{
    public static class DirectoryFast
    {
        [Flags]
        public enum FINDEX_INFO_LEVELS
        {
            FindExInfoStandard = 0,    // Стандартная информация
            FindExInfoBasic = 1,       // Только базовая информация (быстрее!)
            FindExInfoMaxInfoLevel = 2
        }

        [Flags]
        public enum FINDEX_SEARCH_OPS
        {
            FindExSearchNameMatch = 0,           // Обычный поиск
            FindExSearchLimitToDirectories = 1,  // Только директории
            FindExSearchLimitToDevices = 2,      // Только устройства
            FindExSearchMaxSearchOp = 3
        }

        [Flags]
        public enum FINDEX_FLAGS
        {
            FIND_FIRST_EX_CASE_SENSITIVE = 0x00000001,     // Регистрозависимый
            FIND_FIRST_EX_LARGE_FETCH = 0x00000002,        // ОПТИМИЗАЦИЯ: большие буферы
            FIND_FIRST_EX_ON_DISK_ENTRIES_ONLY = 0x00000004 // Только файлы на диске
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public unsafe struct WIN32_FIND_DATA
        {
            public Int32 dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public Int32 nFileSizeHigh;
            public Int32 nFileSizeLow;
            public Int32 dwReserved0;
            public Int32 dwReserved1;
            public fixed Char cFileName[260];
            public fixed Char cAlternateFileName[14];
        }


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstFileEx(
            [In] string lpFileName,
            [In] FINDEX_INFO_LEVELS fInfoLevelId,
            [Out] out WIN32_FIND_DATA lpFindFileData,
            [In] FINDEX_SEARCH_OPS fSearchOp,
            [In] IntPtr lpSearchFilter,
            [In] FINDEX_FLAGS dwAdditionalFlags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindNextFile(
            [In] IntPtr hFindFile,
            [Out] out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindClose(
            [In, Out] IntPtr hFindFile);

        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_NO_MORE_FILES = 18;
        const int ERROR_PATH_NOT_FOUND = 3;

        public static unsafe string[] GetFiles(string path, int count, out int written)
        {
            written = 0;
            string searchPath;
            
            if (path.EndsWith("\\"))
            {
                searchPath = path + "*";
            }
            else
            {
                searchPath = path + "\\" + "*";
            }

            WIN32_FIND_DATA findData;
            IntPtr hFind =IntPtr.Zero;
            hFind = FindFirstFileEx(searchPath,
                    FINDEX_INFO_LEVELS.FindExInfoBasic,
                    out findData,
                    FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                    IntPtr.Zero,
                    FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind == new IntPtr(-1))
            {
                int error = Marshal.GetLastWin32Error();

                // Если нет файлов - возвращаем null
                if (error == 2 | error == 18) // ERROR_FILE_NOT_FOUND или ERROR_NO_MORE_FILES
                {
                    return Array.Empty<string>();
                }
                throw new IOException($"Ошибка FindFirstFileEx: {error}");
            }

            string[] result = new string[count];
            try
            {

                do
                {
                    string name = GetFileNamePtr(&findData);

                    if (name == "." || name == "..")
                    {
                        continue;
                    }

                    result[written++] = name;
                    if (written == count)
                    {
                        break;
                    }
                }
                while (FindNextFile(hFind, out findData));

                return result;

            }
            finally
            {
                FindClose(hFind);
            }
        }

        public static unsafe long GetSizeAllFiles(string path, bool recursive, ref int readed, string pattern = "*")
        {
            string searchPath;
            readed = 0;

            if (path.EndsWith("\\"))
            {
                searchPath = path + "*";
            }
            else
            {
                searchPath = path + "\\" + $"{pattern}";
            }

            WIN32_FIND_DATA findData;
            IntPtr hFind = IntPtr.Zero;

            hFind = FindFirstFileEx(searchPath,
                FINDEX_INFO_LEVELS.FindExInfoBasic,
                out findData,
                FINDEX_SEARCH_OPS.FindExSearchNameMatch,
                IntPtr.Zero,
                FINDEX_FLAGS.FIND_FIRST_EX_LARGE_FETCH);

            if (hFind == new IntPtr(-1))
            {
                int error = Marshal.GetLastWin32Error();

                // Если нет файлов - возвращаем null
                if (error == ERROR_ACCESS_DENIED ||
                    error == ERROR_FILE_NOT_FOUND ||
                    error == ERROR_NO_MORE_FILES ||
                    error == ERROR_PATH_NOT_FOUND)
                {
                    return 0; // просто пропускаем
                }
                throw new IOException($"Ошибка FindFirstFileEx: {error}");
            }

            long totalSize = 0;

            try
            {
                do
                {
                    string name = GetFileNamePtr(&findData);

                    if (name == "." || name == "..")
                    {
                        continue;
                    }

                    bool isDirectory = (findData.dwFileAttributes & (int)FileAttributes.Directory) != 0;

                    if (!isDirectory)
                    {
                        if ((findData.dwFileAttributes & (int)(FileAttributes.Hidden | FileAttributes.System | FileAttributes.Device | FileAttributes.ReparsePoint | FileAttributes.Encrypted | FileAttributes.Compressed)) == 0)
                        {
                            totalSize += ((long)findData.nFileSizeHigh << 32) | (uint)findData.nFileSizeLow;
                            readed++;
                        }
                    }
                    else
                    {
                        string subdir = Path.Combine(path, name);
                        totalSize += GetSizeAllFiles(subdir, true, ref readed);
                    }
                }

                while (FindNextFile(hFind, out findData));
            }
            finally
            {
                FindClose(hFind);
            }
            return totalSize;
        }

        public static unsafe string GetFileNamePtr(WIN32_FIND_DATA* findData)
        {
            char* ptr = findData->cFileName;


            int length = 0;
            while (ptr[length] != '\0' && length < 259) 
            {
                length++;
            }

            return new string(ptr, 0, length);
        }
    }
}
