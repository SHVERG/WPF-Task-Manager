using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;

namespace WpfTaskManager
{
    public class MainVM : INotifyPropertyChanged
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

        RelayCommand exportProjCommand;
        RelayCommand exportAllProjsCommand;
        RelayCommand importProjCommand;

        private double opacity = 1;
        private Project selectedProj;
        private Task selectedTask;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Task> ProjTasks { get; set; }

        // Конструктор
        public MainVM()
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

        // Обновление списка отображаемых задач
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

        // Обновление контекста БД приложения
        private void RefreshExecute()
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
        }

        // Команда обновления контекста БД приложения
        public RelayCommand RefreshCommand 
        { 
            get {
                return refreshCommand ?? (refreshCommand = new RelayCommand((o) =>
                {
                    RefreshExecute();
                }));
            }
        }

        // Создание отчетов
        private void ReportExecute(object o)
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
        }

        // Команда создания отчетов
        public RelayCommand ReportCommand
        {
            get
            {
                return reportCommand ?? (reportCommand = new RelayCommand((o) =>
                {
                    ReportExecute(o);
                }));
            }
        }

        // Добавление записи в журнал
        private void AddLog(bool isProject, int id, int action, string message)
        {
            if (isProject)
                db.ProjectsLogs.Add(new ProjectsActivityLogs(id, action, message));
            else
                db.TasksLogs.Add(new TasksActivityLogs(id, action, message));

            db.SaveChanges();
        }

        // Добавление проекта
        public void AddProj(AddVM addVM)
        {
            ProjectCreator pc = new ProjectCreator();

            Project p = (Project)pc.Create(addVM);

            if (p == null)
            {
                MBWindow mb = new MBWindow();
                mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                return;
            }

            db.Projects.Add(p);
            db.SaveChanges();

            AddLog(true, p.IdProject, 0, $"Project \"{p.Name}\" added.");

            Projects.Add(p);
            SelectedProj = p;
        }

        // Добавление проекта из VM
        private void AddProjExecute(object o)
        {
            AddVM addVM = new AddVM(null);

            var w = new AddWindow()
            {
                DataContext = addVM
            };

            Opacity = 0.5;
            w.Owner = (Window)o;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;

            if (w.DialogResult.Value)
            {
                AddProj(addVM);
            }
        }

        // Команда добавления проекта
        public RelayCommand AddProjCommand
        {
            get
            {
                return addProjCommand ?? (addProjCommand = new RelayCommand((o) =>
                {
                    AddProjExecute(o);
                }));
            }
        }

        // Редактирование проекта
        private void EditProjExecute(object o)
        {
            EditVM editVM = new EditVM(SelectedProj);

            var w = new EditWindow()
            {
                DataContext = editVM
            };

            Opacity = 0.5;
            w.Owner = (Window)o;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;

            if (w.DialogResult.Value)
            {
                Project edit = db.Projects.Find(selectedProj.IdProject);

                if (edit.Name != editVM.Name.Trim())
                {
                    AddLog(true, edit.IdProject, 1, $"Project name changed (\"{edit.Name}\" => \"{editVM.Name.Trim()}\").");
                    edit.Name = editVM.Name.Trim();
                }

                if (edit.Description != editVM.Description.Trim())
                {
                    AddLog(true, edit.IdProject, 1, $"Project \"{edit.Name}\" description changed.");
                    edit.Description = editVM.Description.Trim();
                }

                db.SaveChanges();
            }
        }

        // Команда редактирования проекта
        public RelayCommand EditProjCommand
        {
            get
            {
                return editProjCommand ?? (editProjCommand = new RelayCommand((o) =>
                {
                    EditProjExecute(o);
                }, o => SelectedProj != null));
            }
        }

        // Добавление задачи
        public void AddTask(AddVM addVM)
        {
            Project p = db.Projects.Find(SelectedProj.IdProject);
            TaskCreator tc = new TaskCreator();
            Task t = (Task)tc.Create(addVM);

            if (t == null)
            {
                MBWindow mb = new MBWindow();
                mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                return;
            }

            db.Tasks.Add(t);
            
            if (p.Completed != null)
            {
                p.Completed = null;
                AddLog(true, p.IdProject, 2, $"Project \"{p.Name}\" turned to incompleted.");
            }

            db.SaveChanges();
            AddLog(false, t.IdTask, 0, $"Task \"{t.Name}\" added.");
            ProjTasks.Add(t);
            SelectedTask = t;
        }

        // Добавление задачи из VM
        private void AddTaskExecute(object o)
        {
            AddVM addVM = new AddVM(SelectedProj.IdProject);

            var w = new AddWindow()
            {
                DataContext = addVM
            };

            Opacity = 0.5;
            w.Owner = (Window)o;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;

            if (w.DialogResult.Value)
            {
                AddTask(addVM);
            }
        }

        // Команда добавления задачи
        public RelayCommand AddTaskCommand { 
            get
            {
                return addTaskCommand ?? (addTaskCommand = new RelayCommand((o) =>
                {
                    AddTaskExecute(o);
                }, o => SelectedProj != null && SelectedProj.Deadline >= DateTime.Now));
            }
        }

        // Редактирование задачи
        private void EditTaskExecute(object o)
        {
            EditVM editVM = new EditVM(SelectedTask);

            var w = new EditWindow()
            {
                DataContext = editVM
            };

            Opacity = 0.5;
            w.Owner = (Window)o;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;

            if (w.DialogResult.Value)
            {
                Task edit = db.Tasks.Find(SelectedTask.IdTask);

                if (edit.Name != editVM.Name.Trim())
                {
                    AddLog(false, edit.IdTask, 1, $"Task name changed (\"{edit.Name}\" => \"{editVM.Name.Trim()}\").");
                    edit.Name = editVM.Name.Trim();
                }

                if (edit.Description != editVM.Description.Trim())
                {
                    AddLog(false, edit.IdTask, 1, $"Task \"{edit.Name}\" description changed.");
                    edit.Description = editVM.Description.Trim();
                }

                db.SaveChanges();
            }
        }

        // Команда редактирования задачи
        public RelayCommand EditTaskCommand
        {
            get
            {
                return editTaskCommand ?? (editTaskCommand = new RelayCommand((o) =>
                {
                    EditTaskExecute(o);
                }, o => SelectedTask != null));
            }
        }

        // Команда запуска задачи
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
        private void CompleteTaskExecute()
        {
            Task t = db.Tasks.Find(SelectedTask.IdTask);
            t.Completed = DateTime.Now;
            AddLog(false, t.IdTask, 2, $"Task \"{t.Name}\" completed.");

            bool compProj = true;

            foreach (Task task in db.Tasks.Where(task => task.IdProject == selectedProj.IdProject))
                if (task.Completed == null)
                    compProj = false;

            if (compProj)
            {
                Project p = db.Projects.Find(selectedProj.IdProject);
                p.Completed = DateTime.Now;
                AddLog(true, p.IdProject, 2, $"Project \"{p.Name}\" completed.");
            }

            db.SaveChanges();
        }

        // Команда выполнения задачи
        public RelayCommand CompleteTaskCommand
        {
            get
            {
                return completeTaskCommand ?? (completeTaskCommand = new RelayCommand((o) =>
                {
                    CompleteTaskExecute();
                }, o => SelectedTask != null && SelectedTask.Completed == null));
            }
        }

        // Экспорт проекта
        private void ExportProjExecute()
        {
            XElement proj = new XElement("project");
            XAttribute p_id = new XAttribute("id", SelectedProj.IdProject);
            XElement p_name = new XElement("name", SelectedProj.Name);
            XElement p_desc = new XElement("desc", SelectedProj.Description);
            XElement p_dead = new XElement("deadline", SelectedProj.Deadline);
            XElement p_comp = new XElement("completed", selectedProj.Completed);
            XElement p_tasks = new XElement("tasks");

            foreach (Task t in db.Tasks)
            {
                if (t.IdProject == SelectedProj.IdProject)
                {
                    XElement task = new XElement("task");
                    XAttribute t_id = new XAttribute("id", t.IdTask);
                    XAttribute t_pid = new XAttribute("p_id", t.IdProject);
                    XElement t_name = new XElement("name", t.Name);
                    XElement t_desc = new XElement("desc", t.Description);
                    XElement t_dead = new XElement("deadline", t.Deadline);
                    XElement t_comp = new XElement("completed", t.Completed);
                    XElement t_time = new XElement("timespent", t.Timespent);

                    task.Add(t_id, t_pid, t_name, t_desc, t_dead, t_comp, t_time);
                    p_tasks.Add(task);
                }
            }

            proj.Add(p_id, p_name, p_desc, p_dead, p_comp, p_tasks);

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML file|*.xml";

            if (save.ShowDialog() == true)
            {
                proj.Save(save.FileName);

                MBWindow mb = new MBWindow();
                mb.Show("Export Successful!", $"Project \"{SelectedProj.Name}\" exported successfully.", MessageBoxButton.OK);
            }
        }

        // Команда экспорта проекта
        public RelayCommand ExportProjCommand
        {
            get
            {
                return exportProjCommand ?? (exportProjCommand = new RelayCommand((o) =>
                {
                    ExportProjExecute();
                }, o => SelectedProj != null));
            }
        }

        // Экспорт всех проектов
        private void ExportAllProjsExecute()
        {
            XElement projs = new XElement("projects");

            foreach (Project p in db.Projects)
            {
                XElement proj = new XElement("project");
                XAttribute p_id = new XAttribute("id", p.IdProject);
                XElement p_name = new XElement("name", p.Name);
                XElement p_desc = new XElement("desc", p.Description);
                XElement p_dead = new XElement("deadline", p.Deadline);
                XElement p_comp = new XElement("completed", p.Completed);
                XElement p_tasks = new XElement("tasks");

                foreach (Task t in db.Tasks)
                {
                    if (t.IdProject == p.IdProject)
                    {
                        XElement task = new XElement("task");
                        XAttribute t_id = new XAttribute("id", t.IdTask);
                        XAttribute t_pid = new XAttribute("p_id", t.IdProject);
                        XElement t_name = new XElement("name", t.Name);
                        XElement t_desc = new XElement("desc", t.Description);
                        XElement t_dead = new XElement("deadline", t.Deadline);
                        XElement t_comp = new XElement("completed", t.Completed);
                        XElement t_time = new XElement("timespent", t.Timespent);

                        task.Add(t_id, t_pid, t_name, t_desc, t_dead, t_comp, t_time);
                        p_tasks.Add(task);
                    }
                }

                proj.Add(p_id, p_name, p_desc, p_dead, p_comp, p_tasks);
                projs.Add(proj);
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML file|*.xml";

            if (save.ShowDialog() == true)
            {
                projs.Save(save.FileName);

                MBWindow mb = new MBWindow();
                mb.Show("Export Successful!", "All Projects exported successfully.", MessageBoxButton.OK);
            }
        }

        // Команда экспорта всех проектов
        public RelayCommand ExportAllProjsCommand
        {
            get
            {
                return exportAllProjsCommand ?? (exportAllProjsCommand = new RelayCommand((o) =>
                {
                    ExportAllProjsExecute();
                }, o => db.Projects.Count() > 0));
            }
        }

        // Импорт проектов
        private void ImportProjExecute()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "XML file|*.xml";

            if (open.ShowDialog() == true)
            {
                string names = "";
                var doc = XDocument.Load(open.FileName);
                IEnumerable<XElement> elements = doc.Descendants("project");

                foreach (XElement proj in elements)
                {
                    string name = proj.Element("name").Value;

                    if (db.Projects.Any(p => p.Name == name))
                    {
                        names += $"\n\"{name}\"";
                        continue;
                    }

                    Project p_add = new Project();
                    DateTime p_comp;

                    p_add.Name = proj.Element("name").Value;
                    p_add.Description = proj.Element("desc").Value;
                    p_add.Deadline = DateTime.Parse(proj.Element("deadline").Value);

                    if (DateTime.TryParse(proj.Element("completed").Value, out p_comp))
                        p_add.Completed = p_comp;

                    db.Projects.Add(p_add);
                    db.SaveChanges();

                    foreach (XElement task in proj.Element("tasks").Elements())
                    {
                        Task t_add = new Task();

                        DateTime t_comp;

                        t_add.IdProject = p_add.IdProject;
                        t_add.Name = task.Element("name").Value;
                        t_add.Description = task.Element("desc").Value;
                        t_add.Deadline = DateTime.Parse(task.Element("deadline").Value);
                        t_add.Timespent = int.Parse(task.Element("timespent").Value);

                        if (DateTime.TryParse(task.Element("completed").Value, out t_comp))
                            t_add.Completed = t_comp;

                        db.Tasks.Add(t_add);
                        db.SaveChanges();
                    }

                    Projects.Add(p_add);
                    SelectedProj = p_add;
                }

                MBWindow mb = new MBWindow();
                if (names.Length != 0)
                {
                    mb.Show("Warning!", $"Can't import Projects:{names}", MessageBoxButton.OK);
                }
                else
                {
                    mb.Show("Import Successful!", "All Projects imported successfully.", MessageBoxButton.OK);
                }
            }
        }

        // Команда импорта проектов
        public RelayCommand ImportProjCommand
        {
            get
            {
                return importProjCommand ?? (importProjCommand = new RelayCommand((o) =>
                {
                    ImportProjExecute();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
