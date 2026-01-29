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
            _view.DeleteSelectedItem_Folder += OnDeleteSelectedItem_Folder;
            _view.ChangeSelectedItem_StateStartup_Registry += OnChangeSelectedItem_StateStartup_Registry;
            _view.ChangeSelectedItem_StateStartup_Folder += OnChangeSelectedItem_StateStartup_Folder;
            _view.OpenExplorerClicked += OnOpenExplorerClicked;
        }
        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadStartupItems(StartupType.All);

            StartupType typeRegistry = StartupType.RegistryCurrentUser | StartupType.RegistryLocalMachine;
            StartupType typeFolder = StartupType.StartupFolderCurrentUser | StartupType.StartupFolderAllUsers;

            _view.DisplayRegistryStartupItems(_model.GetRegistryStartupItems(typeRegistry), typeRegistry);
            _view.DisplayFolderStartupItems(_model.GetFolderStartupItems(typeFolder), typeFolder);

            _view.DisplayTaskSchedulerItems(_model.GetTaskSchedulerItems());
        }

        private void OnChangeSelectedItem_StateStartup_Registry(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            if (items[0] is StartupItemRegistry)
            {
                foreach (StartupItemRegistry item in items)
                {
                    bool changed = _model.ChangeStateStartup(item.RegistryName, item.Type, item.Is32Bit);

                    if (changed)
                    {
                        if (item.Type.HasFlag(StartupType.RegistryCurrentUser))
                        {
                            _model.LoadStartupItems(StartupType.RegistryCurrentUser);
                            _view.DisplayRegistryStartupItems(_model.GetRegistryStartupItems(StartupType.RegistryCurrentUser), StartupType.RegistryCurrentUser);
                        }
                        if (item.Type.HasFlag(StartupType.RegistryLocalMachine))
                        {
                            _model.LoadStartupItems(StartupType.RegistryLocalMachine);
                            _view.DisplayRegistryStartupItems(_model.GetRegistryStartupItems(StartupType.RegistryLocalMachine), StartupType.RegistryLocalMachine);
                        }
                    }
                }
            }
        }

        private void OnChangeSelectedItem_StateStartup_Folder(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            if (items[0] is StartupItemFolder)
            {
                foreach (StartupItemFolder item in items)
                {
                    bool changed = _model.ChangeStateStartup(item.NameExtracted, item.Type);

                    if (changed)
                    {
                        if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                        {
                            _model.LoadStartupItems(StartupType.StartupFolderCurrentUser);
                            _view.DisplayFolderStartupItems(_model.GetFolderStartupItems(StartupType.StartupFolderCurrentUser), StartupType.StartupFolderCurrentUser);
                        }
                        if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                        {
                            _model.LoadStartupItems(StartupType.StartupFolderAllUsers);
                            _view.DisplayFolderStartupItems(_model.GetFolderStartupItems(StartupType.StartupFolderAllUsers), StartupType.StartupFolderAllUsers);
                        }
                    }
                }
            }
        }

        private void OnDeleteSelectedItem_Folder(object s, EventArgs e)
        {
            List<object> items = _view.GetSelectedItems();
            if (items.Count == 0) { return; }

            if (items[0] is StartupItemFolder)
            {
                foreach (StartupItemFolder item in items)
                {
                    bool deleted = _model.DeleteFolderStartupRecord(item.PathExtracted);

                    if (deleted)
                    {
                        if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                        {
                            _model.LoadStartupItems(StartupType.StartupFolderCurrentUser);
                            _view.DisplayFolderStartupItems(_model.GetFolderStartupItems(StartupType.StartupFolderCurrentUser), StartupType.StartupFolderCurrentUser);
                        }

                        if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                        {
                            _model.LoadStartupItems(StartupType.StartupFolderAllUsers);
                            _view.DisplayFolderStartupItems(_model.GetFolderStartupItems(StartupType.StartupFolderAllUsers), StartupType.StartupFolderAllUsers);
                        }
                    }
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
