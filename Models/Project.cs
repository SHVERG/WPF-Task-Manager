using System;
using System.ComponentModel.DataAnnotations;

namespace WpfTaskManager
{
    public class Project : Prototype
    {
        [Key] public int IdProject { get; set; }
        private int? idCat;

        public int? IdCat
        {
            get 
            { 
                return idCat; 
            }
            set
            {
                idCat = value;
                OnPropertyChanged();
            }
        }

        public Project() { }

        public Project(string name, string description, int? idCat, DateTime deadline)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
            IdCat = idCat;
        }
    }
}
