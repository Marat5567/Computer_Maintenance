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
            tabPageTaskSheduler = new TabPage();
            listViewRegistryCurrentUser = new ListView();
            listViewRegistryAllUsers = new ListView();
            listViewFolderCurrentUser = new ListView();
            panelFolderAllUsers = new Panel();
            listViewFolderAllUsers = new ListView();
            panelFolderCurrentUser = new Panel();
            panelRegistryAllUsers = new Panel();
            panelRegistryCurrentUser = new Panel();
            tabControlAutoRun.SuspendLayout();
            panelFolderAllUsers.SuspendLayout();
            panelFolderCurrentUser.SuspendLayout();
            panelRegistryAllUsers.SuspendLayout();
            panelRegistryCurrentUser.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(287, 14);
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
            tabControlAutoRun.Controls.Add(tabPageTaskSheduler);
            tabControlAutoRun.Location = new Point(3, 58);
            tabControlAutoRun.Name = "tabControlAutoRun";
            tabControlAutoRun.SelectedIndex = 0;
            tabControlAutoRun.Size = new Size(888, 542);
            tabControlAutoRun.TabIndex = 2;
            // 
            // tabPageRegistryCurrentUser
            // 
            tabPageRegistryCurrentUser.Location = new Point(4, 24);
            tabPageRegistryCurrentUser.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageRegistryCurrentUser.Name = "tabPageRegistryCurrentUser";
            tabPageRegistryCurrentUser.Size = new Size(880, 514);
            tabPageRegistryCurrentUser.TabIndex = 0;
            tabPageRegistryCurrentUser.Text = "Реестр (текущий пользователь)";
            tabPageRegistryCurrentUser.UseVisualStyleBackColor = true;
            // 
            // tabPageRegistryAllUsers
            // 
            tabPageRegistryAllUsers.Location = new Point(4, 24);
            tabPageRegistryAllUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageRegistryAllUsers.Name = "tabPageRegistryAllUsers";
            tabPageRegistryAllUsers.Size = new Size(880, 353);
            tabPageRegistryAllUsers.TabIndex = 1;
            tabPageRegistryAllUsers.Text = "Реестр (все пользователи)";
            tabPageRegistryAllUsers.UseVisualStyleBackColor = true;
            // 
            // tabPageFolderCurrentUser
            // 
            tabPageFolderCurrentUser.Location = new Point(4, 24);
            tabPageFolderCurrentUser.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageFolderCurrentUser.Name = "tabPageFolderCurrentUser";
            tabPageFolderCurrentUser.Size = new Size(880, 353);
            tabPageFolderCurrentUser.TabIndex = 2;
            tabPageFolderCurrentUser.Text = "Папка автозагрузка (текущий пользователь)";
            tabPageFolderCurrentUser.UseVisualStyleBackColor = true;
            // 
            // tabPageFolderAllUsers
            // 
            tabPageFolderAllUsers.Location = new Point(4, 24);
            tabPageFolderAllUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageFolderAllUsers.Name = "tabPageFolderAllUsers";
            tabPageFolderAllUsers.Size = new Size(880, 353);
            tabPageFolderAllUsers.TabIndex = 3;
            tabPageFolderAllUsers.Text = "Папка автозагрзка (все пользователи)";
            tabPageFolderAllUsers.UseVisualStyleBackColor = true;
            // 
            // tabPageTaskSheduler
            // 
            tabPageTaskSheduler.Location = new Point(4, 24);
            tabPageFolderAllUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabPageTaskSheduler.Name = "tabPageTaskSheduler";
            tabPageTaskSheduler.Size = new Size(880, 353);
            tabPageTaskSheduler.TabIndex = 4;
            tabPageTaskSheduler.Text = "Планировщик задач";
            tabPageTaskSheduler.UseVisualStyleBackColor = true;
            // 
            // listViewRegistryCurrentUser
            // 
            listViewRegistryCurrentUser.Dock = DockStyle.Fill;
            listViewRegistryCurrentUser.FullRowSelect = true;
            listViewRegistryCurrentUser.Name = "listViewRegistryCurrentUser";
            listViewRegistryCurrentUser.TabIndex = 1;
            listViewRegistryCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewRegistryCurrentUser.MouseDown += listViewRegistryCurrentUser_MouseDown;
            listViewRegistryCurrentUser.Resize += listViewRegistryCurrentUser_Resize;
            // 
            // listViewRegistryAllUsers
            // 
            listViewRegistryAllUsers.Dock = DockStyle.Fill;
            listViewRegistryAllUsers.FullRowSelect = true;
            listViewRegistryAllUsers.Name = "listViewRegistryAllUsers";
            listViewRegistryAllUsers.TabIndex = 0;
            listViewRegistryAllUsers.UseCompatibleStateImageBehavior = false;
            listViewRegistryAllUsers.MouseDown += listViewRegistryAllUsers_MouseDown;
            listViewRegistryAllUsers.Resize += listViewRegistryAllUsers_Resize;
            // 
            // listViewFolderCurrentUser
            // 
            listViewFolderCurrentUser.Dock = DockStyle.Fill;
            listViewFolderCurrentUser.FullRowSelect = true;
            listViewFolderCurrentUser.Name = "listViewFolderCurrentUser";
            listViewFolderCurrentUser.TabIndex = 0;
            listViewFolderCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewFolderCurrentUser.MouseDown += listViewFolderCurrentUser_MouseDown;
            listViewFolderCurrentUser.Resize += listViewFolderCurrentUser_Resize;

            // 
            // listViewFolderAllUsers
            // 
            listViewFolderAllUsers.Dock = DockStyle.Fill;
            listViewFolderAllUsers.FullRowSelect = true;
            listViewFolderAllUsers.Name = "listViewFolderAllUsers";
            listViewFolderAllUsers.TabIndex = 0;
            listViewFolderAllUsers.UseCompatibleStateImageBehavior = false;
            listViewFolderAllUsers.MouseDown += listViewFolderAllUsers_MouseDown;
            listViewFolderAllUsers.Resize += listViewFolderAllUsers_Resize;
            // 
            // panelFolderAllUsers
            // 
            panelFolderAllUsers.Controls.Add(listViewFolderAllUsers);
            panelFolderAllUsers.Dock = DockStyle.Fill;
            panelFolderAllUsers.Name = "panelFolderAllUsers";
            panelFolderAllUsers.TabIndex = 4;
            // 
            // panelFolderCurrentUser
            // 
            panelFolderCurrentUser.Controls.Add(listViewFolderCurrentUser);
            panelFolderCurrentUser.Dock = DockStyle.Fill;
            panelFolderCurrentUser.Name = "panelFolderCurrentUser";
            panelFolderCurrentUser.TabIndex = 3;
            // 
            // panelRegistryAllUsers
            // 
            panelRegistryAllUsers.Controls.Add(listViewRegistryAllUsers);
            panelRegistryAllUsers.Dock = DockStyle.Fill;
            panelRegistryAllUsers.Name = "panelRegistryAllUsers";
            panelRegistryAllUsers.TabIndex = 2;

            // 
            // panelRegistryCurrentUser
            // 
            panelRegistryCurrentUser.Controls.Add(listViewRegistryCurrentUser);
            panelRegistryCurrentUser.Dock = DockStyle.Fill;
            panelRegistryCurrentUser.Name = "panelRegistryCurrentUser";
            panelRegistryCurrentUser.TabIndex = 0;

            // 
            // StartupManagementControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;

            Controls.Add(tabControlAutoRun);
            Controls.Add(label1);

            Name = "StartupManagementControl";
            Size = new Size(894, 600);
            Load += StartupManagementControl_Load;
            tabControlAutoRun.ResumeLayout(false);
            panelFolderAllUsers.ResumeLayout(false);
            panelFolderAllUsers.PerformLayout();
            panelFolderCurrentUser.ResumeLayout(false);
            panelFolderCurrentUser.PerformLayout();
            panelRegistryAllUsers.ResumeLayout(false);
            panelRegistryAllUsers.PerformLayout();
            panelRegistryCurrentUser.ResumeLayout(false);
            panelRegistryCurrentUser.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListView listViewRegistryCurrentUser;
        private ListView listViewRegistryAllUsers;
        private ListView listViewFolderCurrentUser;
        private ListView listViewFolderAllUsers;
        private TabControl tabControlAutoRun;
        private TabPage tabPageRegistryCurrentUser;
        private TabPage tabPageRegistryAllUsers;
        private TabPage tabPageFolderCurrentUser;
        private TabPage tabPageFolderAllUsers;
        private TabPage tabPageTaskSheduler;
        private Panel panelFolderAllUsers;
        private Panel panelFolderCurrentUser;
        private Panel panelRegistryAllUsers;
        private Panel panelRegistryCurrentUser;
    }
}
