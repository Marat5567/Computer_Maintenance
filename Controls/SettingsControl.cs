using Computer_Maintenance.Enums;
using Computer_Maintenance.Views;

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
            InitColors(this);
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
        private void InitColors(UserControl userControl)
        {
            foreach (Control control in userControl.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = Globals.GlobalSettings.TextColor;
                }
            }

            this.BackColor = Globals.GlobalSettings.BackgroundColor;
            this.groupBoxThemeButtons.BackColor = Globals.GlobalSettings.BackgroundColor;
            this.groupBoxThemeButtons.ForeColor = Globals.GlobalSettings.TextColor;
            this.buttonSave.BackColor = Globals.GlobalSettings.BackgroundColor;
            this.buttonSave.ForeColor = Globals.GlobalSettings.TextColor;
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
