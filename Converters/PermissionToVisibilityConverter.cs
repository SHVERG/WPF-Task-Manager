using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class PermissionToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false; // Для обратной логики

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || parameter == null)
                return Visibility.Collapsed;

            int roleId;
            if (!int.TryParse(value.ToString(), out roleId))
                return Visibility.Collapsed;

            Role role = App.db.Roles.Find(roleId);
            if (role == null)
                return Visibility.Collapsed;

            string permissionName = parameter.ToString();
            var property = typeof(Role).GetProperty(permissionName, BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return Visibility.Collapsed;

            var result = property.GetValue(role);
            bool hasPermission = false;

            if (result is int intValue)
            {
                hasPermission = intValue != 0;
            }
            else if (result is bool boolValue)
            {
                hasPermission = boolValue;
            }

            if (Invert)
                hasPermission = !hasPermission;

            return hasPermission ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
