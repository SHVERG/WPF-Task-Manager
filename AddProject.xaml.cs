using System.Windows;

namespace WpfTaskManager
{
    /// <summary>
    /// Логика взаимодействия для AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        AppContext db = new AppContext();

        public AddProject()
        {
            InitializeComponent();

            Deadline_datepicker.BlackoutDates.AddDatesInPast();
        }

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

        private void NameChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.SelectedDate != null)
                AddProject_button.IsEnabled = true;
            else
                AddProject_button.IsEnabled = false;
        }

        private void Deadline_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.SelectedDate != null)
                AddProject_button.IsEnabled = true;
            else
                AddProject_button.IsEnabled = false;
        }
    }
}
