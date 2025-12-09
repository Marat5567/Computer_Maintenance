using Computer_Maintenance.Model.Enums;

namespace Computer_Maintenance.Model.Structs
{
    public class CleaningInformation
    {
        //public TypeCleaning TypeCleaning { get; set; } //Тип очистки для выбора метода очистки
        //public StorageSize StorageSize { get; set; } //Размер
        //public string Name { get; set; } //Имя раздела
        //public string CurrentPath { get; set; } //Путь
        //public CleaningInformation_Pattern includePattern { get; set; }//Включающий паттерн
        ////public List<string> ExcludePatterns { get; set; } //Паттерн исключения шаблонов при уадаления
        //public AccessLevel RequiredAccess { get; set; } //Права доступа
        //public bool Recursive { get; set; } //Очищать рекурсивно

        public string SectionName { get; set; } // Имя раздела
        public string Path { get; set; } // Основной путь
        public bool RecursiveSearch { get; set; } //Очищать рекурсивно
        public TypeCleaning TypeCleaning { get; set; } // Тип очистки для выбора метода очистки
        public CleaningInformation_Pattern Pattern {get;set;} // Паттерны
        public StorageSize Size { get; set; } //Размер раздела
    }
}
