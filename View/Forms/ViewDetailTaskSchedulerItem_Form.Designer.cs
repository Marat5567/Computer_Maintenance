namespace Computer_Maintenance.View.Forms
{
    partial class ViewDetailTaskSchedulerItem_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            labelDateCreate = new Label();
            textBoxAuthor = new TextBox();
            richTextBoxDescription = new RichTextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(46, 15);
            label1.TabIndex = 0;
            label1.Text = "Автор :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 48);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 0;
            label2.Text = "Дата создания :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 76);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 0;
            label3.Text = "Описание :";
            // 
            // labelDateCreate
            // 
            labelDateCreate.AutoSize = true;
            labelDateCreate.Location = new Point(120, 48);
            labelDateCreate.Name = "labelDateCreate";
            labelDateCreate.Size = new Size(38, 15);
            labelDateCreate.TabIndex = 1;
            labelDateCreate.Text = "label4";
            // 
            // textBoxAuthor
            // 
            textBoxAuthor.HideSelection = false;
            textBoxAuthor.Location = new Point(64, 15);
            textBoxAuthor.Name = "textBoxAuthor";
            textBoxAuthor.ReadOnly = true;
            textBoxAuthor.Size = new Size(408, 23);
            textBoxAuthor.TabIndex = 2;
            // 
            // richTextBoxDescription
            // 
            richTextBoxDescription.HideSelection = false;
            richTextBoxDescription.Location = new Point(86, 76);
            richTextBoxDescription.Name = "richTextBoxDescription";
            richTextBoxDescription.ReadOnly = true;
            richTextBoxDescription.Size = new Size(386, 73);
            richTextBoxDescription.TabIndex = 3;
            richTextBoxDescription.Text = "";
            // 
            // ViewDetailTaskSchedulerItem_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 154);
            Controls.Add(richTextBoxDescription);
            Controls.Add(textBoxAuthor);
            Controls.Add(labelDateCreate);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ViewDetailTaskSchedulerItem_Form";
            Text = "Подробное описание";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelDateCreate;
        private TextBox textBoxAuthor;
        private RichTextBox richTextBoxDescription;
    }
}