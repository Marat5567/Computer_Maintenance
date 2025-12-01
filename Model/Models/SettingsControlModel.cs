using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.DTO;

namespace Computer_Maintenance.Model.Models
{
    public class SettingsControlModel
    {
        private readonly string _fileName = "settings.json";
        public SettingsControlModel()
        {
        }
        public void SaveDataToJson(SettingsData saveData)
        {
            if (saveData != null)
            {
                saveData.BackgroundColor = ColorTranslator.ToHtml(GlobalSettings.BackgroundColor);
                saveData.TextColor = ColorTranslator.ToHtml(GlobalSettings.TextColor);

                JsonService.Save(_fileName, saveData);
            }
        }
        public SettingsData LoadDataFromJson()
        {
            SettingsData data = JsonService.Load<SettingsData>(_fileName);

            if (data != null)
            {
                return data;
            }
            return new SettingsData();
        }
    }
}
