using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;
using Microsoft.Win32;
using System.Xml.Linq;

namespace Computer_Maintenance.Presenter
{
    public class StartupManagementPresenter
    {
        private readonly IStartupManagementView _view;
        private readonly StartupManagementModel _model;

        private List<object> _selectedItems = new List<object>();
        private StartupType _lastSelectedType = StartupType.None;

        public StartupManagementPresenter(IStartupManagementView view, StartupManagementModel model)
        {
            _view = view;
            _model = model;

            _view.LoadControl += OnLoadControl;
            _view.OpenExplorerClicked += OnOpenExplorerClicked;
            _view.ChangeStateSelectedItems += OnChangeStateSelectedItems;
            _view.CopyClipboardClicked += OnCopyClipboardClicked;
            _view.DeleteUnusedRecords_Click += OnDeleteUnusedRecords_Click;
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

            UpdateSelectedPathInfo();
        }

        private void OnDeleteTaskItem(object s, EventArgs e)
        {

        }

        private void UpdateSelectedPathInfo()
        {
            if (_selectedItems.Count == 0)
            {
                _view.SelectedPath = (isFile: false, path: String.Empty);
                _view.LastFolderSelectionSource = _lastSelectedType;
                return;
            }

            if (_selectedItems.Count == 1)
            {
                object firstItem = _selectedItems[0];

                if (firstItem is StartupItemFolder folderItem)
                {
                    _view.SelectedPath = (isFile: true, path: folderItem.PathExtracted);
                    _view.LastFolderSelectionSource = folderItem.Type;
                }
                else if (firstItem is StartupItemRegistry registryItem)
                {
                    _view.SelectedPath = (isFile: true, path: registryItem.PathExtracted);
                }
                else if (firstItem is TaskSchedulerItem taskItem)
                {
                    _view.SelectedPath = (isFile: true, path: taskItem.PathExtracted);
                    _view.LastFolderSelectionSource = taskItem.Type;
                }
            }
            else
            {
                if (_selectedItems[0] is StartupItemRegistry registryItem)
                {
                    _view.SelectedPath = (isFile: false, path: registryItem.PathExtracted);
                }
                else if (_selectedItems[0] is StartupItemFolder folderItem)
                {
                    _view.SelectedPath = (isFile: false, path: folderItem.PathExtracted);
                    _view.LastFolderSelectionSource = folderItem.Type;
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
                    if (_model.RunTask(taskSchedulerItem.File))
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
                    if (_model.CompleteTask(taskSchedulerItem.File))
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
                        taskSchedulerItem.OriginalPath,
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
                    if (typeRefresh == StartupType.None)
                    {
                        typeRefresh = registry.Type;
                    }

                    try
                    {
                        _model.DeleteRegistryRecord(registry.RegistryName, registry.NameExtracted, registry.Type, registry.Is32Bit);
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

        private void OnDeleteUnusedRecords_Click(object s, EventArgs e)
        {
            _model.DeleteUnusedRecords_Click(StartupType.RegistryCurrentUser);
            RefreshStartupItems(StartupType.RegistryCurrentUser);

            _model.DeleteUnusedRecords_Click(StartupType.RegistryLocalMachine);
            RefreshStartupItems(StartupType.RegistryLocalMachine);

            _model.DeleteUnusedRecords_Click(StartupType.RegistryLocalMachine, true);
            RefreshStartupItems(StartupType.RegistryLocalMachine);

            _model.DeleteUnusedRecords_Click(StartupType.StartupFolderCurrentUser);
            RefreshStartupItems(StartupType.StartupFolderCurrentUser);

            _model.DeleteUnusedRecords_Click(StartupType.StartupFolderAllUsers);
            RefreshStartupItems(StartupType.StartupFolderAllUsers);

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
                    if (_model.ChangeStateStartup(registryItem.RegistryName, registryItem.PathExtracted, registryItem.Type, registryItem.Is32Bit))
                    {
                        typesToRefresh.Add(registryItem.Type);
                    }
                }
                else if (item is StartupItemFolder folderItem)
                {
                    if (_model.ChangeStateStartup(folderItem.NameExtracted, folderItem.PathExtracted, folderItem.Type))
                    {
                        typesToRefresh.Add(folderItem.Type);
                    }
                }
                else if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.ChangeStateStartup(taskSchedulerItem.File, taskSchedulerItem.PathExtracted, taskSchedulerItem.Type))
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
                if (item is StartupItemRegistry registryItem)
                {
                    _model.CopyToClipboard(registryItem.PathExtracted);
                }
                else if (item is StartupItemFolder folderItem)
                {
                    _model.CopyToClipboard(folderItem.PathExtracted);
                }
                else if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    _model.CopyToClipboard(taskSchedulerItem.PathExtracted);
                }
                else
                {
                    return;
                }
            }
        }

        private void OnOpenExplorerClicked(object s, EventArgs e)
        {
            _model.OpenPathToExplorer(_view.SelectedPath.isFile, _view.SelectedPath.path, _view.LastFolderSelectionSource);
        }
    }
}