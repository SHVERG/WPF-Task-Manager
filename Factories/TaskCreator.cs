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

            using (AppContext db = new AppContext())
            {
                if (db.Projects.Find(vm.proj.IdProject).Deadline.Date == vm.Deadline && time > db.Projects.Find(vm.proj.IdProject).Deadline.TimeOfDay)
                    time = db.Projects.Find(vm.proj.IdProject).Deadline.TimeOfDay;

                if (vm.Deadline == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                {
                    return null;
                }
            }

            DateTime date = ((DateTime)vm.Deadline).Add(time);
            return new Task(vm.proj.IdProject, vm.Name.Trim(), vm.Description, date, vm.SelectedUser.IdUser);
        }
    }
}
