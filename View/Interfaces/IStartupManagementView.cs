using Computer_Maintenance.Model.Enums.StartupManagement;


namespace Computer_Maintenance.View.Interfaces
{
    public interface IStartupManagementView
    {
        event EventHandler LoadControl;
        event EventHandler OpenExplorerClicked;
        event EventHandler ChangeStateSelectedItems;
        event EventHandler CopyClipboardClicked;
        event EventHandler DeleteUnusedRecords_Click;
        event EventHandler DeleteRegistryRecordClick;
        event EventHandler ViewDetailClick;
        event EventHandler CompleteTaskClick;
        event EventHandler RunTaskClick;
        event EventHandler DeleteTaskClick;
        event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        void DisplayItems(List<object> items, StartupType type);
    }

    public class SelectionChangedEventArgs : EventArgs
    {
        public List<object> SelectedItems { get; }
        public StartupType SelectionSource { get; }

        public SelectionChangedEventArgs(List<object> selectedItems, StartupType selectionSource)
        {
            SelectedItems = selectedItems;
            SelectionSource = selectionSource;
        }
    }
}