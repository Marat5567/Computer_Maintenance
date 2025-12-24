namespace Computer_Maintenance.Model.Enums.SystemCleaning
{
    [Flags]
    public enum SearchScope
    {
        CurrentDirectory = 1 << 0,  // Только текущая директория
        Recursive = 1 << 1,  // Рекурсивно в поддиректориях
    }
}
