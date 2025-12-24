using Computer_Maintenance.Model.Enums.SystemCleaning;
namespace Computer_Maintenance.Model.Structs
{
    //[Flags]
    //public enum PatternType
    //{
    //    None = 0,
    //    Folder = 1,
    //    File = 2,
    //    All = Folder | File 
    //}
    public unsafe class SearchPattern
    {
        public bool IsActive { get; set; } = false; // Активен ли паттерн
        public string Pattern { get; set; } = "*";  // Шаблон поиска
        public PatternMatchType PatternMatchType { get; set; } = PatternMatchType.Exact;
        //public bool IsCaseSensitive { get; set; } = false;
        public SearchConfiguration ChildConfiguration { get; set; } = new SearchConfiguration();// Родительская конфигурация
    }
}
