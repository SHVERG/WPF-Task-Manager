using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class AddProjectVM : INotifyPropertyChanged
    {
        private RelayCommand closeCommand;
        private RelayCommand addCommand;
        private string name = "";
        private string description = "";
        private DateTime? deadline = null;

        public AddProjectVM()
        {

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

        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ?? (closeCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    Window owner = w.Owner as Window;
                    w.Close();
                }));
            }
        }

        private bool isUnique()
        {
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

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.DialogResult = true;
                }, o => Name != null && Deadline != null && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && Description.Trim().Length <= 150 && isUnique()));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
