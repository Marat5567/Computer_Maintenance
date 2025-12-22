using Computer_Maintenance.Model.Enums;
namespace Computer_Maintenance.Model.Structs
{
    public class SearchConfiguration
    {
        public string BasePath { get; set; } = String.Empty;
        public FilesAndDirectories.SearchTarget SearchTarget { get; set; } = FilesAndDirectories.SearchTarget.Files;
        public FilesAndDirectories.SearchScope SearchScope { get; set; } = FilesAndDirectories.SearchScope.CurrentDirectory;
        public FilesAndDirectories.DeleteScope DeleteScope { get; set; } = FilesAndDirectories.DeleteScope.OnlyFiles;

        // Паттерны включения (если пусто - включать все)
        public List<SearchPattern> IncludePatterns { get; set; } = new();

        // Паттерны исключения (приоритет выше чем у Include)
        public List<SearchPattern> ExcludePatterns { get; set; } = new();
    }
}
