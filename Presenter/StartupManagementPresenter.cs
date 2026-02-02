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
        public StartupManagementPresenter(IStartupManagementView view, StartupManagementModel model)
        {
            _view = view;
            _model = model;

            _view.LoadControl += OnLoadControl;
            _view.OpenExplorerClicked += OnOpenExplorerClicked;
            _view.ChangeStateSelectedItems += OnChangeStateSelectedItems;
            _view.CopyClipboardClicked += OnCopyClipboardClicked;
        }
        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadStartupItems(StartupType.All);

            _view.DisplayItems(_model.GetStartupItems(StartupType.RegistryCurrentUser), StartupType.RegistryCurrentUser);
            _view.DisplayItems(_model.GetStartupItems(StartupType.RegistryLocalMachine), StartupType.RegistryLocalMachine);
            _view.DisplayItems(_model.GetStartupItems(StartupType.StartupFolderCurrentUser), StartupType.StartupFolderCurrentUser);
            _view.DisplayItems(_model.GetStartupItems(StartupType.StartupFolderAllUsers), StartupType.StartupFolderAllUsers);
            _view.DisplayItems(_model.GetStartupItems(StartupType.TaskScheduler), StartupType.TaskScheduler);
        }

        private void OnChangeStateSelectedItems(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            foreach (object item in items)
            {
                if (item is StartupItemRegistry registryItem)
                {
                    if (_model.ChangeStateStartup(registryItem.RegistryName, registryItem.Type, registryItem.Is32Bit))
                    {
                        _model.LoadStartupItems(registryItem.Type);
                        _view.DisplayItems(_model.GetStartupItems(registryItem.Type), registryItem.Type);
                    }
                }
                else if (item is StartupItemFolder folderItem)
                {
                    if (_model.ChangeStateStartup(folderItem.NameExtracted, folderItem.Type))
                    {
                        _model.LoadStartupItems(folderItem.Type);
                        _view.DisplayItems(_model.GetStartupItems(folderItem.Type), folderItem.Type);
                    }
                }
                else if (item is TaskSchedulerItem taskSchedulerItem)
                {
                    if (_model.ChangeStateStartup(taskSchedulerItem.Name, taskSchedulerItem.Type))
                    {
                        _model.LoadStartupItems(taskSchedulerItem.Type);
                        _view.DisplayItems(_model.GetStartupItems(taskSchedulerItem.Type), taskSchedulerItem.Type);
                    }
                }
                else
                {
                    return;
                }
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
