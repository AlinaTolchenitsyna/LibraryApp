using LibraryApp.UI.Pgs.LibrarianModule;
using LibraryApp.UI.Pgs.ReaderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TPage
    {
        /// <summary>
        /// Метод перехода по страницам для библиотекаря
        /// </summary>
        /// <param name="NavigationListBox"></param>
        /// <param name="ContentFrame"></param>
        internal static void DisplayLibrarianPage(ListBox NavigationListBox, Frame ContentFrame)
        {
            if (NavigationListBox.SelectedItem is ListBoxItem item)
            {
                switch (item.Content.ToString())
                {
                    case "Книги":
                        ContentFrame.Navigate(new BooksLibrarianPage());
                        break;
                    case "Читатели":
                        ContentFrame.Navigate(new ReadersPage());
                        break;
                    case "Мероприятия":
                        ContentFrame.Navigate(new EventLibrarianPage());
                        break;
                    case "Статистика":
                        ContentFrame.Navigate(new StatisticsPage());
                        break;
                }
            }
        }

        /// <summary>
        /// Метод перехода по страницам для читателя
        /// </summary>
        /// <param name="NavigationListBox"></param>
        /// <param name="ContentFrame"></param>
        internal static void DisplayReaderPage(ListBox NavigationListBox, Frame ContentFrame)
        {
            if (NavigationListBox.SelectedItem is ListBoxItem item)
            {
                switch (item.Content.ToString())
                {
                    case "Книги":
                        ContentFrame.Navigate(new BooksReaderPage());
                        break;
                    case "Мероприятия":
                        ContentFrame.Navigate(new EventReaderPage());
                        break;
                    case "Моя история":
                        ContentFrame.Navigate(new HistoryPage());
                        break;
                    case "Рекомендации":
                        ContentFrame.Navigate(new BooksRecommendationPage());
                        break;

                }
            }
        }
    }
}
