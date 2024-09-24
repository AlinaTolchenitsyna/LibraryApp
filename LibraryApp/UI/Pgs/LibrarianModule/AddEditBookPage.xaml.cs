using DocumentFormat.OpenXml.Drawing.Charts;
using LibraryApp.BL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для AddEditBookPage.xaml
    /// </summary>
    public partial class AddEditBookPage : Page
    {
        private DB.Book originalBook;
        private DB.Book currentBook = new DB.Book();
        private byte[] originalImageData;
        private byte[] currentImageData;

        public AddEditBookPage(DB.Book selectedBook)
        {
            InitializeComponent();

            BL.TBook.CloneBook(selectedBook, originalBook, ref currentBook, originalImageData, BookImage);

            DataContext = currentBook;

            CbPublishingHouse.ItemsSource = DB.LibraryEntities.GetContext().PublishingHouse.ToList();
            CbGenre.ItemsSource = DB.LibraryEntities.GetContext().Genre.ToList();

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BL.TBook.CancelBookChanges(e, currentBook, originalBook, currentImageData, originalImageData, BookImage);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Вы точно хотите сохранить книгу?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (BL.TBook.IsBookCorrect(currentBook, CbPublishingHouse, CbGenre))
                {
                    
                    BL.TBook.SaveBook(currentImageData, currentBook);
                    TFrame.mainFrame.Navigate(new BooksLibrarianPage());
                }
            }
        }

        

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            BL.TImage.SelectImage(ref currentImageData, BookImage);
        }

        

        private void BtnDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            var currentBook = DataContext as DB.Book;
            if (currentBook.Photo != null)
            {
                BL.TBook.DeleteBookImage(currentBook, BookImage);
            }
            else
            {
                MessageBox.Show("У книги отсутствует изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }
    }
}

