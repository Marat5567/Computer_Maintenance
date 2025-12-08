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
        private MainPresenter _mainControlPresenter;
        private MainModel _mainControlModel;

        private SettingsModel _settingsControlModel;

        public MainFormPresenter(IMainFormView mainFormView, MainFormModel mainFormModel)
        {
            _mainFormModel = mainFormModel;
            _mainFormView = mainFormView;

            InitializeMainControl();
        }
        private void InitializeMainControl()
        {
            _settingsControlModel = new SettingsModel();
            SettingsData settingsDtoData = _settingsControlModel.LoadDataFromJson();

            ApplicationSettings.BackgroundColor = ColorTranslator.FromHtml(settingsDtoData.BackgroundColor);
            ApplicationSettings.TextColor = ColorTranslator.FromHtml(settingsDtoData.TextColor);
            ApplicationSettings.CurrentTheme = settingsDtoData.SelectedTheme;

            _mainControl = new MainControl();
            _mainControlModel = new MainModel();

            _mainControlPresenter = new MainPresenter(_mainControl, _mainControlModel, _settingsControlModel);

            _mainFormView.SetMainControl(_mainControl);
            _mainFormView.ApplyTheme();
        }
    }
}
