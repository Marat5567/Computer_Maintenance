using Computer_Maintenance.Views;

namespace Computer_Maintenance.Controls
{
    public partial class MainControl : UserControl, IMainControlView
    {
        public event EventHandler ThemeChanged;
        public MainControl()
        {
            InitializeComponent();
        }

        private void MainControl_Load(object sender, EventArgs e)
        {
        }

        //Метод установки контрола настроек
        public void SetSettingsControl(UserControl control) 
        {
            tableLayoutPanelContolsPositions.Controls.Add(control, 0, 0);

            if (control is SettingsControl settings)
            {
                settings.ThemeChanged += (s, e) => ThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //Метод вызываемый из presenter для обновления темы
        public void RefreshTheme(Color background, Color text)
        {
            ApplyColorsToControls(this, background, text);
        }

        //Метод обновления темы всех контролоов находящихся внутри MainControl 
        private void ApplyColorsToControls(Control parent, Color background, Color text)
        {
            foreach (Control control in parent.Controls)
            {
                control.BackColor = background;
                control.ForeColor = text;

                if (control.HasChildren)
                {
                    ApplyColorsToControls(control, background, text);
                }
            }
        }

    }
}
