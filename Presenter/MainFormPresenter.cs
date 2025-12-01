using Computer_Maintenance.Controls;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.DTO;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

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
            SettingsData settingsDtoData = _settingsControlModel.LoadDataFromJson();

            GlobalSettings.BackgroundColor = ColorTranslator.FromHtml(settingsDtoData.BackgroundColor);
            GlobalSettings.TextColor = ColorTranslator.FromHtml(settingsDtoData.TextColor);
            GlobalSettings.CurrentTheme = settingsDtoData.SelectedTheme;

            _mainControl = new MainControl();
            _mainControlModel = new MainControlModel();

            _mainControlPresenter = new MainControlPresenter(_mainControl, _mainControlModel, _settingsControlModel);

            _mainFormView.SetMainControl(_mainControl);
            _mainFormView.ApplyTheme();
        }
    }
}
