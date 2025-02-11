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
    /// Логика взаимодействия для BooksLibrarianPage.xaml
    /// </summary>
    public partial class BooksLibrarianPage : Page
    {
        public BooksLibrarianPage()
        {
            InitializeComponent();

            LbxBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList();
            DataObject.AddPastingHandler(TbxSearchId, OnPaste);

            BL.TGenre.LoadGenres(CbGenre);
        }

        private void CbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DB.LibraryEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LbxBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditBookPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditBookPage((sender as Button)
                .DataContext as DB.Book));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            BL.TBook.DeleteBook(LbxBooks);
            TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }



        private void TbxSearchId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            BL.TTextbox.CheckSymbol(e);
            if (!string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                TbxSearchAuthorName.Clear();
            }
            TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            BL.TTextbox.CheckPasting(e);
        }

        

        private void TbxSearchId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TbxSearchAuthorName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchId.Text))
            {
                TbxSearchId.Clear();
            }
            TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void TbxSearchAuthorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
            else if (string.IsNullOrEmpty(TbxSearchId.Text))
            {
                TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }

        }

        private void TbxSearchId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchId.Text))
            {
                TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
            else if (string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
        }

        private void CbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        

        private void BtnMoreDetailed_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new BookFeedbackLibrarianPage((sender as Button).DataContext as DB.Book));
        }
    }
}
