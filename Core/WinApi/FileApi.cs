using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Computer_Maintenance.Core.WinApi
{
    public static partial class FileApi
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly string GetFileName()
            {
                fixed (char* ptr = cFileName)
                {
                    return new string(ptr);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly long GetFileSize()
            {
                return ((long)nFileSizeHigh << 32) | nFileSizeLow;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly bool IsDirectory()
            {
                return (dwFileAttributes & (int)FileAttributes.Directory) != 0;
            }
        }

        /// <summary>
        /// Коды ошибок
        /// </summary>
        public const int ERROR_SUCCESS = 0;                 // Успешно
        public const int ERROR_FILE_NOT_FOUND = 2;          // Файл не найден
        public const int ERROR_PATH_NOT_FOUND = 3;          // Путь не найден
        public const int ERROR_ACCESS_DENIED = 5;           // Нет доступа
        public const int ERROR_INVALID_HANDLE = 6;          // Неверный дескриптор
        public const int ERROR_SHARING_VIOLATION = 32;      // Файл занят процессом
        public const int ERROR_FILE_EXISTS = 80;            // Файл уже существует
        public const int ERROR_ALREADY_EXISTS = 183;        // Объект уже существует
        public const int ERROR_NO_MORE_FILES = 18;          // Больше нет файлов
        public const int ERROR_DIR_NOT_EMPTY = 145;         // Папка не пустая
        public const int INVALID_HANDLE_VALUE = -1;         // Неверный дескриптор


        /// <summary>
        /// Функции
        /// </summary>
        /// 

        [LibraryImport("kernel32.dll", EntryPoint = "FindFirstFileExW", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static unsafe partial IntPtr FindFirstFileExW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            FINDEX_INFO_LEVELS fInfoLevelId,
            WIN32_FIND_DATA* lpFindFileData,
            FINDEX_SEARCH_OPS fSearchOp,
            IntPtr lpSearchFilter,
            FINDEX_FLAGS dwAdditionalFlags);


        [LibraryImport("kernel32.dll", EntryPoint = "FindNextFileW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static unsafe partial bool FindNextFileW(
            IntPtr hFindFile,
            WIN32_FIND_DATA* lpFindFileData);


        [LibraryImport("kernel32.dll", EntryPoint = "FindClose")]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static partial bool FindClose(
         IntPtr hFindFile);

        [LibraryImport("kernel32.dll", EntryPoint = "DeleteFileW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static partial bool DeleteFileW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);


        [LibraryImport("kernel32.dll", EntryPoint = "RemoveDirectoryW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static partial bool RemoveDirectoryW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

        [LibraryImport("kernel32.dll", SetLastError = true, EntryPoint = "SetFileAttributesW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetFileAttributesW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            uint dwFileAttributes);
    }
}
