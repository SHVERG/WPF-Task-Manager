using System;

namespace WpfTaskManager
{
    public class TaskCreator : Creator
    {
        public override Prototype Create(AddVM vm)
        {
            TimeSpan time = new TimeSpan(23, 59, 59);

            if (vm.Time != null)
            {
                time = ((DateTime)vm.Time).TimeOfDay;
            }

            //using (AppContext db = new AppContext())
            //{
                if (App.db.Projects.Find(vm.Project.IdProject).Deadline.Date == vm.Deadline && time > App.db.Projects.Find(vm.Project.IdProject).Deadline.TimeOfDay)
                    time = App.db.Projects.Find(vm.Project.IdProject).Deadline.TimeOfDay;

                if (vm.Deadline == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                {
                    return null;
                }
            //}

            DateTime deadlineDate = ((DateTime)vm.Deadline).Add(time);
            return new Task(vm.Project.IdProject, vm.Name.Trim(), vm.Description, vm.StartDate.Value, deadlineDate, vm.SelectedUser.IdUser);
        }
    }
}
