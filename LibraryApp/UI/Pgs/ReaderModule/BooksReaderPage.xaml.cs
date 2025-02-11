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
    /// Логика взаимодействия для BooksReaderPage.xaml
    /// </summary>
    public partial class BooksReaderPage : Page
    {
        public BooksReaderPage()
        {
            InitializeComponent();
            LbxBooks.ItemsSource = DB.LibraryEntities.GetContext().Book.ToList().Where(b => b.Status.Name.ToString() == "Доступна");
            DataObject.AddPastingHandler(TbxSearchId, OnPaste);
            BL.TGenre.LoadGenres(CbGenre);
        }

        

        private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void CbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void TbxSearchId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            BL.TTextbox.CheckSymbol(e);
            
            if (!string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                TbxSearchAuthorName.Clear();
            }
            BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            BL.TTextbox.CheckPasting(e);
        }

        

        private void TbxSearchId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            BL.TTextbox.BlockSpace(e);
        }

        private void TbxSearchAuthorName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchId.Text))
            {
                TbxSearchId.Clear();
            }
            BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void TbxSearchAuthorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
            else if (string.IsNullOrEmpty(TbxSearchId.Text))
            {
                BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }

        }

        private void TbxSearchId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxSearchId.Text))
            {
                BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
            else if (string.IsNullOrEmpty(TbxSearchAuthorName.Text))
            {
                BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
            }
        }

        

        private void CbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BL.TListBox.UpdateListBoxBooks(LbxBooks, TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LblNoResults);
        }

        private void BtnMoreDetailed_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new DetailedInfoBookPage((sender as Button).DataContext as DB.Book));
        }
    }
}
