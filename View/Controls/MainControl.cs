using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class MainControl : UserControl, IMainView
    {
        public event EventHandler HomeClicked;
        public event EventHandler SettingsClicked;

        private Size pictureBoxHomeSize, pictureBoxSettingSize;
        private Point pictureBoxHomePoint, pictureBoxSettingsPoint;
        public MainControl()
        {
            InitializeComponent();
        }
        private void MainControl_Load(object sender, EventArgs e)
        {
        }

        private void pictureBoxHome_Click(object sender, EventArgs e)
        {
            HomeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            SettingsClicked?.Invoke(this, EventArgs.Empty);
        }

        //Метод установки контрола в таблицу
        public void SetControlToTable(UserControl control)
        {
            control.Dock = DockStyle.Fill;
            ClearCell(tableLayoutPanelContolsPositions, 0, 0);
            tableLayoutPanelContolsPositions.Controls.Add(control, 0, 0);
            
        }


        //Метод вызываемый из presenter для обновления темы
        private void ClearCell(TableLayoutPanel panel, int column, int row)
        {
            Control control = panel.GetControlFromPosition(column, row);

            if (control != null)
            {
                panel.Controls.Remove(control);
            }
        }

        private void pictureBoxSettings_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSettingSize = pictureBoxSettings.Size;
            pictureBoxSettingsPoint = pictureBoxSettings.Location;

            pictureBoxSettings.Size = new Size(pictureBoxSettingSize.Width - 10, pictureBoxSettingSize.Height - 10);
            pictureBoxSettings.Location = new Point(pictureBoxSettingsPoint.X + 5, pictureBoxSettingsPoint.Y + 5);

            pictureBoxSettings.Cursor = Cursors.Hand;
        }

        private void pictureBoxSettings_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSettings.Size = new Size(pictureBoxSettingSize.Width, pictureBoxSettingSize.Height);
            pictureBoxSettings.Location = new Point(pictureBoxSettingsPoint.X, pictureBoxSettingsPoint.Y);

            pictureBoxSettings.Cursor = Cursors.Default;
        }

        private void pictureBoxHome_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxHomeSize = pictureBoxHome.Size;
            pictureBoxHomePoint = pictureBoxHome.Location;

            pictureBoxHome.Size = new Size(pictureBoxHomeSize.Width - 10, pictureBoxHomeSize.Height - 10);
            pictureBoxHome.Location = new Point(pictureBoxHomePoint.X + 5, pictureBoxHomePoint.Y + 5);

            pictureBoxHome.Cursor = Cursors.Hand;
        }

        private void pictureBoxHome_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxHome.Size = new Size(pictureBoxHomeSize.Width , pictureBoxHomeSize.Height);
            pictureBoxHome.Location = new Point(pictureBoxHomePoint.X, pictureBoxHomePoint.Y);

            pictureBoxHome.Cursor = Cursors.Default;
        }
    }
}
