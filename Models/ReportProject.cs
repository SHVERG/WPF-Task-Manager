using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public class ReportProject
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Completed { get; set; }

        public ReportProject(string name, DateTime deadline, string completed)
        {
            Name = name;
            Deadline = deadline;
            Completed = completed;
        }
    }
}
