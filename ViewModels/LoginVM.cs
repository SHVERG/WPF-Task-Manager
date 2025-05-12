using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfTaskManager.Properties;

namespace WpfTaskManager
{
    public class LoginVM : INotifyPropertyChanged
    {
        private RelayCommand loginNavCommand, loginCommand, signupNavCommand, signupCommand;
        private UserControl selectedViewModel;

        private string username, name, email;
        private bool incorrectUserOrPass, incorrectUsername, rememberMe;
        private User user;

        // Конструктор
        public LoginVM()
        {
            SelectedViewModel = new LoginUC()
            {
                DataContext = this
            };

            IncorrectUserOrPass = false;
            IncorrectUsername = false;
        }

        // Свойства
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
            Window main = new MainWindow()
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
                Settings.Default.SavedUsername = Username;
                Settings.Default.AutoLogin = true;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.SavedUsername = string.Empty;
                Settings.Default.AutoLogin = false;
                Settings.Default.Save();
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
                    User user = App.db.Users.FirstOrDefault(u => u.Username == Username);
                    if (user == null)
                    {
                        IncorrectUserOrPass = true;
                    }
                    else if (PasswordHelper.VerifyPassword(((PasswordBox)o).Password, user.PasswordHash, user.Salt))
                    {
                        if (user.IdRole == App.db.Roles.FirstOrDefault(r => r.Name == "Unregistered").IdRole)
                        {
                            MBWindow mb = new MBWindow();
                            mb.Show(Application.Current.TryFindResource("login_request_not_confirmed_header").ToString(), Application.Current.TryFindResource("login_request_not_confirmed_body").ToString().Replace("\\n", Environment.NewLine), MessageBoxButton.OK);
                        }
                        else
                        {
                            User = user;
                            SaveAutoLogin();
                            OpenMainWindow();
                        }
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

        // Команда регистрации
        public RelayCommand SignUpCommand
        {
            get
            {
                return signupCommand ?? (signupCommand = new RelayCommand((o) =>
                {
                    if (App.db.Users.FirstOrDefault(u => u.Username == Username) != null)
                    {
                        IncorrectUsername = true;
                    }
                    else
                    {
                        string pass = ((PasswordBox)o).Password;
                        PasswordHelper.CreatePasswordHash(pass, out string hash, out string salt);


                        User user = new User(Username, Name, Email, hash, salt);
                        App.db.Users.Add(user);
                        App.db.SaveChanges();

                        MBWindow mb = new MBWindow();

                        Username = null;
                        Email = null;
                        Name = null;
                        ((PasswordBox)o).Password = null;

                        mb.Show(Application.Current.TryFindResource("login_request_sent_header").ToString(), Application.Current.TryFindResource("login_request_sent_body").ToString().Replace("\\n", Environment.NewLine), MessageBoxButton.OK);
                        }
                }, o => Username != null && Username.Length >= 6 && ((PasswordBox)o).SecurePassword != null && ((PasswordBox)o).SecurePassword.Length >= 6));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
