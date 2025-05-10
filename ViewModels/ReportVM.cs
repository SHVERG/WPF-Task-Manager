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
        private bool isProj;
        private string title;
        private string choiceLabel;
        private bool isReliableChecked;

        private int groupChoiceIndex = 0;
        private int byChoiceIndex = 0;
        private DateTime? startDate;
        private DateTime? endDate;
        private User reliable;

        public ObservableCollection<Report> DGSource { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private RelayCommand closeCommand;
        private RelayCommand showCommand;
        private RelayCommand saveCommand;
        private RelayCommand clearCommand;

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

            //using (AppContext db = new AppContext())
            //{
                Users = new ObservableCollection<User>(App.db.Users);
            //}

            if (isProj)
            {
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        {
                            Title = "ОТЧЕТ ПО ПРОЕКТАМ";
                            ChoiceLabel = "Проекты";
                            break;
                        }
                    default:
                        {
                            Title = "PROJECTS REPORT";
                            ChoiceLabel = "Projects";
                            break;
                        }
                }
            }
            else
            {
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        {
                            title = "ОТЧЕТ ПО ЗАДАЧАМ";
                            choiceLabel = "Задачи";
                            break;
                        }
                    default:
                        {
                            title = "TASKS REPORT";
                            choiceLabel = "Tasks";
                            break;
                        }
                }
            }
        }

        // Свойства
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
            string Result_ru;

            DGSource.Clear();

            //using (AppContext db = new AppContext())
            //{
                DateTime NNStartDate = StartDate!=null?StartDate.Value:DateTime.MinValue;
                DateTime NNEndDate = EndDate!=null?EndDate.Value:DateTime.MaxValue;

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
                                {
                                    Result = "Completed in time";
                                    Result_ru = "Выполнен вовремя";
                                }
                                else
                                {
                                    Result = "Completed in bad time";
                                    Result_ru = "Выполнен невовремя";
                                }
                            }
                            else
                            {
                                Result = "Not completed";
                                Result_ru = "Не выполнен";
                            }

                            string comp;
                            if (p.Completed != null)
                                comp = p.Completed.Value.ToString("dd.MM.yyyy");
                            else
                                comp = "-";

                            switch (App.Language.Name)
                            {
                                case "ru-RU":
                                    DGSource.Add(new Report(p.Name, null, null, p.StartDate, p.Deadline, comp, Result_ru, ts));
                                    break;
                                default:
                                    DGSource.Add(new Report(p.Name, null, null, p.StartDate, p.Deadline, comp, Result, ts));
                                    break;
                            }
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
                            {
                                Result = "Completed in time";
                                Result_ru = "Выполнена вовремя";
                            }
                            else
                            {
                                Result = "Completed in bad time";
                                Result_ru = "Выполнена невовремя";
                            }
                        }
                        else
                        {
                            Result = "Not completed";
                            Result_ru = "Не выполнена";
                        }

                        string comp;
                        if (t.Completed != null)
                            comp = t.Completed.Value.ToString("dd.MM.yyyy");
                        else
                            comp = "-";

                        switch (App.Language.Name)
                        {
                            case "ru-RU":
                                DGSource.Add(new Report(t.Name, App.db.Projects.Find(t.IdProject).Name, App.db.Users.Find(t.IdUser).Name, t.StartDate, t.Deadline, comp, Result_ru, SecondsToTimeSpan(t.Timespent)));
                                break;
                            default:
                                DGSource.Add(new Report(t.Name, App.db.Projects.Find(t.IdProject).Name, App.db.Users.Find(t.IdUser).Name, t.StartDate, t.Deadline, comp, Result, SecondsToTimeSpan(t.Timespent)));
                                break;
                        }
                    }
                }
            //}
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

                    switch (App.Language.Name)
                    {
                        case "ru-RU":
                            str = "Название;Начат;Крайний срок;Выполнен;Затрачено времени;Результат\n";
                            break;
                        default:

                            str = "Name;Start;Deadline;Completed;Timespent;Result\n";
                            break;
                    }

                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.StartDate.ToString()}\";\"{r.Deadline.ToString()}\";\"{r.S_Completed.ToString()}\";\"{r.Timespent}\";\"{r.Result}\"\n";
                    }
                }
                else
                {
                    switch (App.Language.Name)
                    {
                        case "ru-RU":
                            str = "Название;Название проекта;Ответственный;Начата;Крайний срок;Выполнена;Затрачено времени;Результат\n";
                            break;
                        default:

                            str = "Name;Project Name;Reliable;Start;Deadline;Completed;Timespent;Result\n";
                            break;
                    }

                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.ProjectName}\";\"{r.Reliable}\";\"{r.StartDate}\";\"{r.Deadline.ToString()}\";\"{r.S_Completed}\";\"{r.Timespent}\";\"{r.Result}\"\n";
                    }
                }

                File.WriteAllText(save.FileName, str, Encoding.UTF8);

                MBWindow mb = new MBWindow();
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        mb.Show("Файл сохранен успешно!", "Все записи сохранены.", MessageBoxButton.OK);
                        break;
                    default:
                        mb.Show("Saving successful!", "All records saved successfully.", MessageBoxButton.OK);
                        break;
                }
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
