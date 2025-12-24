using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Structs;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class SystemCleaningPresenter
    {
        private readonly ISystemCleaningView _view; 
        private readonly SystemCleaningModel _model;

        private List<DriveInfo>? _selectedDrives; //Список выбранных дисков
        private bool _scanClicked = false; //Состояние нажатия сканирование

        public SystemCleaningPresenter(ISystemCleaningView view, SystemCleaningModel model)
        {
            _view = view;
            _model = model;

            _view.LoadDrivesRequested += OnLoadDrivesRequested;
            _view.StartScanClicked += async (s,e) => await OnStartScanAsync(s,e);
            _view.StartCleanClicked += OnStartClean;
        }

        private void OnLoadDrivesRequested(object sender, EventArgs e)
        {
            if (_selectedDrives != null && _selectedDrives.Count > 0)
            {
                _selectedDrives.Clear();
            }
            List<DriveInfo> _allDrives = _model.GetDrives();
            _view.ShowAvailableDrives(_allDrives);
        }

        private async Task OnStartScanAsync(object sender, EventArgs e)
        {
            _selectedDrives = _view.GetSelectedDrives();

            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выберите хотя бы один диск");
                return;
            }
            if (_scanClicked)
            {
                ShowInfo("Дождитесь начатого сканирования");
                return;
            }
            _scanClicked = true;
            _view.ClearInfoDrives();

            foreach (DriveInfo dInfo in _selectedDrives)
            {
                if (dInfo == null) { continue; }

                List<Task> tasks = new List<Task>();

                List<CleaningInformation> cleaningInformations = _model.GetLocationsForDrive(dInfo);
                foreach (CleaningInformation cleaningInfo in cleaningInformations)
                {
                    if (cleaningInfo == null) { continue; }
                    tasks.Add(Task.Run(async () =>
                    {
                        StorageSize totalSize = new StorageSize();
                        if (cleaningInfo.IsSingleItem)
                        {
                            totalSize = _model.GetSizeSubSection(cleaningInfo.SingleItem);
                        }
                        else if (cleaningInfo.SubItems != null && cleaningInfo.SubItems.Count > 0)
                        {
                            List<Task<StorageSize>> subTasks = new List<Task<StorageSize>>();

                            foreach (SubCleaningInformation subInfo in cleaningInfo.SubItems)
                            {
                                if (subInfo == null) { continue; }

                                subTasks.Add(Task.Run(() =>
                                {
                                    subInfo.Size = _model.GetSizeSubSection(subInfo);
                                    return subInfo.Size;
                                }));
                            }
                            StorageSize[] subSizes = await Task.WhenAll(subTasks);
                            foreach (StorageSize size in subSizes)
                            {
                                totalSize.AddSize(size);
                            }
                        }
                        cleaningInfo.Size = totalSize;
                    }));
                }
                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка: {ex.Message}");
                }
                _view.ShowCheckedDriveSafe(dInfo, cleaningInformations);
            }
            _scanClicked = false;
        }

        public void OnStartClean(object sender, EventArgs e)
        {
            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выполните сканирование");
                return;
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
