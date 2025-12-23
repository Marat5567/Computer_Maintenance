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

        private List<DriveInfo> _allDrives; //Список всех доступных дисков
        private List<DriveInfo> _selectedDrives; //Список выбранных дисков
        private bool _scanClicked = false; //Состояние нажатия сканирование

        public SystemCleaningPresenter(ISystemCleaningView view, SystemCleaningModel model)
        {
            _view = view;
            _model = model;

            _view.LoadDrivesRequested += OnLoadDrivesRequested;
            _view.StartScanClicked += OnStartScan;
            _view.StartCleanClicked += OnStartClean;
        }

        private void OnLoadDrivesRequested(object sender, EventArgs e)
        {
            if (_allDrives != null && _allDrives.Count > 0)
            {
                _allDrives.Clear();
            }

            if (_selectedDrives != null && _selectedDrives.Count > 0)
            {
                _selectedDrives.Clear();
            }

            _allDrives = _model.GetDrives();
            _view.ShowAvailableDrives(ref _allDrives);
        }

        private async void OnStartScan(object sender, EventArgs e)
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

                List<CleaningInformation> cleaningInformation = _model.GetLocationsForDrive(dInfo);

                //if (cleaningInformation == null || cleaningInformation.Count == 0) { continue; }

                //Фильтрация несуществующих путей
                foreach (CleaningInformation info in cleaningInformation.ToList())
                {
                    if (info.IsSingleItem)
                    {
                        if (!Directory.Exists(info.SingleItem.SearchConfig.BasePath))
                        {
                            cleaningInformation.Remove(info);
                        }
                    }
                    else
                    {
                        info.SubItems = info.SubItems.Where(s => Directory.Exists(s.SearchConfig.BasePath)).ToList();
                        if (info.SubItems.Count == 0)
                        {
                            cleaningInformation.Remove(info);
                        }
                    }
                }

                object lockObject = new object();
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < cleaningInformation.Count; i++)
                {
                    int index = i;
                    tasks.Add(Task.Run(() =>
                    {
                        CleaningInformation cleaningInfo = cleaningInformation[index];

                        if (cleaningInfo == null) return;

                        List<SubCleaningInformation> localSubItems = cleaningInfo.SubItems?.ToList() ?? new List<SubCleaningInformation>();

                        StorageSize totalSize = new StorageSize();

                        if (cleaningInfo.IsSingleItem)
                        {
                            totalSize = _model.GetSizeSubSection(cleaningInfo.SingleItem);
                        }
                        else
                        {

                            foreach (SubCleaningInformation subCleaningInformation in localSubItems)
                            {
                                if (subCleaningInformation == null) continue;

                                StorageSize size = _model.GetSizeSubSection(subCleaningInformation);
                                subCleaningInformation.Size = size;

                                totalSize.AddSize(size);
                            }
                        }

                        lock (lockObject)
                        {
                            cleaningInfo.Size = totalSize;
                        }
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

                _view.ShowCheckedDriveSafe(dInfo, cleaningInformation);
                _scanClicked = false;
            }
        }


        public void OnStartClean(object sender, EventArgs e)
        {
            if (_selectedDrives.Count <= 0)
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
