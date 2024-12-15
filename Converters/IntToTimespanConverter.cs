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

            string s_hrs, s_mins;

            switch (App.Language.Name)
            {
                case "ru-RU":
                    {
                        s_hrs = "часов";
                        s_mins = "минут";
                        break;
                    }
                default:
                    {
                        s_hrs = "hours";
                        s_mins = "minutes";
                        break;
                    }
            }

            return $"{hrs} {s_hrs} {mins} {s_mins}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
