using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        /*
        Task t = null;
        Project p = null;
        bool isProject = false;
        */

        public EditWindow()
        {
            InitializeComponent();

            /*try
            {
                t = (Task)obj;
            }
            catch (InvalidCastException)
            {
                p = (Project)obj;
            }

            if (t != null)
            {
                ChangeName("TASK");
                Name_textbox.Text = t.Name;
                Description_textbox.Text = t.Description;
            }
            else
            {
                ChangeName("PROJECT");
                Name_textbox.Text = p.Name;
                Description_textbox.Text = p.Description;
                isProject = true;
            }*/
        }

        /*
        private void ChangeName(string s)
        {
            Name_label.Content = Name_label.Content.ToString().Insert(0, s);
            Edit_button.Content = Edit_button.Content.ToString().Insert(Edit_button.Content.ToString().Length, s);
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            //this.Owner.Opacity = 1;
            Close();
        }

        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Opacity = 1;
            this.DialogResult = true;
        }

        private void Check()
        {
            if (isProject)
            {
                using (AppContext db = new AppContext())
                {
                    Edit_button.IsEnabled = (Name_textbox.Text.Trim().Length != 0 && Name_textbox.Text.Trim().Length <= 30 && Description_textbox.Text.Length <= 150)&&!(db.Projects.Where(x => x.IdProject != p.IdProject && x.Name == Name_textbox.Text.Trim()).Any());
                }
            }
            else
                Edit_button.IsEnabled = (Name_textbox.Text.Trim().Length != 0 && Name_textbox.Text.Trim().Length <= 30 && Description_textbox.Text.Length <= 150);
        }

        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            Check();
        }
        */
    }
}
