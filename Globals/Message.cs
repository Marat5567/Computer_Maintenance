namespace Computer_Maintenance.Globals
{
    public static class Message
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
