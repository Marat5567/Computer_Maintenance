using Computer_Maintenance.Controls;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class MainControlPresenter
    {
        private readonly IMainControlView _mainControlView;
        private readonly MainControlModel _mainControlModel;
        private readonly SettingsControlModel _settingsControlModel;

        private SettingsControl _settingsControl;
        private SettingsControlPresenter _settingsPresenter;

        private HomeControl _homeControl;
        private HomeControlPresenter _homePresenter;
        private HomeControlModel _homeModel;

        public MainControlPresenter(IMainControlView mainControlView, MainControlModel mainControlModel, SettingsControlModel settingsControlModel)
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
            _settingsPresenter = new SettingsControlPresenter(_settingsControl, _settingsControlModel);
        }
        private void InitializeHomeControl()
        {
            _homeControl = new HomeControl();
            _homeModel = new HomeControlModel();
            _homePresenter = new HomeControlPresenter(_homeControl, _homeModel);
        }
    }
}
