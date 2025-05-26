using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTaskManager
{
    public class CompletionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? original = value as DateTime?;

            return !original.HasValue ? App.Current.TryFindResource("report_not_comp").ToString() : App.Current.TryFindResource("comp").ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
