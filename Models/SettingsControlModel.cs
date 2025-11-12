using Computer_Maintenance.DTOS;
using Computer_Maintenance.Enums;
using Computer_Maintenance.Globals;
using Computer_Maintenance.Jsons;
using System.Text.Json;

namespace Computer_Maintenance.Models
{
    public class SettingsControlModel
    {
        private readonly string _filePathJson;
        public SettingsControlModel()
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
            }
            return new SettingsDtoData();
        }
    }
}
