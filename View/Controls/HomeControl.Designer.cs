namespace Computer_Maintenance.Controls
{
    partial class HomeControl
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
            splitContainer = new SplitContainer();
            treeViewListFunctionality = new TreeView();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(treeViewListFunctionality);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.ForeColor = SystemColors.ControlText;
            splitContainer.Size = new Size(1280, 800);
            splitContainer.SplitterDistance = 426;
            splitContainer.SplitterWidth = 8;
            splitContainer.TabIndex = 0;
            // 
            // treeViewListFunctionality
            // 
            treeViewListFunctionality.Dock = DockStyle.Fill;
            treeViewListFunctionality.Location = new Point(0, 0);
            treeViewListFunctionality.Name = "treeViewListFunctionality";
            treeViewListFunctionality.Size = new Size(426, 800);
            treeViewListFunctionality.TabIndex = 0;
            treeViewListFunctionality.AfterSelect += treeViewListFunctionality_AfterSelect;
            // 
            // HomeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer);
            Name = "HomeControl";
            Size = new Size(1280, 800);
            Load += HomeControl_Load;
            splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer;
        private TreeView treeViewListFunctionality;
    }
}
