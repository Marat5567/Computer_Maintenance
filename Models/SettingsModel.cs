using Computer_Maintenance.DTOS;
using Computer_Maintenance.Enums;
using Computer_Maintenance.Globals;
using Computer_Maintenance.Jsons;
using System.Text.Json;

namespace Computer_Maintenance.Models
{
    public class SettingsModel
    {
        private readonly string _filePathJson;
        public SettingsModel()
        {
            _filePathJson = Path.Combine(BaseDirectoryPath.BaseDirectory, "settings.json"); 
        }
        public void SaveDataToJson(SettingsDtoData saveData)
        {
            saveData.BackgroundColor = ColorTranslator.ToHtml(GlobalSettings.BackgroundColor);
            saveData.TextColor = ColorTranslator.ToHtml(GlobalSettings.TextColor);

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePathJson, json);
        }
        public SettingsDtoData LoadDataFromJson()
        {
            if (File.Exists(_filePathJson))
            {
                string json = File.ReadAllText(_filePathJson);
                SettingsDtoData data = JsonSerializer.Deserialize<SettingsDtoData>(json);
                
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return new SettingsDtoData();
                }
            }
            return new SettingsDtoData();
        }
        public void ApplyTheme(ThemeType theme)
        {
            (Color background, Color text) = GetColorsByTheme(theme);
            GlobalSettings.BackgroundColor = background;
            GlobalSettings.TextColor = text;
        }
        private (Color background, Color text) GetColorsByTheme(ThemeType theme)
        {
            switch (theme)
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
