
namespace Computer_Maintenance.Model.Enums.StartupManagement
{
    [Flags]
    public enum StartupType
    {
        RegistryCurrentUser = 1 << 0,
        RegistryLocalMachine = 1 << 1,
        StartupFolderCurrentUser = 1 << 2,
        StartupFolderAllUsers = 1 << 3,
        TaskScheduler = 1 << 4,
        Service = 1 << 5,
        GroupPolicy = 1 << 6,
        ShellExtension = 1 << 7
    }
}
