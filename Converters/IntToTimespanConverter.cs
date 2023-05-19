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

            int hrs = original / 3600;
            int mins = (original - hrs * 3600) / 60;

            return $"{hrs} hours {mins} minutes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
