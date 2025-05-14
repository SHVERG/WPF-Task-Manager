using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class TasksActivityLogs : LogBase
    {
        [Key] public int IdLog { get; set; }
        public int IdTask { get; set; }

        public AppContext AppContext
        {
            get => default;
            set
            {
            }
        }

        public TasksActivityLogs() { }

        public TasksActivityLogs(int idTask, int action, string message) : base(action, message)
        {
            IdTask = IdTask;
        }
    }
}
