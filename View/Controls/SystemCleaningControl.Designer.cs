namespace Computer_Maintenance.Controls
{
    partial class SystemCleaningControl
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
            flowLayoutPanelDrives = new FlowLayoutPanel();
            label2 = new Label();
            panelScroolable = new Panel();
            buttonRefreshDrives = new Button();
            tableLayoutPanelElementsPosition = new TableLayoutPanel();
            panelButtons = new Panel();
            buttonStartScanClean = new Button();
            flowLayoutPanelInfoDrives = new FlowLayoutPanel();
            checkBoxSaveFileDeleteFailLogs = new CheckBox();
            buttonStartClean = new Button();
            label3 = new Label();
            panelScroolable.SuspendLayout();
            tableLayoutPanelElementsPosition.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(364, 12);
            label1.Name = "label1";
            label1.Size = new Size(172, 25);
            label1.TabIndex = 0;
            label1.Text = "Очистка системы";
            // 
            // flowLayoutPanelDrives
            // 
            flowLayoutPanelDrives.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanelDrives.AutoSize = true;
            flowLayoutPanelDrives.BackColor = SystemColors.Control;
            flowLayoutPanelDrives.Location = new Point(3, 3);
            flowLayoutPanelDrives.Name = "flowLayoutPanelDrives";
            flowLayoutPanelDrives.Size = new Size(879, 0);
            flowLayoutPanelDrives.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(6, 68);
            label2.Name = "label2";
            label2.Size = new Size(122, 21);
            label2.TabIndex = 2;
            label2.Text = "Все накопители";
            // 
            // panelScroolable
            // 
            panelScroolable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelScroolable.AutoScroll = true;
            panelScroolable.BackColor = SystemColors.Control;
            panelScroolable.Controls.Add(buttonRefreshDrives);
            panelScroolable.Controls.Add(tableLayoutPanelElementsPosition);
            panelScroolable.Controls.Add(label3);
            panelScroolable.Controls.Add(label2);
            panelScroolable.Location = new Point(3, 40);
            panelScroolable.Name = "panelScroolable";
            panelScroolable.Size = new Size(891, 821);
            panelScroolable.TabIndex = 4;
            // 
            // buttonRefreshDrives
            // 
            buttonRefreshDrives.Location = new Point(6, 16);
            buttonRefreshDrives.Name = "buttonRefreshDrives";
            buttonRefreshDrives.Size = new Size(150, 42);
            buttonRefreshDrives.TabIndex = 6;
            buttonRefreshDrives.Text = "Обновить накопители";
            buttonRefreshDrives.UseVisualStyleBackColor = true;
            buttonRefreshDrives.Click += buttonRefreshDrives_Click;
            // 
            // tableLayoutPanelElementsPosition
            // 
            tableLayoutPanelElementsPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanelElementsPosition.AutoSize = true;
            tableLayoutPanelElementsPosition.ColumnCount = 1;
            tableLayoutPanelElementsPosition.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelElementsPosition.Controls.Add(flowLayoutPanelDrives, 0, 0);
            tableLayoutPanelElementsPosition.Controls.Add(panelButtons, 0, 1);
            tableLayoutPanelElementsPosition.Controls.Add(flowLayoutPanelInfoDrives, 0, 2);
            tableLayoutPanelElementsPosition.Controls.Add(checkBoxSaveFileDeleteFailLogs, 0, 3);
            tableLayoutPanelElementsPosition.Controls.Add(buttonStartClean, 0, 4);
            tableLayoutPanelElementsPosition.Location = new Point(3, 89);
            tableLayoutPanelElementsPosition.Name = "tableLayoutPanelElementsPosition";
            tableLayoutPanelElementsPosition.RowCount = 5;
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.Size = new Size(885, 143);
            tableLayoutPanelElementsPosition.TabIndex = 5;
            // 
            // panelButtons
            // 
            panelButtons.AutoSize = true;
            panelButtons.Controls.Add(buttonStartScanClean);
            panelButtons.Dock = DockStyle.Fill;
            panelButtons.Location = new Point(3, 9);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(879, 54);
            panelButtons.TabIndex = 8;
            // 
            // buttonStartScanClean
            // 
            buttonStartScanClean.Location = new Point(3, 1);
            buttonStartScanClean.Name = "buttonStartScanClean";
            buttonStartScanClean.RightToLeft = RightToLeft.No;
            buttonStartScanClean.Size = new Size(147, 50);
            buttonStartScanClean.TabIndex = 4;
            buttonStartScanClean.Text = "Начать сканирование для очистки";
            buttonStartScanClean.UseVisualStyleBackColor = true;
            buttonStartScanClean.Click += buttonStartScanClean_Click;
            // 
            // flowLayoutPanelInfoDrives
            // 
            flowLayoutPanelInfoDrives.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanelInfoDrives.AutoSize = true;
            flowLayoutPanelInfoDrives.Location = new Point(3, 69);
            flowLayoutPanelInfoDrives.Name = "flowLayoutPanelInfoDrives";
            flowLayoutPanelInfoDrives.Size = new Size(879, 0);
            flowLayoutPanelInfoDrives.TabIndex = 6;
            // 
            // checkBoxSaveFileDeleteFailLogs
            // 
            checkBoxSaveFileDeleteFailLogs.AutoSize = true;
            checkBoxSaveFileDeleteFailLogs.Location = new Point(3, 75);
            checkBoxSaveFileDeleteFailLogs.Name = "checkBoxSaveFileDeleteFailLogs";
            checkBoxSaveFileDeleteFailLogs.Size = new Size(285, 19);
            checkBoxSaveFileDeleteFailLogs.TabIndex = 7;
            checkBoxSaveFileDeleteFailLogs.Text = "Сохранить логи ошибок при уадлении файлов";
            checkBoxSaveFileDeleteFailLogs.UseVisualStyleBackColor = true;
            checkBoxSaveFileDeleteFailLogs.CheckedChanged += checkBoxSaveFileDeleteFailLogs_CheckedChanged;
            // 
            // buttonStartClean
            // 
            buttonStartClean.Location = new Point(3, 100);
            buttonStartClean.Name = "buttonStartClean";
            buttonStartClean.Size = new Size(150, 40);
            buttonStartClean.TabIndex = 7;
            buttonStartClean.Text = "Начать очистку";
            buttonStartClean.UseVisualStyleBackColor = true;
            buttonStartClean.Click += buttonStartClean_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(517, 196);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 3;
            // 
            // SystemCleaningControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelScroolable);
            Controls.Add(label1);
            Name = "SystemCleaningControl";
            Size = new Size(894, 861);
            Load += SystemCleaningControl_Load;
            panelScroolable.ResumeLayout(false);
            panelScroolable.PerformLayout();
            tableLayoutPanelElementsPosition.ResumeLayout(false);
            tableLayoutPanelElementsPosition.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private FlowLayoutPanel flowLayoutPanelDrives;
        private Label label2;
        private Panel panelScroolable;
        private Label label3;
        private Button buttonStartScanClean;
        private TableLayoutPanel tableLayoutPanelElementsPosition;
        private FlowLayoutPanel flowLayoutPanelInfoDrives;
        private Button buttonStartClean;
        private Button buttonRefreshDrives;
        private CheckBox checkBoxSaveFileDeleteFailLogs;
        private Panel panelButtons;
    }
}
