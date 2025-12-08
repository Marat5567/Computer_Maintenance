namespace Computer_Maintenance.Model.Structs
{
    public enum PatternType
    {
        Folder,
        File,
        All
    }
    public struct CleaningInformation_Pattern
    {
        public PatternType Type { get; set; }
        public List<string> IncludePattern { get; set; }
        public List<string> ExcludePattern { get; set; }
    }
}
