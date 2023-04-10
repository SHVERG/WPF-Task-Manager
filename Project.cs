using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public class Project
    {
        [Key] public int IdProject { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public string Completed { get; set; }
        public int State { get; set; }
        
        public Project() { }
        
        public Project(string name, string description, string deadline)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
        }

        public Project(string name, string deadline)
        {
            Name = name;
            Description = null;
            Deadline = deadline;
        }

    }
}
