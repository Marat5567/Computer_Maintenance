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
            tableLayoutPanel = new TableLayoutPanel();
            panelRegistryCurrentUser = new Panel();
            listViewRegistryCurrentUser = new ListView();
            label2 = new Label();
            panelRegistryAllUsers = new Panel();
            listViewRegistryAllUsers = new ListView();
            label3 = new Label();
            panelFolderCurrentUser = new Panel();
            label4 = new Label();
            listViewFolderCurrentUser = new ListView();
            panelFolderAllUsers = new Panel();
            label5 = new Label();
            listViewFolderAllUsers = new ListView();
            tableLayoutPanel.SuspendLayout();
            panelRegistryCurrentUser.SuspendLayout();
            panelRegistryAllUsers.SuspendLayout();
            panelFolderCurrentUser.SuspendLayout();
            panelFolderAllUsers.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(305, 13);
            label1.Name = "label1";
            label1.Size = new Size(276, 25);
            label1.TabIndex = 0;
            label1.Text = "Управление автозагрузками";
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.AutoScroll = true;
            tableLayoutPanel.BackColor = Color.Transparent;
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(panelRegistryCurrentUser, 0, 1);
            tableLayoutPanel.Controls.Add(panelRegistryAllUsers, 0, 2);
            tableLayoutPanel.Controls.Add(panelFolderCurrentUser, 0, 3);
            tableLayoutPanel.Controls.Add(panelFolderAllUsers, 0, 4);
            tableLayoutPanel.Location = new Point(0, 41);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 7;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new Size(894, 784);
            tableLayoutPanel.TabIndex = 1;
            // 
            // panelRegistryCurrentUser
            // 
            panelRegistryCurrentUser.AutoSize = true;
            panelRegistryCurrentUser.Controls.Add(listViewRegistryCurrentUser);
            panelRegistryCurrentUser.Controls.Add(label2);
            panelRegistryCurrentUser.Dock = DockStyle.Fill;
            panelRegistryCurrentUser.Location = new Point(3, 3);
            panelRegistryCurrentUser.Name = "panelRegistryCurrentUser";
            panelRegistryCurrentUser.Size = new Size(888, 240);
            panelRegistryCurrentUser.TabIndex = 0;
            // 
            // listViewRegistryCurrentUser
            // 
            listViewRegistryCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewRegistryCurrentUser.FullRowSelect = true;
            listViewRegistryCurrentUser.Location = new Point(0, 34);
            listViewRegistryCurrentUser.Name = "listViewRegistryCurrentUser";
            listViewRegistryCurrentUser.Size = new Size(888, 206);
            listViewRegistryCurrentUser.TabIndex = 1;
            listViewRegistryCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.MouseDown += listViewRegistryCurrentUser_MouseDown;
            listViewRegistryCurrentUser.Resize += listViewRegistryCurrentUser_Resize;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(3, 10);
            label2.Name = "label2";
            label2.Size = new Size(261, 21);
            label2.TabIndex = 0;
            label2.Text = "Реестр (текущий пользователь)";
            // 
            // panelRegistryAllUsers
            // 
            panelRegistryAllUsers.Controls.Add(listViewRegistryAllUsers);
            panelRegistryAllUsers.Controls.Add(label3);
            panelRegistryAllUsers.Dock = DockStyle.Fill;
            panelRegistryAllUsers.Location = new Point(3, 249);
            panelRegistryAllUsers.Name = "panelRegistryAllUsers";
            panelRegistryAllUsers.Size = new Size(888, 240);
            panelRegistryAllUsers.TabIndex = 2;
            // 
            // listViewRegistryAllUsers
            // 
            listViewRegistryAllUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewRegistryAllUsers.FullRowSelect = true;
            listViewRegistryAllUsers.Location = new Point(0, 34);
            listViewRegistryAllUsers.Name = "listViewRegistryAllUsers";
            listViewRegistryAllUsers.Size = new Size(888, 206);
            listViewRegistryAllUsers.TabIndex = 0;
            listViewRegistryAllUsers.UseCompatibleStateImageBehavior = false;
            listViewRegistryAllUsers.MouseDown += listViewRegistryAllUsers_MouseDown;
            listViewRegistryAllUsers.Resize += listViewRegistryAllUsers_Resize;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.Location = new Point(3, 10);
            label3.Name = "label3";
            label3.Size = new Size(220, 21);
            label3.TabIndex = 0;
            label3.Text = "Реестр (все пользователи)";
            // 
            // panelFolderCurrentUser
            // 
            panelFolderCurrentUser.Controls.Add(label4);
            panelFolderCurrentUser.Controls.Add(listViewFolderCurrentUser);
            panelFolderCurrentUser.Dock = DockStyle.Fill;
            panelFolderCurrentUser.Location = new Point(3, 495);
            panelFolderCurrentUser.Name = "panelFolderCurrentUser";
            panelFolderCurrentUser.Size = new Size(888, 140);
            panelFolderCurrentUser.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label4.Location = new Point(3, 10);
            label4.Name = "label4";
            label4.Size = new Size(366, 21);
            label4.TabIndex = 1;
            label4.Text = "Папка автозагрузки (текущий пользователь)";
            // 
            // listViewFolderCurrentUser
            // 
            listViewFolderCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewFolderCurrentUser.FullRowSelect = true;
            listViewFolderCurrentUser.Location = new Point(0, 34);
            listViewFolderCurrentUser.Name = "listViewFolderCurrentUser";
            listViewFolderCurrentUser.Size = new Size(888, 100);
            listViewFolderCurrentUser.TabIndex = 0;
            listViewFolderCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewFolderCurrentUser.MouseDown += listViewFolderCurrentUser_MouseDown;
            listViewFolderCurrentUser.Resize += listViewFolderCurrentUser_Resize;
            // 
            // panelFolderAllUsers
            // 
            panelFolderAllUsers.Controls.Add(label5);
            panelFolderAllUsers.Controls.Add(listViewFolderAllUsers);
            panelFolderAllUsers.Dock = DockStyle.Fill;
            panelFolderAllUsers.Location = new Point(3, 641);
            panelFolderAllUsers.Name = "panelFolderAllUsers";
            panelFolderAllUsers.Size = new Size(888, 140);
            panelFolderAllUsers.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label5.Location = new Point(3, 13);
            label5.Name = "label5";
            label5.Size = new Size(325, 21);
            label5.TabIndex = 1;
            label5.Text = "Папка автозагрузки (все пользователи)";
            // 
            // listViewFolderAllUsers
            // 
            listViewFolderAllUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewFolderAllUsers.FullRowSelect = true;
            listViewFolderAllUsers.Location = new Point(0, 37);
            listViewFolderAllUsers.Name = "listViewFolderAllUsers";
            listViewFolderAllUsers.Size = new Size(888, 100);
            listViewFolderAllUsers.TabIndex = 0;
            listViewFolderAllUsers.UseCompatibleStateImageBehavior = false;
            listViewFolderAllUsers.MouseDown += listViewFolderAllUsers_MouseDown;
            listViewFolderAllUsers.Resize += listViewFolderAllUsers_Resize;
            // 
            // StartupManagementControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(tableLayoutPanel);
            Controls.Add(label1);
            Name = "StartupManagementControl";
            Size = new Size(894, 861);
            Load += StartupManagementControl_Load;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            panelRegistryCurrentUser.ResumeLayout(false);
            panelRegistryCurrentUser.PerformLayout();
            panelRegistryAllUsers.ResumeLayout(false);
            panelRegistryAllUsers.PerformLayout();
            panelFolderCurrentUser.ResumeLayout(false);
            panelFolderCurrentUser.PerformLayout();
            panelFolderAllUsers.ResumeLayout(false);
            panelFolderAllUsers.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TableLayoutPanel tableLayoutPanel;
        private Panel panelRegistryCurrentUser;
        private Label label2;
        private ListView listViewRegistryCurrentUser;
        private Panel panelRegistryAllUsers;
        private ListView listViewRegistryAllUsers;
        private Label label3;
        private Panel panelFolderCurrentUser;
        private ListView listViewFolderCurrentUser;
        private Label label4;
        private Panel panelFolderAllUsers;
        private ListView listViewFolderAllUsers;
        private Label label5;
    }
}
