using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTaskManager
{
    public partial class MBWindow : Window
    {
        public MBWindow()
        {
            InitializeComponent();
        }

        void AddButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("OK", MessageBoxResult.OK);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException("Unknown button value", "buttons");
            }
        }

        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            Button button = new Button() { Content = text, IsCancel = isCancel, Margin = new Thickness(5, 0, 5, 0) };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }

        MessageBoxResult Result = MessageBoxResult.None;

        public MessageBoxResult Show(string caption, string message, MessageBoxButton buttons)
        {
            CaptionContainer.Content = caption;
            MessageContainer.Text = message;
            AddButtons(buttons);
            ShowDialog();
            return Result;
        }
    }

}
