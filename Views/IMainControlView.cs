namespace Computer_Maintenance.Views
{
    public interface IMainControlView
    {
        event EventHandler ThemeChanged; //Событие изменения темы из settings
        void SetSettingsControl(UserControl control);
        void RefreshTheme(Color background, Color text); //Метод применения темы

    }
}
