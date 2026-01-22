using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;
using Computer_Maintenance.Model.Structs.StartupManagement;

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
            _view.DeleteSelectedItem += OnDeleteSelectedItem;
        }
        private void OnLoadControl(object s, EventArgs e)
        {
            _model.LoadAllStartupItems();
           
            _view.DisplayStartupItems(_model.GetStartupItems());

        }
        private void OnDeleteSelectedItem(object s, EventArgs e)
        {
            List<StartupItem> items = _view.GetSelectedStartupItems();


            foreach (StartupItem item in items)
            {
                bool deleted = _model.DeleteRegistryRecord(item.Type, item.OriginalRegistryName);
                //if (deleted)
                //{
                //    MessageBox.Show($"{item.Name} Успешно удален из автозагрузки");
                //}
                //else 
                //{
                //    _model.ShowInfo("")
                //    MessageBox.Show($"{item.Name} Ну удалось удалить из автозагрузки");
                //}
            }
        }

    }
}
