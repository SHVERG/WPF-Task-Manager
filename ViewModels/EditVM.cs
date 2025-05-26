using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class EditVM : INotifyPropertyChanged
    {
        private string name, description;
        public bool isProject = true;
        private Task t;
        private Project p;
        private User selected_user = null;

        private RelayCommand closeCommand, editCommand;

        public ObservableCollection<User> Users { get; set; }

        // Конструкторы
        public EditVM()
        {
        }

        public EditVM(object obj)
        {
            p = obj as Project;

            if (p != null)
            {
                Name = p.Name;
                Description = p.Description;
            }
            else
            {
                isProject = false;
                t = obj as Task;
                Name = t.Name;
                Description = t.Description;

                Users = new ObservableCollection<User>(App.db.Users.Where(p => p.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Member").IdRole));
                SelectedUser = App.db.Users.Find(t.IdUser);
            }
        }

        // Свойства
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

        // Проверка на уникальность изменяемой задачи/проекта
        private bool isUnique()
        {
            if (isProject)
            {
                if (p.Name == Name && p.Description == Description)
                    return false;

                foreach (Project pr in App.db.Projects)
                {
                    if (pr.Name == Name.Trim() && pr.IdProject != p.IdProject)
                    {
                        return false;
                    }

                }

                return true;
            }
            else
            {
                return t.Name != Name || t.Description != Description || (SelectedUser != null && SelectedUser.IdUser != t.IdUser);
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

        // Условие запуска команды подтверждения изменения
        private bool EditCanExecute()
        {
            return Name != null && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && Description.Trim().Length <= 150 && isUnique();
        }


        // Команда подтверждения изменения
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.DialogResult = true;
                }, o => EditCanExecute()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
