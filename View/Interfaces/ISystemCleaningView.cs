using Computer_Maintenance.Model.Structs;

namespace Computer_Maintenance.View.Interfaces
{
    public interface ISystemCleaningView
    {
        event EventHandler LoadDrivesRequested; //Событие для загрузки дисков
        event EventHandler StartScanClicked; //Событие на нажате кнопки начать сканирование
        event EventHandler StartCleanClicked; //Событие на нажате кнопки начать очистку
       
        void ShowAvailableDrives(List<DriveInfo> dInfos); //Метод для показа доступных дисков
        List<DriveInfo> GetSelectedDrives(); //Метод для получения выбранных дисков
        List<CleaningInformation> GetSelectedOptions(); //Метод для получения выбранных опций
        void ShowCheckedDriveSafe(DriveInfo dInfo, List<CleaningInformation> cleaningInformation); //Метод для безопасного вывода инормации об очистки выбранных дисков
        void ClearInfoDrives(); // Метод для очистки информации о выбранных дисках
    }
}
