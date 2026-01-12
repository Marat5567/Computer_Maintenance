using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Enums.SystemCleaning;
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

            _view.DirectoryPath += OnChangeDirectory;
            _view.LoadDrivesRequested += OnLoadDrivesRequested;
            _view.StartScanCleanClicked += async (s,e) => await OnStartScanCleanAsync(s,e);
            _view.StartScanClicked += async (s, e) => await OnStartScanAsync(s, e);
            _view.StartCleanClicked += OnStartClean;
        }

        private void OnLoadDrivesRequested(object sender, EventArgs e)
        {
            if (_selectedDrives != null && _selectedDrives.Count > 0)
            {
                _selectedDrives.Clear();
            }
            List<DriveInfo> _allDrives = _model.GetDrives();
            _scanClicked = false;
            _view.ShowAvailableDrives(_allDrives);
        }

        private async Task OnStartScanCleanAsync(object sender, EventArgs e)
        {
            _selectedDrives = _view.GetSelectedDrives();

            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выберите хотя бы один диск");
                _view.ClearInfoDrives();
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
                _view.ShowCheckedDriveSafe(dInfo, cleaningInformations, ShowTypeInfo.CleaningInfo);
            }
            _scanClicked = false;
        }

        private async Task OnStartScanAsync(object sender, EventArgs e)
        {
            _selectedDrives = _view.GetSelectedDrives();

            if (_selectedDrives == null || _selectedDrives.Count == 0 || _selectedDrives.Count > 1)
            {
                ShowInfo("Выберите только один диск");
                _view.ClearInfoDrives();
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
                List<DirectoryContents> directoryContents = new List<DirectoryContents>();
                directoryContents = _model.GetDirectoryContents(dInfo.Name);

                _view.ShowCheckedDriveForSizeInfo(directoryContents);

            }
            _scanClicked = false;
        }

        private void OnStartClean(object sender, EventArgs e)
        {
            if (_selectedDrives == null || _selectedDrives.Count == 0)
            {
                ShowInfo("Выполните сканирование");
                return;
            }

            List<SubCleaningInformation> cleaningInformations = _view.GetSelectedOptions();

            if (cleaningInformations == null || cleaningInformations.Count == 0)
            {
                ShowInfo("Нету опция для очистки, выберите опции для очистки");
                return;
            }

            DialogResult reult = MessageService.ShowMessage(
                null, 
                "Вы уверены что хотите начать очистку?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            switch (reult)
            {
                case DialogResult.Yes:
                    _model._successfulDeletedFiles.Clear();
                    _model._failedDeletedFiles.Clear();

                    foreach (SubCleaningInformation subInf in cleaningInformations)
                    {
                        _model.StartDelete(subInf);
                    }

                    ShowInfo($"Удалось очистить {_model._successfulDeletedFiles.Count} файлов\n" +
                        $"Не удалось очистить {_model._failedDeletedFiles.Count} файлов");

                    if (_view.SaveFileDeleteFail_Logs)
                    {
                        string path = _model.SaveFileDeleteFailLogs();
                        if (path != String.Empty)
                        {
                            ShowInfo($"Файл с логами ошибок при уадлении файлов сохранен по пути: {path}");
                        }
                    }
                    break;
                case DialogResult.No:
                    break;
                default:
                    return;
            }
        }

        private void OnChangeDirectory(object sender, EventArgs e)
        {
            if (_view.DirectoryPath != String.Empty)
            {
                List<DirectoryContents> directoryContents = new List<DirectoryContents>();
                directoryContents = _model.GetDirectoryContents( _view.DirectoryPath);

                _view.ShowCheckedDriveForSizeInfo(directoryContents);
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
