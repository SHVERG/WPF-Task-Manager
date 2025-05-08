using System;
using System.Reflection.Emit;

namespace WpfTaskManager
{
    public class Report : Prototype
    {
        private string s_completed, projectName, reliable, result;
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

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
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

        public string Reliable
        {
            get
            {
                return reliable;
            }
            set
            {
                reliable = value;
                OnPropertyChanged();
            }
        }

        public Report(string name, string projectName, string reliable, DateTime startdate, DateTime deadline, string s_completed, string result, TimeSpan timespent)
        {
            Name = name;
            ProjectName = projectName;
            Reliable = reliable;
            StartDate = startdate;
            Deadline = deadline;
            S_Completed = s_completed;
            Result = result;
            Timespent = timespent;
        }
    }
}
