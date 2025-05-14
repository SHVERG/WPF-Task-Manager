using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class ProjectsActivityLogs : LogBase
    {
        [Key] public int IdLog { get; set; }
        public int IdProject { get; set; }

        public AppContext AppContext
        {
            get => default;
            set
            {
            }
        }

        public ProjectsActivityLogs() { }

        public ProjectsActivityLogs(int idProject, int action, string message) : base(action, message)
        {
            IdProject = idProject;
        }
    }
}
