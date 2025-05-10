using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace WpfTaskManager
{
    public class AddCatVM : INotifyPropertyChanged
    {
        private string name;
        private Color color;
        private RelayCommand closeCommand;
        public RelayCommand addCommand;

        public AddCatVM()
        {
            color = new Color();
            color.A = 255;
            color.R = 255;
            color.G = 0;
            color.B = 0;
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

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged();
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


        // Проверка на уникальность названия создаваемого проекта
        private bool isUnique()
        {
            //using (AppContext db = new AppContext())
            //{
                foreach (Category c in App.db.Categories)
                {
                    if (c.Name == Name.Trim() || (c.Color_R == Color.R && c.Color_G == Color.G && c.Color_B == Color.B))
                    {
                        return false;
                    }
                }
            //}

            return true;
        }

        // Условие запуска команды добавления проекта/задачи
        private bool AddCanExecute()
        {
            return Name != null && Name.Trim().Length != 0 && Name.Trim().Length <= 30 && isUnique();
        }

        // Команда добавления проекта/задачи
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand((o) =>
                {
                    Window w = o as Window;
                    w.DialogResult = true;
                }, o => AddCanExecute()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
