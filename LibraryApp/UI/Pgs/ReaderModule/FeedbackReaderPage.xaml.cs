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
    /// Логика взаимодействия для FeedbackReaderPage.xaml
    /// </summary>
    public partial class FeedbackReaderPage : Page
    {
        DB.Checkout existingCheckout;
        public FeedbackReaderPage(DB.Checkout currentCheckout)
        {
            InitializeComponent();
            DataContext = currentCheckout;
            TBook.bookId = currentCheckout.IdBook;
            var myFeedback = DB.LibraryEntities.GetContext().Feedback.Where(f => f.IdBook == currentCheckout.IdBook && f.IdReader == TUser.userId).ToList();
            var allFeedbacks = DB.LibraryEntities.GetContext().Feedback.Where(f => f.IdBook == currentCheckout.IdBook && f.IdReader != TUser.userId).ToList();
            LbxFeedbacks.ItemsSource = allFeedbacks;
            LbxMyFeedback.ItemsSource = myFeedback;
            
            if (myFeedback.Count == 0)
            {
                BtnAdd.Visibility = Visibility.Visible;
                LblAddFeedback.Visibility = Visibility.Visible;
            }
            
            if (allFeedbacks.Count == 0)
            {
                LblNoFeedbacks.Visibility = Visibility.Visible;
            }
            existingCheckout = currentCheckout;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TCheckout.checkoutId = existingCheckout.IdCheckout;
            TFrame.mainFrame.Navigate(new AddEditFeedbackPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            TCheckout.checkoutId = existingCheckout.IdCheckout;
            TFrame.mainFrame.Navigate(new AddEditFeedbackPage((sender as Button)
                .DataContext as DB.Feedback));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var feedbackForReceiving = (sender as Button).DataContext as DB.Feedback;

            BL.TFeedback.DeleteMyFeedback(feedbackForReceiving, LbxMyFeedback, existingCheckout);
            var myFeedBack = DB.LibraryEntities.GetContext().Feedback.Where(f => f.IdBook == existingCheckout.IdBook && f.IdReader == TUser.userId).ToList();
            if (myFeedBack.Count() == 0)
            {
                BtnAdd.Visibility = Visibility.Visible;
                LblAddFeedback.Visibility = Visibility.Visible;
            }

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }
    }
}
