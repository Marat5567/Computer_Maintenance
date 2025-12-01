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
            tableLayoutPanelElementsPosition = new TableLayoutPanel();
            buttonStartScan = new Button();
            flowLayoutPanelInfoDrives = new FlowLayoutPanel();
            label3 = new Label();
            buttonStartClean = new Button();
            panelScroolable.SuspendLayout();
            tableLayoutPanelElementsPosition.SuspendLayout();
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
            label2.Location = new Point(3, 9);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 2;
            label2.Text = "Все накопители";
            // 
            // panelScroolable
            // 
            panelScroolable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelScroolable.AutoScroll = true;
            panelScroolable.BackColor = SystemColors.Control;
            panelScroolable.Controls.Add(tableLayoutPanelElementsPosition);
            panelScroolable.Controls.Add(label3);
            panelScroolable.Controls.Add(label2);
            panelScroolable.Location = new Point(3, 40);
            panelScroolable.Name = "panelScroolable";
            panelScroolable.Size = new Size(891, 821);
            panelScroolable.TabIndex = 4;
            // 
            // tableLayoutPanelElementsPosition
            // 
            tableLayoutPanelElementsPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanelElementsPosition.AutoSize = true;
            tableLayoutPanelElementsPosition.ColumnCount = 1;
            tableLayoutPanelElementsPosition.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelElementsPosition.Controls.Add(flowLayoutPanelDrives, 0, 0);
            tableLayoutPanelElementsPosition.Controls.Add(buttonStartScan, 0, 1);
            tableLayoutPanelElementsPosition.Controls.Add(flowLayoutPanelInfoDrives, 0, 2);
            tableLayoutPanelElementsPosition.Controls.Add(buttonStartClean, 0, 3);
            tableLayoutPanelElementsPosition.Location = new Point(3, 36);
            tableLayoutPanelElementsPosition.Name = "tableLayoutPanelElementsPosition";
            tableLayoutPanelElementsPosition.RowCount = 4;
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.RowStyles.Add(new RowStyle());
            tableLayoutPanelElementsPosition.Size = new Size(885, 104);
            tableLayoutPanelElementsPosition.TabIndex = 5;
            // 
            // buttonStartScan
            // 
            buttonStartScan.Location = new Point(3, 9);
            buttonStartScan.Name = "buttonStartScan";
            buttonStartScan.RightToLeft = RightToLeft.No;
            buttonStartScan.Size = new Size(150, 40);
            buttonStartScan.TabIndex = 4;
            buttonStartScan.Text = "Начать сканирование";
            buttonStartScan.UseVisualStyleBackColor = true;
            buttonStartScan.Click += buttonStartScan_Click;
            // 
            // flowLayoutPanelInfoDrives
            // 
            flowLayoutPanelInfoDrives.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanelInfoDrives.AutoSize = true;
            flowLayoutPanelInfoDrives.Location = new Point(3, 55);
            flowLayoutPanelInfoDrives.Name = "flowLayoutPanelInfoDrives";
            flowLayoutPanelInfoDrives.Size = new Size(879, 0);
            flowLayoutPanelInfoDrives.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(517, 196);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 3;
            // 
            // buttonStartClean
            // 
            buttonStartClean.Location = new Point(3, 61);
            buttonStartClean.Name = "buttonStartClean";
            buttonStartClean.Size = new Size(150, 40);
            buttonStartClean.TabIndex = 7;
            buttonStartClean.Text = "Начать очистку";
            buttonStartClean.UseVisualStyleBackColor = true;
            buttonStartClean.Click += buttonStartClean_Click;
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private FlowLayoutPanel flowLayoutPanelDrives;
        private Label label2;
        private Panel panelScroolable;
        private Label label3;
        private Button buttonStartScan;
        private TableLayoutPanel tableLayoutPanelElementsPosition;
        private FlowLayoutPanel flowLayoutPanelInfoDrives;
        private Button buttonStartClean;
    }
}
