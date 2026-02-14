namespace Computer_Maintenance.View.Forms
{
    public partial class ViewDetailTaskSchedulerItem_Form : Form
    {
        public ViewDetailTaskSchedulerItem_Form(string author, string description, DateTime createDate)
        {
            InitializeComponent();

            textBoxAuthor.Text = author;
            labelDateCreate.Text = createDate.ToString("dd.MM.yyyy HH:mm:ss");
            richTextBoxDescription.AppendText(description);
        }
    }
}
