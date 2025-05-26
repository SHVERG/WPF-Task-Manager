using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? original = (DateTime?)value;

            return !original.HasValue ? App.Current.TryFindResource("report_not_comp") : original.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string original = (string)value;
            return DateTime.Parse(original);
        }
    }
}
