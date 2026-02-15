namespace Computer_Maintenance.View.Forms
{
    public partial class ViewDetailTaskSchedulerItem_Form : Form
    {
        public ViewDetailTaskSchedulerItem_Form(string author, string originalScript, string description, DateTime createTime, DateTime nextTimeStart, DateTime oldTimeStart, int resultLastStart, string trigger)
        {
            InitializeComponent();

            textBoxAuthor.Text = author;
            richTextBoxFullScript.AppendText(originalScript);
            labelTimeCreate.Text = createTime == DateTime.MinValue ? "Не определно" : createTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelNextTimeStart.Text = nextTimeStart == DateTime.MinValue ? "Не назначено" : nextTimeStart.ToString("dd.MM.yyyy HH:mm:ss");
            labelOldTimeStart.Text = oldTimeStart == DateTime.MinValue ? "Не определено" : oldTimeStart.ToString("dd.MM.yyyy HH:mm:ss");
            labelLastStartResult.Text = string.IsNullOrEmpty(resultLastStart.ToString()) ? "Нет данных" : $"0x{resultLastStart}";

            richTextBoxTriggers.AppendText(trigger);
            richTextBoxDescription.AppendText(description);
        }
    }
}
