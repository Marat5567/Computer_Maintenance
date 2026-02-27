namespace Computer_Maintenance.View.Controls
{
    partial class StartupManagementControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            tabControlAutoRun = new TabControl();
            tabPageRegistryCurrentUser = new TabPage();
            tabPageRegistryAllUsers = new TabPage();
            tabPageFolderCurrentUser = new TabPage();
            tabPageFolderAllUsers = new TabPage();
            tabPageTaskSсheduler = new TabPage();
            listViewRegistryCurrentUser = new ListView();
            listViewRegistryAllUsers = new ListView();
            listViewFolderCurrentUser = new ListView();
            listViewTaskScheduler = new ListView();
            panelFolderAllUsers = new Panel();
            listViewFolderAllUsers = new ListView();
            panelFolderCurrentUser = new Panel();
            panelRegistryAllUsers = new Panel();
            panelRegistryCurrentUser = new Panel();
            panelTaskScheduler = new Panel();
            buttonRefresh = new Button();
            labelInfo = new Label();
            tabControlAutoRun.SuspendLayout();
            panelFolderAllUsers.SuspendLayout();
            panelFolderCurrentUser.SuspendLayout();
            panelRegistryAllUsers.SuspendLayout();
            panelRegistryCurrentUser.SuspendLayout();
            panelTaskScheduler.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(305, 15);
            label1.Name = "label1";
            label1.Size = new Size(276, 25);
            label1.TabIndex = 0;
            label1.Text = "Управление автозагрузками";
            // 
            // tabControlAutoRun
            // 
            tabControlAutoRun.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlAutoRun.Controls.Add(tabPageRegistryCurrentUser);
            tabControlAutoRun.Controls.Add(tabPageRegistryAllUsers);
            tabControlAutoRun.Controls.Add(tabPageFolderCurrentUser);
            tabControlAutoRun.Controls.Add(tabPageFolderAllUsers);
            tabControlAutoRun.Controls.Add(tabPageTaskSсheduler);
            tabControlAutoRun.Location = new Point(3, 102);
            tabControlAutoRun.Name = "tabControlAutoRun";
            tabControlAutoRun.SelectedIndex = 0;
            tabControlAutoRun.Size = new Size(888, 498);
            tabControlAutoRun.TabIndex = 2;
            // 
            // tabPageRegistryCurrentUser
            // 
            tabPageRegistryCurrentUser.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageRegistryCurrentUser.Location = new Point(4, 24);
            tabPageRegistryCurrentUser.Name = "tabPageRegistryCurrentUser";
            tabPageRegistryCurrentUser.Size = new Size(880, 470);
            tabPageRegistryCurrentUser.TabIndex = 0;
            tabPageRegistryCurrentUser.Text = "Реестр (текущий пользователь)";
            tabPageRegistryCurrentUser.UseVisualStyleBackColor = true;
            // 
            // tabPageRegistryAllUsers
            // 
            tabPageRegistryAllUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageRegistryAllUsers.Location = new Point(4, 24);
            tabPageRegistryAllUsers.Name = "tabPageRegistryAllUsers";
            tabPageRegistryAllUsers.Size = new Size(880, 470);
            tabPageRegistryAllUsers.TabIndex = 1;
            tabPageRegistryAllUsers.Text = "Реестр (все пользователи)";
            tabPageRegistryAllUsers.UseVisualStyleBackColor = true;
            // 
            // tabPageFolderCurrentUser
            // 
            tabPageFolderCurrentUser.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageFolderCurrentUser.Location = new Point(4, 24);
            tabPageFolderCurrentUser.Name = "tabPageFolderCurrentUser";
            tabPageFolderCurrentUser.Size = new Size(880, 470);
            tabPageFolderCurrentUser.TabIndex = 2;
            tabPageFolderCurrentUser.Text = "Папка автозагрузка (текущий пользователь)";
            tabPageFolderCurrentUser.UseVisualStyleBackColor = true;
            // 
            // tabPageFolderAllUsers
            // 
            tabPageFolderAllUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageFolderAllUsers.Location = new Point(4, 24);
            tabPageFolderAllUsers.Name = "tabPageFolderAllUsers";
            tabPageFolderAllUsers.Size = new Size(880, 470);
            tabPageFolderAllUsers.TabIndex = 3;
            tabPageFolderAllUsers.Text = "Папка автозагрзка (все пользователи)";
            tabPageFolderAllUsers.UseVisualStyleBackColor = true;
            // 
            // tabPageTaskSсheduler
            // 
            tabPageTaskSсheduler.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageTaskSсheduler.Location = new Point(4, 24);
            tabPageTaskSсheduler.Name = "tabPageTaskSсheduler";
            tabPageTaskSсheduler.Size = new Size(880, 470);
            tabPageTaskSсheduler.TabIndex = 4;
            tabPageTaskSсheduler.Text = "Планировщик задач";
            tabPageTaskSсheduler.UseVisualStyleBackColor = true;
            // 
            // listViewRegistryCurrentUser
            // 
            listViewRegistryCurrentUser.Dock = DockStyle.Fill;
            listViewRegistryCurrentUser.FullRowSelect = true;
            listViewRegistryCurrentUser.Location = new Point(0, 0);
            listViewRegistryCurrentUser.Name = "listViewRegistryCurrentUser";
            listViewRegistryCurrentUser.Size = new Size(200, 100);
            listViewRegistryCurrentUser.TabIndex = 1;
            listViewRegistryCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewRegistryCurrentUser.MouseDown += listViewRegistryCurrentUser_MouseDown;
            listViewRegistryCurrentUser.Resize += listViewRegistryCurrentUser_Resize;
            // 
            // listViewRegistryAllUsers
            // 
            listViewRegistryAllUsers.Dock = DockStyle.Fill;
            listViewRegistryAllUsers.FullRowSelect = true;
            listViewRegistryAllUsers.Location = new Point(0, 0);
            listViewRegistryAllUsers.Name = "listViewRegistryAllUsers";
            listViewRegistryAllUsers.Size = new Size(200, 100);
            listViewRegistryAllUsers.TabIndex = 0;
            listViewRegistryAllUsers.UseCompatibleStateImageBehavior = false;
            listViewRegistryAllUsers.MouseDown += listViewRegistryAllUsers_MouseDown;
            listViewRegistryAllUsers.Resize += listViewRegistryAllUsers_Resize;
            // 
            // listViewFolderCurrentUser
            // 
            listViewFolderCurrentUser.Dock = DockStyle.Fill;
            listViewFolderCurrentUser.FullRowSelect = true;
            listViewFolderCurrentUser.Location = new Point(0, 0);
            listViewFolderCurrentUser.Name = "listViewFolderCurrentUser";
            listViewFolderCurrentUser.Size = new Size(200, 100);
            listViewFolderCurrentUser.TabIndex = 0;
            listViewFolderCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewFolderCurrentUser.MouseDown += listViewFolderCurrentUser_MouseDown;
            listViewFolderCurrentUser.Resize += listViewFolderCurrentUser_Resize;
            // 
            // listViewTaskScheduler
            // 
            listViewTaskScheduler.Dock = DockStyle.Fill;
            listViewTaskScheduler.FullRowSelect = true;
            listViewTaskScheduler.Location = new Point(0, 0);
            listViewTaskScheduler.Name = "listViewTaskScheduler";
            listViewTaskScheduler.Size = new Size(200, 100);
            listViewTaskScheduler.TabIndex = 0;
            listViewTaskScheduler.UseCompatibleStateImageBehavior = false;
            listViewTaskScheduler.MouseDown += listViewTaskScheduler_MouseDown;
            listViewTaskScheduler.Resize += listViewTaskScheduler_Resize;
            // 
            // panelFolderAllUsers
            // 
            panelFolderAllUsers.Controls.Add(listViewFolderAllUsers);
            panelFolderAllUsers.Dock = DockStyle.Fill;
            panelFolderAllUsers.Location = new Point(0, 0);
            panelFolderAllUsers.Name = "panelFolderAllUsers";
            panelFolderAllUsers.Size = new Size(200, 100);
            panelFolderAllUsers.TabIndex = 4;
            // 
            // listViewFolderAllUsers
            // 
            listViewFolderAllUsers.Dock = DockStyle.Fill;
            listViewFolderAllUsers.FullRowSelect = true;
            listViewFolderAllUsers.Location = new Point(0, 0);
            listViewFolderAllUsers.Name = "listViewFolderAllUsers";
            listViewFolderAllUsers.Size = new Size(200, 100);
            listViewFolderAllUsers.TabIndex = 0;
            listViewFolderAllUsers.UseCompatibleStateImageBehavior = false;
            listViewFolderAllUsers.MouseDown += listViewFolderAllUsers_MouseDown;
            listViewFolderAllUsers.Resize += listViewFolderAllUsers_Resize;
            // 
            // panelFolderCurrentUser
            // 
            panelFolderCurrentUser.Controls.Add(listViewFolderCurrentUser);
            panelFolderCurrentUser.Dock = DockStyle.Fill;
            panelFolderCurrentUser.Location = new Point(0, 0);
            panelFolderCurrentUser.Name = "panelFolderCurrentUser";
            panelFolderCurrentUser.Size = new Size(200, 100);
            panelFolderCurrentUser.TabIndex = 3;
            // 
            // panelRegistryAllUsers
            // 
            panelRegistryAllUsers.Controls.Add(listViewRegistryAllUsers);
            panelRegistryAllUsers.Dock = DockStyle.Fill;
            panelRegistryAllUsers.Location = new Point(0, 0);
            panelRegistryAllUsers.Name = "panelRegistryAllUsers";
            panelRegistryAllUsers.Size = new Size(200, 100);
            panelRegistryAllUsers.TabIndex = 2;
            // 
            // panelRegistryCurrentUser
            // 
            panelRegistryCurrentUser.Controls.Add(listViewRegistryCurrentUser);
            panelRegistryCurrentUser.Dock = DockStyle.Fill;
            panelRegistryCurrentUser.Location = new Point(0, 0);
            panelRegistryCurrentUser.Name = "panelRegistryCurrentUser";
            panelRegistryCurrentUser.Size = new Size(200, 100);
            panelRegistryCurrentUser.TabIndex = 0;
            // 
            // panelTaskScheduler
            // 
            panelTaskScheduler.Controls.Add(listViewTaskScheduler);
            panelTaskScheduler.Dock = DockStyle.Fill;
            panelTaskScheduler.Location = new Point(0, 0);
            panelTaskScheduler.Name = "panelTaskScheduler";
            panelTaskScheduler.Size = new Size(200, 100);
            panelTaskScheduler.TabIndex = 1;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(3, 56);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(150, 40);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "Обновить";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelInfo.ForeColor = Color.Red;
            labelInfo.Location = new Point(159, 64);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(48, 21);
            labelInfo.TabIndex = 4;
            labelInfo.Text = "label";
            // 
            // StartupManagementControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(labelInfo);
            Controls.Add(buttonRefresh);
            Controls.Add(tabControlAutoRun);
            Controls.Add(label1);
            ForeColor = SystemColors.ControlText;
            Name = "StartupManagementControl";
            Size = new Size(894, 600);
            Load += StartupManagementControl_Load;
            tabControlAutoRun.ResumeLayout(false);
            panelFolderAllUsers.ResumeLayout(false);
            panelFolderCurrentUser.ResumeLayout(false);
            panelRegistryAllUsers.ResumeLayout(false);
            panelRegistryCurrentUser.ResumeLayout(false);
            panelTaskScheduler.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListView listViewRegistryCurrentUser;
        private ListView listViewRegistryAllUsers;
        private ListView listViewFolderCurrentUser;
        private ListView listViewFolderAllUsers;
        private ListView listViewTaskScheduler;
        private TabControl tabControlAutoRun;
        private TabPage tabPageRegistryCurrentUser;
        private TabPage tabPageRegistryAllUsers;
        private TabPage tabPageFolderCurrentUser;
        private TabPage tabPageFolderAllUsers;
        private TabPage tabPageTaskSсheduler;
        private Panel panelFolderAllUsers;
        private Panel panelFolderCurrentUser;
        private Panel panelRegistryAllUsers;
        private Panel panelRegistryCurrentUser;
        private Panel panelTaskScheduler;
        private Button buttonRefresh;
        private Label labelInfo;
    }
}
