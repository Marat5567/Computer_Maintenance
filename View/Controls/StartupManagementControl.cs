using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.View.Controls
{
    public partial class StartupManagementControl : UserControl, IStartupManagementView
    {
        private ListView _activeListView;

        public event EventHandler LoadControl;
        public event EventHandler MouseClicked_ListView;

        public event EventHandler DeleteSelectedItem_Registry;
        public event EventHandler DeleteSelectedItem_Folder;
        public event EventHandler ChangeSelectedItem;
        public event EventHandler ChangeSelectedItem_StateStartup_Registry;
        public event EventHandler ChangeSelectedItem_StateStartup_Folder;

        public event EventHandler OpenExplorerClicked;

        public (bool isFile, string path) SelectedPath { get; set; }
        public StartupType LastFolderSelectionSource { get; set; }


        public StartupManagementControl()
        {
            InitializeComponent();

            // Подписка на активацию списков один раз
            listViewFolderCurrentUser.MouseDown += ListView_Activate;
            listViewFolderAllUsers.MouseDown += ListView_Activate;
            listViewRegistryCurrentUser.MouseDown += ListView_Activate;
            listViewRegistryAllUsers.MouseDown += ListView_Activate;
        }
        private void InitTabControl()
        {
            tabPageRegistryCurrentUser.Controls.Add(panelRegistryCurrentUser);
            tabPageRegistryAllUsers.Controls.Add(panelRegistryAllUsers);
            tabPageFolderCurrentUser.Controls.Add(panelFolderCurrentUser);
            tabPageFolderAllUsers.Controls.Add(panelFolderAllUsers);
            //tabPageTaskSheduler.Controls.Add(panelTaskSheduler);
        }
        private void StartupManagementControl_Load(object sender, EventArgs e)
        {
            Init_ListViewRegistryCurrentUser();
            Init_ListViewRegistryAllUsers();
            Init_ListViewFolderCurrentUser();
            Init_ListViewFolderAllUsers();

            InitTabControl();

            LoadControl?.Invoke(this, EventArgs.Empty);
        }
        public void DisplayRegistryStartupItems(List<StartupItemRegistry> startupItems, StartupType type)
        {
            if ((type & StartupType.RegistryCurrentUser) != 0)
            {
                listViewRegistryCurrentUser.BeginUpdate();
                listViewRegistryCurrentUser.Items.Clear();
                foreach (StartupItemRegistry item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.RegistryCurrentUser))
                    {
                        ListViewItem viewItem = new ListViewItem(item.NameExtracted);
                        if (item.State == StartupState.Enabled)
                        {
                            viewItem.SubItems.Add("Включено");
                        }
                        else if (item.State == StartupState.Disabled)
                        {
                            viewItem.SubItems.Add("Отключено");
                        }
                        else if (item.State == StartupState.None)
                        {
                            viewItem.SubItems.Add("Неизвестно");
                        }
                        viewItem.SubItems.Add(item.PathExtracted);
                        viewItem.Tag = item;
                        listViewRegistryCurrentUser.Items.Add(viewItem);
                    }
                }
                listViewRegistryCurrentUser.EndUpdate();
            }

            if ((type & StartupType.RegistryLocalMachine) != 0)
            {
                listViewRegistryAllUsers.BeginUpdate();
                listViewRegistryAllUsers.Items.Clear();
                foreach (StartupItemRegistry item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.RegistryLocalMachine))
                    {
                        ListViewItem viewItem = new ListViewItem(item.NameExtracted);

                        if (item.State == StartupState.Enabled)
                        {
                            viewItem.SubItems.Add("Включено");
                        }
                        else if (item.State == StartupState.Disabled)
                        {
                            viewItem.SubItems.Add("Отключено");
                        }
                        else if (item.State == StartupState.None)
                        {
                            viewItem.SubItems.Add("Неизвестно");
                        }

                        viewItem.SubItems.Add(item.PathExtracted);

                        viewItem.SubItems.Add(item.Is32Bit ? "32 бит" : "");
                        viewItem.Tag = item;
                        listViewRegistryAllUsers.Items.Add(viewItem);
                    }
                }
                listViewRegistryAllUsers.EndUpdate();
            }
        }

        public void DisplayFolderStartupItems(List<StartupItemFolder> startupItems, StartupType type)
        {
            if ((type & StartupType.StartupFolderCurrentUser) != 0)
            {
                listViewFolderCurrentUser.BeginUpdate();
                listViewFolderCurrentUser.Items.Clear();
                foreach (StartupItemFolder item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                    {
                        ListViewItem viewItem = new ListViewItem(item.NameExtracted);
                        if (item.State == StartupState.Enabled)
                        {
                            viewItem.SubItems.Add("Включено");
                        }
                        else if (item.State == StartupState.Disabled)
                        {
                            viewItem.SubItems.Add("Отключено");
                        }
                        else if (item.State == StartupState.None)
                        {
                            viewItem.SubItems.Add("Неизвестно");
                        }
                        viewItem.SubItems.Add(item.PathExtracted);
                        viewItem.Tag = item;
                        listViewFolderCurrentUser.Items.Add(viewItem);
                    }
                }
                listViewFolderCurrentUser.EndUpdate();
            }

            if ((type & StartupType.StartupFolderAllUsers) != 0)
            {
                listViewFolderAllUsers.BeginUpdate();
                listViewFolderAllUsers.Items.Clear();
                foreach (StartupItemFolder item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                    {
                        ListViewItem viewItem = new ListViewItem(item.NameExtracted);
                        if (item.State == StartupState.Enabled)
                        {
                            viewItem.SubItems.Add("Включено");
                        }
                        else if (item.State == StartupState.Disabled)
                        {
                            viewItem.SubItems.Add("Отключено");
                        }
                        else if (item.State == StartupState.None)
                        {
                            viewItem.SubItems.Add("Неизвестно");
                        }
                        viewItem.SubItems.Add(item.PathExtracted);
                        viewItem.Tag = item;
                        listViewFolderAllUsers.Items.Add(viewItem);
                    }
                }
                listViewFolderAllUsers.EndUpdate();
            }
        }


        public List<StartupItemFolder> GetSelectedStartupItems_Folder()
        {
            SelectedPath = (isFile: false, path: string.Empty);


            if (_activeListView == listViewFolderCurrentUser)
            {
                LastFolderSelectionSource = StartupType.StartupFolderCurrentUser;
            }
            else if (_activeListView == listViewFolderAllUsers)
            {
                LastFolderSelectionSource = StartupType.StartupFolderAllUsers;
            }
            else
            {
                LastFolderSelectionSource = StartupType.None;
                return new();
            }

            List<StartupItemFolder> selectedItems = new();

            foreach (ListViewItem listViewItem in _activeListView.SelectedItems)
            {
                if (listViewItem.Tag is StartupItemFolder startupItem)
                    selectedItems.Add(startupItem);
            }

            if (selectedItems.Count == 1)
            {
                SelectedPath = (isFile: true, path: selectedItems[0].PathExtracted);
            }
            else if (selectedItems.Count > 1)
            {
                SelectedPath = (isFile: false, path: selectedItems[0].PathExtracted);
            }

            return selectedItems;
        }
        public List<StartupItemRegistry> GetSelectedStartupItems_Registry()
        {
            if (_activeListView != listViewRegistryCurrentUser && _activeListView != listViewRegistryAllUsers)
                return new();

            SelectedPath = (isFile: true, path: String.Empty);
            List<StartupItemRegistry> selectedItems = new List<StartupItemRegistry>();

            foreach (ListViewItem listViewItem in _activeListView.SelectedItems)
            {
                if (listViewItem.Tag is StartupItemRegistry startupItem)
                {
                    selectedItems.Add(startupItem);
                }
            }

            if (selectedItems.Count > 0)
            {
                if (selectedItems.Count == 1)
                {
                    SelectedPath = (isFile: true, path: selectedItems[0].PathExtracted);
                }
            }

            return selectedItems;
        }

        private void Init_ListViewFolderCurrentUser()
        {
            listViewFolderCurrentUser.View = System.Windows.Forms.View.Details;
            listViewFolderCurrentUser.Columns.Add("Имя", 180);
            listViewFolderCurrentUser.Columns.Add("Состояние", 110);
            listViewFolderCurrentUser.Columns.Add("Путь", 600);
            listViewFolderCurrentUser.BackColor = Color.FromArgb(146, 156, 155);
            listViewFolderCurrentUser.ForeColor = Color.Black;
        }

        private void Init_ListViewFolderAllUsers()
        {
            listViewFolderAllUsers.View = System.Windows.Forms.View.Details;
            listViewFolderAllUsers.Columns.Add("Имя", 180);
            listViewFolderAllUsers.Columns.Add("Состояние", 110);
            listViewFolderAllUsers.Columns.Add("Путь", 600);
            listViewFolderAllUsers.BackColor = Color.FromArgb(146, 156, 155);
            listViewFolderAllUsers.ForeColor = Color.Black;
        }

        private void Init_ListViewRegistryCurrentUser()
        {
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.Columns.Add("Имя", 180);
            listViewRegistryCurrentUser.Columns.Add("Состояние", 110);
            listViewRegistryCurrentUser.Columns.Add("Путь", 600);
            listViewRegistryCurrentUser.BackColor = Color.FromArgb(146, 156, 155);
            listViewRegistryCurrentUser.ForeColor = Color.Black;
        }

        private void Init_ListViewRegistryAllUsers()
        {
            listViewRegistryAllUsers.View = System.Windows.Forms.View.Details;
            listViewRegistryAllUsers.Columns.Add("Имя", 180);
            listViewRegistryAllUsers.Columns.Add("Состояние", 110);
            listViewRegistryAllUsers.Columns.Add("Путь", 600);
            listViewRegistryAllUsers.Columns.Add("Разрядность", 120);
            listViewRegistryAllUsers.BackColor = Color.FromArgb(146, 156, 155);
            listViewRegistryAllUsers.ForeColor = Color.Black;
        }

        private void listViewFolderCurrentUser_Resize(object sender, EventArgs e)
        {
            if (listViewFolderCurrentUser.Columns.Count >= 3)
                listViewFolderCurrentUser.Columns[2].Width = listViewFolderCurrentUser.ClientSize.Width - listViewFolderCurrentUser.Columns[0].Width - listViewFolderCurrentUser.Columns[1].Width;
        }

        private void listViewFolderAllUsers_Resize(object sender, EventArgs e)
        {
            if (listViewFolderAllUsers.Columns.Count >= 3)
                listViewFolderAllUsers.Columns[2].Width = listViewFolderAllUsers.ClientSize.Width - listViewFolderAllUsers.Columns[0].Width - listViewFolderAllUsers.Columns[1].Width;
        }

        private void listViewRegistryCurrentUser_Resize(object sender, EventArgs e)
        {
            if (listViewRegistryCurrentUser.Columns.Count >= 3)
                listViewRegistryCurrentUser.Columns[2].Width = listViewRegistryCurrentUser.ClientSize.Width - listViewRegistryCurrentUser.Columns[0].Width - listViewRegistryCurrentUser.Columns[1].Width;
        }

        private void listViewRegistryAllUsers_Resize(object sender, EventArgs e)
        {
            if (listViewRegistryAllUsers.Columns.Count >= 4)
                listViewRegistryAllUsers.Columns[2].Width = listViewRegistryAllUsers.ClientSize.Width - (listViewRegistryAllUsers.Columns[0].Width + listViewRegistryAllUsers.Columns[1].Width + listViewRegistryAllUsers.Columns[3].Width);
        }

        private void listViewFolderCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderCurrentUser, StartupType.StartupFolderCurrentUser);
                contextMenu.Show(listViewFolderCurrentUser, e.Location);
            }
        }

        private void listViewFolderAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderAllUsers, StartupType.StartupFolderAllUsers);
                contextMenu.Show(listViewFolderAllUsers, e.Location);
            }
        }

        private void listViewRegistryCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryCurrentUser, StartupType.RegistryCurrentUser);
                contextMenu.Show(listViewRegistryCurrentUser, e.Location);
            }
        }

        private void listViewRegistryAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryAllUsers, StartupType.RegistryLocalMachine);
                contextMenu.Show(listViewRegistryAllUsers, e.Location);
            }
        }

        private ContextMenuStrip CreateContextMenuItem(ListView listView, StartupType startupType)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem openExplorer = new ToolStripMenuItem("Открыть в проводнике");
            ToolStripMenuItem add = new ToolStripMenuItem("Добавить");

            contextMenu.Items.Add(openExplorer);
            contextMenu.Items.Add(add);

            openExplorer.Click += (s, e) => OpenExplorerClicked?.Invoke(this, EventArgs.Empty);

            if (listView.Items.Count > 0)
            {
                ToolStripMenuItem changeStateItem = new ToolStripMenuItem("Изменить состояние");
                ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить");
                ToolStripMenuItem changeItem = new ToolStripMenuItem("Изменить");
                ToolStripMenuItem copyPathItem = new ToolStripMenuItem("Копировать путь");

                contextMenu.Items.Add(changeStateItem);
                contextMenu.Items.Add(deleteItem);
                contextMenu.Items.Add(changeItem);
                contextMenu.Items.Add(copyPathItem);

                if (startupType.HasFlag(StartupType.StartupFolderCurrentUser) || startupType.HasFlag(StartupType.StartupFolderAllUsers))
                {
                    deleteItem.Click += (s, e) => DeleteSelectedItem_Folder?.Invoke(this, EventArgs.Empty);
                    changeStateItem.Click += (s, e) => ChangeSelectedItem_StateStartup_Folder?.Invoke(this, EventArgs.Empty);
                }

                if (startupType.HasFlag(StartupType.RegistryCurrentUser) || startupType.HasFlag(StartupType.RegistryLocalMachine))
                {
                    changeStateItem.Click += (s, e) => ChangeSelectedItem_StateStartup_Registry?.Invoke(this, EventArgs.Empty);
                }

            }

            return contextMenu;
        }

        private void ListView_Activate(object sender, MouseEventArgs e)
        {
            if (!(e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
                return;

            var current = sender as ListView;
            if (current == null) return;

            if (e.Button == MouseButtons.Left)
            {
                // Левый клик: переключаем активный список и очищаем выделение другого
                if (_activeListView != null && _activeListView != current)
                    _activeListView.SelectedItems.Clear();

                _activeListView = current;
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Правый клик: просто помечаем активный список
                _activeListView = current;
            }
        }
    }
}
