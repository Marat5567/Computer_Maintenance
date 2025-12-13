using Computer_Maintenance.Model.Structs;

namespace Computer_Maintenance.View.Interfaces
{
    public interface ISystemCleaningView
    {
        event EventHandler LoadDrivesRequested; //Событие для загрузки дисков
        event EventHandler StartScanClicked; //Событие на нажате кнопки начать сканирование
        event EventHandler StartCleanClicked; //Событие на нажате кнопки начать очистку
       
        void ShowAvailableDrives(ref List<DriveInfo> dInfos); //Метод для показа доступных дисков
        List<DriveInfo> GetSelectedDrives(); //Метод для получения выбранных дисков
        List<CleaningInformation> GetSelectedOptions(); //Метод для получения выбранных опций
        void ShowCheckedDrive(DriveInfo dInfo, ref List<CleaningInformation> cleaningInformation); //Метод для вывода выбранных дисков
        void ShowCheckedDriveSafe(DriveInfo dInfo, List<CleaningInformation> cleaningInformation);
        void ClearCheckedDrives(); //Метод для очистки панели выбранных дисков
        void ClearAllByEvent_LoadDrives(); //Метод для очистки всех эдеметов по событию LoadDrives
    }
}
