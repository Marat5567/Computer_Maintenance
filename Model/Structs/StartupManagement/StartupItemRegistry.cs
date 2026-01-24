using Computer_Maintenance.Model.Enums.StartupManagement;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct StartupItemRegistry
    {
        public string NameExtracted { get; set; } // Имя исполняемого файла без пути
        public string RegistryName { get; set; } // Имя записи в реестре

        public string PathExtracted { get; set; } //Путь к исполняемому файлу

        //public string RegistryPathValue { get; set; }

        public string Bit { get; set; }
        public StartupType Type { get; set; }

    }
}
