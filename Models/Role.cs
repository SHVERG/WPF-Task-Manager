using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class Role
    {
        [Key] public int IdRole { get; set; }
        public string Name { get; set; }
        public int CanPerformTasks { get; set; }
        public int CanCreateTasks { get; set; }
        public int CanAcceptRegs { get; set; }
        public int CanDeleteTasks { get; set; }
        public int CanCreateReports { get; set; }
        public int CanOpenLogs { get; set; }
        public int CanEditTasks { get; set; }
        public int CanExportTasks { get; set; }

    }
}
