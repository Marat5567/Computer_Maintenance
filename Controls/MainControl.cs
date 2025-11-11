using Computer_Maintenance.Views;

namespace Computer_Maintenance.Controls
{
    public partial class MainControl : UserControl, IMainControlView
    {

        public event EventHandler ThemeChanged;  // событие, о котором узнает Presenter
        public MainControl()
        {
            InitializeComponent();
        }

        private void MainControl_Load(object sender, EventArgs e)
        {
        }

        public void SetSettingsControl(UserControl control)
        {
            tableLayoutPanelContolsPositions.Controls.Add(control, 0, 0);

            if (control is SettingsControl settings)
            {
                settings.ThemeChanged += (s, e) => ThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RefreshTheme(Color background, Color text)
        {
            ApplyColorsToControls(this, background, text);
        }
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
