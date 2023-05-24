using System;

namespace WpfTaskManager
{
    public class Report : Prototype
    {
        private string s_completed, projectName;
        private TimeSpan timespent;

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

        public string S_Completed
        {
            get
            {
                return s_completed;
            }
            set
            {
                s_completed = value;
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

        public Report(string name, string projectName, DateTime deadline, string s_completed, TimeSpan timespent)
        {
            Name = name;
            ProjectName = projectName;
            Deadline = deadline;
            S_Completed = s_completed;
            Timespent = timespent;
        }
    }
}
