using Computer_Maintenance.Model.Config;

namespace Computer_Maintenance.Core.Services
{
    public static class ThemeService
    {
        public static void RefreshTheme(Control control)
        {
            control.BackColor = ApplicationSettings.BackgroundColor;
            control.ForeColor = ApplicationSettings.TextColor;

            foreach (Control child in control.Controls)
            {
                RefreshTheme(child);
            }

            control.Invalidate();
        }

    }
}
