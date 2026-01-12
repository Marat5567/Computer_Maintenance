namespace Computer_Maintenance.Model.Structs
{
    public class DirectoryContents
    {
        public string FullPath { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public ulong Size { get; set; } = 0;
        public Icon? Icon { get; set; }
        public bool IsDirectory { get; set; } = false;
    }
}
