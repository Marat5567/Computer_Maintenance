using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.DTO;

namespace Computer_Maintenance.Model.Models
{
    public class SettingsModel
    {
        private readonly string _fileName = "settings.json";
        public SettingsModel()
        {
        }

        public void SaveDataToJson(SettingsData saveData)
        {
            if (saveData != null)
            {
                if (string.IsNullOrWhiteSpace(saveData.BackgroundColor))
                    saveData.BackgroundColor = ColorTranslator.ToHtml(ApplicationSettings.BackgroundColor);
                if (string.IsNullOrWhiteSpace(saveData.TextColor))
                    saveData.TextColor = ColorTranslator.ToHtml(ApplicationSettings.TextColor);

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
