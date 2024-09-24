
using System;
using System.Collections.Generic;
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

namespace LibraryApp.UI.Wnds
{
    /// <summary>
    /// Логика взаимодействия для LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (TbxLogin.Foreground == Brushes.Gainsboro)
            {
                errors.AppendLine("Введите логин");
            }
            if (PswBox.Password == "")
            {
                errors.AppendLine("Введите пароль");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BL.TUser.Authorize(TbxLogin, PswBox, this);
        }

        private void TbxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbxLogin.Foreground == Brushes.Gainsboro)
            {
                TbxLogin.Text = "";
                TbxLogin.Foreground = Brushes.Black;
            }
        }

        private void TbxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TbxLogin.Text == "")
            {
                TbxLogin.Text = "Введите логин";
                TbxLogin.Foreground = Brushes.Gainsboro;

            }
        }

        private void TbxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            PswBox.Visibility = Visibility.Visible;
            TbxPassword.Visibility = Visibility.Collapsed;
            PswBox.Focus();
        }

        private void PswBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PswBox.Password == "")
            {
                TbxPassword.Visibility = Visibility.Visible;
                PswBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
