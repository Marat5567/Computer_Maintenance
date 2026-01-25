using Computer_Maintenance.Model.Enums.StartupManagement;
using Computer_Maintenance.Model.Structs.StartupManagement;

namespace Computer_Maintenance.View.Interfaces
{
    public interface IStartupManagementView
    {
        event EventHandler LoadControl;

        event EventHandler DeleteSelectedItem_Registry;
        event EventHandler DeleteSelectedItem_Folder;
        event EventHandler ChangeSelectedItem;
        event EventHandler ChangeSelectedItem_StateStartup_Registry;
        event EventHandler ChangeSelectedItem_StateStartup_Folder;
        event EventHandler OpenExplorerClicked;


        (bool isFile, string path) SelectedPath { get; set; }
        public StartupType LastFolderSelectionSource { get; set; }
        void DisplayRegistryStartupItems(List<StartupItemRegistry> startupItems, StartupType type);
        void DisplayFolderStartupItems(List<StartupItemFolder> startupItems, StartupType type);
        List<StartupItemFolder> GetSelectedStartupItems_Folder();
        List<StartupItemRegistry> GetSelectedStartupItems_Registry();
    }
}
