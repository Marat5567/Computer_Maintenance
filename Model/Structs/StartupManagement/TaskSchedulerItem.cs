using Computer_Maintenance.Model.Enums.StartupManagement;
using Microsoft.Win32.TaskScheduler;

namespace Computer_Maintenance.Model.Structs.StartupManagement
{
    public struct TaskSchedulerItem
    {
        public string File { get; set; }
        public string NameExtractedFromPath { get; set; }
        public string PathExtracted { get; set; }
        public string OriginalPath { get; set; }
        public string Arguments { get; set; }
        public TaskState State { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime NextTimeStart { get; set; }
        public DateTime OldTimeStart { get; set; }
        public int ResultLastStart { get; set; }
        public string Trigger { get; set; }
        public StartupType Type { get; set; }
    }
}
