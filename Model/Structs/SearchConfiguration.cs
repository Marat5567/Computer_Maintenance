using Computer_Maintenance.Model.Enums.SystemCleaning;
namespace Computer_Maintenance.Model.Structs
{
    public class SearchConfiguration
    {
        public string BasePath { get; set; } = String.Empty;
        public SearchTarget SearchTarget { get; set; } = SearchTarget.Files;
        public SearchScope SearchScope { get; set; } = SearchScope.CurrentDirectory;
        public DeleteScope DeleteScope { get; set; } = DeleteScope.OnlyFiles;

        // Паттерны включения (если пусто - включать все)
        public List<SearchPattern> IncludePatterns { get; set; } = new();

        // Паттерны исключения (приоритет выше чем у Include)
        public List<SearchPattern> ExcludePatterns { get; set; } = new();
    }
}
