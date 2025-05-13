using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTaskManager
{
    public class ProjectSelectVM : INotifyPropertyChanged
    {
        private RelayCommand confirmCommand, closeCommand;

        // Конструкторы
        public ProjectSelectVM() { }

        public ProjectSelectVM(IEnumerable<Project> userProjects)
        {
            AvailableProjects = new ObservableCollection<Project>(userProjects);
        }

        public ObservableCollection<Project> AvailableProjects { get; }

        public Project SelectedProject { get; set; }

        // Команда подтверждения
        public RelayCommand ConfirmCommand
        {
            get
            {
                return confirmCommand ?? (confirmCommand = new RelayCommand((o) =>
                {
                    ProjectSelected?.Invoke(SelectedProject);
                }, o => SelectedProject != null));
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

        public event Action<Project> ProjectSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
