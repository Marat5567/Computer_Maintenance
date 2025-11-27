using Computer_Maintenance.Views;
using Computer_Maintenance.Models;

namespace Computer_Maintenance.Presenters
{
    public class SystemCleaningControlPresenter
    {
        private readonly ISystemCleaningControlView _systemCleaningControlView;
        private readonly SystemCleaningControlModel _systemCleaningControlModel;
        private List<DriveInfoModel> _driveModels;
        private List<DriveInfoModel> _userCheckedDrives;
        public SystemCleaningControlPresenter(ISystemCleaningControlView systemCleaningControlView, SystemCleaningControlModel systemCleaningControlModel)
        {
            _systemCleaningControlView = systemCleaningControlView;
            _systemCleaningControlModel = systemCleaningControlModel;

            _systemCleaningControlView.LoadDrivesRequested += OnLoadDrivesRequested;
            _systemCleaningControlView.StartScanClicked += OnStartScan;
            _systemCleaningControlView.StartCleanClicked += OnStartClean;
        }

        private void OnLoadDrivesRequested(object sebder, EventArgs e)
        {
            try
            {
                var drives = _systemCleaningControlModel.GetDrives();
                var systemDrive = _systemCleaningControlModel.GetSystemDriveName();

                _driveModels = new List<DriveInfoModel>();

                foreach (var drive in drives)
                {
                    DriveInfoModel model = new DriveInfoModel
                    {
                        Name = drive.Name,
                        DiskType = _systemCleaningControlModel.GetDiskType(drive),
                        IsSystem = drive.Name.Equals(systemDrive, StringComparison.OrdinalIgnoreCase),
                        TotalGB = _systemCleaningControlModel.BytesToGB(drive.TotalSize),
                        FreeGB = _systemCleaningControlModel.BytesToGB(drive.AvailableFreeSpace),
                    };

                    _driveModels.Add(model);
                }

                _systemCleaningControlView.DisplayDrives(_driveModels);
            }
            catch (Exception ex)
            {
                Globals.Message.ShowMessage(null, msg: ex.ToString(), headerName: "Ошибка", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }
        }
        private void OnStartScan(object sender, EventArgs e)
        {
            _systemCleaningControlView.DisplayClearInfoDrives();

            _userCheckedDrives = _systemCleaningControlView.GetCheckedDrives();
            if (_userCheckedDrives.Count == 0)
            {
                Globals.Message.ShowMessage(null, msg: "Выберите хотя бы 1 диск", headerName: "Информация", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                return;
            }

            Dictionary<(string DiskName, CleanOption Option), long> optionSizes = new Dictionary<(string DiskName, CleanOption Option), long>();
            foreach (var disk in _userCheckedDrives)
            {
                foreach (var opt in CleaningRules.Rules[disk.DiskType])
                {
                    long size = _systemCleaningControlModel.GetOptionSize(disk, opt);
                    optionSizes.Add((disk.Name, opt.Option), size);
                }
            }

            _systemCleaningControlView.DisplayInfoDrives(_userCheckedDrives, Globals.Access.CurrentAccess, optionSizes);
        }
        public void OnStartClean(object sender, EventArgs e)
        {
   
            List<(DriveInfoModel Disk, OptionInfo Option)> selectedOptions = _systemCleaningControlView.GetSelectedOptions();

            // Список дисков без выбранных опций
            List<DriveInfoModel> drivesWithoutOptions = new List<DriveInfoModel>();

            foreach (DriveInfoModel disk in _userCheckedDrives)
            {
                bool hasOption = false;

                foreach (var selected in selectedOptions)
                {
                    if (selected.Disk == disk)
                    {
                        hasOption = true;
                        break;
                    }
                }

                if (!hasOption)
                {
                    drivesWithoutOptions.Add(disk);
                }
            }

            if (drivesWithoutOptions.Count > 0)
            {
                string disksList = "";
                for (int i = 0; i < drivesWithoutOptions.Count; i++)
                {
                    disksList += drivesWithoutOptions[i].Name.Remove(2);
                    if (i < drivesWithoutOptions.Count - 1)
                    {
                        disksList += ", ";
                    }
                }

                Globals.Message.ShowMessage(
                    null,
                    msg: $"Для следующих дисков не выбраны опции: {disksList}, Выберите опции",
                    headerName: "Информация",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Information
                );
                return;
            }

            // Выполняем очистку для выбранных опций
            for (int i = 0; i < selectedOptions.Count; i++)
            {
                DriveInfoModel disk = selectedOptions[i].Disk;
                OptionInfo option = selectedOptions[i].Option;

                _systemCleaningControlModel.PerformCleanup(disk, option);
            }

            Globals.Message.ShowMessage(
                null,
                msg: "Очистка завершена",
                headerName: "Информация",
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Information
            );
        }


    }
}
