using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;
using Microsoft.Win32.TaskScheduler;
using System.ComponentModel;

namespace Computer_Maintenance.View.Controls
{
    public partial class StartupManagementControl : UserControl, IStartupManagementView
    {
        public event EventHandler LoadControl;
        public event EventHandler OpenExplorerClicked;
        public event EventHandler ChangeStateSelectedItems;
        public event EventHandler CopyClipboardClicked;

        public (bool isFile, string path) SelectedPath { get; set; }
        public StartupType LastFolderSelectionSource { get; set; }


        private ListView _activeListView;

        public StartupManagementControl()
        {
            InitializeComponent();

            listViewFolderCurrentUser.MouseDown += ListView_Activate;
            listViewFolderAllUsers.MouseDown += ListView_Activate;
            listViewRegistryCurrentUser.MouseDown += ListView_Activate;
            listViewRegistryAllUsers.MouseDown += ListView_Activate;
            listViewTaskScheduler.MouseDown += ListView_Activate;
        }
        private void StartupManagementControl_Load(object sender, EventArgs e)
        {
            Init_ListViewRegistryCurrentUser();
            Init_ListViewRegistryAllUsers();
            Init_ListViewFolderCurrentUser();
            Init_ListViewFolderAllUsers();
            Init_ListViewTaskScheduler();
            InitTabControl();

            ThemeService.RefreshTheme(this);
            LoadControl?.Invoke(this, EventArgs.Empty);
        }
        private void Init_ListViewRegistryCurrentUser()
        {
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.Columns.Add("Имя", 180);
            listViewRegistryCurrentUser.Columns.Add("Состояние", 110);
            listViewRegistryCurrentUser.Columns.Add("Путь", 600);
            listViewRegistryCurrentUser.BackColor = Color.FromArgb(209, 205, 205);
            listViewRegistryCurrentUser.ForeColor = Color.Black;
        }
        private void Init_ListViewRegistryAllUsers()
        {
            listViewRegistryAllUsers.View = System.Windows.Forms.View.Details;
            listViewRegistryAllUsers.Columns.Add("Имя", 180);
            listViewRegistryAllUsers.Columns.Add("Состояние", 110);
            listViewRegistryAllUsers.Columns.Add("Путь", 600);
            listViewRegistryAllUsers.Columns.Add("Разрядность", 120);
            listViewRegistryAllUsers.BackColor = Color.FromArgb(209, 205, 205);
            listViewRegistryAllUsers.ForeColor = Color.Black;
        }
        private void Init_ListViewFolderCurrentUser()
        {
            listViewFolderCurrentUser.View = System.Windows.Forms.View.Details;
            listViewFolderCurrentUser.Columns.Add("Имя", 180);
            listViewFolderCurrentUser.Columns.Add("Состояние", 110);
            listViewFolderCurrentUser.Columns.Add("Путь", 600);
            listViewFolderCurrentUser.BackColor = Color.FromArgb(209, 205, 205);
            listViewFolderCurrentUser.ForeColor = Color.Black;
        }
        private void Init_ListViewFolderAllUsers()
        {
            listViewFolderAllUsers.View = System.Windows.Forms.View.Details;
            listViewFolderAllUsers.Columns.Add("Имя", 180);
            listViewFolderAllUsers.Columns.Add("Состояние", 110);
            listViewFolderAllUsers.Columns.Add("Путь", 600);
            listViewFolderAllUsers.BackColor = Color.FromArgb(209, 205, 205);
            listViewFolderAllUsers.ForeColor = Color.Black;
        }
        private void Init_ListViewTaskScheduler()
        {
            listViewTaskScheduler.View = System.Windows.Forms.View.Details;
            listViewTaskScheduler.Columns.Add("Файл", 150);
            listViewTaskScheduler.Columns.Add("Имя", 180);
            listViewTaskScheduler.Columns.Add("Состояние", 110);
            listViewTaskScheduler.Columns.Add("Путь", 600);
            listViewTaskScheduler.BackColor = Color.FromArgb(209, 205, 205);
            listViewTaskScheduler.ForeColor = Color.Black;
        }
        private void InitTabControl()
        {
            tabPageRegistryCurrentUser.Controls.Add(panelRegistryCurrentUser);
            tabPageRegistryAllUsers.Controls.Add(panelRegistryAllUsers);
            tabPageFolderCurrentUser.Controls.Add(panelFolderCurrentUser);
            tabPageFolderAllUsers.Controls.Add(panelFolderAllUsers);
            tabPageTaskSсheduler.Controls.Add(panelTaskScheduler);
        }
        private void listViewRegistryCurrentUser_Resize(object sender, EventArgs e)
        {
            if (listViewRegistryCurrentUser.Columns.Count >= 3)
            {
                listViewRegistryCurrentUser.Columns[2].Width = listViewRegistryCurrentUser.ClientSize.Width - listViewRegistryCurrentUser.Columns[0].Width - listViewRegistryCurrentUser.Columns[1].Width;
            }
        }
        private void listViewRegistryAllUsers_Resize(object sender, EventArgs e)
        {
            if (listViewRegistryAllUsers.Columns.Count >= 4)
            {
                listViewRegistryAllUsers.Columns[2].Width = listViewRegistryAllUsers.ClientSize.Width - (listViewRegistryAllUsers.Columns[0].Width + listViewRegistryAllUsers.Columns[1].Width + listViewRegistryAllUsers.Columns[3].Width);
            }
        }
        private void listViewFolderCurrentUser_Resize(object sender, EventArgs e)
        {
            if (listViewFolderCurrentUser.Columns.Count >= 3)
            {
                listViewFolderCurrentUser.Columns[2].Width = listViewFolderCurrentUser.ClientSize.Width - listViewFolderCurrentUser.Columns[0].Width - listViewFolderCurrentUser.Columns[1].Width;
            }
        }
        private void listViewFolderAllUsers_Resize(object sender, EventArgs e)
        {
            if (listViewFolderAllUsers.Columns.Count >= 3)
            {
                listViewFolderAllUsers.Columns[2].Width = listViewFolderAllUsers.ClientSize.Width - listViewFolderAllUsers.Columns[0].Width - listViewFolderAllUsers.Columns[1].Width;
            }
        }
        private void listViewTaskScheduler_Resize(object sender, EventArgs e)
        {
            if (listViewTaskScheduler.Columns.Count >= 4)
            {
                listViewTaskScheduler.Columns[3].Width = listViewTaskScheduler.ClientSize.Width - (listViewTaskScheduler.Columns[0].Width + listViewTaskScheduler.Columns[1].Width + listViewTaskScheduler.Columns[2].Width);
            }
        }



