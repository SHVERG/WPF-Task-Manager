using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class LogVM : INotifyPropertyChanged
    {
        private int choice_index = 0;
        private DateTime? startDate;
        private DateTime? endDate;
        public ObservableCollection<LogBase> DGSource { get; set; }

        private RelayCommand closeCommand;
        private RelayCommand showCommand;
        private RelayCommand saveCommand;

        // Конструктор
        public LogVM() 
        {
            DGSource = new ObservableCollection<LogBase>();
        }

        // Свойства
        public int ChoiceIndex
        {
            get
            {
                return choice_index;
            }
            set
            {
                choice_index = value;
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


        // Показ отчета
        public void ShowExecute()
        {
            List<LogBase> temp = new List<LogBase>();
            DGSource.Clear();

            using (AppContext db = new AppContext())
            {
                if (ChoiceIndex == 0 || ChoiceIndex == 1)
                    foreach (ProjectsActivityLogs log in db.ProjectsLogs.Where(l => l.Date >= ((DateTime)StartDate) && l.Date <= ((DateTime)EndDate)))
                    {
                        temp.Add(
                            new LogBase(log.Action, log.Message)
                            {
                                Date = log.Date
                            }
                        );
                    }

                if (ChoiceIndex == 0 || ChoiceIndex == 2)
                    foreach (TasksActivityLogs log in db.TasksLogs.Where(l => l.Date >= ((DateTime)StartDate) && l.Date <= ((DateTime)EndDate)))
                    {
                        temp.Add(
                            new LogBase(log.Action, log.Message)
                            {
                                Date = log.Date
                            }
                        );
                    }

                DGSource.AddRange(temp.OrderBy(t => t.Date));
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
                }, o => StartDate != null && EndDate != null));
            }
        }


        // Сохранение отчета
        private void SaveExecute()
        {
            /*SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file|*.csv";

            if (save.ShowDialog() == true)
            {
                string str;

                if (IsProj)
                {
                    str = "Name;Deadline;Timespent;Completed\n";
                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.Deadline.ToString()}\";\"{r.Timespent}\";\"{r.S_Completed}\"\n";
                    }
                }
                else
                {
                    str = "Name;Project Name;Deadline;Timespent;Completed\n";
                    foreach (Report r in DGSource)
                    {
                        str += $"\"{r.Name}\";\"{r.ProjectName}\";\"{r.Deadline.ToString()}\";\"{r.Timespent}\";\"{r.S_Completed}\"\n";
                    }
                }

                File.WriteAllText(save.FileName, str, Encoding.UTF8);

                MBWindow mb = new MBWindow();
                mb.Show("Saving successful!", "All records saved successfully.", MessageBoxButton.OK);
            }*/
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
