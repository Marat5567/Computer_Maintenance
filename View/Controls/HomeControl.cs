using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class HomeControl : UserControl, IHomeView
    {
        public event EventHandler<TreeViewEventArgs> FunctionalityClicked; //Событие на выбранный функционал
        public HomeControl()
        {
            InitializeComponent();
        }

        private void HomeControl_Load(object sender, EventArgs e)
        {
            InitElements();
            ThemeService.RefreshTheme(this);
        }

        private void InitElements()
        {
            splitContainer.FixedPanel = FixedPanel.Panel1;
            splitContainer.SplitterDistance = 250;

            treeViewListFunctionality.BeginUpdate();
            TreeNode cleaningNode = new TreeNode("Очистка системы");
            TreeNode startupNode = new TreeNode("Управление автозагрузкой");

            cleaningNode.Tag = FunctionalityType.SystemCleaning;
            startupNode.Tag = FunctionalityType.StartupManagement;

            treeViewListFunctionality.Nodes.Add(cleaningNode);
            treeViewListFunctionality.Nodes.Add(startupNode);

            treeViewListFunctionality.EndUpdate();
        }
        private void treeViewListFunctionality_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FunctionalityClicked?.Invoke(this, e);
        }
        public void SetFunctionalityControl(UserControl functionalityControl)
        {
            functionalityControl.Dock = DockStyle.Fill;

            splitContainer.Panel2.Controls.Clear();
            splitContainer.Panel2.Controls.Add(functionalityControl);
        }
    }
}
