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

        public StartupManagementControl()
        {
            InitializeComponent();

            // Подписка на активацию списков один раз
            listViewFolderCurrentUser.MouseDown += ListView_Activate;
            listViewFolderAllUsers.MouseDown += ListView_Activate;
            listViewRegistryCurrentUser.MouseDown += ListView_Activate;
            listViewRegistryAllUsers.MouseDown += ListView_Activate;
        }

        private void StartupManagementControl_Load(object sender, EventArgs e)
        {
            Init_ListViewRegistryCurrentUser();
            Init_ListViewRegistryAllUsers();
            Init_ListViewFolderCurrentUser();
            Init_ListViewFolderAllUsers();

            LoadControl?.Invoke(this, EventArgs.Empty);
        }
        public void DisplayRegistryStartupItems(List<StartupItemRegistry> startupItems, StartupType type)
        {
            if ((type & StartupType.RegistryCurrentUser) != 0)
            {
                listViewRegistryCurrentUser.BeginUpdate();
                listViewRegistryCurrentUser.Items.Clear();
                foreach (var item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.RegistryCurrentUser))
                    {
                        var viewItem = new ListViewItem(item.NameExtracted);
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
                foreach (var item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.RegistryLocalMachine))
                    {
                        var viewItem = new ListViewItem(item.NameExtracted);
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
                        listViewRegistryAllUsers.Items.Add(viewItem);
                    }
                }
                listViewRegistryAllUsers.EndUpdate();
            }
        }
        //    listViewRegistryCurrentUser.BeginUpdate();
        //    listViewRegistryAllUsers.BeginUpdate();

        //    listViewRegistryCurrentUser.Items.Clear();
        //    listViewRegistryAllUsers.Items.Clear();

        //    foreach (StartupItemRegistry item in startupItems)
        //    {
        //        ListViewItem viewItem = new ListViewItem(item.NameExtracted);
        //        viewItem.SubItems.Add("");
        //        viewItem.SubItems.Add(item.PathExtracted);
        //        viewItem.Tag = item;

        //        switch (item.Type)
        //        {
        //            case StartupType.RegistryCurrentUser:
        //                listViewRegistryCurrentUser.Items.Add(viewItem);
        //                break;
        //            case StartupType.RegistryLocalMachine:
        //                if (item.Is32Bit)
        //                {
        //                    viewItem.SubItems.Add("32 бит");
        //                }

        //                listViewRegistryAllUsers.Items.Add(viewItem);
        //                break;
        //        }
        //    }

        //    listViewRegistryCurrentUser.EndUpdate();
        //    listViewRegistryAllUsers.EndUpdate();
        //}

        public void DisplayFolderStartupItems(List<StartupItemFolder> startupItems, StartupType type)
        {
            if ((type & StartupType.StartupFolderCurrentUser) != 0)
            {
                listViewFolderCurrentUser.BeginUpdate();
                listViewFolderCurrentUser.Items.Clear();
                foreach (var item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderCurrentUser))
                    {
                        var viewItem = new ListViewItem(item.NameExtracted);
                        viewItem.SubItems.Add("");
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
                foreach (var item in startupItems)
                {
                    if (item.Type.HasFlag(StartupType.StartupFolderAllUsers))
                    {
                        var viewItem = new ListViewItem(item.NameExtracted);
                        viewItem.SubItems.Add("");
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
            if (_activeListView != listViewFolderCurrentUser && _activeListView != listViewFolderAllUsers)
                return new();

            List<StartupItemFolder> selectedItems = new List<StartupItemFolder>();

            foreach (ListViewItem listViewItem in _activeListView.SelectedItems)
            {
                if (listViewItem.Tag is StartupItemFolder startupItem)
                    selectedItems.Add(startupItem);
            }
            return selectedItems;
        }
        public List<StartupItemRegistry> GetSelectedStartupItems_Registry()
        {
            if (_activeListView != listViewRegistryCurrentUser && _activeListView != listViewRegistryAllUsers)
                return new();

            List<StartupItemRegistry> selectedItems = new List<StartupItemRegistry>();

            foreach (ListViewItem listViewItem in _activeListView.SelectedItems)
            {
                if (listViewItem.Tag is StartupItemRegistry startupItem)
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
            listViewFolderCurrentUser.Columns.Add("Состояние", 100);
            listViewFolderCurrentUser.Columns.Add("Путь", 600);
            listViewFolderCurrentUser.BackColor = Color.FromArgb(146, 156, 155);
            listViewFolderCurrentUser.ForeColor = Color.Black;
        }

        private void Init_ListViewFolderAllUsers()
        {
            listViewFolderAllUsers.View = System.Windows.Forms.View.Details;
            listViewFolderAllUsers.Columns.Add("Имя", 200);
            listViewFolderAllUsers.Columns.Add("Состояние", 100);
            listViewFolderAllUsers.Columns.Add("Путь", 600);
            listViewFolderAllUsers.BackColor = Color.FromArgb(146, 156, 155);
            listViewFolderAllUsers.ForeColor = Color.Black;
        }

        private void Init_ListViewRegistryCurrentUser()
        {
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.Columns.Add("Имя", 200);
            listViewRegistryCurrentUser.Columns.Add("Состояние", 100);
            listViewRegistryCurrentUser.Columns.Add("Путь", 600);
            listViewRegistryCurrentUser.BackColor = Color.FromArgb(146, 156, 155);
            listViewRegistryCurrentUser.ForeColor = Color.Black;
        }

        private void Init_ListViewRegistryAllUsers()
        {
            listViewRegistryAllUsers.View = System.Windows.Forms.View.Details;
            listViewRegistryAllUsers.Columns.Add("Имя", 200);
            listViewRegistryAllUsers.Columns.Add("Состояние", 100);
            listViewRegistryAllUsers.Columns.Add("Путь", 600);
            listViewRegistryAllUsers.Columns.Add("Разрядность", 100);
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

            contextMenu.Items.Add(new ToolStripMenuItem("Добавить"));
            contextMenu.Items.Add(new ToolStripMenuItem("Открыть в проводнике"));

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
