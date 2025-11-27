using Computer_Maintenance.Enums;
using Computer_Maintenance.Globals;
using Computer_Maintenance.Models;
using Computer_Maintenance.Presenters;
using Computer_Maintenance.Views;
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
                    Access.CurrentAccess = UserAccess.Administrator;
                    this.Text = "Обслуживание ПК [Администратор]";
                    break;
                case false:
                    Access.CurrentAccess = UserAccess.User;
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
            Globals.Methods.RefreshTheme(this);
        }

    }
}
