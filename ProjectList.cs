using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public class ProjectList
    {
        public string Name { get; set; }
        public string State { get; set; }
        public DateTime Deadline { get; set; }

        public ProjectList(string name, string state, DateTime deadline)
        {
            Name = name;
            State = state;
            Deadline = deadline;
        }
    }
}
