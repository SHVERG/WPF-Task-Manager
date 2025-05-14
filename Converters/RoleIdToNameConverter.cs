using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class RoleIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id)
            {
                Role role = App.db.Roles.FirstOrDefault(r => r.IdRole == id);
                return role?.Name ?? $"Роль {id}";
            }

            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
