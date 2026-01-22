using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.View.Controls
{
    public partial class StartupManagementControl : UserControl, IStartupManagementView
    {
        private ContextMenuStrip contextMenuStrip;

        public event EventHandler LoadControl;
        public event EventHandler MouseClicked_ListView;

        public event EventHandler DeleteSelectedItem;
        public event EventHandler ChangeSelectedItem;
        public void DisplayStartupItems(List<StartupItem> startupItems)
        {
            listViewRegistryCurrentUser.Items.Clear();

            foreach (StartupItem item in startupItems)
            {
                switch (item.Type)
                {
                    case StartupType.RegistryCurrentUser:

                        ListViewItem viewItem = new ListViewItem(item.Name);
                        viewItem.SubItems.Add(item.Path);
                        viewItem.Tag = item;

                        listViewRegistryCurrentUser.Items.Add(viewItem);
                        break;

                }
            }
        }
        public StartupManagementControl()
        {
            InitializeComponent();
        }
        private void StartupManagementControl_Load(object sender, EventArgs e)
        {
            Init_ListViewRegistryCurrentUser();
            InitContextMenu();
            LoadControl?.Invoke(this, EventArgs.Empty);
        }
        private void listViewRegistryCurrentUser_MouseClick(object sender, MouseEventArgs e)
        {
        }
        public List<StartupItem> GetSelectedStartupItems()
        {
            List<StartupItem> selectedItems = new List<StartupItem>();

            foreach (ListViewItem listViewItem in listViewRegistryCurrentUser.SelectedItems)
            {
                if (listViewItem.Tag is StartupItem startupItem)
                {
                    selectedItems.Add(startupItem);
                }
            }

            return selectedItems;
        }

        private void Init_ListViewRegistryCurrentUser()
        {
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.Columns.Add("Имя", 200);
            listViewRegistryCurrentUser.Columns.Add("Путь", 600);
            listViewRegistryCurrentUser.BackColor = ApplicationSettings.BackgroundColor;
            listViewRegistryCurrentUser.ForeColor = ApplicationSettings.TextColor;
        }
        private void InitContextMenu()
        {
            contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
            deleteMenuItem.Click += (s,e) => DeleteSelectedItem?.Invoke(this, EventArgs.Empty);

            ToolStripMenuItem changeMenuItem = new ToolStripMenuItem("Изменить");

            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                deleteMenuItem,
                changeMenuItem,
            });

            listViewRegistryCurrentUser.ContextMenuStrip = contextMenuStrip;
        }

    }
}
