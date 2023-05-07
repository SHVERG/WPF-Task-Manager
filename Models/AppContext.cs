using System.Data.Entity;

namespace WpfTaskManager
{
    public class AppContext : DbContext
    {
        private static AppContext instance;
        
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        private AppContext() : base("DefaultConnection") { }

        public static AppContext Create()
        {
            if (instance == null)
            {
                instance = new AppContext();
            }
            return instance;
        }

        public static AppContext ReCreate()
        {
            instance = new AppContext();
            return instance;
        }
    }
}
