using System;

namespace WpfTaskManager
{
    public class ProjectCreator : Creator
    {
        public override Prototype Create(AddVM vm)
        {
            TimeSpan time = new TimeSpan(23, 59, 59);

            if (vm.Time != null)
            {
                time = ((DateTime)vm.Time).TimeOfDay;
            }

            if (vm.Deadline == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
            {
                return null;
            }

            DateTime date = ((DateTime)vm.Deadline).Add(time);
            return new Project(vm.Name.Trim(), vm.Description, vm.IdCat, vm.StartDate.Value, date);
        }
    }
}
