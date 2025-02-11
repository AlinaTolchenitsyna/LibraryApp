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
    /// Логика взаимодействия для BookFeedbackLibrarianPage.xaml
    /// </summary>
    public partial class BookFeedbackLibrarianPage : Page
    {
        public BookFeedbackLibrarianPage(DB.Book selectedBook)
        {
            InitializeComponent();
            DataContext = selectedBook;
            TBook.bookId = selectedBook.IdBook;
            var currentFeedbacks = DB.LibraryEntities.GetContext().Feedback.Where(b => b.IdBook == selectedBook.IdBook).ToList();
            LbxFeedbacks.ItemsSource = currentFeedbacks;
            int contentCount = currentFeedbacks.Count;
            BL.TLabel.UpdateLabel(contentCount, LblNoFeedbacks);

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var feedbackForReceiving = (sender as Button).DataContext as DB.Feedback;
            BL.TFeedback.DeleteFeedback(feedbackForReceiving, LbxFeedbacks);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }
    }
}
