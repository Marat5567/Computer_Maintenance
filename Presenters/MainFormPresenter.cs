using Computer_Maintenance.Controls;
using Computer_Maintenance.DTOS;
using Computer_Maintenance.Models;
using Computer_Maintenance.Views;

namespace Computer_Maintenance.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainFormView _mainFormView;
        private readonly MainFormModel _mainFormModel;

        private MainControl _mainControl;
        private MainControlPresenter _mainControlPresenter;
        private MainControlModel _mainControlModel;

        private SettingsControlModel _settingsControlModel;

        public MainFormPresenter(IMainFormView mainFormView, MainFormModel mainFormModel)
        {
            _mainFormModel = mainFormModel;
            _mainFormView = mainFormView;

            InitializeMainControl();
        }
        private void InitializeMainControl()
        {
            _settingsControlModel = new SettingsControlModel();
            SettingsDtoData settingsDtoData = _settingsControlModel.LoadDataFromJson();

            Globals.GlobalSettings.BackgroundColor = ColorTranslator.FromHtml(settingsDtoData.BackgroundColor);
            Globals.GlobalSettings.TextColor = ColorTranslator.FromHtml(settingsDtoData.TextColor);

            _mainControl = new MainControl();
            _mainControlModel = new MainControlModel();

            _mainControlPresenter = new MainControlPresenter(_mainControl, _mainControlModel, _settingsControlModel);

            _mainFormView.SetMainControl(_mainControl);
        }
    }
}
