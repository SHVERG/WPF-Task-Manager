using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WpfTaskManager
{
    public class EditVM : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private bool isProject = true;
        private Task t;
        private Project p;

        private RelayCommand closeCommand;
        private RelayCommand editCommand;
        
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

        private bool isUnique()
        {
            if (isProject)
            {
                if (p.Name == Name && p.Description == Description)
                    return false;

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
            else
            {
                return t.Name != Name || t.Description != Description;
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

        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.DialogResult = true;
                }, o => Name != null && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && Description.Trim().Length <= 150 && isUnique()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
