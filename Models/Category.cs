using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WpfTaskManager
{
    public class Category : INotifyPropertyChanged
    {
        [Key] public int IdCat { get; set; }

        private string name;
        private int color_r, color_g, color_b;

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

        public int Color_R
        {
            get
            {
                return color_r;
            }
            set
            {
                color_r = value;
                OnPropertyChanged();
            }
        }

        public int Color_G
        {
            get
            {
                return color_g;
            }
            set
            {
                color_g = value;
                OnPropertyChanged();
            }
        }
        public int Color_B
        {
            get
            {
                return color_b;
            }
            set
            {
                color_b = value;
                OnPropertyChanged();
            }
        }

        public Category() { }

        public Category(string name, int r, int g, int b)
        {
            Name = name;
            Color_R = r;
            Color_G = g;
            Color_B = b;
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
