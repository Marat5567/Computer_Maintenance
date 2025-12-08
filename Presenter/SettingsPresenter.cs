using Computer_Maintenance.Core.Managers;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.DTO;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

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
            _settingsView.ThemeChanged += OnThemeTypeSelected;
        }

        private void OnPressedSave(object sender, EventArgs e)
        {
            SettingsData saveData = new SettingsData()
            {
                SelectedTheme = _settingsView.ThemeTypeSelected,
            };
            _settingsModel.SaveDataToJson(saveData);
        }
        private void OnInitItemsState(object sender, EventArgs e)
        {
            _settingsView.SetRadioButtonTheme(ApplicationSettings.CurrentTheme);

        }
        private void OnThemeTypeSelected(object sender, EventArgs e)
        {
            ApplicationSettings.CurrentTheme = _settingsView.ThemeTypeSelected;
            (ApplicationSettings.BackgroundColor, ApplicationSettings.TextColor) = GetThemeColors(ApplicationSettings.CurrentTheme);

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
