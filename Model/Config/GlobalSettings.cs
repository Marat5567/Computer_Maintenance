using Computer_Maintenance.Model.Enums;

namespace Computer_Maintenance.Model.Config
{
    public static class GlobalSettings
    {
        public static ThemeType CurrentTheme { get; set; }
        public static Color BackgroundColor { get; set; }
        public static Color TextColor { get; set; }
        public static string FontName { get; set; } = "Segoe UI";
        public static int FontSize { get; set; }
    }
}
