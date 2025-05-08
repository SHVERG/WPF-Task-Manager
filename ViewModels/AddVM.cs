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
        public Project proj { get; private set; }
        private DateTime startDateLimitStart;
        private DateTime deadlineLimitStart;
        private DateTime startDateLimitEnd;
        private DateTime deadlineLimitEnd;
        private string name = "";
        private string description = "";
        private DateTime? deadline = null;
        private DateTime? startDate = null;
        private User selected_user = null;
        private DateTime? time = null;
        private int? idCat = null;

        private RelayCommand closeCommand;
        private RelayCommand addCommand;

        // Конструкторы
        public AddVM()
        {
        }

        public ObservableCollection<User> Users { get; set; }

        public AddVM(int? id)
        {

            if (id.HasValue)
            {
                using (AppContext db = new AppContext())
                {
                    proj = db.Projects.Find(id);

                    StartDateLimitStart = db.Projects.Find(id).StartDate;
                    DeadlineLimitStart = db.Projects.Find(id).StartDate;

                    StartDateLimitEnd = db.Projects.Find(id).Deadline.AddDays(-1);
                    DeadlineLimitEnd = db.Projects.Find(id).Deadline;
                    Users = new ObservableCollection<User>(db.Users.Where(p => p.IdRole==3));
                }
            }
            else
            {
                StartDateLimitStart = DateTime.Now;
                DeadlineLimitStart = DateTime.Now;

                StartDateLimitEnd = DateTime.MaxValue;
                DeadlineLimitEnd = DateTime.MaxValue;
            }
        }

        // Свойства
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
            if (proj != null) return true;

            using (AppContext db = new AppContext())
            {
                foreach (Project pr in db.Projects)
                {
                    if (pr.Name == Name.Trim())
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        // Условие запуска команды добавления проекта/задачи
        private bool AddCanExecute()
        {
            return Name != null && Deadline != null && (SelectedUser != null || proj == null) && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && Description.Trim().Length <= 500 && isUnique();
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
