using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.Model.Enums.StartupManagement;

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
        }
        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadStartupItems(StartupType.All);

            List<StartupItemRegistry> registryItems = _model.GetRegistryStartupItems(StartupType.RegistryCurrentUser | StartupType.RegistryLocalMachine);
            _view.DisplayRegistryStartupItems(registryItems, StartupType.RegistryCurrentUser | StartupType.RegistryLocalMachine);

            List<StartupItemFolder> folderItems = _model.GetFolderStartupItems(StartupType.StartupFolderCurrentUser | StartupType.StartupFolderAllUsers);
            _view.DisplayFolderStartupItems(folderItems, StartupType.StartupFolderCurrentUser | StartupType.StartupFolderAllUsers);


        }
        private void OnChangeSelectedItem_StateStartup_Registry(object s, EventArgs e)
        {
            List<StartupItemRegistry> items = _view.GetSelectedStartupItems_Registry();

            foreach (StartupItemRegistry item in items)
            {
                bool changed = _model.ChangeStateStartup(item.RegistryName, item.Type, item.Is32Bit);

                if (changed)
                {
                    if (item.Type.HasFlag(StartupType.RegistryCurrentUser))
                    {
                        _model.LoadStartupItems(StartupType.RegistryCurrentUser);
                        _view.DisplayRegistryStartupItems(_model.RegistryStartupItems_CurrentUser, StartupType.RegistryCurrentUser);
                    }
                    if (item.Type.HasFlag(StartupType.RegistryLocalMachine))
                    {
                        _model.LoadStartupItems(StartupType.RegistryLocalMachine);
                        _view.DisplayRegistryStartupItems(_model.RegistryStartupItems_AllUsers, StartupType.RegistryLocalMachine);
                    }
                }
            }

        }

        private void OnDeleteSelectedItem_Folder(object s, EventArgs e)
        {
            List<StartupItemFolder> items = _view.GetSelectedStartupItems_Folder();

            foreach (StartupItemFolder item in items)
            {
                bool deleted = _model.DeleteFolderStartupRecord(item.PathExtracted);

                if (deleted)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                    {
                        _model.LoadStartupItems(StartupType.StartupFolderCurrentUser);
                        _view.DisplayFolderStartupItems(_model.FolderStartupItems_CurrentUser, StartupType.StartupFolderCurrentUser);
                    }

                    if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                    {
                        _model.LoadStartupItems(StartupType.StartupFolderAllUsers);
                        _view.DisplayFolderStartupItems(_model.FolderStartupItems_AllUsers, StartupType.StartupFolderAllUsers);
                    }
                }
            }

        }


    }
}
