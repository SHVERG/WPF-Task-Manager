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
        AppContext db;
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

            db = new AppContext();

            UncompProjs_listbox.ItemsSource = uncomp_projects;
            CompProjs_listbox.ItemsSource = comp_projects;
            Tasks_listbox.ItemsSource = proj_tasks;
            
            UncompProjs_listbox.DisplayMemberPath = "Name";
            CompProjs_listbox.DisplayMemberPath = "Name";
            Tasks_listbox.DisplayMemberPath = "Name";

            Refresh_db();
        }

        // Обновление данных из БД
        private void Refresh_db()
        {
            // Обновляем списки
            projects = new ObservableCollection<Project>(db.Projects.ToList());
            tasks = new ObservableCollection<Task>(db.Tasks.ToList());

            foreach (Project p in projects)
            {
                p.State = 0;
                int count = 0, comp_count = 0;

                // Ищем выполненные задачи
                foreach (Task t in tasks)
                {
                    if (p.IdProject == t.IdProject)
                    {
                        count++;
                        if (t.State == 1)
                            comp_count++;
                    }
                }

                if (count == comp_count && count != 0)
                    p.State = 1;

                if (p.State == 0 && !uncomp_projects.Contains(p))
                    uncomp_projects.Insert(0, p);
                else if (p.State == 1 && !comp_projects.Contains(p))
                    comp_projects.Insert(0, p);
            }

            foreach (Project p in uncomp_projects.ToArray())
                if (p.State == 1)
                    uncomp_projects.Remove(p);

            foreach (Project p in comp_projects.ToArray())
                if (p.State == 0)
                    comp_projects.Remove(p);
        }

        private void Refresh_tasks(Project p)
        {
            foreach (Task t in tasks)
            {
                if (t.IdProject == p.IdProject && !proj_tasks.Contains(t))
                    proj_tasks.Insert(0, t);
                else if (t.IdProject != p.IdProject && proj_tasks.Contains(t))
                    proj_tasks.Remove(t);
            }
        }

        private string DaysLeft(string deadline_string)
        {
            DateTime deadline = DateTime.ParseExact(deadline_string, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (deadline.AddDays(1) > DateTime.Now)
                return (deadline - DateTime.Now).Days.ToString() + " day(s) left";
            else
                return "Expired";
        }

        private void Refresh_db_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Refresh_db();
        }

        public void AddProject_button_Click(object sender, RoutedEventArgs e)
        {
            AddProject p = new AddProject();
            p.Owner = this;
            this.Opacity = 0.5;
            p.ShowDialog();
            if (p.DialogResult.Value)
                Refresh_db();
        }

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
            }
            else
                AddTask_button.IsEnabled = false;
        }

        private void AddTask_button_Click(object sender, RoutedEventArgs e)
        {
            AddTask t = new AddTask(idProj);
            t.Owner = this;
            this.Opacity = 0.5;
            t.ShowDialog();

            if (t.DialogResult.Value)
            {
                Refresh_db();

                foreach (Project p in UncompProjs_listbox.Items)
                {
                    if (p.IdProject == idProj)
                    {
                        UncompProjs_listbox.SelectedItem = p;
                        Refresh_tasks(p);
                    }
                }

            }
        }

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
                }
                else
                {
                    CompleteTask_button.IsEnabled = false;
                    StartTask_button.IsEnabled = false;
                }
            }
            else
            {
                CompleteTask_button.IsEnabled = false;
                StartTask_button.IsEnabled = false;
            }
        }

        private void CompleteTask_button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Exit_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Hide_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
