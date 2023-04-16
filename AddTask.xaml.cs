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
            range.Start = db.Projects.Find(id).Deadline;
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
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.Text.Trim().Length != 0)
            {
                this.Owner.Opacity = 1;
                this.DialogResult = true;
            }
            else
            {
                MessageBox mb = new MessageBox();
                mb.Owner = this;
                mb.Show("Error!", "\"Name\" or \"Deadline\" fields are not filled!", MessageBoxButton.OK);
            }
        }
    }
}
