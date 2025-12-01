using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.View.Interfaces;
using Computer_Maintenance.Core.Services;

namespace Computer_Maintenance.Presenters
{
    public class SystemCleaningControlPresenter
    {
        private readonly ISystemCleaningControlView _view;
        private readonly SystemCleaningControlModel _model;

        private List<DriveInfoModel> _availableDrives;
        private List<DriveInfoModel> _selectedDrives;

        public SystemCleaningControlPresenter(
            ISystemCleaningControlView view,
            SystemCleaningControlModel model)
        {
            _view = view;
            _model = model;

            _view.LoadDrivesRequested += OnLoadDrivesRequested;
            _view.StartScanClicked += OnStartScan;
            _view.StartCleanClicked += OnStartClean;
        }

        private void OnLoadDrivesRequested(object sender, EventArgs e)
        {
            try
            {
                var drives = _model.GetDrives();
                var systemDriveName = _model.GetSystemDriveName();

                _availableDrives = new List<DriveInfoModel>();

                foreach (var drive in drives)
                {
                    DriveInfoModel model = new DriveInfoModel();
                    model.Name = drive.Name;
                    model.DiskType = _model.GetDiskType(drive);
                    model.IsSystem = drive.Name.Equals(systemDriveName, StringComparison.OrdinalIgnoreCase);
                    model.TotalGB = _model.BytesToGB(drive.TotalSize);
                    model.FreeGB = _model.BytesToGB(drive.AvailableFreeSpace);

                    _availableDrives.Add(model);
                }

                _view.DisplayDrives(_availableDrives);
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
        }

        private void OnStartScan(object sender, EventArgs e)
        {
            _view.DisplayClearInfoDrives();

            _selectedDrives = _view.GetCheckedDrives();
            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выберите хотя бы 1 диск");
                return;
            }

            Dictionary<(string DiskName, CleanOption Option), long> optionSizes =
                new Dictionary<(string DiskName, CleanOption Option), long>();

            foreach (DriveInfoModel disk in _selectedDrives)
            {
                List<OptionInfo> options = CleaningRules.Rules[disk.DiskType];
                foreach (OptionInfo option in options)
                {
                    long size = _model.GetOptionSize(disk, option);
                    optionSizes.Add((disk.Name, option.Option), size);
                }
            }

            _view.DisplayInfoDrives(_selectedDrives, Access.CurrentAccess, optionSizes);
        }

        public void OnStartClean(object sender, EventArgs e)
        {
            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выполните сканирование");
                return;
            }

            List<(DriveInfoModel Disk, OptionInfo Option)> selectedOptions = _view.GetSelectedOptions();
            List<DriveInfoModel> drivesWithoutOptions = GetDrivesWithoutOptions(selectedOptions);

            if (drivesWithoutOptions.Count > 0)
            {
                ShowDisksWithoutOptions(drivesWithoutOptions);
                return;
            }

            try
            {
                PerformCleanup(selectedOptions);
                ShowInfo("Очистка завершена");
            }
            catch (Exception ex)
            {
                ShowError($"Произошла ошибка: {ex}");
            }
            finally
            {
                if (_availableDrives != null)
                {
                    _availableDrives.Clear();
                }
                if (_selectedDrives != null)
                {
                    _selectedDrives.Clear();
                }
            }
        }


        private List<DriveInfoModel> GetDrivesWithoutOptions(
            List<(DriveInfoModel Disk, OptionInfo Option)> selectedOptions)
        {
            List<DriveInfoModel> result = new List<DriveInfoModel>();

            foreach (var disk in _selectedDrives)
            {
                bool hasOption = false;

                foreach (var pair in selectedOptions)
                {
                    if (pair.Disk == disk)
                    {
                        hasOption = true;
                        break;
                    }
                }

                if (!hasOption)
                    result.Add(disk);
            }

            return result;
        }

        private void ShowDisksWithoutOptions(List<DriveInfoModel> disks)
        {
            string disksList = string.Empty;

            for (int i = 0; i < disks.Count; i++)
            {
                disksList += disks[i].Name.Remove(2);
                if (i < disks.Count - 1)
                {
                    disksList += ", ";
                }
            }

            ShowInfo($"Для следующих дисков не выбраны опции: {disksList}. Выберите опции.");
        }

        private void PerformCleanup(List<(DriveInfoModel Disk, OptionInfo Option)> selectedOptions)
        {
            foreach (var pair in selectedOptions)
            {
                _model.PerformCleanup(pair.Disk, pair.Option);
            }
        }

        private void ShowInfo(string message)
        {
            MessageService.ShowMessage(null, message, "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowError(string message)
        {
            MessageService.ShowMessage(null, message, "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
