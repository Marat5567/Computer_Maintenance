using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Models;

namespace Computer_Maintenance.View.Interfaces
{
    public interface ISystemCleaningControlView
    {
        void DisplayDrives(List<DriveInfoModel> drives); //Метод для вывода дисков
        void DisplayInfoDrives(List<DriveInfoModel> drivesChecked, UserAccess access, Dictionary<(string DiskName, CleanOption Option), long> optionSizes); //Метод для вывода информации об очистки
        void DisplayClearInfoDrives(); // Метод для очистки содержимого из информации об очистки
        List<DriveInfoModel> GetCheckedDrives(); //Метод для получении информации о дисках которые выбрал пользователь
        List<(DriveInfoModel Disk, OptionInfo Option)> GetSelectedOptions(); //Метод для получении информации о выбранных опциях очистки для каждого диска


        event EventHandler LoadDrivesRequested; //Событие для загрузки дисков
        event EventHandler StartScanClicked; //Событие на нажате кнопки начать сканирование
        event EventHandler StartCleanClicked; //Событие на нажате кнопки начать очистку
    }
}
