using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfTaskManager
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        AppContext db = new AppContext();
        ObservableCollection<ReportProject> projs = new ObservableCollection<ReportProject>();
        ObservableCollection<ReportTask> tasks = new ObservableCollection<ReportTask>();
        bool isProject;

        public Report(bool proj)
        {
            InitializeComponent();
            isProject = proj;

            if (proj)
            {
                Title_label.Content = Title_label.Content.ToString().Insert(0, "PROJECT");
                Report_datagrid.ItemsSource = projs;
            }
            else
            {
                Title_label.Content = Title_label.Content.ToString().Insert(0, "TASK");
                Report_datagrid.ItemsSource = tasks;
            }
        }

        private void Show_button_Click(object sender, RoutedEventArgs e)
        {
            projs.Clear();
            tasks.Clear();

            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                string comp;

                foreach (Project p in db.Projects)
                {
                    if (p.Deadline >= StartDate.SelectedDate && p.Deadline <= EndDate.SelectedDate)
                    {
                        if (ProjectsBox.SelectedIndex == 0)
                        {
                            if (p.Completed != null)
                            {
                                if (p.Completed > p.Deadline)
                                    comp = "Completed in bad time";
                                else
                                    comp = "Completed in time";
                            }
                            else 
                                comp = "Not completed";

                            projs.Add(new ReportProject(p.Name, p.Deadline, comp));
                        }
                        else if (ProjectsBox.SelectedIndex == 1)
                        {
                            if (p.Completed != null)
                            {
                                if (p.Completed > p.Deadline)
                                    comp = "Completed in bad time";
                                else
                                    comp = "Completed in time";

                                projs.Add(new ReportProject(p.Name, p.Deadline, comp));
                            }
                        }
                        else
                        {
                            if (p.Completed == null)
                            {
                                comp = "Not completed";
                                projs.Add(new ReportProject(p.Name, p.Deadline, comp));
                            }
                        }
                    }
                }

                foreach (Task t in db.Tasks)
                {
                    if (t.Deadline >= StartDate.SelectedDate && t.Deadline <= EndDate.SelectedDate)
                    {
                        if (ProjectsBox.SelectedIndex == 0)
                        {
                            if (t.Completed != null)
                            {
                                if (t.Completed > t.Deadline)
                                    comp = "Completed in bad time";
                                else
                                    comp = "Completed in time";
                            }
                            else
                                comp = "Not completed";

                            tasks.Add(new ReportTask(t.Name, db.Projects.Find(t.IdProject).Name, t.Deadline, comp));
                        }
                        else if (ProjectsBox.SelectedIndex == 1)
                        {
                            if (t.Completed != null)
                            {
                                if (t.Completed > t.Deadline)
                                    comp = "Completed in bad time";
                                else
                                    comp = "Completed in time";

                                tasks.Add(new ReportTask(t.Name, db.Projects.Find(t.IdProject).Name, t.Deadline, comp));
                            }
                        }
                        else
                        {
                            comp = "Not completed";
                            tasks.Add(new ReportTask(t.Name, db.Projects.Find(t.IdProject).Name, t.Deadline, comp));
                        }
                    }
                }
            }
            else
            {
                MessageBox mb = new MessageBox();
                mb.Owner = this;
                mb.Show("Error!", "\"Start Date\" or \"End Date\" fields are not filled!", MessageBoxButton.OK); 
            }
        }

        private void Save_button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file|*.csv";

            save.ShowDialog();
            IDataObject objectSave = Clipboard.GetDataObject();


            Report_datagrid.SelectAllCells();
            Report_datagrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader; 
            ApplicationCommands.Copy.Execute(null, Report_datagrid);
            Report_datagrid.UnselectAllCells();

            string pattern = @"^;(.*)$";
            //System.Windows.MessageBox.Show(Clipboard.GetText(TextDataFormat.Text));
            string str = (Clipboard.GetText(TextDataFormat.Text)).Replace("\t", ";");
            str = Regex.Replace(str, pattern, "$1", RegexOptions.Multiline);
            File.WriteAllText(save.FileName, str, Encoding.UTF8);

            if (objectSave != null)
            {
                Clipboard.SetDataObject(objectSave);
            }

        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Opacity = 1;
            Close();
        }
    }
}
