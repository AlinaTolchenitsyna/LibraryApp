using LibraryApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace LibraryApp.BL
{
    internal class TListBox
    {
        /// <summary>
        /// Получение списка книг
        /// </summary>
        /// <param name="TbxSearchAuthorName"></param>
        /// <param name="TbxSearchId"></param>
        /// <param name="CbGenre"></param>
        /// <param name="CbSorting"></param>
        /// <param name="LbxBooks"></param>
        /// <param name="LblNoResults"></param>
        /// <returns></returns>
        private static List<Book> GetUpdatedBooks(TextBox TbxSearchAuthorName, 
            TextBox TbxSearchId, ComboBox CbGenre, ComboBox CbSorting, 
            ListBox LbxBooks, Label LblNoResults)
        {
            var currentBooks = DB.LibraryEntities.GetContext().Book.ToList();
            if (!string.IsNullOrWhiteSpace(TbxSearchAuthorName.Text))
            {
                currentBooks = currentBooks.Where(p => p.Author.ToLower()
                .Contains(TbxSearchAuthorName.Text.ToLower()) || p.Name.ToLower()
                .Contains(TbxSearchAuthorName.Text.ToLower())).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(TbxSearchId.Text))
            {
                currentBooks = currentBooks
                    .Where(p => p.IdBook == int.Parse(TbxSearchId.Text)).ToList();
            }
            if (CbGenre.SelectedItem != null)
            {

                string textGenre = (CbGenre.SelectedItem as DB.Genre).ToString();
                if (CbGenre.SelectedIndex != 0)
                {
                    currentBooks = currentBooks
                        .Where(p => p.Genre == CbGenre.SelectedItem as DB.Genre).ToList();
                }
            }
            if (CbSorting.SelectedItem != null)
            {
                string textSort = 
                    (CbSorting.SelectedItem as ComboBoxItem).Content.ToString();
                switch (textSort)
                {
                    case "Названию: от А до Я":
                        currentBooks = currentBooks.OrderBy(b => b.Name).ToList();
                        break;
                    case "Названию: от Я до А":
                        currentBooks = currentBooks.OrderByDescending(b => b.Name).ToList();
                        break;
                    case "Количеству страниц по возрастанию":
                        currentBooks = currentBooks.OrderBy(b => b.NumberOfPages).ToList();
                        break;
                    case "Количеству страниц по убыванию":
                        currentBooks = currentBooks
                            .OrderByDescending(b => b.NumberOfPages).ToList();
                        break;
                }
            }
            int contentCount = currentBooks.Count();
            TLabel.UpdateLabel(contentCount, LblNoResults);
            return currentBooks;
        }
    
        /// <summary>
        /// Метод обновления ListBox, содержащего список книг
        /// </summary>
        internal static void UpdateListBoxBooks(ListBox LbxBooks, TextBox TbxSearchAuthorName, TextBox TbxSearchId, ComboBox CbGenre, ComboBox CbSorting, Label LblNoResults)
        {
            LbxBooks.ItemsSource = GetUpdatedBooks(TbxSearchAuthorName, TbxSearchId, CbGenre, CbSorting, LbxBooks, LblNoResults);
        }
    }
}
