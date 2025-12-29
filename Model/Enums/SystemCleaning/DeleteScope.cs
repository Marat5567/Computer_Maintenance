namespace Computer_Maintenance.Model.Enums.SystemCleaning
{
    [Flags]
    public enum DeleteScope
    {
        None = 0,
        OnlyFiles = 1 << 0,     // Удалять только файлы
        AllContents = 1 << 1,        // Удалять все содержимое включая папки
    }
}
