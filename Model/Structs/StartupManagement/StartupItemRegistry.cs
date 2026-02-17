using Computer_Maintenance.Model.Enums.StartupManagement;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct StartupItemRegistry
    {
        public string RegistryName { get; set; } // Имя записи в реестре

        public string Path { get; set; }//Путь к исполняемому файлу 
        public bool Is32Bit { get; set; }
        public StartupState State { get; set; }
        public StartupType Type { get; set; }

    }
}
