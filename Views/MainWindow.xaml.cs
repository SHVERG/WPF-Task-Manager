using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTaskManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обновление данных из БД
        /*private void Refresh_db()
        {
            Projs_datagrid.ItemsSource = new ObservableCollection<Project>(db.Projects);
            Projs_datagrid.Columns[0].Visibility = Visibility.Hidden;
            Projs_datagrid.Columns[2].Visibility = Visibility.Hidden;

            foreach (Project p in db.Projects)
            {

                bool iscomp = true;

                if (db.Tasks.Where(x => x.IdProject == p.IdProject).FirstOrDefault() == null) 
                { 
                    iscomp = false; 
                }

                foreach (Task t in db.Tasks)
                {
                    if (t.IdProject == p.IdProject && t.Completed == null)
                        iscomp = false;
                }

                if (iscomp)
                {
                    DateTime date = DateTime.MinValue;
                    DateTime taskdate = new DateTime();

                    foreach (Task t in db.Tasks)
                    {
                        if (t.IdProject == p.IdProject)
                            taskdate = (DateTime)t.Completed;

                        if (taskdate > date)
                            date = taskdate;
                    }

                    p.Completed = date;
                }
                else
                {
                    p.Completed = null;
                }

                db.SaveChanges();

                Projs_datagrid.Items.Refresh();

            }

            if (Tasks_listbox.SelectedItem != null)
            {
                Task t = Tasks_listbox.SelectedItem as Task;

                string state;

                if (t.Completed == null)
                    state = "Uncompleted";
                else
                    state = $"Completed ({t.Completed})";

                TaskInfo_textbox.Text = $"Name: {t.Name}\n\nDescription: {t.Description}\n\nDeadline: {t.Deadline.ToShortDateString()} {t.Deadline.ToShortTimeString()}\n({DaysLeft(t.Deadline)})\n\nState: {state}\n\nTime spent: {t.Timespent}";
            }

            Projs_datagrid.Items.Refresh();
            Tasks_listbox.Items.Refresh();
        }
        */

        /*
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
        */

        // Рассчет оставшегося времени до дедлайна в днях
        /*
        private string DaysLeft(DateTime deadline)
        {
            if (deadline > DateTime.Now)
                return (deadline - DateTime.Now).Days.ToString() + " day(s) " + (deadline - DateTime.Now).Hours + " hour(s) " + (deadline - DateTime.Now).Minutes + " minute(s) left";
            else
                return "Expired";
        }
        */

        // Событие для кнопки меню "Обновить"
        /*
        private void Refresh_db_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Refresh_db();
        }
        */

        // Событие для кнопки добавления проекта
        /*
        public void AddProject_button_Click(object sender, RoutedEventArgs e)
        {
            AddProject w = new AddProject();
            w.Owner = this;
            this.Opacity = 0.5;
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
                    mb.Owner = this;
                    mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                    return;
                }

                DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);
                
                Project p = new Project(w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);
                
                db.Projects.Add(p);
                db.SaveChanges();

                Refresh_db();
            }
        }
        */

        // Событие при выделении проекта из списков
        /*
        private void Projs_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            
            if (dg.SelectedIndex != -1)
            {
                Project p = (Project)dg.SelectedItem;

                ProjectInfo_textbox.Text = $"Name: {p.Name}\n\nDescription: {p.Description}\n\nDeadline: {p.Deadline.ToShortDateString()} {p.Deadline.ToShortTimeString()}\n({DaysLeft(p.Deadline)})";

                Refresh_tasks(p);

                EditProject_contextmenuitem.IsEnabled = true;
                AddTask_button.IsEnabled = true;
                EditProject_menuitem.IsEnabled = true;
            }
            else
            {
                AddTask_button.IsEnabled = false;
                EditProject_menuitem.IsEnabled = false;
                EditProject_contextmenuitem.IsEnabled = false;
            }
        }
        */

        /*
        private int idProj()
        {
            if (Projs_datagrid.SelectedIndex != -1)
                return ((Project)Projs_datagrid.SelectedItem).IdProject;
            else
                return 0;
        }
        */

        // Событие для кнопки добавления задачи
        /*
        private void AddTask_button_Click(object sender, RoutedEventArgs e)
        {
            AddTask w = new AddTask(idProj());
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();

            if (w.DialogResult.Value)
            {
                TimeSpan time = new TimeSpan(23, 59, 59);

                if (w.Deadline_timepicker.SelectedTime != null)
                {
                    time = ((DateTime)w.Deadline_timepicker.SelectedTime).TimeOfDay;
                }

                if ( db.Projects.Find(idProj()).Deadline.Date == w.Deadline_datepicker.SelectedDate && time > db.Projects.Find(idProj()).Deadline.TimeOfDay)
                    time = db.Projects.Find(idProj()).Deadline.TimeOfDay;
                
                if (w.Deadline_datepicker.SelectedDate == DateTime.Now.Date && time < DateTime.Now.TimeOfDay)
                {
                    MessageBox mb = new MessageBox();
                    mb.Owner = this;
                    mb.Show("Error!", "Can't set Deadline:\nDeadline is expired!", MessageBoxButton.OK);
                    return;
                }

                DateTime date = ((DateTime)w.Deadline_datepicker.SelectedDate).Add(time);

                Task t = new Task(idProj(), w.Name_textbox.Text.Trim(), w.Description_textbox.Text, date);

                db.Tasks.Add(t);
                db.SaveChanges();

                Refresh_db();

                Refresh_tasks(db.Projects.Find(idProj()));

                Tasks_listbox.SelectedItem = t;
            }
        }
        */

        // Событие при выделении задачи из списка
        /*
        private void Tasks_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tasks_listbox.SelectedIndex != -1)
            {
                Task t = (Task)Tasks_listbox.SelectedItem;
                string state;

                if (t.Completed == null)
                    state = "Uncompleted";
                else
                    state = $"Completed ({((DateTime)t.Completed).ToShortDateString()})";

                TaskInfo_textbox.Text = $"Name: {t.Name}\n\nDescription: {t.Description}\n\nDeadline: {t.Deadline.ToShortDateString()} {t.Deadline.ToShortTimeString()}\n({DaysLeft(t.Deadline)})\n\nState: {state}\n\nTime spent: {t.Timespent}";

                if (t.Completed == null)
                {
                    CompleteTask_button.IsEnabled = true;
                    StartTask_button.IsEnabled = true;
                    EditTask_menuitem.IsEnabled = true;

                    CompleteTask_contextmenuitem.IsEnabled = true;
                    StartTask_contextmenuitem.IsEnabled = true;
                    EditTask_contextmenuitem.IsEnabled = true;
                }
                else
                {
                    CompleteTask_button.IsEnabled = false;
                    StartTask_button.IsEnabled = false;
                    EditTask_menuitem.IsEnabled = true;

                    CompleteTask_contextmenuitem.IsEnabled = false;
                    StartTask_contextmenuitem.IsEnabled = false;
                    EditTask_contextmenuitem.IsEnabled = true;
                }
            }
            else
            {
                CompleteTask_button.IsEnabled = false;
                StartTask_button.IsEnabled = false;
                EditTask_menuitem.IsEnabled = false;

                CompleteTask_contextmenuitem.IsEnabled = false;
                StartTask_contextmenuitem.IsEnabled = false;
                EditTask_contextmenuitem.IsEnabled = false;
            }
        }
        */

        // Событие для кнопки завершения задачи
        /*
        private void CompleteTask_button_Click(object sender, RoutedEventArgs e)
        {
            Task t = db.Tasks.Find(((Task)Tasks_listbox.SelectedItem).IdTask);
            t.Completed = DateTime.Now;
            db.SaveChanges();
            Refresh_db();

            CompleteTask_button.IsEnabled = false;
            StartTask_button.IsEnabled = false;
            
            CompleteTask_contextmenuitem.IsEnabled = false;
            StartTask_contextmenuitem.IsEnabled = false;
        }

        // Событие для кнопки редактирования проекта
        private void EditProject_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Project p = Projs_datagrid.SelectedItem as Project;
            
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
            }
        }
        */

        // Событие для кнопки редактирования задачи
        /*
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

                edit.Name = w.Name_textbox.Text.Trim();
                edit.Description = w.Description_textbox.Text.Trim();

                db.SaveChanges();

                Refresh_db();
            }
        }

        // События для меню отчета
        private void ReportProject_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Report w = new Report(true);
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();
        }
        private void ReportTask_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Report w = new Report(false);
            w.Owner = this;
            this.Opacity = 0.5;
            w.ShowDialog();
        }
        */

        // Перемещение окна
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Restore_button_Click(null, null);
            else
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

        // Разворачивание на весь экран
        private void Restore_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        // Событие при изменении статуса окна
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                Restore_button.Content = "1";
            else
                Restore_button.Content = "2";
        }

    }
}
