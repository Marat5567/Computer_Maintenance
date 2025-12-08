using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Presenters;
using Computer_Maintenance.View.Interfaces;
using System.Security.Principal;

namespace Computer_Maintenance
{
    public partial class MainForm : Form, IMainFormView
    {
        private MainFormModel _mainFormModel;
        private MainFormPresenter _mainFormPresenter;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

            switch (isAdmin)
            {
                case true:
                    this.Text = "Обслуживание ПК [Администратор]";
                    break;
                case false:
                    this.Text = "Обслуживание ПК [Ползователь]";
                    break;
            }

            _mainFormModel = new MainFormModel();
            _mainFormPresenter = new MainFormPresenter(this, _mainFormModel);
        }

        public void SetMainControl(UserControl control)
        {
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }
        public void RestartApplication()
        {
            //Application.Restart();
            //Environment.Exit(0);
        }
        public void ApplyTheme()
        {
            ThemeService.RefreshTheme(this);
        }

    }
}
