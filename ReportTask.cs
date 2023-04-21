using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public class ReportTask
    {
        public string Name { get; set; }
        public string Project_Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Completed { get; set; }

        public ReportTask(string name, string project, DateTime deadline, string completed)
        {
            Name = name;
            Project_Name = project;
            Deadline = deadline;
            Completed = completed;
        }
    }
}
