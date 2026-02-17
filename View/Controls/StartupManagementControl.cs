using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.View.Controls
{
    public partial class StartupManagementControl : UserControl, IStartupManagementView
    {
        public event EventHandler LoadControl;
        public event EventHandler OpenExplorerClicked;
        public event EventHandler ChangeStateSelectedItems;
        public event EventHandler CopyClipboardClicked;
        public event EventHandler DeleteUnusedRecords_Click;
        public event EventHandler DeleteRegistryRecordClick;
        public event EventHandler ViewDetailClick;
        public event EventHandler CompleteTaskClick;
        public event EventHandler RunTaskClick;
        public event EventHandler DeleteTaskClick;
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public StartupManagementControl()
        {
            InitializeComponent();
            labelInfo.Text = String.Empty;


            listViewFolderCurrentUser.ItemSelectionChanged += OnListViewSelectedIndexChanged;
            listViewFolderCurrentUser.MouseDown += OnListViewSelectedIndexChanged;

            listViewFolderAllUsers.SelectedIndexChanged += OnListViewSelectedIndexChanged;
            listViewFolderAllUsers.MouseDown += OnListViewSelectedIndexChanged;

            listViewRegistryCurrentUser.SelectedIndexChanged += OnListViewSelectedIndexChanged;

            listViewRegistryAllUsers.SelectedIndexChanged += OnListViewSelectedIndexChanged;

            listViewTaskScheduler.SelectedIndexChanged += OnListViewSelectedIndexChanged;
        }
      
        private void OnListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView == null)
                return;

            List<object> selectedItems = new List<object>();

            foreach (ListViewItem item in listView.SelectedItems)
            {
                if (item.Tag != null)
                {
                    selectedItems.Add(item.Tag);
                }
            }

            StartupType selectionSource = StartupType.None;
            if (listView == listViewRegistryCurrentUser)
                selectionSource = StartupType.RegistryCurrentUser;
            else if (listView == listViewRegistryAllUsers)
                selectionSource = StartupType.RegistryLocalMachine;
            else if (listView == listViewFolderCurrentUser)
                selectionSource = StartupType.StartupFolderCurrentUser;
            else if (listView == listViewFolderAllUsers)
                selectionSource = StartupType.StartupFolderAllUsers;
            else if (listView == listViewTaskScheduler)
                selectionSource = StartupType.TaskScheduler;

            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(selectedItems, selectionSource));

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
            listViewTaskScheduler.Columns.Add("Имя", 180);
            listViewTaskScheduler.Columns.Add("Состояние", 110);
            listViewTaskScheduler.Columns.Add("Путь", 350);
            listViewTaskScheduler.Columns.Add("Аргументы ", 250);
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

            tabPageRegistryCurrentUser.Enter += TabPage_Enter;
            tabPageRegistryAllUsers.Enter += TabPage_Enter;
            tabPageFolderCurrentUser.Enter += TabPage_Enter;
            tabPageFolderAllUsers.Enter += TabPage_Enter;
            tabPageTaskSсheduler.Enter += TabPage_Enter;
        }

        private void TabPage_Enter(object sender, EventArgs e)
        {
            TabPage currentTab = sender as TabPage;
            if (currentTab == null) return;

            // Сбрасываем выбор всех ListView, кроме текущей вкладки
            if (currentTab != tabPageRegistryCurrentUser) listViewRegistryCurrentUser.SelectedItems.Clear();
            if (currentTab != tabPageRegistryAllUsers) listViewRegistryAllUsers.SelectedItems.Clear();
            if (currentTab != tabPageFolderCurrentUser) listViewFolderCurrentUser.SelectedItems.Clear();
            if (currentTab != tabPageFolderAllUsers) listViewFolderAllUsers.SelectedItems.Clear();
            if (currentTab != tabPageTaskSсheduler) listViewTaskScheduler.SelectedItems.Clear();


            // Сброс информации на label
            labelInfo.Text = string.Empty;

            // Ваши сообщения о правах администратора
            if (currentTab == tabPageRegistryAllUsers || currentTab == tabPageFolderAllUsers)
            {
                if (ApplicationAccess.CurrentAccess == ApplicationAccess.Access.User)
                {
                    labelInfo.ForeColor = Color.Red;
                    labelInfo.Text = "Нету прав на этот раздел реестра (только чтение), необходимо права АДМИНА";
                }
            }
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
                if (listViewRegistryCurrentUser.Items.Count > 0)
                {
                    ContextMenuStrip contextMenu = CreateContextMenuItemForRegistry(StartupType.RegistryCurrentUser, ApplicationAccess.CurrentAccess);
                    contextMenu.Show(listViewRegistryCurrentUser, e.Location);
                }
            }
        }

        private void listViewRegistryAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewRegistryAllUsers.Items.Count > 0)
                {
                    ContextMenuStrip contextMenu = CreateContextMenuItemForRegistry(StartupType.RegistryLocalMachine, ApplicationAccess.CurrentAccess);
                    contextMenu.Show(listViewRegistryAllUsers, e.Location);
                }
            }
        }

        private void listViewFolderCurrentUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewFolderCurrentUser.Items.Count > 0)
                {
                    ContextMenuStrip contextMenu = CreateContextMenuItemForFolder(StartupType.StartupFolderCurrentUser, ApplicationAccess.CurrentAccess);
                    contextMenu.Show(listViewFolderCurrentUser, e.Location);
                }
            }
        }

        private void listViewFolderAllUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewFolderAllUsers.Items.Count > 0)
                {
                    ContextMenuStrip contextMenu = CreateContextMenuItemForFolder(StartupType.StartupFolderAllUsers, ApplicationAccess.CurrentAccess);
                    contextMenu.Show(listViewFolderAllUsers, e.Location);
                }
            }
        }

        private void listViewTaskScheduler_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewTaskScheduler.Items.Count > 0)
                {
                    ContextMenuStrip contextMenu = CreateContextMenuItemForTaskScheduler(ApplicationAccess.CurrentAccess);
                    contextMenu.Show(listViewTaskScheduler, e.Location);
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadControl?.Invoke(this, EventArgs.Empty);
        }

        private void buttonDeleteUnusedRecords_Click(object sender, EventArgs e)
        {
            DeleteUnusedRecords_Click?.Invoke(this, EventArgs.Empty);
        }

        public void DisplayItems(List<object> items, StartupType type)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

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
                case StartupType.None:
                    return;
            }

            if (currentListView == null)
            {
                return;
            }

            currentListView.BeginUpdate();
            currentListView.Items.Clear();

            foreach (object item in items)
            {
                ListViewItem viewItem = null;

                switch (item)
                {
                    case StartupItemRegistry registryItem:
                        viewItem = new ListViewItem(registryItem.RegistryName);
                        viewItem.SubItems.Add(GetStateText(registryItem.State));
                        viewItem.SubItems.Add(registryItem.Path);

                        if (registryItem.Type.HasFlag(StartupType.RegistryLocalMachine))
                        {
                            viewItem.SubItems.Add(registryItem.Is32Bit ? "32 бит" : "");
                        }
                        viewItem.Tag = registryItem;
                        break;
                    case StartupItemFolder folderItem:
                        viewItem = new ListViewItem(folderItem.NameExtracted);
                        viewItem.SubItems.Add(GetStateText(folderItem.State));
                        viewItem.SubItems.Add(folderItem.Path);
                        viewItem.Tag = folderItem;
                        break;
                    case TaskSchedulerItem taskSchedulerItem:
                        viewItem = new ListViewItem(taskSchedulerItem.Name);
                        viewItem.SubItems.Add(taskSchedulerItem.State.ToString());
                        viewItem.SubItems.Add(taskSchedulerItem.Path);
                        viewItem.SubItems.Add(taskSchedulerItem.Arguments);
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

        private ContextMenuStrip CreateContextMenuItemForRegistry(StartupType startupType, ApplicationAccess.Access access)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem? openPathExplorer = new ToolStripMenuItem("Открыть в проводнике");
            ToolStripMenuItem? copyPath = new ToolStripMenuItem("Копировать путь");
            ToolStripMenuItem? changeState = null;
            ToolStripMenuItem? deleteItem = null;

            switch (access)
            {
                case ApplicationAccess.Access.User:
                    switch (startupType)
                    {
                        case StartupType.RegistryCurrentUser:
                            General();
                            break;
                    }
                    break;
                case ApplicationAccess.Access.Admin:
                    switch (startupType)
                    {
                        case StartupType.RegistryCurrentUser:
                        case StartupType.RegistryLocalMachine:
                            General();
                            break;
                    }
                    break;
            }
            openPathExplorer.Click += (s, e) => OpenExplorerClicked?.Invoke(this, EventArgs.Empty);
            copyPath.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);

            if (openPathExplorer != null) { contextMenuStrip.Items.Add(openPathExplorer); }
            if (copyPath != null) { contextMenuStrip.Items.Add(copyPath); }
            if (changeState != null) { contextMenuStrip.Items.Add(changeState); }
            if (deleteItem != null) { contextMenuStrip.Items.Add(deleteItem); }

            return contextMenuStrip;

            void General()
            {
                changeState = new ToolStripMenuItem("Изменить состояние");
                deleteItem = new ToolStripMenuItem("Удалить из реестра");

                changeState.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
                deleteItem.Click += (s, e) => DeleteRegistryRecordClick?.Invoke(this, EventArgs.Empty);
            }
        }
        private ContextMenuStrip CreateContextMenuItemForFolder(StartupType startupType, ApplicationAccess.Access access)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem? openPathExplorer = new ToolStripMenuItem("Открыть в проводнике");
            ToolStripMenuItem? copyPath = new ToolStripMenuItem("Копировать путь");
            ToolStripMenuItem? changeState = null;

            switch (access)
            {
                case ApplicationAccess.Access.User:
                    switch (startupType)
                    {
                        case StartupType.StartupFolderCurrentUser:
                            General();
                            break;
                    }
                    break;
                case ApplicationAccess.Access.Admin:
                    switch (startupType)
                    {
                        case StartupType.StartupFolderCurrentUser:
                        case StartupType.StartupFolderAllUsers:
                            General();
                            break;
                    }
                    break;
            }
            openPathExplorer.Click += (s, e) => OpenExplorerClicked?.Invoke(this, EventArgs.Empty);
            copyPath.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);

            if (openPathExplorer != null) { contextMenuStrip.Items.Add(openPathExplorer); }
            if (copyPath != null) { contextMenuStrip.Items.Add(copyPath); }
            if (changeState != null) { contextMenuStrip.Items.Add(changeState); }

            return contextMenuStrip;

            void General()
            {
                changeState = new ToolStripMenuItem("Изменить состояние");
                changeState.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
            }
        }

        private ContextMenuStrip CreateContextMenuItemForTaskScheduler(ApplicationAccess.Access access)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem? openPathExplorer = new ToolStripMenuItem("Открыть в проводнике");
            ToolStripMenuItem? additionally = new ToolStripMenuItem("Дополнительно");
            ToolStripMenuItem? copyPath = new ToolStripMenuItem("Копировать путь");
            ToolStripMenuItem? onORoff = new ToolStripMenuItem("Включить/Выключить");
            ToolStripMenuItem? completeTheTask = new ToolStripMenuItem("Завершить выполнение задачи");
            ToolStripMenuItem? startTheTask = new ToolStripMenuItem("Выполнить задачу");
            ToolStripMenuItem? deleteTaskItem = new ToolStripMenuItem("Удалить задачу");

            switch (access)
            {
                case ApplicationAccess.Access.User:
                case ApplicationAccess.Access.Admin:
                    openPathExplorer.Click += (s, e) => OpenExplorerClicked?.Invoke(this, EventArgs.Empty);
                    additionally.Click += (s, e) => ViewDetailClick?.Invoke(this, EventArgs.Empty);
                    copyPath.Click += (s, e) => CopyClipboardClicked?.Invoke(this, EventArgs.Empty);
                    onORoff.Click += (s, e) => ChangeStateSelectedItems?.Invoke(this, EventArgs.Empty);
                    completeTheTask.Click += (s, e) => CompleteTaskClick?.Invoke(this, EventArgs.Empty);
                    startTheTask.Click += (s, e) => RunTaskClick?.Invoke(this, EventArgs.Empty);
                    deleteTaskItem.Click += (s, e) => DeleteTaskClick?.Invoke(this, EventArgs.Empty);
                    break;
            }
            contextMenuStrip.Items.AddRange
                (
                    new ToolStripMenuItem[] {
                    openPathExplorer,
                    additionally,
                    copyPath,
                    onORoff,
                    completeTheTask,
                    startTheTask,
                    deleteTaskItem
                    }
                );
            return contextMenuStrip;
        }
    }
}