using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class AddVM : INotifyPropertyChanged
    {
        private DateTime startDateLimitStart, deadlineLimitStart, startDateLimitEnd, deadlineLimitEnd;
        private string name = "";
        private string description = "";
        private DateTime? deadline = null;
        private DateTime? startDate = null;
        private DateTime? time = null;
        private User selected_user = null;
        private int? idCat = null;

        private RelayCommand closeCommand, addCommand;

        public ObservableCollection<User> Users { get; set; }

        // Конструкторы
        public AddVM()
        {
        }

        public AddVM(int? id)
        {
            StartDateLimitStart = DateTime.Now;
            DeadlineLimitStart = DateTime.Now.AddDays(1);

            if (id.HasValue)
            {
                Project = App.db.Projects.Find(id);

                StartDateLimitEnd = Project.Deadline.AddDays(-1);
                DeadlineLimitEnd = Project.Deadline;

                Users = new ObservableCollection<User>(App.db.Users.Where(p => p.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Member").IdRole));
            }
            else
            {
                StartDateLimitEnd = DateTime.MaxValue;
                DeadlineLimitEnd = DateTime.MaxValue;
            }
        }

        // Свойства
        public Project Project { get; private set; }

        public DateTime StartDateLimitStart
        {
            get
            {
                return startDateLimitStart;
            }
            set
            {
                startDateLimitStart = value;
                OnPropertyChanged();
            }
        }
        public DateTime DeadlineLimitStart
        {
            get
            {
                return deadlineLimitStart;
            }
            set
            {
                deadlineLimitStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDateLimitEnd
        {
            get
            {
                return startDateLimitEnd;
            }
            set
            {
                startDateLimitEnd = value;
                OnPropertyChanged();
            }
        }

        public DateTime DeadlineLimitEnd
        {
            get
            {
                return deadlineLimitEnd;
            }
            set
            {
                deadlineLimitEnd = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value;
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

        public DateTime? Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        public int? IdCat
        {
            get
            {
                return idCat;
            }
            set
            {
                idCat = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get
            {
                return selected_user;
            }
            set
            {
                selected_user = value;
                OnPropertyChanged();
            }
        }

        // Команда закрытия
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

        // Проверка на уникальность названия создаваемого проекта
        private bool isUnique()
        {
            if (Project != null) return true;

            foreach (Project pr in App.db.Projects)
            {
                if (pr.Name == Name.Trim())
                {
                    return false;
                }

            }

            return true;
        }

        // Условие запуска команды добавления проекта/задачи
        private bool AddCanExecute()
        {
            return Name != null && StartDate != null && Deadline != null && (SelectedUser != null || Project == null) && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && Description.Trim().Length <= 500 && isUnique();
        }

        // Команда добавления проекта/задачи
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.DialogResult = true;
                }, o => AddCanExecute()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
