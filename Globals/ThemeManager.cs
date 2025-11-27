using Computer_Maintenance.Enums;

namespace Computer_Maintenance.Globals
{
    public static class ThemeManager
    {
        public static event Action ThemeChanged;

        private static ThemeType _currentTheme = ThemeType.Light;
        public static ThemeType CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                ThemeChanged?.Invoke();
            }
        }
        public static void SetTheme(ThemeType theme)
        {
            CurrentTheme = theme;
        }
    }
}
