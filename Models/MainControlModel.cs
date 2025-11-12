using Computer_Maintenance.Globals;

namespace Computer_Maintenance.Models
{
    public class MainControlModel
    {
        public Color BackgroundColor { get; set; } = GlobalSettings.BackgroundColor;
        public Color TextColor { get; set; } = GlobalSettings.TextColor;

    }
}
