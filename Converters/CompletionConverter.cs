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

            if (!original.HasValue)
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        return "Не завершено";
                    default:
                        return "Not Completed";

                }
            else
                switch (App.Language.Name)
                {
                    case "ru-RU":
                        return "Завершено";
                    default:
                        return "Completed";
                }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
