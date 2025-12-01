namespace Computer_Maintenance.Core.Services
{
    public static class MessageService
    {
        public static DialogResult ShowMessage(
            IWin32Window owner,
            string msg,
            string headerName = "",
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel,
            MessageBoxIcon icon = MessageBoxIcon.Information
        )
        {
            return MessageBox.Show(owner, msg, headerName, buttons, icon);
        }
    }
}
