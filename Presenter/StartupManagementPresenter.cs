using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenter
{
    public class StartupManagementPresenter
    {
        private readonly IStartupManagementView _view;
        private readonly StartupManagementModel _model;

        private List<object> _selectedItems = new List<object>();
        private StartupType _lastSelectedType = StartupType.None;
        private string _currentSelectedPath = string.Empty;

        public StartupManagementPresenter(IStartupManagementView view, StartupManagementModel model)
        {
            _view = view;
            _model = model;

            _view.LoadControl += OnLoadControl;
            _view.OpenExplorerClicked += OnOpenExplorerClicked;
            _view.ChangeStateSelectedItems += OnChangeStateSelectedItems;
            _view.CopyClipboardClicked += OnCopyClipboardClicked;
            _view.DeleteRegistryRecordClick += OnDeleteRegistryRecord;
            _view.ViewDetailClick += OnViewDetailClick;
            _view.CompleteTaskClick += OnCompleteTaskClick;
            _view.RunTaskClick += OnRunTaskClick;
            _view.DeleteTaskClick += OnDeleteTaskItem;
            _view.SelectionChanged += OnViewSelectionChanged;
        }

        private void OnViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItems = e.SelectedItems;
            _lastSelectedType = e.SelectionSource;

            UpdateSelectedPathInfo(e.SelectionSource);
        }

        private void OnDeleteTaskItem(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
            {
                return;
            }

            DialogResult msgBoxResult = MessageService.ShowMessage(
                owner: null,
                msg: $"Удалить выбранные значения ({_selectedItems.Count} шт.)?",
                headerName: "Удаление из планировщика задач",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question);

            if (msgBoxResult != DialogResult.Yes)
                return;

            StartupType typeRefresh = StartupType.None;

            foreach (object item in _selectedItems)
            {
                if (item is TaskSchedulerItem taskSchedulerItem)
                {

                    typeRefresh = taskSchedulerItem.Type;

                    try
                    {
                        _model.DeleteTaskItem(taskSchedulerItem.Name);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowMessage(
                            owner: null,
                            msg: $"{ex.Message} файл: {taskSchedulerItem.Name}",
                            headerName: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
                        continue;
                    }
                }
                else
                {
                    return;
                }
            }

            RefreshStartupItems(typeRefresh);
            _selectedItems.Clear();
        }

        private void UpdateSelectedPathInfo(StartupType type)
        {
            _currentSelectedPath = String.Empty;

            if (_selectedItems.Count == 0)
            {
  
                if (type == StartupType.StartupFolderCurrentUser)
                {
                    _currentSelectedPath = StartupManagementModel.FOLDER_CURRENT_USER;
                }
                else if (type == StartupType.StartupFolderAllUsers)
                {
                    _currentSelectedPath = StartupManagementModel.FOLDER_All_USERS;
                }
                else
                {
                    _currentSelectedPath = String.Empty;
                }
                return;
            }

            if (_selectedItems.Count == 1)
            {
                object firstItem = _selectedItems[0];

                if (firstItem is StartupItemFolder folderItem)
                {
                    _currentSelectedPath = folderItem.Path;
                }
                else if (firstItem is StartupItemRegistry registryItem)
                {
                    _currentSelectedPath = registryItem.Path;
                }
                else if (firstItem is TaskSchedulerItem taskItem)
                {
                    _currentSelectedPath = taskItem.Path;
                }
            }

        }

        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadStartupItems(StartupType.All);

            RefreshStartupItems(StartupType.RegistryCurrentUser);
            RefreshStartupItems(StartupType.RegistryLocalMachine);
            RefreshStartupItems(StartupType.StartupFolderCurrentUser);
            RefreshStartupItems(StartupType.StartupFolderAllUsers);
            RefreshStartupItems(StartupType.TaskScheduler);

            _selectedItems.Clear();
        }

        private void OnRunTaskClick(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
                return;

            foreach (object item in _selectedItems)
            {
                if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.RunTask(taskSchedulerItem.Name))
                    {
                        RefreshStartupItems(taskSchedulerItem.Type);
                    }
                }
            }
        }

        private void OnCompleteTaskClick(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
                return;

            foreach (object item in _selectedItems)
            {
                if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.CompleteTask(taskSchedulerItem.Name))
                    {
                        RefreshStartupItems(taskSchedulerItem.Type);
                    }
                }
            }
        }

        private void OnViewDetailClick(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
                return;

            foreach (object item in _selectedItems)
            {
                if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    _model.ViewDetailTaskSchedulerItem(
                        taskSchedulerItem.Author, 
                        taskSchedulerItem.Path,
                        taskSchedulerItem.Description, 
                        taskSchedulerItem.Created, 
                        taskSchedulerItem.NextTimeStart, 
                        taskSchedulerItem.OldTimeStart,
                        taskSchedulerItem.ResultLastStart, 
                        taskSchedulerItem.Trigger);
                }
            }
        }

        private void RefreshStartupItems(StartupType type)
        {
            _model.LoadStartupItems(type);
            _view.DisplayItems(_model.GetStartupItems(type), type);
        }

        private void OnDeleteRegistryRecord(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
            {
                return;
            }

            DialogResult msgBoxResult = MessageService.ShowMessage(
                owner: null,
                msg: $"Удалить выбранные значения ({_selectedItems.Count} шт.)?",
                headerName: "Удаление из реестра",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question);

            if (msgBoxResult != DialogResult.Yes)
                return;

            StartupType typeRefresh = StartupType.None;

            foreach (object item in _selectedItems)
            {
                if (item is StartupItemRegistry registry)
                {
                    typeRefresh = registry.Type;

                    try
                    {
                        _model.DeleteRegistryRecord(registry.RegistryName, registry.Type, registry.Is32Bit);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowMessage(
                            owner: null,
                            msg: ex.Message,
                            headerName: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
                        continue;
                    }
                }
                else
                {
                    return;
                }
            }

            RefreshStartupItems(typeRefresh);
            _selectedItems.Clear();
        }

        private void OnChangeStateSelectedItems(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0)
            {
                return;
            }

            HashSet<StartupType> typesToRefresh = new HashSet<StartupType>();

            foreach (object item in _selectedItems)
            {
                if (item is StartupItemRegistry registryItem)
                {
                    if (_model.ChangeStateStartup(registryItem.RegistryName, registryItem.Path, registryItem.Type, registryItem.Is32Bit))
                    {
                        typesToRefresh.Add(registryItem.Type);
                    }
                }
                else if (item is StartupItemFolder folderItem)
                {
                    if (_model.ChangeStateStartup(folderItem.NameExtracted, folderItem.Path, folderItem.Type))
                    {
                        typesToRefresh.Add(folderItem.Type);
                    }
                }
                else if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.ChangeStateStartup(taskSchedulerItem.Name, taskSchedulerItem.Path, taskSchedulerItem.Type))
                    {
                        typesToRefresh.Add(taskSchedulerItem.Type);
                    }
                }
                else
                {
                    return;
                }
            }

            foreach (StartupType type in typesToRefresh)
            {
                RefreshStartupItems(type);
            }
        }

        public void OnCopyClipboardClicked(object s, EventArgs e)
        {
            if (_selectedItems.Count == 0 || _selectedItems.Count > 1)
            {
                return;
            }

            foreach (object item in _selectedItems)
            {
                try
                {
                    if (item is StartupItemRegistry registryItem)
                    {
                        _model.CopyToClipboard(registryItem.Path);
                    }
                    else if (item is StartupItemFolder folderItem)
                    {
                        _model.CopyToClipboard(folderItem.Path);
                    }
                    else if (item is TaskSchedulerItem taskSchedulerItem)
                    {
                        _model.CopyToClipboard(taskSchedulerItem.Path);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        private void OnOpenExplorerClicked(object s, EventArgs e)
        {
            try
            {
                _model.OpenPathToExplorer(_currentSelectedPath);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        private void ShowError(string msg)
        {
            MessageService.ShowMessage(
                    owner: null,
                    msg: msg,
                    headerName: "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }
}