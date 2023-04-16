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
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Completed { get; set; }
        
        public Project() { }
        
        public Project(string name, string description, DateTime deadline)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
        }

        public Project(string name, DateTime deadline)
        {
            Name = name;
            Description = null;
            Deadline = deadline;
        }

    }
}
