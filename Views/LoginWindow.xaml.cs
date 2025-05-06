using System;
using System.Windows;
using System.Windows.Input;

namespace WpfTaskManager
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var vm = new LoginVM();
            vm.LoginSucceeded += () =>
            {
                Close();
            };

            DataContext = vm;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Выход из приложения
        private void Exit_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Сворачивание окна
        private void Hide_button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
