using System.Data.Entity;

namespace WpfTaskManager
{
    public class AppContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ProjectsActivityLogs> ProjectsLogs { get; set; }
        public DbSet<TasksActivityLogs> TasksLogs { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
