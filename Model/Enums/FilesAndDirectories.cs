namespace Computer_Maintenance.Model.Enums
{
    public static class FilesAndDirectories
    {
        [Flags]
        public enum SearchTarget
        {
            None = 0,
            Files = 1 << 0,     // Поиск файлов
            Directories = 1 << 1, // Поиск папок
            All = Files | Directories
        }

        [Flags]
        public enum SearchScope
        {
            CurrentDirectory = 1 << 0,  // Только текущая директория
            Recursive = 1 << 1,  // Рекурсивно в поддиректориях
        }

        [Flags]
        public enum DeleteScope
        {
            OnlyFiles = 1 << 0,     // Удалять только файлы
            EmptyDirectories = 1 << 1,   // Удалять пустые папки
            AllContents = 1 << 2,        // Удалять все содержимое
        }

        public enum PatternMatchType
        {
            Simple,     // Простой поиск (*.txt)
            Regex,      // Регулярные выражения
            Exact,      // Точное совпадение
            Contains    // Содержит подстроку
        }
    }
}
