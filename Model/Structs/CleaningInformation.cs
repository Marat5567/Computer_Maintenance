using Computer_Maintenance.Model.Enums;

namespace Computer_Maintenance.Model.Structs
{
    public class CleaningInformation
    {

        public string SectionName { get; set; } = String.Empty; // Имя основного раздела
        public bool IsSingleItem { get; set; } = false;//Только 1 пункт
        public StorageSize Size { get; set; } = new(); //Размер раздела


        // Если IsSingleItem = true, используем SingleItem
        // Иначе используем SubItems
        public SubCleaningInformation? SingleItem { get; set; } = new();
        public List<SubCleaningInformation> SubItems { get; set; } = new(); //Вложенные разделы
    }

    public class SubCleaningInformation
    {
        public string SectionName { get; set; } = String.Empty; // Имя основного раздела
        public TypeCleaning TypeCleaning { get; set; } // Тип очистки для выбора метода очистки
        public SearchConfiguration SearchConfig { get; set; } = new(); // Кофигурация поиска
        public StorageSize Size { get; set; } //Размер раздела

        public bool IsEnable { get; set; } = true; // Состояние

    }
}
