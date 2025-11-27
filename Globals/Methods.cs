namespace Computer_Maintenance.Globals
{
    public static class Methods
    {
        public static void RefreshTheme(Control control)
        {
            control.BackColor = GlobalSettings.BackgroundColor;
            control.ForeColor = GlobalSettings.TextColor;

            foreach (Control child in control.Controls)
            {
                RefreshTheme(child);
            }

            control.Invalidate();
        }

    }
}
