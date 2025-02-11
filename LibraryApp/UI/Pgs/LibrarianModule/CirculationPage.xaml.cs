using Irony.Parsing;
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
    /// Логика взаимодействия для CirculationPage.xaml
    /// </summary>
    public partial class CirculationPage : Page
    {
        private DB.Reader currentReader = new DB.Reader();
        public CirculationPage(DB.Reader selectedReader)
        {
            InitializeComponent();
            if (selectedReader != null)
            {
                currentReader = selectedReader;
            }
            DataContext = currentReader;
            LbxBooksOnHands.ItemsSource = BL.TBook.UploadBooksOnHands(currentReader, LbxBooksOnHands, LblNoResults);
            LbxAvailableBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList().Where(b => b.Status.Name.ToString() == "Доступна");
            DataObject.AddPastingHandler(TbxNumberOfBook, OnPaste);
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выдать эту книгу?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                BL.TCheckout.CreateCheckout(TbxNumberOfBook, currentReader);
                LbxBooksOnHands.ItemsSource = BL.TBook.UploadBooksOnHands(currentReader, LbxBooksOnHands, LblNoResults);
                LbxAvailableBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList().Where(b => b.Status.Name.ToString() == "Доступна");
            }
        }

        

        private void BtnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите принять эту книгу?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Button button = sender as Button;
                DB.Checkout selectedCheckout = button.DataContext as DB.Checkout;

                BL.TBook.ReturnBook(selectedCheckout);

                // Обновление интерфейса
                LbxBooksOnHands.ItemsSource = BL.TBook.UploadBooksOnHands(currentReader, LbxBooksOnHands, LblNoResults);
                LbxAvailableBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList().Where(b => b.Status.Name.ToString() == "Доступна");
            }

        }
            

        private void TbxNumberOfBook_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            BL.TTextbox.CheckSymbol(e);
            if (!string.IsNullOrEmpty(TbxNumberOfBook.Text))
            {
                TbxNumberOfBook.Clear();
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            BL.TTextbox.CheckPasting(e);
        }

        

        private void TbxSearchId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            BL.TTextbox.BlockSpace(e);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }

        private void BtnCheckOutFromLB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выдать эту книгу?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Button button = sender as Button;
                var currentBook = button.DataContext as DB.Book;
                BL.TCheckout.CreateCheckoutFromLB(currentBook, currentReader);
                LbxBooksOnHands.ItemsSource = BL.TBook.UploadBooksOnHands(currentReader, LbxBooksOnHands, LblNoResults);
                LbxAvailableBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList().Where(b => b.Status.Name.ToString() == "Доступна");
            }
        }
    }


    }