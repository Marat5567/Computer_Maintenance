using Computer_Maintenance.Model.Config;

namespace Computer_Maintenance.Model.Models
{
    public class MainControlModel
    {
        public Color BackgroundColor { get; set; } = GlobalSettings.BackgroundColor;
        public Color TextColor { get; set; } = GlobalSettings.TextColor;

    }
}
