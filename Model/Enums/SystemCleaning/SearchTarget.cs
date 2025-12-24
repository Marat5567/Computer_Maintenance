namespace Computer_Maintenance.Model.Enums.SystemCleaning
{
    [Flags]
    public enum SearchTarget
    {
        Files = 1 << 0,     // Поиск файлов
        Directories = 1 << 1, // Поиск папок
    }
}
