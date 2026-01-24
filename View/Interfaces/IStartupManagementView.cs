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
        void DisplayRegistryStartupItems(List<StartupItemRegistry> startupItems);
        void DisplayFolderStartupItems(List<StartupItemFolder> startupItems, StartupType type);
        List<StartupItemFolder> GetSelectedStartupItems_Folder();
    }
}
