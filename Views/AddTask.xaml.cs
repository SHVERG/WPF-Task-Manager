using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public partial class AddTask : Window
    {
        AppContext db;

        public AddTask(int id)
        {
            InitializeComponent();

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

        private void Check()
        {
            AddTask_button.IsEnabled = (Name_textbox.Text.Trim().Length != 0 && Name_textbox.Text.Trim().Length <= 30 && Description_textbox.Text.Length <= 150 && Deadline_datepicker.SelectedDate != null);
        }

        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            Check();
        }

        private void Deadline_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Check();
        }
    }
}
