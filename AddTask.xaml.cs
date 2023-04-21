using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTaskManager
{
    /// <summary>
    /// Логика взаимодействия для AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        AppContext db;
        int id;

        public AddTask(int id)
        {
            InitializeComponent();

            this.id = id;
            db = new AppContext();

            CalendarDateRange range = new CalendarDateRange();
            range.Start = db.Projects.Find(id).Deadline.AddDays(1);
            range.End = DateTime.MaxValue;

            Deadline_datepicker.BlackoutDates.AddDatesInPast();
            Deadline_datepicker.BlackoutDates.Add(range);
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Opacity = 1;
            Close();
        }

        private void AddTask_button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Opacity = 1;
            this.DialogResult = true;
        }

        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.SelectedDate != null)
                AddTask_button.IsEnabled = true;
            else
                AddTask_button.IsEnabled = false;
        }

        private void Deadline_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.SelectedDate != null)
                AddTask_button.IsEnabled = true;
            else
                AddTask_button.IsEnabled = false;
        }
    }
}
