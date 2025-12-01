using Computer_Maintenance.Model.Enums;

namespace Computer_Maintenance.Model.DTO
{
    public class SettingsData
    {
        public string BackgroundColor { get; set; } = ColorTranslator.ToHtml(Color.White); //Цвет фона, по умолчанию белый
        public string TextColor { get; set; } = ColorTranslator.ToHtml(Color.Black); //Цвет текста, по умолчанию черный
        public ThemeType SelectedTheme { get; set; } = ThemeType.Light; //Выбранная тема по умолчанию белая
    }
}
