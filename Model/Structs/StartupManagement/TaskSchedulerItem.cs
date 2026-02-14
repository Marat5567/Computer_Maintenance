using Computer_Maintenance.Model.Enums.StartupManagement;
using Microsoft.Win32.TaskScheduler;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct TaskSchedulerItem
    {
        public string File { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string OriginalPath { get; set; }
        public TaskState State { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public StartupType Type { get; set; }
    }
}
