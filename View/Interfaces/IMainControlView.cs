namespace Computer_Maintenance.View.Interfaces
{
    public interface IMainControlView
    {
        event EventHandler HomeClicked; //Событие нажатия кнопки главной страницы
        event EventHandler SettingsClicked; //Событие нажатия кнопки настроек
        void SetControlToTable(UserControl control);

    }
}
