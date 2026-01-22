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
            tableLayoutPanel.SuspendLayout();
            panelRegistryCurrentUser.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
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
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.BackColor = Color.Transparent;
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(panelRegistryCurrentUser, 0, 1);
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
            tableLayoutPanel.Size = new Size(894, 820);
            tableLayoutPanel.TabIndex = 1;
            // 
            // panelRegistryCurrentUser
            // 
            panelRegistryCurrentUser.AutoSize = true;
            panelRegistryCurrentUser.BackgroundImageLayout = ImageLayout.None;
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
            listViewRegistryCurrentUser.Size = new Size(888, 200);
            listViewRegistryCurrentUser.TabIndex = 1;
            listViewRegistryCurrentUser.UseCompatibleStateImageBehavior = false;
            listViewRegistryCurrentUser.View = System.Windows.Forms.View.Details;
            listViewRegistryCurrentUser.MouseClick += listViewRegistryCurrentUser_MouseClick;
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TableLayoutPanel tableLayoutPanel;
        private Panel panelRegistryCurrentUser;
        private Label label2;
        private ListView listViewRegistryCurrentUser;
    }
}
