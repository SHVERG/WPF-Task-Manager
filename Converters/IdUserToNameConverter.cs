using System.Collections.ObjectModel;
using System.Globalization;
using System;
using System.Linq;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class IdUserToNameConverter : IValueConverter
    {
        public ObservableCollection<User> Users { get; set; }  // источник пользователей

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Users = new ObservableCollection<User>(App.db.Users);
            if (value is int userId && Users != null)
            {
                User user = Users.FirstOrDefault(u => u.IdUser == userId);
                if (user != null)
                {
                    return user.Name;
                }
            }
            return App.Current.TryFindResource("no_reliable").ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
