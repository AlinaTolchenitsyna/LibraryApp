using LibraryApp.BL;
using LibraryApp.DB;
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
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            var booksOnHands = DB.LibraryEntities.GetContext().Checkout
            .Where(c => c.IdReader == TUser.userId && c.DateReturn == null)
            .ToList();

            var history = DB.LibraryEntities.GetContext().Checkout
            .Where(c => c.IdReader == TUser.userId && c.DateReturn != null)
            .ToList();
            LbxCurrentBooks.ItemsSource = booksOnHands;

            LbxHistory.ItemsSource = history;

            int contentHistoryCount = history.Count;
            BL.TLabel.UpdateLabel(contentHistoryCount, LblNoHistory);

            int contentCurrentBooksCount = booksOnHands.Count;
            BL.TLabel.UpdateLabel(contentCurrentBooksCount, LblNoBooksOnHands);
        }

        


        private void BtnFeedbacks_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new FeedbackReaderPage((sender as Button)
                .DataContext as DB.Checkout));
        }
    }
}
