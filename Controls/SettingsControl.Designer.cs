namespace Computer_Maintenance.Controls
{
    partial class SettingsControl
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
            buttonSave = new Button();
            radioButtonThemeLight = new RadioButton();
            radioButtonThemeDark = new RadioButton();
            groupBoxThemeButtons = new GroupBox();
            groupBoxThemeButtons.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(20, 122);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(176, 48);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Сохранить";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // radioButtonThemeLight
            // 
            radioButtonThemeLight.AutoSize = true;
            radioButtonThemeLight.Location = new Point(6, 22);
            radioButtonThemeLight.Name = "radioButtonThemeLight";
            radioButtonThemeLight.Size = new Size(69, 19);
            radioButtonThemeLight.TabIndex = 3;
            radioButtonThemeLight.TabStop = true;
            radioButtonThemeLight.Text = "Светлая";
            radioButtonThemeLight.UseVisualStyleBackColor = true;
            radioButtonThemeLight.CheckedChanged += radioButtonThemeLight_CheckedChanged;
            // 
            // radioButtonThemeDark
            // 
            radioButtonThemeDark.AutoSize = true;
            radioButtonThemeDark.Location = new Point(81, 22);
            radioButtonThemeDark.Name = "radioButtonThemeDark";
            radioButtonThemeDark.Size = new Size(66, 19);
            radioButtonThemeDark.TabIndex = 3;
            radioButtonThemeDark.TabStop = true;
            radioButtonThemeDark.Text = "Тёмная";
            radioButtonThemeDark.UseVisualStyleBackColor = true;
            radioButtonThemeDark.CheckedChanged += radioButtonThemeDark_CheckedChanged;
            // 
            // groupBoxThemeButtons
            // 
            groupBoxThemeButtons.Controls.Add(radioButtonThemeLight);
            groupBoxThemeButtons.Controls.Add(radioButtonThemeDark);
            groupBoxThemeButtons.Location = new Point(20, 45);
            groupBoxThemeButtons.Name = "groupBoxThemeButtons";
            groupBoxThemeButtons.RightToLeft = RightToLeft.No;
            groupBoxThemeButtons.Size = new Size(148, 54);
            groupBoxThemeButtons.TabIndex = 4;
            groupBoxThemeButtons.TabStop = false;
            groupBoxThemeButtons.Text = "Тема";
            // 
            // SettingsControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBoxThemeButtons);
            Controls.Add(buttonSave);
            Name = "SettingsControl";
            Size = new Size(1280, 600);
            Load += SettingsControl_Load;
            groupBoxThemeButtons.ResumeLayout(false);
            groupBoxThemeButtons.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button buttonSave;
        private RadioButton radioButtonThemeLight;
        private RadioButton radioButtonThemeDark;
        private GroupBox groupBoxThemeButtons;
    }
}
