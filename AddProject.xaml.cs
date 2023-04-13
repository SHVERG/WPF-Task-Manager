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
            if (Name_textbox.Text.Trim().Length != 0 && Deadline_datepicker.Text.Trim().Length != 0)
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
            else
            {
                MessageBox mb = new MessageBox();
                mb.Owner = this;
                mb.Show("Error!", "\"Name\" or \"Deadline\" fields are not filled!", MessageBoxButton.OK);
            }
        }
    }
}
