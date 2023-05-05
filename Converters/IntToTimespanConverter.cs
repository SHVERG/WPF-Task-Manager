using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class IntToTimespanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int original = (int)value;
            return new TimeSpan(original * 10000000);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan original = (TimeSpan)value;
            return (int)(original.Ticks / 10000000);
        }
    }
}
