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
            listViewRegistryAllUsers.Items.Clear();
            listViewFolderCurrentUser.Items.Clear();
            listViewFolderAllUsers.Items.Clear();

            foreach (StartupItem item in startupItems)
            {
                switch (item.Type)
                {
                    case StartupType.RegistryCurrentUser:

                        ListViewItem viewItemCU = new ListViewItem(item.Name);
                        viewItemCU.SubItems.Add(item.Path);
                        viewItemCU.Tag = item;

                        listViewRegistryCurrentUser.Items.Add(viewItemCU);
                        break;
                    case StartupType.RegistryLocalMachine:

                        ListViewItem viewItemLM = new ListViewItem(item.Name);
                        viewItemLM.SubItems.Add(item.Path);
                        viewItemLM.SubItems.Add(item.Bit);
                        viewItemLM.Tag = item;

                        listViewRegistryAllUsers.Items.Add(viewItemLM);
                        break;
                    case StartupType.StartupFolderCurrentUser:

                        ListViewItem viewItemFolderCU = new ListViewItem(item.Name);
                        viewItemFolderCU.SubItems.Add(item.Path);
                        viewItemFolderCU.Tag = item;

                        listViewFolderCurrentUser.Items.Add(viewItemFolderCU);
                        break;
                    case StartupType.StartupFolderAllUsers:
                        ListViewItem viewItemFolderLM = new ListViewItem(item.Name);
                        viewItemFolderLM.SubItems.Add(item.Path);
                        viewItemFolderLM.Tag = item;
                        listViewFolderAllUsers.Items.Add(viewItemFolderLM);
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
            Init_ListViewRegistryAllUsers();
            Init_ListViewFolderCurrentUser();
            Init_ListViewFolderAllUsers();

            InitContextMenu();

            LoadControl?.Invoke(this, EventArgs.Empty);
        }
        private void listViewRegistryCurrentUser_Resize(object sender, EventArgs e)
        {
            listViewRegistryCurrentUser.Columns[1].Width = listViewRegistryCurrentUser.ClientSize.Width - listViewRegistryCurrentUser.Columns[0].Width;
        }
        private void listViewRegistryAllUsers_Resize(object sender, EventArgs e)
        {
            listViewRegistryAllUsers.Columns[1].Width = listViewRegistryAllUsers.ClientSize.Width - (listViewRegistryAllUsers.Columns[0].Width + listViewRegistryAllUsers.Columns[2].Width);
        }
        private void listViewFolderCurrentUser_Resize(object sender, EventArgs e)
        {
            listViewFolderCurrentUser.Columns[1].Width = listViewFolderCurrentUser.ClientSize.Width - listViewFolderCurrentUser.Columns[0].Width;
        }
        private void listViewFolderAllUsers_Resize(object sender, EventArgs e)
        {
            listViewFolderAllUsers.Columns[1].Width = listViewFolderAllUsers.ClientSize.Width - listViewFolderAllUsers.Columns[0].Width;
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
        private void Init_ListViewFolderCurrentUser()
        {
            listViewFolderCurrentUser.View = System.Windows.Forms.View.Details;
            listViewFolderCurrentUser.Columns.Add("Имя", 200);
            listViewFolderCurrentUser.Columns.Add("Путь", 600);

            listViewFolderCurrentUser.Columns[0].TextAlign = HorizontalAlignment.Left;
            listViewFolderCurrentUser.Columns[1].TextAlign = HorizontalAlignment.Left;

            listViewFolderCurrentUser.BackColor = ApplicationSettings.BackgroundColor;
            listViewFolderCurrentUser.ForeColor = ApplicationSettings.TextColor;
        }
        private void Init_ListViewFolderAllUsers()
        {
            listViewFolderAllUsers.View = System.Windows.Forms.View.Details;
            listViewFolderAllUsers.Columns.Add("Имя", 200);
            listViewFolderAllUsers.Columns.Add("Путь", 600);

            listViewFolderAllUsers.Columns[0].TextAlign = HorizontalAlignment.Left;
            listViewFolderAllUsers.Columns[1].TextAlign = HorizontalAlignment.Left;

            listViewFolderAllUsers.BackColor = ApplicationSettings.BackgroundColor;
            listViewFolderAllUsers.ForeColor = ApplicationSettings.TextColor;
        }


        private void Init_ListViewRegistryCurrentUser()
        {
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.Columns.Add("Имя", 200);
            listViewRegistryCurrentUser.Columns.Add("Путь", 600);

            listViewRegistryCurrentUser.Columns[0].TextAlign = HorizontalAlignment.Left;
            listViewRegistryCurrentUser.Columns[1].TextAlign = HorizontalAlignment.Left;

            listViewRegistryCurrentUser.BackColor = ApplicationSettings.BackgroundColor;
            listViewRegistryCurrentUser.ForeColor = ApplicationSettings.TextColor;
        }
        private void Init_ListViewRegistryAllUsers()
        {
            listViewRegistryAllUsers.View = System.Windows.Forms.View.Details;
            listViewRegistryAllUsers.Columns.Add("Имя", 200);
            listViewRegistryAllUsers.Columns.Add("Путь", 600);
            listViewRegistryAllUsers.Columns.Add("Разрядность", 100);

            listViewRegistryAllUsers.Columns[0].TextAlign = HorizontalAlignment.Left;
            listViewRegistryAllUsers.Columns[1].TextAlign = HorizontalAlignment.Left;
            listViewRegistryAllUsers.Columns[2].TextAlign = HorizontalAlignment.Center;

            listViewRegistryAllUsers.BackColor = ApplicationSettings.BackgroundColor;
            listViewRegistryAllUsers.ForeColor = ApplicationSettings.TextColor;
        }
        private void InitContextMenu()
        {
            contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
            deleteMenuItem.Click += (s, e) => DeleteSelectedItem?.Invoke(this, EventArgs.Empty);

            ToolStripMenuItem changeMenuItem = new ToolStripMenuItem("Изменить");


            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                deleteMenuItem,
                changeMenuItem,
            });

            listViewRegistryCurrentUser.ContextMenuStrip = contextMenuStrip;
            listViewRegistryAllUsers.ContextMenuStrip = contextMenuStrip;

        }

        private void listViewFolderAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderAllUsers);
                listViewFolderAllUsers.ContextMenuStrip = contextMenu;
                contextMenu.Show(listViewFolderAllUsers, e.Location);
            }
        }
        private void listViewFolderCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderCurrentUser);
                listViewFolderCurrentUser.ContextMenuStrip = contextMenu;
                contextMenu.Show(listViewFolderCurrentUser, e.Location);
            }
        }
        private void listViewRegistryCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryCurrentUser);
                listViewRegistryCurrentUser.ContextMenuStrip = contextMenu;
                contextMenu.Show(listViewRegistryCurrentUser, e.Location);
            }
        }
        private void listViewRegistryAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryAllUsers);
                listViewRegistryAllUsers.ContextMenuStrip = contextMenu;
                contextMenu.Show(listViewRegistryAllUsers, e.Location);
            }
        }

        private ContextMenuStrip CreateContextMenuItem(ListView listView)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem addMenuItem = new ToolStripMenuItem("Добавить");
            ToolStripMenuItem openFolderItem = new ToolStripMenuItem("Открыть в проводнике");

            contextMenuStrip.Items.Add(addMenuItem);
            contextMenuStrip.Items.Add(openFolderItem);

            if (listView.Items.Count > 0)
            {
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
                ToolStripMenuItem changeMenuItem = new ToolStripMenuItem("Изменить");
                ToolStripMenuItem copyPathItem = new ToolStripMenuItem("Копировать путь");

                contextMenuStrip.Items.Add(deleteMenuItem);
                contextMenuStrip.Items.Add(changeMenuItem);
                contextMenuStrip.Items.Add(copyPathItem);
            }

            return contextMenuStrip;
        }

    }
}
