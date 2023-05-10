using System;
using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class Project : Prototype
    {
        [Key] public int IdProject { get; set; }

        public Project() { }
        
        public Project(string name, string description, DateTime deadline)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
        }
    }
}
