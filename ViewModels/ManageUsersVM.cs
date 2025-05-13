using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class ManageUsersVM : INotifyPropertyChanged
    {
        private User selectedUser;

        private Role selectedRole;

        private RelayCommand applyRoleCommand, deleteUserCommand, closeCommand;

        private bool showOnlyRequests;

        // Конструкторы

        public ManageUsersVM() { }

        public ManageUsersVM(bool showOnlyRequests = false)
        {
            ShowOnlyRequests = showOnlyRequests;

            LoadData();
        }

        // Свойства
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Role> AvailableRoles { get; set; } = new ObservableCollection<Role>();

        public bool ShowOnlyRequests
        {
            get
            {
                return showOnlyRequests;
            }
            set
            {
                showOnlyRequests = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => selectedUser;
            set { selectedUser = value; OnPropertyChanged(); }

        }

        public Role SelectedRole
        {
            get => selectedRole;
            set { selectedRole = value; OnPropertyChanged(); }
        }

        // Инициализация
        private void LoadData()
        {
            if (ShowOnlyRequests)
                Users = new ObservableCollection<User>(App.db.Users.Where(u => u.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Unregistered").IdRole).ToList());
            else
                Users = new ObservableCollection<User>(App.db.Users.Where(u => u.IdRole != App.db.Roles.FirstOrDefault(r => r.Name == "Unregistered").IdRole).ToList());

            AvailableRoles = new ObservableCollection<Role>(App.db.Roles.Where(r => r.Name != "Unregistered").ToList());

            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(AvailableRoles));
        }

        // Применение роли
        private void ApplyRole()
        {
            if (SelectedUser == null || SelectedRole == null) return;

            User user = App.db.Users.Find(SelectedUser.IdUser);
            
            user.IdRole = SelectedRole.IdRole;
            App.db.SaveChanges();


            if (ShowOnlyRequests)
                Users.Remove(SelectedUser);

            SelectedUser = null;
            SelectedRole = null;
        }

        // Команда применения роли
        public RelayCommand ApplyRoleCommand
        {
            get
            {
                return applyRoleCommand ?? (applyRoleCommand = new RelayCommand((o) =>
                {
                    ApplyRole();
                }, o => SelectedUser != null && SelectedRole != null));
            }
        }

        // Удаление пользователя
        private void DeleteUserExecute()
        {
            if (SelectedUser == null) return;

            User user = App.db.Users.Find(SelectedUser.IdUser);
            App.db.Users.Remove(user);
            App.db.SaveChanges();

            Users.Remove(SelectedUser);
            SelectedUser = null;
        }

        // Команда удаления пользователя
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ?? (deleteUserCommand = new RelayCommand((o) =>
                {
                    DeleteUserExecute();
                }, o => SelectedUser != null));
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
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
