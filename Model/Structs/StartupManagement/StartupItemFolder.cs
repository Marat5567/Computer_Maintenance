using Computer_Maintenance.Model.Enums.StartupManagement;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct StartupItemFolder
    {
        public string NameExtracted { get; set; } // Имя исполняемого файла без пути
        public string PathExtracted { get; set; } //Путь к исполняемому файлу
        public StartupType Type { get; set; }
    }
}
