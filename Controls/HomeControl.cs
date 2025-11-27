using Computer_Maintenance.Enums;
using Computer_Maintenance.Views;
namespace Computer_Maintenance.Controls
{
    public partial class HomeControl : UserControl, IHomeControlView
    {
        public event EventHandler<TreeViewEventArgs> FunctionalityClicked; //Событие на выбранный функционал
        public HomeControl()
        {
            InitializeComponent();
        }

        private void HomeControl_Load(object sender, EventArgs e)
        {
            InitElements();
            Globals.Methods.RefreshTheme(this);
        }

        private void InitElements()
        {
            splitContainer.FixedPanel = FixedPanel.Panel1;
            splitContainer.SplitterDistance = 250;

            treeViewListFunctionality.BeginUpdate();
            TreeNode cleaningNode = new TreeNode("Очистка системы");
            cleaningNode.Tag = FunctionalityType.SystemCleaning;


            treeViewListFunctionality.Nodes.Add(cleaningNode);
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
