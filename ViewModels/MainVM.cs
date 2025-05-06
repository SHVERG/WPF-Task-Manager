using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

        RelayCommand addProjCommand;
        RelayCommand editProjCommand;
        RelayCommand addProjCatCommand;
        RelayCommand deleteProjCommand;

        RelayCommand addTaskCommand;
        RelayCommand editTaskCommand;
        RelayCommand startTaskCommand;
        RelayCommand completeTaskCommand;
        RelayCommand deleteTaskCommand;

        RelayCommand reportCommand;
        RelayCommand showLogCommand;
        RelayCommand clearLogCommand;

        RelayCommand exportProjCommand;
        RelayCommand exportAllProjsCommand;
        RelayCommand importProjCommand;

        RelayCommand changeLanguageCommand;
        RelayCommand logoutCommand;

        private bool isLangRussian;
        private double opacity = 1;
        private Project selectedProj;
        private Task selectedTask;
        private User user;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Task> ProjTasks { get; set; }
        public ObservableCollection<User> Users { get; set; }

        // Конструктор
        public MainVM()
        {
            db = new AppContext();
            Projects = new ObservableCollection<Project>(db.Projects);
            ProjTasks = new ObservableCollection<Task>();
            Users = new ObservableCollection<User>(db.Users);
            isLangRussian = App.Language.Equals(new CultureInfo("ru-RU"));
            //User = user;
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

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public List<CultureInfo> Languages
        {
            get
            {
                return App.Languages;
            }
        }

        public bool IsLangRussian
        {
            get
            {
                return isLangRussian;
            }
            set
            {
                isLangRussian = value;
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
            get
            {
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
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Ошибка!", "Невозможно установить крайний срок:\nКрайний срок просрочен!", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                        break;
                }
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

        // Добавление категории проекта
        public void AddProjCat(AddCatVM vm)
        {
            db.Categories.Add(new Category(vm.Name, vm.Color.R, vm.Color.G, vm.Color.B));
            db.SaveChanges();
        }

        private void AddProjCatExecute(object o)
        {
            AddCatVM vm = new AddCatVM();
            var w = new AddCatWindow()
            {
                DataContext = vm
            };

            Opacity = 0.5;
            w.Owner = (Window)o;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;

            if (w.DialogResult.Value)
            {
                AddProjCat(vm);
            }
        }

        public RelayCommand AddProjCatCommand
        {
            get
            {
                return addProjCatCommand ?? (addProjCatCommand = new RelayCommand((o) =>
                {
                    AddProjCatExecute(o);
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

        // Удаление проекта
        public void DeleteProjExecute()
        {
            string title, description;

            switch (App.Language.Name)
            {
                case "ru-RU":
                    {
                        title = "Подтверждение удаления";
                        description = $"Вы действительно хотите удалить Проект \"{SelectedProj.Name}\"?";
                        break;
                    }
                default:
                    {
                        title = "Delete Confirmation";
                        description = $"Do you really want to delete Project \"{SelectedProj.Name}\"?";
                        break;
                    }

            }

            MBWindow conf = new MBWindow();
            if (conf.Show(title, description, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Task t in db.Tasks)
                    if (t.IdProject == selectedProj.IdProject)
                        db.Tasks.Remove(t);

                ProjTasks.Clear();
                db.Projects.Remove(SelectedProj);
                db.SaveChanges();
                AddLog(true, SelectedProj.IdProject, 3, $"Project \"{SelectedProj.Name}\" deleted.");
                Projects.Remove(SelectedProj);
            }
        }

        // Команда удаления проекта
        public RelayCommand DeleteProjCommand
        {
            get
            {
                return deleteProjCommand ?? (deleteProjCommand = new RelayCommand((o) =>
                {
                    DeleteProjExecute();
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

                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Ошибка!", "Невозможно установить крайний срок:\nКрайний срок просрочен!", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                        break;
                }

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
        public RelayCommand AddTaskCommand
        {
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

        // Удаление задачи
        public void DeleteTaskExecute()
        {
            string title, description;

            switch (App.Language.Name)
            {
                case "ru-RU":
                    {
                        title = "Подтверждение удаления";
                        description = $"Вы действительно хотите удалить Задачу \"{SelectedTask.Name}\"?";
                        break;
                    }
                default:
                    {
                        title = "Delete Confirmation";
                        description = $"Do you really want to delete Task \"{SelectedTask.Name}\"?";
                        break;
                    }

            }

            MBWindow conf = new MBWindow();
            if (conf.Show(title, description, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Tasks.Remove(SelectedTask);
                db.SaveChanges();
                AddLog(false, SelectedTask.IdTask, 3, $"Task \"{SelectedTask.Name}\" deleted.");
                ProjTasks.Remove(SelectedTask);
            }
        }

        // Команда удаления задачи
        public RelayCommand DeleteTaskCommand
        {
            get
            {
                return deleteTaskCommand ?? (deleteTaskCommand = new RelayCommand((o) =>
                {
                    DeleteTaskExecute();
                }, o => SelectedTask != null));
            }
        }


        // Создание журнала
        private void LogExecute(object o)
        {
            var w = new LogWindow();

            Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;
        }

        // Команда создания журнала
        public RelayCommand ShowLogCommand
        {
            get
            {
                return showLogCommand ?? (showLogCommand = new RelayCommand((o) =>
                {
                    LogExecute(o);
                }));
            }
        }

        // Очистка журнала
        public void ClearLogExecute()
        {
            string title, description;

            switch (App.Language.Name)
            {
                case "ru-RU":
                    {
                        title = "Подтверждение очистки";
                        description = "Вы действительно хотите очитстить журнал активности?";
                        break;
                    }
                default:
                    {
                        title = "Clear confirmation";
                        description = "Do you really want to clear Actions Log";
                        break;
                    }

            }

            MBWindow conf = new MBWindow();
            if (conf.Show(title, description, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.ProjectsLogs.RemoveRange(db.ProjectsLogs.ToList());
                db.TasksLogs.RemoveRange(db.TasksLogs.ToList());
                db.SaveChanges();

                MBWindow mb = new MBWindow();

                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Успешная очистка!", "Журнал активности успешно очищен.", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Clear Successful!", $"Actions Log cleared successfully.", MessageBoxButton.OK);
                        break;
                }
            }
        }

        // Команда очистки журнала
        public RelayCommand ClearLogCommand
        {
            get
            {
                return clearLogCommand ?? (clearLogCommand = new RelayCommand((o) =>
                {
                    ClearLogExecute();
                }));
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

                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Успешное экспортирование!", $"Проект \"{SelectedProj.Name}\" экспортирован успешно.", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Export Successful!", $"Project \"{SelectedProj.Name}\" exported successfully.", MessageBoxButton.OK);
                        break;
                }
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

        // Смена языка

        public RelayCommand ChangeLanguageCommand
        {
            get
            {
                return changeLanguageCommand ?? (changeLanguageCommand = new RelayCommand((o) =>
                {
                    App.Language = o as CultureInfo;
                    RefreshExecute();
                }));
            }
        }

        // Выход из аккаунта
        public RelayCommand LogOutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new RelayCommand((o) =>
                {
                    Properties.Settings.Default.SavedUsername = string.Empty;
                    Properties.Settings.Default.AutoLogin = false;
                    Properties.Settings.Default.Save();

                    var loginWindow = new LoginWindow();

                    loginWindow.Show();
                    Application.Current.MainWindow = loginWindow;

                    Window w = o as Window;
                    w.Close();
                }));
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

                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Успешное экспортирование!", $"Все проекты экспортированы успешно.", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Export Successful!", $"All Projects exported successfully.", MessageBoxButton.OK);
                        break;
                }
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
                    switch (App.Language.Name)
                    {
                        case "ru-RU":
                            mb.Show("Внимание!", $"Невозможно импортировать проекты:{names}", MessageBoxButton.OK);
                            break;
                        default:
                            mb.Show("Warning!", $"Can't import Projects:{names}", MessageBoxButton.OK);
                            break;
                    }
                }
                else
                {
                    switch (App.Language.Name)
                    {
                        case "ru-RU":
                            mb.Show("Успешное импортирование!", $"Все проекты успешно импортированы.", MessageBoxButton.OK);
                            break;
                        default:
                            mb.Show("Import Successful!", "All Projects imported successfully.", MessageBoxButton.OK);
                            break;
                    }
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
