using Computer_Maintenance.DTOS;
using Computer_Maintenance.Enums;
using Computer_Maintenance.Globals;

namespace Computer_Maintenance.Models
{
    public class SettingsControlModel
    {
        private readonly string _fileName = "settings.json";
        public SettingsControlModel()
        {
        }
        public void SaveDataToJson(SettingsDtoData saveData)
        {
            if (saveData != null)
            {
                saveData.BackgroundColor = ColorTranslator.ToHtml(GlobalSettings.BackgroundColor);
                saveData.TextColor = ColorTranslator.ToHtml(GlobalSettings.TextColor);

                Core.Json.JsonService.Save(_fileName, saveData);
            }
        }
        public SettingsDtoData LoadDataFromJson()
        {
            SettingsDtoData data = Core.Json.JsonService.Load<SettingsDtoData>(_fileName);

            if (data != null)
            {
                return data;
            }
            return new SettingsDtoData();
        }
    }
}
