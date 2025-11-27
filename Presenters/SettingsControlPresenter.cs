using Computer_Maintenance.DTOS;
using Computer_Maintenance.Enums;
using Computer_Maintenance.Globals;
using Computer_Maintenance.Models;
using Computer_Maintenance.Views;

namespace Computer_Maintenance.Presenters
{
    public class SettingsControlPresenter
    {
        private readonly ISettingsControlView _settingsView;
        private readonly SettingsControlModel _settingsModel;

        public SettingsControlPresenter(ISettingsControlView settingsView, SettingsControlModel settingsModel)
        {
            _settingsModel = settingsModel;
            _settingsView = settingsView;

            _settingsView.SaveSettingsClicked += OnPressedSave;
            _settingsView.InitItemsState += OnInitItemsState;
            _settingsView.ThemeChanged += OnThemeTypeSelected;
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
            _settingsView.SetRadioButtonTheme(Globals.GlobalSettings.CurrentTheme);

        }
        private void OnThemeTypeSelected(object sender, EventArgs e)
        {
            Globals.GlobalSettings.CurrentTheme = _settingsView.ThemeTypeSelected;
            (GlobalSettings.BackgroundColor, GlobalSettings.TextColor) = GetThemeColors(GlobalSettings.CurrentTheme);

            ThemeManager.SetTheme(_settingsView.ThemeTypeSelected);
        }

        private (Color background, Color text) GetThemeColors(ThemeType themeType)
        {
            switch (themeType)
            {
                case ThemeType.Light:
                    return (Color.White, Color.Black);
                case ThemeType.Dark:
                    return (Color.Black, Color.White);
                default:
                    return (Color.White, Color.Black);
            }
        }
    }
}
