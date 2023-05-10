using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WpfTaskManager
{
    public abstract class Prototype : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private DateTime deadline;
        private DateTime? completed;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
