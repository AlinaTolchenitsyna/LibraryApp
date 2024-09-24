using LibraryApp.BL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryApp.UI.Pgs.LibrarianModule
{
    /// <summary>
    /// Логика взаимодействия для ReadersPage.xaml
    /// </summary>
    public partial class ReadersPage : Page
    {
        public ReadersPage()
        {
            InitializeComponent();
            LbxReaders.ItemsSource = DB.LibraryEntities.GetContext().Reader.ToList();
        }

        private void BtnIssueBook_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new CirculationPage((sender as Button).DataContext as DB.Reader));
        }

        

        

        

        private void TbxSearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            BL.TReader.UpdateReaders(TbxSearchName, LbxReaders, LblNoResults);
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditReaderPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditReaderPage((sender as Button)
                .DataContext as DB.Reader));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            BL.TReader.DeleteReader(LbxReaders, TbxSearchName, LblNoResults);

        }
    }
}
