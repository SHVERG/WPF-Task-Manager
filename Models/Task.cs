using System;
using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class Task : Prototype
    {
        [Key] public int IdTask { get; set; }
        private int idProject;
        private int timespent;

        public int IdProject 
        {
            get { 
                return idProject; 
            } 
            set
            {
                if (idProject != value)
                {
                    idProject = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Timespent 
        { 
            get
            {
                return timespent;
            }
            set
            {
                if (timespent != value)
                {
                    timespent = value;
                    OnPropertyChanged();
                }
            }
        }

        public Task() { }

        public Task(int idProject, string name, string description, DateTime deadline)
        {
            IdProject = idProject;
            Name = name;
            Description = description;
            Deadline = deadline;
        }
    }
}
