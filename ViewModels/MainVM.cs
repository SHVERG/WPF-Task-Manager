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
        private RelayCommand refreshCommand, addProjCommand, editProjCommand, addProjCatCommand, deleteProjCommand;
        private RelayCommand addTaskCommand, editTaskCommand, startTaskCommand, completeTaskCommand, deleteTaskCommand;
        private RelayCommand reportCommand, showLogCommand, clearLogCommand, manageUsersCommand;
        private RelayCommand exportProjCommand, exportAllProjsCommand, importProjCommand, changeLanguageCommand, logoutCommand;

        private double opacity = 1;
        private Project selectedProj;
        private Task selectedTask;
        private User user;
        private bool isLangRussian;

        // Конструкторы
        public MainVM() { }

        public MainVM(User user)
        {
            IsLangRussian = App.Language.Equals(new CultureInfo("ru-RU"));

            User = user;

            Projects = new ObservableCollection<Project>();
            ProjTasks = new ObservableCollection<Task>();

            RefreshExecute();
        }

        //Свойства
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Task> ProjTasks { get; set; }

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

        public List<CultureInfo> Languages
        {
            get
            {
                return App.Languages;
            }
        }

        // Обновление списка отображаемых задач
        private void RefreshTasks()
        {
            if (selectedProj != null)
            {
                ProjTasks.Clear();
                ProjTasks.AddRange(App.db.Tasks.Where(t => (User.IdRole != App.db.Roles.FirstOrDefault(r => r.Name == "Member").IdRole || t.IdUser == User.IdUser) && t.IdProject == SelectedProj.IdProject));
            }
        }

        // Обновление контекста БД приложения
        private void RefreshExecute()
        {
            //App.db = new AppContext();

            int p_id = -1;
            int t_id = -1;

            if (SelectedProj != null)
            {
                p_id = SelectedProj.IdProject;

                if (SelectedTask != null)
                {
                    t_id = SelectedTask.IdTask;
                }

                RefreshTasks();

                SelectedTask = ProjTasks.FirstOrDefault(t => t_id != -1 && t.IdTask == t_id);
            }

            Projects.Clear();

            if (User != null && User.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Member").IdRole)
            {
                //Projects = new ObservableCollection<Project>();

                foreach (Task task in App.db.Tasks.Where(t => t.IdUser == User.IdUser))
                {
                    if (Projects.FirstOrDefault(p => p.IdProject == task.IdProject) == null)
                    {
                        Projects.Add(App.db.Projects.FirstOrDefault(p => p.IdProject == task.IdProject));
                    }
                }
            }
            else
                Projects.AddRange(App.db.Projects);

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

        // Принятие заявок
        private void ManageUsersExecute(object o)
        {

            var w = new ManageUsersWindow()
            {
                DataContext = new ManageUsersVM(bool.Parse(o.ToString()))
            };

            Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.HasValue)
                Opacity = 1;
        }

        // Команда принятия заявок
        public RelayCommand ManageUsersCommand
        {
            get
            {
                return manageUsersCommand ?? (manageUsersCommand = new RelayCommand((o) =>
                {
                    ManageUsersExecute(o);
                }));
            }
        }

        // Добавление записи в журнал
        private void AddLog(bool isProject, int id, int action, string message)
        {
            if (isProject)
                App.db.ProjectsLogs.Add(new ProjectsActivityLogs(id, action, message));
            else
                App.db.TasksLogs.Add(new TasksActivityLogs(id, action, message));

            App.db.SaveChanges();
        }

        // Добавление проекта
        public void AddProj(AddVM addVM)
        {
            ProjectCreator pc = new ProjectCreator();

            Project p = (Project)pc.Create(addVM);

            if (p == null)
            {
                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("main_error_header").ToString(), Application.Current.TryFindResource("main_deadline_expired_body").ToString().Replace("\\n", Environment.NewLine), MessageBoxButton.OK);
                return;
            }

            App.db.Projects.Add(p);
            App.db.SaveChanges();

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

        // Добавление категории проекта (НЕ РЕАЛИЗОВАНО!!!)
        public void AddProjCat(AddCatVM vm)
        {
            App.db.Categories.Add(new Category(vm.Name, vm.Color.R, vm.Color.G, vm.Color.B));
            App.db.SaveChanges();
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
                Project edit = App.db.Projects.Find(selectedProj.IdProject);

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

                App.db.SaveChanges();
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
            string title = Application.Current.TryFindResource("main_delete_conf_header").ToString();
            string description = Application.Current.TryFindResource("main_delete_conf_body").ToString() + $" \"{SelectedProj.Name}\"?";

            MBWindow conf = new MBWindow();
            if (conf.Show(title, description, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Task t in App.db.Tasks)
                    if (t.IdProject == selectedProj.IdProject)
                        App.db.Tasks.Remove(t);

                ProjTasks.Clear();
                App.db.Projects.Remove(SelectedProj);
                App.db.SaveChanges();
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
            Project p = App.db.Projects.Find(SelectedProj.IdProject);
            TaskCreator tc = new TaskCreator();
            Task t = (Task)tc.Create(addVM);

            if (t == null)
            {
                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("main_error_header").ToString(), Application.Current.TryFindResource("main_deadline_expired_body").ToString().Replace("\\n", Environment.NewLine), MessageBoxButton.OK);

                return;
            }

            App.db.Tasks.Add(t);

            if (p.Completed != null)
            {
                p.Completed = null;
                AddLog(true, p.IdProject, 2, $"Project \"{p.Name}\" turned to incompleted.");
            }

            App.db.SaveChanges();
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
                Task edit = App.db.Tasks.Find(SelectedTask.IdTask);

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

                App.db.SaveChanges();
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
            Task t = App.db.Tasks.Find(SelectedTask.IdTask);
            t.Completed = DateTime.Now;
            AddLog(false, t.IdTask, 2, $"Task \"{t.Name}\" completed.");

            bool compProj = true;

            foreach (Task task in App.db.Tasks.Where(task => task.IdProject == selectedProj.IdProject))
                if (task.Completed == null)
                    compProj = false;

            if (compProj)
            {
                Project p = App.db.Projects.Find(selectedProj.IdProject);
                p.Completed = DateTime.Now;
                AddLog(true, p.IdProject, 2, $"Project \"{p.Name}\" completed.");
            }

            App.db.SaveChanges();
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
            string title = Application.Current.TryFindResource("main_delete_conf_header").ToString();
            string description = Application.Current.TryFindResource("main_task_delete_conf_body").ToString() + $" \"{SelectedTask.Name}\"?";

            MBWindow conf = new MBWindow();
            if (conf.Show(title, description, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.db.Tasks.Remove(SelectedTask);
                App.db.SaveChanges();
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
            MBWindow conf = new MBWindow();

            if (conf.Show(Application.Current.TryFindResource("log_clear_header").ToString(), Application.Current.TryFindResource("log_clear_body").ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.db.ProjectsLogs.RemoveRange(App.db.ProjectsLogs.ToList());
                App.db.TasksLogs.RemoveRange(App.db.TasksLogs.ToList());
                App.db.SaveChanges();

                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("log_clear_success_header").ToString(), Application.Current.TryFindResource("log_clear_success_body").ToString(), MessageBoxButton.OK);
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
            XElement p_start = new XElement("start", SelectedProj.StartDate);
            XElement p_dead = new XElement("deadline", SelectedProj.Deadline);
            XElement p_comp = new XElement("completed", selectedProj.Completed);
            XElement p_tasks = new XElement("tasks");

            foreach (Task t in App.db.Tasks)
            {
                if (t.IdProject == SelectedProj.IdProject)
                {
                    XElement task = new XElement("task");
                    XAttribute t_id = new XAttribute("id", t.IdTask);
                    XAttribute t_pid = new XAttribute("p_id", t.IdProject);
                    XAttribute t_uid = new XAttribute("u_id", t.IdUser);
                    XElement t_name = new XElement("name", t.Name);
                    XElement t_desc = new XElement("desc", t.Description);
                    XElement t_start = new XElement("start", t.StartDate);
                    XElement t_dead = new XElement("deadline", t.Deadline);
                    XElement t_comp = new XElement("completed", t.Completed);
                    XElement t_time = new XElement("timespent", t.Timespent);

                    task.Add(t_id, t_pid, t_uid, t_name, t_desc, t_start, t_dead, t_comp, t_time);
                    p_tasks.Add(task);
                }
            }

            proj.Add(p_id, p_name, p_desc, p_start, p_dead, p_comp, p_tasks);

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML file|*.xml";

            if (save.ShowDialog() == true)
            {
                proj.Save(save.FileName);

                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("export_success_header").ToString(), Application.Current.TryFindResource("project").ToString() + $" \"{SelectedProj.Name}\" " + Application.Current.TryFindResource("export_success_body").ToString(), MessageBoxButton.OK);
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

            foreach (Project p in App.db.Projects)
            {
                XElement proj = new XElement("project");
                XAttribute p_id = new XAttribute("id", p.IdProject);
                XElement p_name = new XElement("name", p.Name);
                XElement p_desc = new XElement("desc", p.Description);
                XElement p_start = new XElement("start", p.StartDate);
                XElement p_dead = new XElement("deadline", p.Deadline);
                XElement p_comp = new XElement("completed", p.Completed);
                XElement p_tasks = new XElement("tasks");

                foreach (Task t in App.db.Tasks)
                {
                    if (t.IdProject == p.IdProject)
                    {
                        XElement task = new XElement("task");
                        XAttribute t_id = new XAttribute("id", t.IdTask);
                        XAttribute t_pid = new XAttribute("p_id", t.IdProject);
                        XAttribute t_uid = new XAttribute("u_id", t.IdUser);
                        XElement t_name = new XElement("name", t.Name);
                        XElement t_desc = new XElement("desc", t.Description);
                        XElement t_start = new XElement("start", t.StartDate);
                        XElement t_dead = new XElement("deadline", t.Deadline);
                        XElement t_comp = new XElement("completed", t.Completed);
                        XElement t_time = new XElement("timespent", t.Timespent);

                        task.Add(t_id, t_pid, t_uid, t_name, t_desc, t_start, t_dead, t_comp, t_time);
                        p_tasks.Add(task);
                    }
                }

                proj.Add(p_id, p_name, p_desc, p_start, p_dead, p_comp, p_tasks);
                projs.Add(proj);
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML file|*.xml";

            if (save.ShowDialog() == true)
            {
                projs.Save(save.FileName);

                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("export_success_header").ToString(), Application.Current.TryFindResource("export_all_success_body").ToString(), MessageBoxButton.OK);
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
                }, o => App.db.Projects.Count() > 0));
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

                    if (App.db.Projects.Any(p => p.Name == name))
                    {
                        names += $"\n\"{name}\"";
                        continue;
                    }

                    Project p_add = new Project();
                    DateTime p_comp;

                    p_add.Name = proj.Element("name").Value;
                    p_add.Description = proj.Element("desc").Value;
                    p_add.StartDate = DateTime.Parse(proj.Element("start").Value);
                    p_add.Deadline = DateTime.Parse(proj.Element("deadline").Value);

                    if (DateTime.TryParse(proj.Element("completed").Value, out p_comp))
                        p_add.Completed = p_comp;

                    App.db.Projects.Add(p_add);
                    App.db.SaveChanges();

                    foreach (XElement task in proj.Element("tasks").Elements())
                    {
                        Task t_add = new Task();

                        DateTime t_comp;

                        t_add.IdProject = p_add.IdProject;
                        t_add.Name = task.Element("name").Value;
                        t_add.IdUser = int.Parse(task.Attribute("u_id").Value);
                        t_add.Description = task.Element("desc").Value;
                        t_add.StartDate = DateTime.Parse(task.Element("start").Value);
                        t_add.Deadline = DateTime.Parse(task.Element("deadline").Value);
                        t_add.Timespent = int.Parse(task.Element("timespent").Value);

                        if (DateTime.TryParse(task.Element("completed").Value, out t_comp))
                            t_add.Completed = t_comp;

                        App.db.Tasks.Add(t_add);
                        App.db.SaveChanges();
                    }

                    Projects.Add(p_add);
                    SelectedProj = p_add;
                }

                MBWindow mb = new MBWindow();
                if (names.Length != 0)
                {
                    mb.Show(Application.Current.TryFindResource("main_warning_header").ToString(), Application.Current.TryFindResource("import_fail_body").ToString() + $"{names}", MessageBoxButton.OK);
                }
                else
                {
                    mb.Show(Application.Current.TryFindResource("import_success_header").ToString(), Application.Current.TryFindResource("import_success_body").ToString(), MessageBoxButton.OK);
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
