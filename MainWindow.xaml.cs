using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Для работы с БД
        AppContext db = new AppContext();

        ObservableCollection<Project> projects;
        ObservableCollection<Project> uncomp_projects = new ObservableCollection<Project>();
        ObservableCollection<Project> comp_projects = new ObservableCollection<Project>();

        ObservableCollection<Task> tasks;
        ObservableCollection<Task> proj_tasks = new ObservableCollection<Task>();

        // id выделенного проекта
        int idProj;
        
        // Конструктор
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            projects = new ObservableCollection<Project>(db.Projects);
            tasks = new ObservableCollection<Task>(db.Tasks);

            UncompProjs_listbox.ItemsSource = uncomp_projects;
            CompProjs_listbox.ItemsSource = comp_projects;
            Tasks_listbox.ItemsSource = proj_tasks;

            Refresh_db();
        }

        // Обновление данных из БД
        private void Refresh_db()
        {

            foreach (Project p in db.Projects)
            {
                bool iscomp = true;

                if (db.Tasks.Where(x => x.IdProject == p.IdProject).FirstOrDefault() == null) 
                { 
                    iscomp = false; 
                }

                foreach (Task t in db.Tasks)
                {
                    if (t.IdProject == p.IdProject && t.State == 0)
                        iscomp = false;
                }

                if (iscomp)
                    p.State = 1;
                else
                    p.State = 0;

                db.SaveChanges();

                if (p.State == 0 && !uncomp_projects.Contains(p))
                {
                    comp_projects.Remove(p);
                    uncomp_projects.Insert(0, p);
                }
                else if (p.State == 1 && !comp_projects.Contains(p))
                {
                    uncomp_projects.Remove(p);
                    comp_projects.Insert(0, p);
                }
            }

            UncompProjs_listbox.Items.Refresh();
            CompProjs_listbox.Items.Refresh();
        }

        // Обновление списка задач выбранного проекта
        private void Refresh_tasks(Project p)
        {
            
            if (p == null)
                return;
            
            foreach (Task t in db.Tasks)
            {
                if (t.IdProject == p.IdProject && !proj_tasks.Contains(t))
                    proj_tasks.Insert(0, t);
                else if (t.IdProject != p.IdProject && proj_tasks.Contains(t))
                    proj_tasks.Remove(t);
            }
        }

        // Рассчет оставшегося времени до дедлайна в днях
        private string DaysLeft(string deadline_string)
        {
            DateTime deadline = DateTime.ParseExact(deadline_string, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (deadline.AddDays(1) > DateTime.Now)
                return (deadline - DateTime.Now).Days.ToString() + " day(s) left";
            else
                return "Expired";
        }

        // Событие для кнопки меню "Обновить"
        private void Refresh_db_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Refresh_db();
        }

        // Событие для кнопки добавления проекта
        public void AddProject_button_Click(object sender, RoutedEventArgs e)
        {
            AddProject w = new AddProject();
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.Value)
            {
                string deadline = string.Format("{0:yyyy-MM-dd}", w.Deadline_datepicker.SelectedDate);
                Project p = new Project(w.Name_textbox.Text.Trim(), w.Description_textbox.Text, deadline);

                db.Projects.Add(p);
                db.SaveChanges();

                Refresh_db();

                UncompProjs_listbox.SelectedItem = p;
            }
        }

        // Событие при выделении проекта из списков
        private void Projs_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if (lb.SelectedIndex != -1)
            {
                if (lb == UncompProjs_listbox)
                    CompProjs_listbox.SelectedIndex = -1;
                else
                    UncompProjs_listbox.SelectedIndex = -1;

                Project p = (Project)lb.SelectedItem;

                idProj = p.IdProject;

                ProjectInfo_textbox.Text = $"Name: {p.Name}\n\nDescription: {p.Description}\n\nDeadline: {p.Deadline} ({DaysLeft(p.Deadline)})";

                Refresh_tasks(p);

                AddTask_button.IsEnabled = true;
                EditProject_menuitem.IsEnabled = true;
            }
            else
            {
                AddTask_button.IsEnabled = false;
                EditProject_menuitem.IsEnabled = false;
            }
        }

        // Событие для кнопки добавления задачи
        private void AddTask_button_Click(object sender, RoutedEventArgs e)
        {
            AddTask w = new AddTask(idProj);
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.Value)
            {

                string deadline = string.Format("{0:yyyy-MM-dd}", w.Deadline_datepicker.SelectedDate);
                Task t = new Task(idProj, w.Name_textbox.Text.Trim(), w.Description_textbox.Text, deadline);

                db.Tasks.Add(t);
                db.SaveChanges();

                Refresh_db();

                Refresh_tasks(db.Projects.Find(idProj));

                Tasks_listbox.SelectedItem = t;

            }
        }

        // Событие при выделении задачи из списка
        private void Tasks_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tasks_listbox.SelectedIndex != -1)
            {
                Task t = (Task)Tasks_listbox.SelectedItem;
                string state;

                if (t.State == 0)
                    state = "Uncompleted";
                else
                    state = "Completed";

                TaskInfo_textbox.Text = $"Name: {t.Name}\n\nDescription: {t.Description}\n\nDeadline: {t.Deadline} ({DaysLeft(t.Deadline)})\n\nState: {state}\n\nTime spent: {t.Timespent}";

                if (t.State == 0)
                {
                    CompleteTask_button.IsEnabled = true;
                    StartTask_button.IsEnabled = true;
                    EditTask_menuitem.IsEnabled = true;
                }
                else
                {
                    CompleteTask_button.IsEnabled = false;
                    StartTask_button.IsEnabled = false;
                    EditTask_menuitem.IsEnabled = false;
                }
            }
            else
            {
                CompleteTask_button.IsEnabled = false;
                StartTask_button.IsEnabled = false;
                EditTask_menuitem.IsEnabled = false;
            }
        }

        // Событие для кнопки завершения задачи
        private void CompleteTask_button_Click(object sender, RoutedEventArgs e)
        {

        }

        // Событие для кнопки редактирования проекта
        private void EditProject_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Project p = null;
            if (UncompProjs_listbox.SelectedIndex != -1)
                p = UncompProjs_listbox.SelectedItem as Project;
            else
                p = CompProjs_listbox.SelectedItem as Project;

            EditWindow w = new EditWindow(p);
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.Value)
            {
                Project edit = db.Projects.Find(p.IdProject);

                edit.Name = w.Name_textbox.Text.Trim();
                edit.Description = w.Description_textbox.Text.Trim();

                db.SaveChanges();

                Refresh_db();

                ProjectInfo_textbox.Text = $"Name: {edit.Name}\n\nDescription: {edit.Description}\n\nDeadline: {edit.Deadline} ({DaysLeft(edit.Deadline)})";
            }
        }

        // Событие для кнопки редактирования задачи
        private void EditTask_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Task t = Tasks_listbox.SelectedItem as Task;

            EditWindow w = new EditWindow(t);
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.Value)
            {

                Task edit = db.Tasks.Find(t.IdTask);

                string state;

                if (edit.State == 0)
                    state = "Uncompleted";
                else
                    state = "Completed";

                edit.Name = w.Name_textbox.Text.Trim();
                edit.Description = w.Description_textbox.Text.Trim();

                db.SaveChanges();

                Refresh_db();

                TaskInfo_textbox.Text = $"Name: {edit.Name}\n\nDescription: {edit.Description}\n\nDeadline: {edit.Deadline} ({DaysLeft(edit.Deadline)})\n\nState: {state}\n\nTime spent: {edit.Timespent}";
            }
        }

        // Перемещение окна
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // Выход из приложения
        private void Exit_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Сворачивание окна
        private void Hide_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
