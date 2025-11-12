using Computer_Maintenance.DTOS;
using Computer_Maintenance.Enums;
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
            DialogResult dialogResult = Globals.Message.ShowMessage(owner: null, msg: "Перезапустить приложение для применения темы?", headerName: "Действие", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information);
            switch (dialogResult)
            {           
                case DialogResult.Yes:
                    Form form = Application.OpenForms[0];
                    if (form is MainForm mainForm)
                    {
                        mainForm.RestartApplication();
                    }
                    break;
            }

        }
        private void OnInitItemsState(object sender, EventArgs e)
        {
            SettingsDtoData settingsData = _settingsModel.LoadDataFromJson();

            if (settingsData != null)
            {
                _settingsView.SetRadioButtonTheme(settingsData.SelectedTheme);

                (Color backgroundColor, Color textColor) = GetThemeColors(_settingsView.ThemeTypeSelected);
                Globals.GlobalSettings.BackgroundColor = backgroundColor;
                Globals.GlobalSettings.TextColor = textColor;
            }
            else
            {
                _settingsView.SetRadioButtonTheme(ThemeType.Light);
            }

        }
        private void OnThemeTypeSelected(object sender, EventArgs e)
        {
            (Color backgroundColor, Color textColor) = GetThemeColors(_settingsView.ThemeTypeSelected);

            Globals.GlobalSettings.BackgroundColor = backgroundColor;
            Globals.GlobalSettings.TextColor = textColor;
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
