using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public partial class AddProject : Window
    {
        //AppContext db = new AppContext();

        public AddProject()
        {
            InitializeComponent();
            Deadline_datepicker.BlackoutDates.AddDatesInPast();
        }

        /*

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Opacity = 1;
            Close();
        }

        private void AddProject_button_Click(object sender, RoutedEventArgs e)
        {
            bool isUnique = true;

            foreach (Project pr in db.Projects)
            {
                if (pr.Name == Name_textbox.Text.Trim())
                {
                    isUnique = false;
                    MessageBox mb = new MessageBox();
                    mb.Owner = this;
                    mb.Show("Error!", "\"Name\" field must be unique!", MessageBoxButton.OK);
                }

            }

            if (isUnique)
            {
                this.Owner.Opacity = 1;
                this.DialogResult = true;
            }
        }

        private void Check()
        {
            AddProject_button.IsEnabled = (Name_textbox.Text.Trim().Length != 0 && Name_textbox.Text.Trim().Length <= 30 && Description_textbox.Text.Length <= 150 && Deadline_datepicker.SelectedDate != null);
        }

        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            Check();
        }

        private void Deadline_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Check();
        }
        */
    }
}
