using Computer_Maintenance.Model.Enums.SystemCleaning;
using Computer_Maintenance.Model.Structs.SystemCleaning;

namespace Computer_Maintenance.View.Interfaces
{
    public interface ISystemCleaningView
    {
        event EventHandler LoadDrivesRequested; //Событие для загрузки дисков
        event EventHandler StartScanCleanClicked; //Событие на нажатие кнопки начать сканирование для удаления
        event EventHandler StartScanClicked; //Событие на нажатие кнопки начать сканирование
        event EventHandler StartCleanClicked; //Событие на нажате кнопки начать очистку
        bool SaveFileDeleteFail_Logs { get; set; }
        string DirectoryPath { get; set; } // Путь для вывода размера каталогов и содержимого

        void ShowCheckedDriveForSizeInfo(List<DirectoryContents> directoryContents);
        void ShowAvailableDrives(List<DriveInfo> dInfos); //Метод для показа доступных дисков
        List<DriveInfo> GetSelectedDrives(); //Метод для получения выбранных дисков
        List<SubCleaningInformation> GetSelectedOptions(); //Метод для получения выбранных опций
        void ShowCheckedDriveSafe(DriveInfo dInfo, List<CleaningInformation> cleaningInformation, ShowTypeInfo showTypeInfo); //Метод для безопасного вывода инормации об очистки выбранных дисков
        void ClearInfoDrives(); // Метод для очистки информации о выбранных дисках
    }
}
