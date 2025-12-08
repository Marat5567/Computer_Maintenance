using Computer_Maintenance.Controls;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView _mainControlView;
        private readonly MainModel _mainControlModel;
        private readonly SettingsModel _settingsControlModel;

        private SettingsControl _settingsControl;
        private SettingsPresenter _settingsPresenter;

        private HomeControl _homeControl;
        private HomePresenter _homePresenter;
        private HomeModel _homeModel;

        public MainPresenter(IMainView mainControlView, MainModel mainControlModel, SettingsModel settingsControlModel)
        {
            _mainControlView = mainControlView;
            _mainControlModel = mainControlModel;
            _settingsControlModel = settingsControlModel;

            _mainControlView.HomeClicked += OnHomeClicked;
            _mainControlView.SettingsClicked += OnSettingsClicked;

            InitializeSettingsControl();
            InitializeHomeControl();

            _mainControlView.SetControlToTable(_homeControl); //Устанавливаем контрол главной страницы по умолчанию
        }
        public void OnHomeClicked(object sender, EventArgs e)
        {
            _mainControlView.SetControlToTable(_homeControl);
        }
        public void OnSettingsClicked(object sender, EventArgs e)
        {
            _mainControlView.SetControlToTable(_settingsControl);
        }   
        private void InitializeSettingsControl()
        {
            _settingsControl = new SettingsControl();
            _settingsPresenter = new SettingsPresenter(_settingsControl, _settingsControlModel);
        }
        private void InitializeHomeControl()
        {
            _homeControl = new HomeControl();
            _homeModel = new HomeModel();
            _homePresenter = new HomePresenter(_homeControl, _homeModel);
        }
    }
}
