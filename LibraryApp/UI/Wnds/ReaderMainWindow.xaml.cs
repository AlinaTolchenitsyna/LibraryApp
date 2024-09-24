using LibraryApp.BL;
using LibraryApp.UI.Pgs;
using LibraryApp.UI.Pgs.ReaderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для MainWindowReader.xaml
    /// </summary>
    public partial class ReaderMainWindow : Window
    {
        public ReaderMainWindow()
        {
            InitializeComponent();
            TFrame.mainFrame = ContentFrame;
            BL.TCheckout.DisplayCheckoutNotification();
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TPage.DisplayReaderPage(NavigationListBox, ContentFrame);
            tbkChoosePage.Visibility = Visibility.Hidden;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Close();
        }
    }
}
