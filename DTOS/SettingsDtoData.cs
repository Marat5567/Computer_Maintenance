using Computer_Maintenance.Enums;

namespace Computer_Maintenance.DTOS
{
    public class SettingsDtoData
    {
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public ThemeType SelectedTheme { get; set; }
    }
}
