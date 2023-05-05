using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfTaskManager
{
    public class Report : INotifyPropertyChanged
    {
        private string name, projectName, completed;
        private DateTime deadline;
        private TimeSpan timespent;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value; 
                OnPropertyChanged();
            }
        }

        public DateTime Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value; 
                OnPropertyChanged();
            }
        }

        public string Completed
        {
            get
            {
                return completed;
            }
            set
            {
                completed = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Timespent
        {
            get
            {
                return timespent;
            }
            set
            {
                timespent = value;
                OnPropertyChanged();
            }
        }

        public Report(string name, string projectName, DateTime deadline, string completed, TimeSpan timespent)
        {
            Name = name;
            ProjectName = projectName;
            Deadline = deadline;
            Completed = completed;
            Timespent = timespent;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
