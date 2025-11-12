using Computer_Maintenance.Enums;

namespace Computer_Maintenance.DTOS
{
    public class SettingsDtoData
    {
        public string BackgroundColor { get; set; } = ColorTranslator.ToHtml(Color.White); //Цвет фона, по умолчанию белый
        public string TextColor { get; set; } = ColorTranslator.ToHtml(Color.Black); //Цвет текста, по умолчанию черный
        public ThemeType SelectedTheme { get; set; } = ThemeType.Light; //Выбранная тема по умолчанию белая
    }
}
