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
            if (!original.HasValue)
            {
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        return "Не выполнен(а)";
                    default:
                        return "Not completed";
                }
            }
            return original.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string original = (string)value;
            return DateTime.Parse(original);
        }
    }
}
