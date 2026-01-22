using Computer_Maintenance.Model.Structs.StartupManagement;

namespace Computer_Maintenance.View.Interfaces
{
    public interface IStartupManagementView
    {
        event EventHandler LoadControl;

        event EventHandler DeleteSelectedItem;
        event EventHandler ChangeSelectedItem;
        void DisplayStartupItems(List<StartupItem> startupItems);
        List<StartupItem> GetSelectedStartupItems();
    }
}
