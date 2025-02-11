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

namespace LibraryApp.UI.Pgs.ReaderModule
{
    /// <summary>
    /// Логика взаимодействия для BooksRecommendationPage.xaml
    /// </summary>
    public partial class BooksRecommendationPage : Page
    {
        public BooksRecommendationPage()
        {
            InitializeComponent();
            int readerId = TUser.userId;
            var recommendedBooks = TBook.RecommendBooks(readerId);

            
            foreach (var book in recommendedBooks)
            {
                LbxBooks.Items.Add(book);
            }
            if (LbxBooks.Items.Count == 0)
            {
                LblNoResults.Visibility = Visibility.Visible;
            }
        }

        private void BtnMoreDetailed_Click(object sender, RoutedEventArgs e)
        {
            
            TFrame.mainFrame.Navigate(new DetailedInfoBookPage((sender as Button).DataContext as DB.Book));
        }
    }
}
