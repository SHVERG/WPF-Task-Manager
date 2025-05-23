﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class EditVM : INotifyPropertyChanged
    {
        private string name, description;
        private bool isProject = true;
        private Task t;
        private Project p;

        private RelayCommand closeCommand, editCommand;

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
                return t.Name != Name || t.Description != Description;
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
