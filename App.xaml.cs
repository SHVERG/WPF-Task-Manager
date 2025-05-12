using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Globalization;
using WpfTaskManager.Properties;

namespace WpfTaskManager
{
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public static AppContext db;

        public App()
        {
            InitializeComponent();
            db = new AppContext();
            LanguageChanged += App_LanguageChanged;

            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("ru-RU"));

            Language = Settings.Default.DefaultLanguage;
        }

        //Ивент для оповещения всех окон приложения
        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                // Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                // Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                // Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Current.Resources.MergedDictionaries.Add(dict);
                }

                // Вызываем ивент для оповещения всех окон
                LanguageChanged(Current, new EventArgs());
            }
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            Settings.Default.DefaultLanguage = Language;
            Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Settings.Default.AutoLogin && db.Users.FirstOrDefault(u => u.Username == Settings.Default.SavedUsername && u.IdRole == db.Roles.FirstOrDefault(r => r.HasAccess == 1).IdRole) != null)
            {
                var main = new MainWindow()
                {
                    DataContext = new MainVM()
                    {
                        User = db.Users.FirstOrDefault(u => u.Username == Settings.Default.SavedUsername)
                    }
                };
                main.Show();
                Current.MainWindow = main;
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        }
    }
}
