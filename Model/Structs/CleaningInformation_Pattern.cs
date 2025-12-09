namespace Computer_Maintenance.Model.Structs
{
    [Flags]
    public enum PatternType
    {
        None = 0,
        Folder = 1,
        File = 2,
        All = Folder | File 
    }
    public struct CleaningInformation_Pattern
    {
        public PatternType Type { get; set; }
        public List<string> IncludePattern { get; set; }
        public List<string> ExcludePattern { get; set; }
        public bool RecursiveDelete { get; set; }
    }
}
