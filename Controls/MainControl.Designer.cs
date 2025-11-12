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
            pictureBoxHome = new PictureBox();
            pictureBoxSettings = new PictureBox();
            tableLayoutPanelContolsPositions = new TableLayoutPanel();
            panelconsBottom = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHome).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSettings).BeginInit();
            tableLayoutPanelContolsPositions.SuspendLayout();
            panelconsBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxHome
            // 
            pictureBoxHome.Anchor = AnchorStyles.Bottom;
            pictureBoxHome.BackgroundImage = Resource.home;
            pictureBoxHome.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxHome.Location = new Point(570, 5);
            pictureBoxHome.Name = "pictureBoxHome";
            pictureBoxHome.Size = new Size(55, 55);
            pictureBoxHome.TabIndex = 0;
            pictureBoxHome.TabStop = false;
            pictureBoxHome.Click += pictureBoxHome_Click;
            pictureBoxHome.MouseEnter += pictureBoxHome_MouseEnter;
            pictureBoxHome.MouseLeave += pictureBoxHome_MouseLeave;
            // 
            // pictureBoxSettings
            // 
            pictureBoxSettings.Anchor = AnchorStyles.Bottom;
            pictureBoxSettings.BackgroundImage = Resource.Settings_Icon;
            pictureBoxSettings.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxSettings.Location = new Point(652, 5);
            pictureBoxSettings.Name = "pictureBoxSettings";
            pictureBoxSettings.Size = new Size(55, 55);
            pictureBoxSettings.TabIndex = 0;
            pictureBoxSettings.TabStop = false;
            pictureBoxSettings.Click += pictureBoxSettings_Click;
            pictureBoxSettings.MouseEnter += pictureBoxSettings_MouseEnter;
            pictureBoxSettings.MouseLeave += pictureBoxSettings_MouseLeave;
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
            panelconsBottom.BackColor = SystemColors.Control;
            panelconsBottom.Controls.Add(pictureBoxSettings);
            panelconsBottom.Controls.Add(pictureBoxHome);
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
            ((System.ComponentModel.ISupportInitialize)pictureBoxHome).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSettings).EndInit();
            tableLayoutPanelContolsPositions.ResumeLayout(false);
            panelconsBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxHome;
        private PictureBox pictureBoxSettings;
        private TableLayoutPanel tableLayoutPanelContolsPositions;
        private Panel panelconsBottom;
    }
}
