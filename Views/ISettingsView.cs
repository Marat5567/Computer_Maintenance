using Computer_Maintenance.Enums;

namespace Computer_Maintenance.Views
{
    public interface ISettingsView
    {
        event EventHandler InitItemsState; //Событие инициализации состояния элеметов
        event EventHandler SaveSettingsClicked; //Событие нажатия кнопки сохранить
        event EventHandler ThemeChanged; //Событие изменения темы
        ThemeType ThemeTypeSelected { get; set; } //Свойства для выбора темы

        void SetTheme(ThemeType theme); //Метод для установки цвета выбранной темы
    }
}
