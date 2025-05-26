using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace WpfTaskManager
{
    public class ReportVM : INotifyPropertyChanged
    {
        private bool isProj, isReliableChecked;
        private string title, choiceLabel;

        private int groupChoiceIndex = 0;
        private int byChoiceIndex = 0;
        private DateTime? startDate, endDate;
        private User reliable;

        private RelayCommand closeCommand, showCommand, saveCommand, clearCommand;

        // Конструкторы
        public ReportVM()
        {
            DGSource = new ObservableCollection<Report>();
            Users = new ObservableCollection<User>();
        }

        public ReportVM(bool isProj)
        {
            this.isProj = isProj;
            DGSource = new ObservableCollection<Report>();

            Users = new ObservableCollection<User>(App.db.Users.Where(p => p.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Member").IdRole));

            if (isProj)
            {
                Title = App.Current.TryFindResource("report_projects_title").ToString();
                ChoiceLabel = App.Current.TryFindResource("projects").ToString();
            }
            else
            {
                Title = App.Current.TryFindResource("report_tasks_title").ToString();
                ChoiceLabel = App.Current.TryFindResource("tasks").ToString();
            }
        }

        // Свойства
        public ObservableCollection<Report> DGSource { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public bool IsProj
        {
            get
            {
                return isProj;
            }
            set
            {
                isProj = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string ChoiceLabel
        {
            get
            {
                return choiceLabel;
            }
            set
            {
                choiceLabel = value;
                OnPropertyChanged();
            }
        }

        public int GroupChoiceIndex
        {
            get
            {
                return groupChoiceIndex;
            }
            set
            {
                groupChoiceIndex = value;
                OnPropertyChanged();
            }
        }

        public int ByChoiceIndex
        {
            get
            {
                return byChoiceIndex;
            }
            set
            {
                byChoiceIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsReliableChecked
        {
            get
            {
                return isReliableChecked;
            }
            set
            {
                isReliableChecked = value;
                OnPropertyChanged();
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public User Reliable
        {
            get
            {
                return reliable;
            }
            set
            {
                reliable = value; 
                OnPropertyChanged();
            }
        }

        // Команда закрытия окна
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ?? (closeCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.Close();
                }));
            }
        }

        // Преобразование секунд в TimeSpan
        private TimeSpan SecondsToTimeSpan(int val)
        {
            int hrs = val / 3600;
            int mins = (val - hrs * 3600) / 60;
            int secs = (val - hrs * 3600 - mins * 60) / 60;

            return new TimeSpan(hrs, mins, secs);
        }

        // Показ отчета
        public void ShowExecute()
        {
            string Result;

            DGSource.Clear();

            DateTime NNStartDate = StartDate != null ? StartDate.Value : DateTime.MinValue;
            DateTime NNEndDate = EndDate != null ? EndDate.Value : DateTime.MaxValue;

            if (IsProj)
            {
                List<Project> projs = new List<Project>();

                if (GroupChoiceIndex == 2)
                {

                    if (ByChoiceIndex == 0)
                        projs = App.db.Projects.Where(p => p.Completed == null && p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        projs = App.db.Projects.Where(p => p.Completed == null && p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }
                else if (GroupChoiceIndex == 1)
                {

                    if (ByChoiceIndex == 0)
                        projs = App.db.Projects.Where(p => p.Completed != null && p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        projs = App.db.Projects.Where(p => p.Completed != null && p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }
                else
                {
                    if (ByChoiceIndex == 0)
                        projs = App.db.Projects.Where(p => p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        projs = App.db.Projects.Where(p => p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }

                foreach (Project p in projs)
                {
                    TimeSpan ts = new TimeSpan();

                    foreach (Task t in App.db.Tasks.Where(t => t.IdProject == p.IdProject))
                    {
                        ts = ts.Add(SecondsToTimeSpan(t.Timespent));
                    }

                    if (p.Completed != null)
                    {
                        if (p.Completed <= p.Deadline)
                            Result = App.Current.TryFindResource("report_comp_in_time").ToString();
                        else
                            Result = App.Current.TryFindResource("report_comp_in_bad_time").ToString();
                    }
                    else
                        Result = App.Current.TryFindResource("report_not_comp").ToString();

                    string comp;
                    if (p.Completed != null)
                        comp = p.Completed.Value.ToString("dd.MM.yyyy");
                    else
                        comp = "-";

                    DGSource.Add(new Report(p.Name, null, null, p.StartDate, p.Deadline, comp, Result, ts));
                }
            }
            else
            {
                List<Task> tasks = new List<Task>();

                if (GroupChoiceIndex == 2)
                {
                    if (ByChoiceIndex == 0)
                        tasks = App.db.Tasks.Where(p => p.Completed == null && p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        tasks = App.db.Tasks.Where(p => p.Completed == null && p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }
                else if (GroupChoiceIndex == 1)
                {

                    if (ByChoiceIndex == 0)
                        tasks = App.db.Tasks.Where(p => p.Completed != null && p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        tasks = App.db.Tasks.Where(p => p.Completed != null && p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }
                else
                {
                    if (ByChoiceIndex == 0)
                        tasks = App.db.Tasks.Where(p => p.StartDate >= NNStartDate && p.StartDate <= NNEndDate).ToList();
                    else
                        tasks = App.db.Tasks.Where(p => p.Deadline >= NNStartDate && p.Deadline <= NNEndDate).ToList();
                }

                if (IsReliableChecked == true)
                    tasks = tasks.Where(t => t.IdUser == Reliable.IdUser).ToList();

                foreach (Task t in tasks)
                {
                    if (t.Completed != null)
                    {
                        if (t.Completed <= t.Deadline)
                            Result = App.Current.TryFindResource("report_comp_in_time").ToString();
                        else
                            Result = App.Current.TryFindResource("report_comp_in_bad_time").ToString();
                    }
                    else
                        Result = App.Current.TryFindResource("report_not_comp").ToString();

                    string comp;
                    if (t.Completed != null)
                        comp = t.Completed.Value.ToString("dd.MM.yyyy");
                    else
                        comp = "-";

                    string name = App.db.Users.Find(t.IdUser) != null ? App.db.Users.Find(t.IdUser).Name : "-";

                    DGSource.Add(new Report(t.Name, App.db.Projects.Find(t.IdProject).Name, name, t.StartDate, t.Deadline, comp, Result, SecondsToTimeSpan(t.Timespent)));
                }
            }
        }

        // Команда показа отчета
        public RelayCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new RelayCommand((o) =>
                {
                    ShowExecute();
                }, o => (!IsProj && ((IsReliableChecked && Reliable != null) || !IsReliableChecked)) || IsProj ));
            }
        }

        // Сохранение отчета
        private void SaveExecute()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file|*.csv";

            if (save.ShowDialog() == true)
            {
                string str;

                if (IsProj)
                {

                    str = App.Current.TryFindResource("report_saving_proj").ToString().Replace("\\n", Environment.NewLine);

                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.StartDate.ToString()}\";\"{r.Deadline.ToString()}\";\"{r.S_Completed.ToString()}\";\"{r.Timespent}\";\"{r.Result}\"\n";
                    }
                }
                else
                {
                    str = App.Current.TryFindResource("report_saving_task").ToString().Replace("\\n", Environment.NewLine);

                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.ProjectName}\";\"{r.Reliable}\";\"{r.StartDate}\";\"{r.Deadline.ToString()}\";\"{r.S_Completed}\";\"{r.Timespent}\";\"{r.Result}\"\n";
                    }
                }

                File.WriteAllText(save.FileName, str, Encoding.UTF8);

                MBWindow mb = new MBWindow();
                mb.Show(Application.Current.TryFindResource("log_file_save_success_header").ToString(), Application.Current.TryFindResource("log_file_save_success_body").ToString(), MessageBoxButton.OK);
            }
        }

        // Команда сохранения отчета
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand((o) =>
                {
                    SaveExecute();
                }, o => DGSource.Count > 0));
            }
        }

        // Команда очистки дат
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ?? (clearCommand = new RelayCommand((o) => 
                {
                    StartDate = null;
                    EndDate = null;
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
