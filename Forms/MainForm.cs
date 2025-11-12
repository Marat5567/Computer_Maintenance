using Computer_Maintenance.Controls;
using Computer_Maintenance.Models;
using Computer_Maintenance.Presenters;
using Computer_Maintenance.Views;

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
            _mainFormModel = new MainFormModel();
            _mainFormPresenter = new MainFormPresenter(this, _mainFormModel);
        }
        public void SetMainControl(UserControl control)
        {
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }
    }
}
