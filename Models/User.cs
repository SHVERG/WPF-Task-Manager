using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WpfTaskManager
{
    public class User : INotifyPropertyChanged
    {
        [Key]
        public int IdUser { get; set; }
        [ForeignKey("Roles")]
        private int idRole;
        private string username;
        private string name;
        private string email;
        private string password;

        public int IdRole
        {
            get
            {
                return idRole;
            }
            set
            {
                idRole = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
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

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public User() { }

        public User(string username, string name, string email, string password)
        {
            IdRole = 3;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
