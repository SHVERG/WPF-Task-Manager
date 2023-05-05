using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public class AppVM : INotifyPropertyChanged
    {
        AppContext db;
        RelayCommand refreshCommand;
        RelayCommand reportCommand;

        RelayCommand addProjCommand;
        RelayCommand editProjCommand;
        
        RelayCommand addTaskCommand;
        RelayCommand editTaskCommand;
        RelayCommand startTaskCommand;
        RelayCommand completeTaskCommand;

        private double opacity = 1;
        private Project selectedProj;
        private Task selectedTask;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Task> ProjTasks { get; set; }

        // Конструктор
        public AppVM()
        {
            db = new AppContext();
            Projects = new ObservableCollection<Project>(db.Projects);
            ProjTasks = new ObservableCollection<Task>();
        }

        //Свойства
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
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

                RefreshTasks();

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
                OnPropertyChanged();
            }
        }

        private void RefreshTasks()
        {
            if (selectedProj != null)
            {
                foreach (Task t in db.Tasks)
                {
                    if (t.IdProject == SelectedProj.IdProject && !ProjTasks.Contains(t))
                        ProjTasks.Insert(0, t);
                    else if (t.IdProject != SelectedProj.IdProject && ProjTasks.Contains(t))
                        ProjTasks.Remove(t);
                }
            }
        }

        // Обновление данных приложения при внешнем обновлении БД
        public RelayCommand RefreshCommand 
        { 
            get {
                return refreshCommand ?? (refreshCommand = new RelayCommand((o) =>
                {
                    db = new AppContext();

                    int p_id = -1;
                    int t_id = -1;

                    if (SelectedProj != null)
                    {
                        p_id = SelectedProj.IdProject;
                        
                        if (SelectedTask != null)
                        {
                            t_id = SelectedTask.IdTask;
                        }

                        ProjTasks.Clear();
                        ProjTasks.AddRange(db.Tasks.Where(t => t.IdProject == SelectedProj.IdProject));

                        SelectedTask = ProjTasks.FirstOrDefault(t => t_id != -1 && t.IdTask == t_id);
                    }
                 
                    Projects.Clear();
                    Projects.AddRange(db.Projects);

                    SelectedProj = Projects.FirstOrDefault(p => p_id != -1 && p.IdProject == p_id);

                }));
            }
        }

        // Создание отчетов
        public RelayCommand ReportCommand
        {
            get
            {
                return reportCommand ?? (reportCommand = new RelayCommand((o) =>
                {
                    string param = o as string;
                    if (param != null)
                    {
                        bool.TryParse(param, out bool isProj);

                        ReportVM vm = new ReportVM(isProj);

                        var w = new ReportWindow()
                        {
                            DataContext = vm
                        };

                        Opacity = 0.5;
                        w.ShowDialog();

                        if (w.DialogResult.HasValue) 
                            Opacity = 1;
                    }
                }));
            }
        }

        // Добавление проекта
        public RelayCommand AddProjCommand
        {
            get
            {
                return addProjCommand ?? (addProjCommand = new RelayCommand((o) =>
                {
                    Add w = new Add(null);
                    Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.HasValue)
                        Opacity = 1;

                    if (w.DialogResult.Value)
                    {
                        TimeSpan time = new TimeSpan(23, 59, 59);

                        if (w.Deadline_timepicker.SelectedTime != null)
                        {
                            time = ((DateTime)w.Deadline_timepicker.SelectedTime).TimeOfDay;
                        }

                        if (w.Deadline_datepicker.SelectedDate == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                        {
                            MBWindow mb = new MBWindow();
                            //mb.Owner = w;
                            mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                            return;
                        }

                        DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);

                        Project p = new Project(w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);

                        db.Projects.Add(p);
                        db.SaveChanges();
                        Projects.Add(p);
                        SelectedProj = p;
                    }
                }));
            }
        }

        // Редактирование проекта
        public RelayCommand EditProjCommand
        {
            get
            {
                return editProjCommand ?? (editProjCommand = new RelayCommand((o) =>
                {
                    EditVM editVM = new EditVM(o);

                    var w = new EditWindow()
                    {
                        DataContext = editVM
                    };

                    Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.HasValue)
                        Opacity = 1;

                    if (w.DialogResult.Value)
                    {
                        Project edit = db.Projects.Find(selectedProj.IdProject);

                        edit.Name = w.Name_textbox.Text.Trim();
                        edit.Description = w.Description_textbox.Text.Trim();

                        db.SaveChanges();
                    }
                }, o => SelectedProj != null));
            }
        }

        // Добавление задачи
        public RelayCommand AddTaskCommand { 
            get
            {
                return addTaskCommand ?? (addTaskCommand = new RelayCommand((o) =>
                {
                    Add w = new Add(SelectedProj.IdProject);
                    Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.HasValue)
                        Opacity = 1;

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
                            MBWindow mb = new MBWindow();
                            //mb.Owner = w;
                            mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                            return;
                        }

                        DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);

                        Task t = new Task(SelectedProj.IdProject, w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);

                        db.Tasks.Add(t);
                        db.Projects.Find(SelectedProj.IdProject).Completed = null;
                        db.SaveChanges();
                        ProjTasks.Add(t);
                        SelectedTask = t;
                    }
                }, o => SelectedProj != null && SelectedProj.Deadline >= DateTime.Now));
            }
        }

        // Редактирование задачи
        public RelayCommand EditTaskCommand
        {
            get
            {
                return editTaskCommand ?? (editTaskCommand = new RelayCommand((o) =>
                {
                    EditVM editVM = new EditVM(o);

                    var w = new EditWindow()
                    {
                        DataContext = editVM
                    };

                    Opacity = 0.5;
                    w.ShowDialog();

                    if (w.DialogResult.HasValue)
                        Opacity = 1;

                    if (w.DialogResult.Value)
                    {
                        Task edit = db.Tasks.Find(SelectedTask.IdTask);

                        edit.Name = w.Name_textbox.Text.Trim();
                        edit.Description = w.Description_textbox.Text.Trim();

                        db.SaveChanges();
                    }
                }, o => SelectedTask != null));
            }
        }

        // Запуск задачи
        public RelayCommand StartTaskCommand
        {
            get
            {
                return startTaskCommand ?? (startTaskCommand = new RelayCommand((o) =>
                {

                }, o => SelectedTask != null && SelectedTask.Completed == null));
            }
        }

        // Выполнение задачи
        public RelayCommand CompleteTaskCommand
        {
            get
            {
                return completeTaskCommand ?? (completeTaskCommand = new RelayCommand((o) =>
                {
                    Task task = db.Tasks.Find(SelectedTask.IdTask);
                    task.Completed = DateTime.Now;

                    bool compProj = true;

                    foreach (Task t in db.Tasks.Where(t => t.IdProject == selectedProj.IdProject))
                        if (t.Completed == null)
                            compProj = false;

                    if (compProj)
                        db.Projects.Find(selectedProj.IdProject).Completed = DateTime.Now;

                    db.SaveChanges();
                }, o => SelectedTask != null && SelectedTask.Completed == null));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