        private void listViewRegistryCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryCurrentUser.Items.Count, StartupType.RegistryCurrentUser);
                contextMenu.Show(listViewRegistryCurrentUser, e.Location);
            }
        }
        private void listViewRegistryAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewRegistryAllUsers.Items.Count, StartupType.RegistryLocalMachine);
                contextMenu.Show(listViewRegistryAllUsers, e.Location);
            }
        }
        private void listViewFolderCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderCurrentUser.Items.Count, StartupType.StartupFolderCurrentUser);
                contextMenu.Show(listViewFolderCurrentUser, e.Location);
            }
        }
        private void listViewFolderAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = CreateContextMenuItem(listViewFolderAllUsers.Items.Count, StartupType.StartupFolderAllUsers);
                contextMenu.Show(listViewFolderAllUsers, e.Location);
            }
        }
        private void listViewTaskScheduler_MouseDown(object sender, MouseEventArgs e)
        {

        }



        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadControl?.Invoke(this, EventArgs.Empty);
        }


        public void DisplayItems(List<object> items, StartupType type)
        {
            if (items == null || items.Count == 0) { return; }

            ListView currentListView = null;
            switch (type)
            {
                case StartupType.RegistryCurrentUser:
                    currentListView = listViewRegistryCurrentUser;
                    break;
                case StartupType.RegistryLocalMachine:
                    currentListView = listViewRegistryAllUsers;
                    break;
                case StartupType.StartupFolderCurrentUser:
                    currentListView = listViewFolderCurrentUser;
                    break;
                case StartupType.StartupFolderAllUsers:
                    currentListView = listViewFolderAllUsers;
                    break;
                case StartupType.TaskScheduler:
                    currentListView = listViewTaskScheduler;
                    break;
            }

            if (currentListView == null) { return; }

            currentListView.BeginUpdate();
            currentListView.Items.Clear();

            foreach (object item in items)
            {
                ListViewItem viewItem = null;

                switch (item)
                {
                    case StartupItemRegistry registryItem:
                        viewItem = new ListViewItem(registryItem.NameExtracted);
                        viewItem.SubItems.Add(GetStateText(registryItem.State));
                        viewItem.SubItems.Add(registryItem.PathExtracted);

                        if (registryItem.Type.HasFlag(StartupType.RegistryLocalMachine))
                        {
                            viewItem.SubItems.Add(registryItem.Is32Bit ? "32 бит" : "");
                        }
                        viewItem.Tag = registryItem;
                        break;
                    case StartupItemFolder folderItem:
                        viewItem = new ListViewItem(folderItem.NameExtracted);
                        viewItem.SubItems.Add(GetStateText(folderItem.State));
                        viewItem.SubItems.Add(folderItem.PathExtracted);
                        viewItem.Tag = folderItem;
                        break;

                    case TaskSchedulerItem taskSchedulerItem:
                        viewItem = new ListViewItem(taskSchedulerItem.File);
                        viewItem.SubItems.Add(taskSchedulerItem.Name);
                        viewItem.SubItems.Add(taskSchedulerItem.State.ToString());
                        viewItem.SubItems.Add(taskSchedulerItem.Path);
                        viewItem.Tag = taskSchedulerItem;
                        break;

                    default:
                        continue;
                }

                if (viewItem != null)
                {
                    currentListView.Items.Add(viewItem);
                }
            }

            currentListView.EndUpdate();
        }

        private string GetStateText(StartupState state)
        {
            switch (state)
            {
                case StartupState.Enabled:
                    return "Включено";
                case StartupState.Disabled:
                    return "Отключено";
                case StartupState.None:
                    return "Неизвестно";
                default:
                    return string.Empty;
            }
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
        public void DisplayTaskSchedulerItems(List<TaskSchedulerItem> startupItems)
        {
            listViewTaskScheduler.BeginUpdate();

            listViewTaskScheduler.Items.Clear();
            foreach (TaskSchedulerItem item in startupItems)
            {
                ListViewItem viewItem = new ListViewItem(item.File);
                viewItem.SubItems.Add(item.Name);

                if (item.State == TaskState.Running)
                {
                    viewItem.SubItems.Add("Работает");
                }
                else if (item.State == TaskState.Queued)
                {
                    viewItem.SubItems.Add("В очереди");
                }
                else if (item.State == TaskState.Disabled)
                {
                    viewItem.SubItems.Add("Отключен");
                }
                else if (item.State == TaskState.Ready)
                {
                    viewItem.SubItems.Add("Готово");
                }
                else if (item.State == TaskState.Unknown)
                {
                    viewItem.SubItems.Add("Неизвестно");
                }
                viewItem.SubItems.Add(item.Path);
                viewItem.Tag = item;

                listViewTaskScheduler.Items.Add(viewItem);
            }

            listViewTaskScheduler.EndUpdate();
        }


        public List<object> GetSelectedItems()
        {
            SelectedPath = (isFile: false, path: String.Empty);
            LastFolderSelectionSource = StartupType.None;

            List<object> items = new List<object>();

            if (_activeListView == null)
            {
                return items;
            }

            foreach (ListViewItem listViewItem in _activeListView.SelectedItems)
            {
                if (listViewItem.Tag != null)
                {
                    items.Add(listViewItem.Tag);
                }
            }
            if (items.Count == 0)
            {
                if (_activeListView is ListView listView)
                {
                    if (listView == listViewFolderCurrentUser)
                    {
                        SelectedPath = (isFile: false, path: String.Empty);
                        LastFolderSelectionSource = StartupType.StartupFolderCurrentUser;
                    }
                    else if (listView == listViewFolderAllUsers)
                    {
                        SelectedPath = (isFile: false, path: String.Empty);
                        LastFolderSelectionSource = StartupType.StartupFolderAllUsers;
                    }
                }
                
                return new(); 
            }

            if (items[0] is StartupItemFolder startupItemFolder)
            {
                if (items.Count == 1)
                {
                    SelectedPath = (isFile: true, path: startupItemFolder.PathExtracted);
                    LastFolderSelectionSource = startupItemFolder.Type;
                }
            }
            else if (items[0] is StartupItemRegistry startupItemRegistry)
            {
                if (items.Count == 1)
                {
                    SelectedPath = (isFile: true, path: startupItemRegistry.PathExtracted);
                }
                else if (items.Count > 1)
                {
                    SelectedPath = (isFile: false, path: startupItemRegistry.PathExtracted);
                }
            }

            return items;
        }



        private ContextMenuStrip CreateContextMenuItem(int listViewItemsCount, StartupType startupType)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            List<ToolStripItem> toolStripItems = new List<ToolStripItem>();

            ToolStripMenuItem openExplorer = new ToolStripMenuItem("Открыть в проводнике");
            ToolStripMenuItem add = new ToolStripMenuItem("Добавить");

            ToolStripMenuItem changeStateItem = null;
            ToolStripMenuItem copyPathItem = null;
            ToolStripMenuItem deleteItem = null;
            ToolStripMenuItem changeItem = null;

            openExplorer.Click += (s, e) => OpenExplorerClicked?.Invoke(this, EventArgs.Empty);

            if (listViewItemsCount > 0)
            {
                changeStateItem = new ToolStripMenuItem("Изменить состояние");
                deleteItem = new ToolStripMenuItem("Удалить");
                changeItem = new ToolStripMenuItem("Изменить");
                copyPathItem = new ToolStripMenuItem("Копировать путь");

                contextMenu.Items.Add(changeStateItem);
                contextMenu.Items.Add(deleteItem);
                contextMenu.Items.Add(changeItem);
                contextMenu.Items.Add(copyPathItem);

                switch (startupType)
                {
                    case StartupType.StartupFolderCurrentUser:
                    case StartupType.StartupFolderAllUsers:
                        //deleteItem.Click += (s, e) => DeleteSelectedItem_Folder?.Invoke(this, EventArgs.Empty);
                        changeStateItem.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
                        copyPathItem.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);
                        break;

                    case StartupType.RegistryCurrentUser:
                    case StartupType.RegistryLocalMachine:
                        changeStateItem.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
                        copyPathItem.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);
                        break;

                    case StartupType.TaskScheduler:
                        changeStateItem.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
                        copyPathItem.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);
                        break;
                }
            }

            contextMenu.Items.Add(openExplorer);

            if (changeStateItem != null)
            {
                contextMenu.Items.Add(changeStateItem);
            }
            if (copyPathItem != null)
            {
                contextMenu.Items.Add(copyPathItem);
            }
            if (changeItem != null)
            {
                contextMenu.Items.Add(changeItem);
            }
            if (deleteItem != null)
            {
                contextMenu.Items.Add(deleteItem);
            }
            contextMenu.Items.Add(add);
            return contextMenu;
        }

        private void ListView_Activate(object sender, MouseEventArgs e)
        {
            ListView? current = sender as ListView;
            if (current == null) { return; }

            if (e.Button == MouseButtons.Left)
            {
                if (_activeListView != null && _activeListView != current)
                {
                    _activeListView.SelectedItems.Clear();
                }
                _activeListView = current;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _activeListView = current;
            }
        }
    }
}
