using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        private string passwordhash;
        private string salt;

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

        public string PasswordHash
        {
            get
            {
                return passwordhash;
            }
            set
            {
                passwordhash = value;
                OnPropertyChanged();
            }
        }

        public string Salt
        {
            get
            {
                return salt;
            }
            set
            {
                salt = value;
                OnPropertyChanged();
            }
        }

        public User() { }

        public User(string username, string name, string email, string passwordHash, string salt)
        {
            IdRole = App.db.Roles.FirstOrDefault(r => r.Name == "Unregistered").IdRole;
            Username = username;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public AppContext AppContext
        {
            get => default;
            set
            {
            }
        }
    }
}
