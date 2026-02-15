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
            labelTimeCreate = new Label();
            textBoxAuthor = new TextBox();
            richTextBoxDescription = new RichTextBox();
            label4 = new Label();
            label5 = new Label();
            labelNextTimeStart = new Label();
            labelOldTimeStart = new Label();
            label6 = new Label();
            label7 = new Label();
            richTextBoxTriggers = new RichTextBox();
            labelLastStartResult = new Label();
            label8 = new Label();
            richTextBoxFullScript = new RichTextBox();
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
            label2.Location = new Point(12, 112);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 0;
            label2.Text = "Создан :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 302);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 0;
            label3.Text = "Описание :";
            // 
            // labelTimeCreate
            // 
            labelTimeCreate.AutoSize = true;
            labelTimeCreate.Location = new Point(70, 112);
            labelTimeCreate.Name = "labelTimeCreate";
            labelTimeCreate.Size = new Size(38, 15);
            labelTimeCreate.TabIndex = 1;
            labelTimeCreate.Text = "label4";
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
            richTextBoxDescription.Location = new Point(86, 302);
            richTextBoxDescription.Name = "richTextBoxDescription";
            richTextBoxDescription.ReadOnly = true;
            richTextBoxDescription.Size = new Size(386, 73);
            richTextBoxDescription.TabIndex = 3;
            richTextBoxDescription.Text = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 137);
            label4.Name = "label4";
            label4.Size = new Size(166, 15);
            label4.TabIndex = 0;
            label4.Text = "Время следующего запуска :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 165);
            label5.Name = "label5";
            label5.Size = new Size(154, 15);
            label5.TabIndex = 0;
            label5.Text = "Время прошлого запуска :";
            // 
            // labelNextTimeStart
            // 
            labelNextTimeStart.AutoSize = true;
            labelNextTimeStart.Location = new Point(184, 137);
            labelNextTimeStart.Name = "labelNextTimeStart";
            labelNextTimeStart.Size = new Size(38, 15);
            labelNextTimeStart.TabIndex = 4;
            labelNextTimeStart.Text = "label6";
            // 
            // labelOldTimeStart
            // 
            labelOldTimeStart.AutoSize = true;
            labelOldTimeStart.Location = new Point(172, 165);
            labelOldTimeStart.Name = "labelOldTimeStart";
            labelOldTimeStart.Size = new Size(38, 15);
            labelOldTimeStart.TabIndex = 4;
            labelOldTimeStart.Text = "label6";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 193);
            label6.Name = "label6";
            label6.Size = new Size(256, 15);
            label6.TabIndex = 0;
            label6.Text = "Результат последнего запуска (код ошибки) :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 225);
            label7.Name = "label7";
            label7.Size = new Size(66, 15);
            label7.TabIndex = 0;
            label7.Text = "Триггеры :";
            // 
            // richTextBoxTriggers
            // 
            richTextBoxTriggers.HideSelection = false;
            richTextBoxTriggers.Location = new Point(86, 222);
            richTextBoxTriggers.Name = "richTextBoxTriggers";
            richTextBoxTriggers.ReadOnly = true;
            richTextBoxTriggers.Size = new Size(386, 74);
            richTextBoxTriggers.TabIndex = 3;
            richTextBoxTriggers.Text = "";
            // 
            // labelLastStartResult
            // 
            labelLastStartResult.AutoSize = true;
            labelLastStartResult.Location = new Point(274, 193);
            labelLastStartResult.Name = "labelLastStartResult";
            labelLastStartResult.Size = new Size(38, 15);
            labelLastStartResult.TabIndex = 4;
            labelLastStartResult.Text = "label6";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 54);
            label8.Name = "label8";
            label8.Size = new Size(68, 15);
            label8.TabIndex = 0;
            label8.Text = "Сценарий :";
            // 
            // richTextBoxFullScript
            // 
            richTextBoxFullScript.HideSelection = false;
            richTextBoxFullScript.Location = new Point(86, 51);
            richTextBoxFullScript.Name = "richTextBoxFullScript";
            richTextBoxFullScript.ReadOnly = true;
            richTextBoxFullScript.Size = new Size(386, 58);
            richTextBoxFullScript.TabIndex = 5;
            richTextBoxFullScript.Text = "";
            // 
            // ViewDetailTaskSchedulerItem_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 381);
            Controls.Add(richTextBoxFullScript);
            Controls.Add(labelLastStartResult);
            Controls.Add(labelOldTimeStart);
            Controls.Add(labelNextTimeStart);
            Controls.Add(richTextBoxTriggers);
            Controls.Add(richTextBoxDescription);
            Controls.Add(textBoxAuthor);
            Controls.Add(labelTimeCreate);
            Controls.Add(label8);
            Controls.Add(label3);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximumSize = new Size(496, 420);
            Name = "ViewDetailTaskSchedulerItem_Form";
            Text = "Подробное описание";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelTimeCreate;
        private TextBox textBoxAuthor;
        private RichTextBox richTextBoxDescription;
        private Label label4;
        private Label label5;
        private Label labelNextTimeStart;
        private Label labelOldTimeStart;
        private Label label6;
        private Label label7;
        private RichTextBox richTextBoxTriggers;
        private Label labelLastStartResult;
        private Label label8;
        private RichTextBox richTextBoxFullScript;
    }
}