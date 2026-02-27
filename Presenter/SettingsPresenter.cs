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
            // Формируем цвета согласно выбранной теме и сохраняем в файл.
            var (bg, text) = GetThemeColors(_settingsView.ThemeTypeSelected);

            SettingsData saveData = new SettingsData()
            {
                SelectedTheme = _settingsView.ThemeTypeSelected,
                BackgroundColor = ColorTranslator.ToHtml(bg),
                TextColor = ColorTranslator.ToHtml(text)
            };

            _settingsModel.SaveDataToJson(saveData);

            var result = MessageBox.Show(
                "Изменения темы будут применены после перезапуска приложения.\nПерезапустить сейчас?",
                "Перезапуск приложения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Application.Restart();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
        }

        private void OnInitItemsState(object sender, EventArgs e)
        {
            _settingsView.SetRadioButtonTheme(ApplicationSettings.CurrentTheme);
        }

        private void OnThemeTypeSelected(object sender, EventArgs e)
        {
            // Только локальная установка выбора — не применяем тему в рантайме.
            // Сохранение и перезапуск управляют окончательным применением.
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
