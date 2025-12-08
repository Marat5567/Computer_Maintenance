using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Structs;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class SystemCleaningPresenter
    {
        private readonly ISystemCleaningView _view;
        private readonly SystemCleaningModel _model;

        private List<DriveInfo> _selectedDrives = new List<DriveInfo>(); //Список выбранных дисков

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
            _view.ClearAllByEvent_LoadDrives();

            List<DriveInfo> _allDrives = _model.GetDrives();
            _view.ShowAvailableDrives(ref _allDrives);
        }

        private async void OnStartScan(object sender, EventArgs e)
        {
            _view.ClearCheckedDrives();
            _selectedDrives = _view.GetSelectedDrives();

            if (_selectedDrives.Count <= 0)
            {
                _view.ClearCheckedDrives();

                ShowInfo("Выберите хотя-бы один диск");
                return;
            }

            foreach (DriveInfo dInfo in _selectedDrives)
            {
                List<CleaningInformation> cleaningInformation = _model.GetLocationsByAccessForDrive(dInfo);

                List<Task> tasks = new List<Task>();

                for (int i = 0; i < cleaningInformation.Count; i++)
                {
                    int index = i;
                    //tasks.Add(Task.Run(() =>
                    //{
                        cleaningInformation[index].Size = _model.GetSizeSection(cleaningInformation[index]);
                    //}));
                }

                //try
                //{
                //    await Task.WhenAll(tasks);
                //}
                //catch (Exception ex)
                //{
                //    ShowError($"Ошибка: {ex.Message}");
                //}


                _view.ShowCheckedDrive(dInfo, ref cleaningInformation);
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
