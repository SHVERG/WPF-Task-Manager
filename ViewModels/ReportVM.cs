using System;
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

        public RelayCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new RelayCommand((o) =>
                {
                    string Completed;

                    DGSource.Clear();

                    using (AppContext db = AppContext.ReCreate())
                    {
                        if (isProj)
                        {
                            foreach (Project p in db.Projects.Where(p => ((p.Completed == null && ChoiceIndex == 2) || (p.Completed != null && ChoiceIndex == 1) || ChoiceIndex == 0) && p.Deadline >= ((DateTime)StartDate) && p.Deadline <= ((DateTime)EndDate)))
                            {
                                TimeSpan ts = new TimeSpan();

                                foreach (Task t in db.Tasks.Where(t => t.IdProject == p.IdProject))
                                {
                                    ts = ts.Add(new TimeSpan(t.Timespent * 10000000));
                                }

                                if (p.Completed != null)
                                {
                                    if (p.Completed <= p.Deadline)
                                        Completed = "Completed in time";
                                    else
                                        Completed = "Completed in bad time";
                                }
                                else
                                    Completed = "Not completed";

                                DGSource.Add(new Report(p.Name, null, p.Deadline, Completed, ts));
                            }
                        }
                        else
                        {
                            foreach (Task t in db.Tasks.Where(t => ((t.Completed == null && ChoiceIndex == 2) || (t.Completed != null && ChoiceIndex == 1) || ChoiceIndex == 0) && t.Deadline >= ((DateTime)StartDate) && t.Deadline <= ((DateTime)EndDate)))
                            {

                                if (t.Completed != null)
                                {
                                    if (t.Completed <= t.Deadline)
                                        Completed = "Completed in time";
                                    else
                                        Completed = "Completed in bad time";
                                }
                                else
                                    Completed = "Not completed";

                                DGSource.Add(new Report(t.Name, db.Projects.Find(t.IdProject).Name, t.Deadline, Completed, new TimeSpan(t.Timespent * 10000000)));
                            }
                        }
                    }
                }, o => StartDate != null && EndDate != null));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand((o) =>
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "CSV file|*.csv";

                    if (save.ShowDialog() == true)
                    {
                        IDataObject objectSave = Clipboard.GetDataObject();

                        string str;

                        if (IsProj)
                        {
                            str = "Name;Deadline;Timespent;Completed\n";
                            foreach (Report r in DGSource)
                            {
                                str += $"\"{r.Name}\";\"{r.Deadline.ToString()}\";\"{r.Timespent}\";\"{r.Completed}\"\n";
                            }
                        }
                        else
                        {
                            str = "Name;Project Name;Timespent;Completed\n";
                            foreach (Report r in DGSource)
                            {
                                str += $"\"{r.Name}\";\"{r.ProjectName}\";\"{r.Deadline.ToString()}\";\"{r.Timespent}\";\"{r.Completed}\"\n";
                            }
                        }
                        
                        File.WriteAllText(save.FileName, str, Encoding.UTF8);
                    }
                }, o => DGSource.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
