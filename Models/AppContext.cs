using System.Data.Entity;

namespace WpfTaskManager
{
    public class AppContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
