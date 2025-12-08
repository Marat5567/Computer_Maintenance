using Computer_Maintenance.Model.Enums;

namespace Computer_Maintenance.View.Interfaces
{
    public interface ISettingsView
    {
        event EventHandler InitItemsState; //Событие инициализации состояния элеметов
        event EventHandler SaveSettingsClicked; //Событие нажатия кнопки сохранить
        event EventHandler ThemeChanged; //Событие изменения темы
        void SetRadioButtonTheme(ThemeType theme); //Метод установки радио кнопки темы
        ThemeType ThemeTypeSelected { get; set; } //Свойства для выбора темы
    }
}
