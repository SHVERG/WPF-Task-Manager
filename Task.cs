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
        //public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public string Completed { get; set; }
        public int State { get; set; }
        public int Timespent { get; set; }

        public Task() { }

        public Task(int idProject, string name, string description, string deadline)
        {
            IdProject = idProject;
            Name = name;
            Description = description;
            Deadline = deadline;
        }

        public Task(int idProject, string name, string deadline)
        {
            IdProject = idProject;
            Name = name;
            Description = null;
            Deadline = deadline;
        }

    }
}
