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
        }
        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadAllStartupItems();
           
            _view.DisplayRegistryStartupItems(_model.GetRegistryStartupItems());

            List<StartupItemFolder> items = _model.GetFolderStartupItems(StartupType.StartupFolderCurrentUser | StartupType.StartupFolderAllUsers);
            _view.DisplayFolderStartupItems(items, StartupType.StartupFolderCurrentUser | StartupType.StartupFolderAllUsers);


        }
        private void OnDeleteSelectedItem_Folder(object s, EventArgs e)
        {
            List<StartupItemFolder> items = _view.GetSelectedStartupItems_Folder();

            bool currentUserUpdated = false;
            bool allUsersUpdated = false;

            foreach (StartupItemFolder item in items)
            {
                bool deleted = _model.DeleteFolderStartupRecord(item.PathExtracted);

                if (deleted)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                    {
                        _model.FolderStartupItems_CurrentUser.Remove(item);
                        currentUserUpdated = true;
                    }

                    if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                    {
                        _model.FolderStartupItems_AllUsers.Remove(item);
                        allUsersUpdated = true;
                    }
                }
            }

            if (currentUserUpdated)
            {
                _view.DisplayFolderStartupItems(_model.FolderStartupItems_CurrentUser, StartupType.StartupFolderCurrentUser);
            }

            if (allUsersUpdated)
            {
                _view.DisplayFolderStartupItems(_model.FolderStartupItems_AllUsers, StartupType.StartupFolderAllUsers);
            }

        }


    }
}
