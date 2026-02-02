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

        (bool isFile, string path) SelectedPath { get; set; }
        public StartupType LastFolderSelectionSource { get; set; }
        void DisplayRegistryStartupItems(List<StartupItemRegistry> startupItems, StartupType type);
        void DisplayFolderStartupItems(List<StartupItemFolder> startupItems, StartupType type);
        void DisplayTaskSchedulerItems(List<TaskSchedulerItem> startupItems);
        void DisplayItems(List<object> items, StartupType type);
        public List<object> GetSelectedItems();
    }
}
