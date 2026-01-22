using Computer_Maintenance.Model.Enums.StartupManagement;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct StartupItem
    {
        public string Name { get; set; } 
        public string OriginalRegistryName { get; set; }
        public StartupType Type { get; set; }
        public string Path { get; set; }
        public string OriginalRegistryValue { get; set; }
    }
}
