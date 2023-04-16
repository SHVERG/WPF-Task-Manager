using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public class Task
    {
        [Key] public int IdTask { get; set; }
        public int IdProject { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Completed { get; set; }
        public int Timespent { get; set; }

        public Task() { }

        public Task(int idProject, string name, string description, DateTime deadline)
        {
            IdProject = idProject;
            Name = name;
            Description = description;
            Deadline = deadline;
        }

        public Task(int idProject, string name, DateTime deadline)
        {
            IdProject = idProject;
            Name = name;
            Description = null;
            Deadline = deadline;
        }

    }
}
