using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public class AppViewModel : INotifyPropertyChanged
    {
        AppContext db = new AppContext();
        RelayCommand refreshCommand;
        RelayCommand addProjCommand;
        RelayCommand editProjCommand;
        RelayCommand addTaskCommand;
        RelayCommand EditTaskCommand;
        RelayCommand StartTaskCommand;
        RelayCommand CompleteTaskCommand;

        private Project selectedProj;
        private bool isProjSelected = false;
        private Task selectedTask;
        private bool isTaskSelected = false;
        private bool isTaskIncompleted = false;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<Task> ProjTasks { get; set; }

        public AppViewModel()
        {
            Projects = new ObservableCollection<Project>(db.Projects);
            Tasks = new ObservableCollection<Task>(db.Tasks);
            ProjTasks = new ObservableCollection<Task>();
        }

        public bool IsProjSelected
        {
            get 
            { 
                return isProjSelected; 
            }
            set
            {
                isProjSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTaskSelected
        {
            get 
            { 
                return isTaskSelected; 
            }
            set
            {
                isTaskSelected = value; 
                OnPropertyChanged();
            }
        }

        public bool IsTaskIncompleted
        {
            get
            { 
                return isTaskIncompleted; 
            }
            set
            {
                isTaskIncompleted = value; 
                OnPropertyChanged();
            }
        }

        public Project SelectedProj
        {
            get 
            { 
                return selectedProj; 
            }
            set
            {
                selectedProj = value;

                if (selectedProj != null)
                {
                    IsProjSelected = true;
                    foreach (Task t in db.Tasks)
                    {
                        if (t.IdProject == SelectedProj.IdProject && !ProjTasks.Contains(t))
                            ProjTasks.Insert(0, t);
                        else if (t.IdProject != SelectedProj.IdProject && ProjTasks.Contains(t))
                            ProjTasks.Remove(t);
                    }
                }
                else
                {
                    IsProjSelected = false;
                }

                OnPropertyChanged();
            }
        }

        public Task SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value; 
                
                if (selectedTask != null)
                {
                    IsTaskSelected = true;
                    if (SelectedTask.Completed != null)
                        IsTaskIncompleted = false;
                    else
                        IsTaskIncompleted = true;
                }
                else
                {
                    IsTaskSelected = false;
                    IsTaskIncompleted = false;
                }

                OnPropertyChanged();
            }
        }

        /*
        private string DaysLeft(DateTime deadline)
        {
            if (deadline > DateTime.Now)
                return (deadline - DateTime.Now).Days.ToString() + " day(s) " + (deadline - DateTime.Now).Hours + " hour(s) " + (deadline - DateTime.Now).Minutes + " minute(s) left";
            else
                return "Expired";
        }
        */

        public void Refresh()
        {
        }

        public RelayCommand RefreshCommand { 
            get {
                return refreshCommand ?? (refreshCommand = new RelayCommand((o) =>
                {
                    ((DataGrid)o).Items.Refresh();
                }));
            }
        }

        public RelayCommand AddProjCommand
        {
            get
            {
                return addProjCommand ?? (addProjCommand = new RelayCommand((o) =>
                {
                    AddProject w = new AddProject();
                    //this.Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.Value)
                    {
                        TimeSpan time = new TimeSpan(23, 59, 59);

                        if (w.Deadline_timepicker.SelectedTime != null)
                        {
                            time = ((DateTime)w.Deadline_timepicker.SelectedTime).TimeOfDay;
                        }

                        if (w.Deadline_datepicker.SelectedDate == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                        {
                            MessageBox mb = new MessageBox();
                            mb.Owner = w;
                            mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                            return;
                        }

                        DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);

                        Project p = new Project(w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);

                        db.Projects.Add(p);
                        db.SaveChanges();
                        //RefreshProjs();
                    }
                }));
            }
        }

        public RelayCommand EditProjCommand
        {
            get
            {
                return editProjCommand ?? (editProjCommand = new RelayCommand((o) =>
                {
                    EditWindow w = new EditWindow(selectedProj);
                    //w.Owner = this;
                    //this.Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.Value)
                    {
                        Project edit = db.Projects.Find(selectedProj.IdProject);

                        edit.Name = w.Name_textbox.Text.Trim();
                        edit.Description = w.Description_textbox.Text.Trim();

                        db.SaveChanges();

                        //Refresh_db();
                    }
                }));
            }
        }

        public RelayCommand AddTaskCommand { 
            get
            {
                return addTaskCommand ?? (addTaskCommand = new RelayCommand((o) =>
                {
                    AddTask w = new AddTask(SelectedProj.IdProject);
                    //w.Owner = this;
                    //this.Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.Value)
                    {
                        TimeSpan time = new TimeSpan(23, 59, 59);

                        if (w.Deadline_timepicker.SelectedTime != null)
                        {
                            time = ((DateTime)w.Deadline_timepicker.SelectedTime).TimeOfDay;
                        }

                        if (db.Projects.Find(SelectedProj.IdProject).Deadline.Date == w.Deadline_datepicker.SelectedDate && time > db.Projects.Find(SelectedProj.IdProject).Deadline.TimeOfDay)
                            time = db.Projects.Find(SelectedProj.IdProject).Deadline.TimeOfDay;

                        if (w.Deadline_datepicker.SelectedDate == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                        {
                            MessageBox mb = new MessageBox();
                            //mb.Owner = this;
                            mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                            return;
                        }

                        DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);

                        Task t = new Task(SelectedProj.IdProject, w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);

                        db.Tasks.Add(t);
                        db.SaveChanges();

                        //Refresh_db();

                        //Refresh_tasks(db.Projects.Find(idProj()));

                        //Tasks_listbox.SelectedItem = t;
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
