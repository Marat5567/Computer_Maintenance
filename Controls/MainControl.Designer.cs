namespace Computer_Maintenance.Controls
{
    partial class MainControl
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            tableLayoutPanelContolsPositions = new TableLayoutPanel();
            panelconsBottom = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tableLayoutPanelContolsPositions.SuspendLayout();
            panelconsBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom;
            pictureBox1.BackgroundImage = Resource.home;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(564, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(65, 65);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Bottom;
            pictureBox2.BackgroundImage = Resource.Settings_Icon;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(646, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(65, 65);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // tableLayoutPanelContolsPositions
            // 
            tableLayoutPanelContolsPositions.BackColor = SystemColors.Control;
            tableLayoutPanelContolsPositions.ColumnCount = 1;
            tableLayoutPanelContolsPositions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelContolsPositions.Controls.Add(panelconsBottom, 0, 1);
            tableLayoutPanelContolsPositions.Dock = DockStyle.Fill;
            tableLayoutPanelContolsPositions.Location = new Point(0, 0);
            tableLayoutPanelContolsPositions.Name = "tableLayoutPanelContolsPositions";
            tableLayoutPanelContolsPositions.RowCount = 2;
            tableLayoutPanelContolsPositions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelContolsPositions.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanelContolsPositions.Size = new Size(1280, 800);
            tableLayoutPanelContolsPositions.TabIndex = 1;
            // 
            // panelconsBottom
            // 
            panelconsBottom.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelconsBottom.BackColor = SystemColors.AppWorkspace;
            panelconsBottom.Controls.Add(pictureBox2);
            panelconsBottom.Controls.Add(pictureBox1);
            panelconsBottom.Location = new Point(3, 733);
            panelconsBottom.Name = "panelconsBottom";
            panelconsBottom.Size = new Size(1274, 64);
            panelconsBottom.TabIndex = 2;
            // 
            // MainControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelContolsPositions);
            Name = "MainControl";
            Size = new Size(1280, 800);
            Load += MainControl_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tableLayoutPanelContolsPositions.ResumeLayout(false);
            panelconsBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private TableLayoutPanel tableLayoutPanelContolsPositions;
        private Panel panelconsBottom;
    }
}
