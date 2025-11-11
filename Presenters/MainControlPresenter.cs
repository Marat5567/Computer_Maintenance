using Computer_Maintenance.Views;
using Computer_Maintenance.Models;
using Computer_Maintenance.Controls;

namespace Computer_Maintenance.Presenters
{
    public class MainControlPresenter
    {
        private readonly IMainControlView _mainControlView;
        private readonly MainControlModel _mainControlModel;

        private SettingsControl _settingsControl;
        private SettingsPresenter _settingsPresenter;
        private SettingsModel _settingsModel;

        public MainControlPresenter(IMainControlView mainControlView, MainControlModel mainControlModel)
        {
            _mainControlView = mainControlView;
            _mainControlModel = mainControlModel;

            _mainControlView.ThemeChanged += OnThemeChanged;

            InitializeSettingsControl();

            _mainControlView.RefreshTheme(_mainControlModel.BackgroundColor, _mainControlModel.TextColor);

        }
        private void InitializeSettingsControl()
        {
            _settingsControl = new SettingsControl();
            _settingsModel = new SettingsModel();
            _settingsPresenter = new SettingsPresenter(_settingsControl, _settingsModel);

            _mainControlView.SetSettingsControl(_settingsControl);
        }
        private void OnThemeChanged(object sender, EventArgs e)
        {
            _mainControlModel.BackgroundColor = Globals.GlobalSettings.BackgroundColor;
            _mainControlModel.TextColor = Globals.GlobalSettings.TextColor;

            _mainControlView.RefreshTheme(_mainControlModel.BackgroundColor, _mainControlModel.TextColor);
        }
    }
}
