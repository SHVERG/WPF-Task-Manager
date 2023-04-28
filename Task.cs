using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WpfTaskManager
{
    public class Task : INotifyPropertyChanged
    {
        [Key] public int IdTask { get; set; }
        private int idProject;
        private string name;
        private string description;
        private DateTime deadline;
        private DateTime? completed;
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
        
        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Description 
        { 
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataType(DataType.Date)]
        public DateTime Deadline 
        { 
            get
            {
                return deadline;
            }
            set
            {
                if (deadline != value)
                {
                    deadline = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataType(DataType.Date)]
        public DateTime? Completed 
        { 
            get
            {
                return completed;
            } 
            set
            {
                if (completed != value)
                {
                    completed = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string SDeadline
        {
            get
            {
                return deadline.ToString();
            }
        }

        public string SCompleted
        {
            get
            {
                if (completed != null)
                {
                    return completed.ToString();
                }
                else
                {
                    return "Not Completed";
                }
            }
        }

        public TimeSpan TSTimespent
        {
            get
            {
                return new TimeSpan(timespent*10000000);
            }
        }
    }
}
