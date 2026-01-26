using Microsoft.Win32.TaskScheduler;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct TaskSchedulerItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public TaskState State { get; set; }
    }
}
