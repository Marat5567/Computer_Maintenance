using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;

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
        event EventHandler DeleteFolderItemClick;

        (bool isFile, string path) SelectedPath { get; set; }
        public StartupType LastFolderSelectionSource { get; set; }
        void DisplayItems(List<object> items, StartupType type);
        public List<object> GetSelectedItems();
    }
}
