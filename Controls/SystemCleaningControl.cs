using Computer_Maintenance.Views;

namespace Computer_Maintenance.Controls
{
    public partial class SystemCleaningControl : UserControl, ISystemCleaningControlView
    {
        public SystemCleaningControl()
        {
            InitializeComponent();
        }

        private void SystemCleaningControl_Load(object sender, EventArgs e)
        {
            InitColors(this);
        }
        private void InitColors(UserControl userControl)
        {
            this.BackColor = Globals.GlobalSettings.BackgroundColor;
            foreach (Control control in userControl.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = Globals.GlobalSettings.TextColor;
                }
            }
        }
    }
}
