﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Linq;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace WpfTaskManager
{
    public class ReportVM : INotifyPropertyChanged
    {
        private bool isProj;
        private string title = "TASKS REPORT";
        private string choice_label = "Tasks";

        private int choice_index = 0;
        private DateTime? startDate;
        private DateTime? endDate;
        public ObservableCollection<Report> DGSource { get; set; }

        private RelayCommand closeCommand;
        private RelayCommand showCommand;
        private RelayCommand saveCommand;

        // Конструкторы
        public ReportVM() 
        {
            DGSource = new ObservableCollection<Report>();
        }

        public ReportVM(bool isProj) 
        { 
            this.isProj = isProj;
            DGSource = new ObservableCollection<Report>();

            if (isProj)
            {
                Title = "PROJECTS REPORT";
                ChoiceLabel = "Projects";
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
                return choice_label;
            }
            set
            {
                choice_label = value; 
                OnPropertyChanged();
            }
        }

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
            string S_Completed;

            DGSource.Clear();

            using (AppContext db = new AppContext())
            {
                if (IsProj)
                {
                    foreach (Project p in db.Projects.Where(p => ((p.Completed == null && ChoiceIndex == 2) || (p.Completed != null && ChoiceIndex == 1) || ChoiceIndex == 0) && p.Deadline >= ((DateTime)StartDate) && p.Deadline <= ((DateTime)EndDate)))
                    {
                        TimeSpan ts = new TimeSpan();

                        foreach (Task t in db.Tasks.Where(t => t.IdProject == p.IdProject))
                        {
                            ts = ts.Add(SecondsToTimeSpan(t.Timespent));
                        }

                        if (p.Completed != null)
                        {
                            if (p.Completed <= p.Deadline)
                                S_Completed = "Completed in time";
                            else
                                S_Completed = "Completed in bad time";
                        }
                        else
                            S_Completed = "Not completed";

                        DGSource.Add(new Report(p.Name, null, p.Deadline, S_Completed, ts));
                    }
                }
                else
                {
                    foreach (Task t in db.Tasks.Where(t => ((t.Completed == null && ChoiceIndex == 2) || (t.Completed != null && ChoiceIndex == 1) || ChoiceIndex == 0) && t.Deadline >= ((DateTime)StartDate) && t.Deadline <= ((DateTime)EndDate)))
                    {
                        if (t.Completed != null)
                        {
                            if (t.Completed <= t.Deadline)
                                S_Completed = "Completed in time";
                            else
                                S_Completed = "Completed in bad time";
                        }
                        else
                            S_Completed = "Not completed";

                        DGSource.Add(new Report(t.Name, db.Projects.Find(t.IdProject).Name, t.Deadline, S_Completed, SecondsToTimeSpan(t.Timespent)));
                    }
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
                }, o => StartDate != null && EndDate != null));
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
