using Computer_Maintenance.Enums;

namespace Computer_Maintenance.Views
{
    public interface ISettingsControlView
    {
        event EventHandler InitItemsState; //Событие инициализации состояния элеметов
        event EventHandler SaveSettingsClicked; //Событие нажатия кнопки сохранить
        event EventHandler ThemeChanged; //Событие изменения темы
        void SetRadioButtonTheme(ThemeType theme); //Метод установки радио кнопки темы
        ThemeType ThemeTypeSelected { get; set; } //Свойства для выбора темы
    }
}
