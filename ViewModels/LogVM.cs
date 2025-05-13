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
    public class LogVM : INotifyPropertyChanged
    {
        private int choice_index = 0;
        private DateTime? startDate, endDate;
        private RelayCommand closeCommand, showCommand, saveCommand, clearCommand;

        // Конструктор
        public LogVM()
        {
            DGSource = new ObservableCollection<LogBase>();
        }

        // Свойства
        public ObservableCollection<LogBase> DGSource { get; set; }

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
        private void ShowExecute()
        {
            List<LogBase> temp = new List<LogBase>();
            DGSource.Clear();
            DateTime NNStartDate = StartDate == null ? DateTime.MinValue : StartDate.Value;
            DateTime NNEndDate = EndDate == null ? DateTime.MaxValue : EndDate.Value;

            if (ChoiceIndex == 0 || ChoiceIndex == 1)
                foreach (ProjectsActivityLogs log in App.db.ProjectsLogs.Where(l => l.Date >= NNStartDate && l.Date <= NNEndDate))
                {
                    temp.Add(
                        new LogBase(log.Action, log.Message)
                        {
                            Date = log.Date
                        }
                    );
                }

            if (ChoiceIndex == 0 || ChoiceIndex == 2)
                foreach (TasksActivityLogs log in App.db.TasksLogs.Where(l => l.Date >= NNStartDate && l.Date <= NNEndDate))
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

        // Команда показа отчета
        public RelayCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new RelayCommand((o) =>
                {
                    ShowExecute();
                }));
            }
        }


        // Сохранение отчета
        private void SaveExecute()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file|*.csv";

            if (save.ShowDialog() == true)
            {
                string str = "Message;Date\n";
                foreach (LogBase log in DGSource)
                {
                    str += $"\"{log.Message}\";\"{log.Date}\"\n";
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
