
namespace Computer_Maintenance.Model.Enums.StartupManagement
{
    public enum StartupType
    {
        RegistryCurrentUser,
        RegistryLocalMachine,
        StartupFolderCurrentUser,
        StartupFolderAllUsers,
        TaskScheduler,
        Service,
        GroupPolicy,
        ShellExtension
    }
}
