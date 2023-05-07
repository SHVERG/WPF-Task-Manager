using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public partial class Add : Window
    {
        public Add(int? id)
        {
            if (id.HasValue)
            {
                using (AppContext db = AppContext.ReCreate())
                {
                    CalendarDateRange range = new CalendarDateRange();
                    range.Start = db.Projects.Find(id).Deadline.AddDays(1);
                    range.End = DateTime.MaxValue;
                }
            }

            InitializeComponent();
            Deadline_datepicker.BlackoutDates.AddDatesInPast();
        }
    }
}
