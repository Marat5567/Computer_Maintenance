using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class SettingsControl : UserControl, ISettingsControlView
    {
        public event EventHandler InitItemsState; //Событие инициализации элеметов
        public event EventHandler SaveSettingsClicked; //Событие нажатия кнопки сохранить
        public event EventHandler ThemeChanged; //Событие изменения темы

        private ThemeType _themeTypeSelected;
        public ThemeType ThemeTypeSelected //Свойство для выбора темы
        {
            get => _themeTypeSelected;
            set
            {
                _themeTypeSelected = value;
            }
        }
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            InitItemsState?.Invoke(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettingsClicked?.Invoke(this, EventArgs.Empty);
        }
        
        private void radioButtonThemeLight_CheckedChanged(object sender, EventArgs e)
        {
            ThemeTypeSelected = ThemeType.Light;
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void radioButtonThemeDark_CheckedChanged(object sender, EventArgs e)
        {
            ThemeTypeSelected = ThemeType.Dark;
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
        public void SetRadioButtonTheme(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.Light:
                    radioButtonThemeLight.Checked = true;
                    break;
                case ThemeType.Dark:
                    radioButtonThemeDark.Checked = true;
                    break;
            }
        }

    }
}
