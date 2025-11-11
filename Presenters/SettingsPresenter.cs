using Computer_Maintenance.DTOS;
using Computer_Maintenance.Models;
using Computer_Maintenance.Views;

namespace Computer_Maintenance.Presenters
{
    public class SettingsPresenter
    {
        private readonly ISettingsView _settingsView;
        private readonly SettingsModel _settingsModel;

        public SettingsPresenter(ISettingsView settingsView, SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
            _settingsView = settingsView;

            _settingsView.SaveSettingsClicked += OnPressedSave;
            _settingsView.InitItemsState += OnInitItemsState;
            _settingsView.ThemeChanged += OnThemeChanged;
        }

        private void OnPressedSave(object sender, EventArgs e)
        {
            SettingsDtoData saveData = new SettingsDtoData()
            {
                SelectedTheme = _settingsView.ThemeTypeSelected,
            };
            _settingsModel.SaveDataToJson(saveData);
        }
        private void OnInitItemsState(object sender, EventArgs e)
        {
            SettingsDtoData settingsData = _settingsModel.LoadDataFromJson();

            _settingsView.SetTheme(settingsData.SelectedTheme);
            OnThemeChanged(this, EventArgs.Empty);
        }
        private void OnThemeChanged(object sender, EventArgs e)
        {
            _settingsModel.ApplyTheme(_settingsView.ThemeTypeSelected);
        }
    }
}
