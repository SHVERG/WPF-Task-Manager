using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public class LoginVM : INotifyPropertyChanged
    {
        AppContext db;

        private RelayCommand loginNavCommand;
        private RelayCommand loginCommand;
        private RelayCommand signupNavCommand;
        private RelayCommand signupCommand;
        private UserControl selectedViewModel;

        private string username;
        private string name;
        private string email;
        private bool incorrectUserOrPass;
        private bool incorrectUsername;
        private bool rememberMe;
        private User user;


        public LoginVM()
        {
            db = new AppContext();
            SelectedViewModel = new LoginUC()
            {
                DataContext = this
            };

            IncorrectUserOrPass = false;
            IncorrectUsername = false;

            /*User = db.Users.Where(u => u.Username == Properties.Settings.Default.SavedUsername).FirstOrDefault();
            if (User != null)
                OpenMainWindow();*/
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

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;

                OnPropertyChanged();
            }
        }

        public bool IncorrectUserOrPass
        {
            get
            {
                return incorrectUserOrPass;
            }
            set
            {
                incorrectUserOrPass = value;
                OnPropertyChanged();
            }
        }

        public bool IncorrectUsername
        {
            get
            {
                return incorrectUsername;
            }
            set
            {
                incorrectUsername = value;
                OnPropertyChanged();
            }
        }

        public bool RememberMe
        {
            get
            { 
                return rememberMe; 
            }
            set
            {
                rememberMe = value;
                OnPropertyChanged();
            }
        }

        public UserControl SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        public event Action LoginSucceeded;

        // Открытие основного окна
        private void OpenMainWindow()
        {
            var main = new MainWindow()
            {
                DataContext = new MainVM()
                {
                    User = User
                }
            };
            main.Show();
            Application.Current.MainWindow = main;

            LoginSucceeded?.Invoke();
        }

        private void SaveAutoLogin()
        {
            if (RememberMe)
            {
                Properties.Settings.Default.SavedUsername = Username;
                Properties.Settings.Default.AutoLogin = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.SavedUsername = string.Empty;
                Properties.Settings.Default.AutoLogin = false;
                Properties.Settings.Default.Save();
            }
        }

        // Команда открытия окна авторизации
        public RelayCommand LogInNavCommand
        {
            get
            {
                return loginNavCommand ?? (loginNavCommand = new RelayCommand((o) =>
                {
                    SelectedViewModel = new LoginUC()
                    {
                        DataContext = this
                    };
                }));
            }
        }

        // Команда авторизации
        public RelayCommand LogInCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand((o) =>
                {
                    User user = db.Users.FirstOrDefault(u => u.Username == Username);
                    if (user == null)
                    {
                        IncorrectUserOrPass = true;
                    }
                    else if (user.Password == SecureStringToString(((PasswordBox)o).SecurePassword))
                    {
                        User = user;
                        SaveAutoLogin();
                        OpenMainWindow();
                    }
                    else
                    {
                        IncorrectUserOrPass = true;
                    }
                }));
            }
        }

        // Команда открытия окна регистрации
        public RelayCommand SignUpNavCommand
        {
            get
            {
                return signupNavCommand ?? (signupNavCommand = new RelayCommand((o) =>
                {
                    SelectedViewModel = new SignupUC()
                    {
                        DataContext = this
                    };
                }));
            }
        }

        // Хэширование пароля
        /*public static string Encode(SecureString value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value.ToString() ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");

        }*/

        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        // Команда регистрации
        public RelayCommand SignUpCommand
        {
            get
            {
                return signupCommand ?? (signupCommand = new RelayCommand((o) =>
                {
                    if (db.Users.FirstOrDefault(u => u.Username == Username) != null)
                    {
                        IncorrectUsername = true;
                    }
                    else
                    {
                        User user = new User(Username, Name, Email, SecureStringToString(((PasswordBox)o).SecurePassword));
                        db.Users.Add(user);
                        db.SaveChanges();
                        User = user;
                        SaveAutoLogin();
                        OpenMainWindow();
                    }
                }, o => Username != null && Username.Length > 5 && ((PasswordBox)o).SecurePassword != null && ((PasswordBox)o).SecurePassword.Length > 5));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
