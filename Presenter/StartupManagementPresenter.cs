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
            _view.DeleteFolderItemClick += OnDeleteFolderItemClick;
        }

        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadStartupItems(StartupType.All);

            RefreshStartupItems(StartupType.RegistryCurrentUser);
            RefreshStartupItems(StartupType.RegistryLocalMachine);
            RefreshStartupItems(StartupType.StartupFolderCurrentUser);
            RefreshStartupItems(StartupType.StartupFolderAllUsers);
            RefreshStartupItems(StartupType.TaskScheduler);
        }

        private void OnDeleteFolderItemClick(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) return;

            if (MessageService.ShowMessage(
                owner: null,
                msg: $"Удалить выбранные приложения ({items.Count} шт.)?",
                headerName: "Подтверждение удаления",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            StartupType typeRefresh = StartupType.None;

            foreach (object item in items)
            {
                if (item is StartupItemFolder folder)
                {
                    if (typeRefresh == StartupType.None)
                    {
                        typeRefresh = folder.Type;
                    }

                    try
                    {
                        _model.DeleteFolderRecord(folder.NameExtracted, folder.PathExtracted, folder.Type);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowMessage(
                            owner: null,
                            msg: $"Не удалось удалить '{folder.NameExtracted}': {ex.Message}",
                            headerName: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
                    }
                }
            }

            RefreshStartupItems(typeRefresh);
        }

        private void RefreshStartupItems(StartupType type)
        {
            _model.LoadStartupItems(type);
            _view.DisplayItems(_model.GetStartupItems(type), type);
        }

        private void OnDeleteRegistryRecord(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            DialogResult msgBoxResult = MessageService.ShowMessage(
                owner: null,
                msg: $"Удалить выбранные значения ({items.Count} шт.)?",
                headerName: "Удаление из реестра",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question);

            if (msgBoxResult != DialogResult.Yes) return;

            StartupType typeRefresh = StartupType.None;

            foreach (object item in items)
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
        }

        private void OnChangeStateSelectedItems(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            StartupType? registryType = null;
            StartupType? folderType = null;
            StartupType? taskType = null;

            foreach (object item in items)
            {
                if (item is StartupItemRegistry registryItem)
                {
                    if (_model.ChangeStateStartup(registryItem.RegistryName, registryItem.PathExtracted, registryItem.Type, registryItem.Is32Bit))
                    {
                        registryType = registryItem.Type;
                    }
                }
                else if (item is StartupItemFolder folderItem)
                {
                    if (_model.ChangeStateStartup(folderItem.NameExtracted, folderItem.PathExtracted, folderItem.Type))
                    {
                        folderType = folderItem.Type;
                    }
                }
                else if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.ChangeStateStartup(taskSchedulerItem.Name, taskSchedulerItem.Path, taskSchedulerItem.Type))
                    {
                        taskType = taskSchedulerItem.Type;
                    }
                }
                else
                {
                    return;
                }
            }

            if (registryType.HasValue)
            {
                RefreshStartupItems(registryType.Value);
            }
            if (folderType.HasValue)
            {
                RefreshStartupItems(folderType.Value);
            }
            if (taskType.HasValue)
            {
                RefreshStartupItems(taskType.Value);
            }
        }

        public void OnCopyClipboardClicked(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0 || items.Count > 1) { return; }

            foreach (object item in items)
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
                    _model.CopyToClipboard(taskSchedulerItem.Path);
                }
                else
                {
                    return;
                }
            }
        }

        private void OnOpenExplorerClicked(object s, EventArgs e)
        {
            _view.GetSelectedItems();
            _model.OpenPathToExplorer(_view.SelectedPath.isFile, _view.SelectedPath.path, _view.LastFolderSelectionSource);
        }
    }
}