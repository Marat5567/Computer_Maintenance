using System.Runtime.InteropServices;
using System.Security;

namespace Computer_Maintenance.Core.WinApi
{
    public static class FileApi
    {
        /// <summary>
        /// Enums
        /// </summary>
        [Flags]
        public enum FINDEX_INFO_LEVELS : uint
        {
            FindExInfoStandard = 0,    // Стандартная информация
            FindExInfoBasic = 1,       // Только базовая информация (быстрее!)
            FindExInfoMaxInfoLevel = 2
        }

        [Flags]
        public enum FINDEX_SEARCH_OPS : uint
        {
            FindExSearchNameMatch = 0,           // Обычный поиск
            FindExSearchLimitToDirectories = 1,  // Только директории
            FindExSearchLimitToDevices = 2,      // Только устройства
            FindExSearchMaxSearchOp = 3
        }

        [Flags]
        public enum FINDEX_FLAGS : uint
        {
            FIND_FIRST_EX_CASE_SENSITIVE = 0x00000001,     // Регистрозависимый
            FIND_FIRST_EX_LARGE_FETCH = 0x00000002,        // ОПТИМИЗАЦИЯ: большие буферы
            FIND_FIRST_EX_ON_DISK_ENTRIES_ONLY = 0x00000004 // Только файлы на диске
        }

        /// <summary>
        /// Структуы
        /// </summary>
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

        /// <summary>
        /// Коды ошибок
        /// </summary>
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_NO_MORE_FILES = 18;
        public const int ERROR_PATH_NOT_FOUND = 3;
        public const int INVALID_HANDLE_VALUE = -1;

        /// <summary>
        /// Функции
        /// </summary>
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr FindFirstFileExW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            FINDEX_INFO_LEVELS fInfoLevelId,
            out WIN32_FIND_DATA lpFindFileData,
            FINDEX_SEARCH_OPS fSearchOp,
            IntPtr lpSearchFilter,
            FINDEX_FLAGS dwAdditionalFlags);


        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool FindNextFileW(
            IntPtr hFindFile,
            out WIN32_FIND_DATA lpFindFileData);
        

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool FindClose(
         IntPtr hFindFile);
    }
}
