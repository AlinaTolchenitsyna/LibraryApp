using LibraryApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TGenre
    {
        /// <summary>
        /// Метод загрузки списка жанров в ComboBox
        /// </summary>
        /// <param name="CbGenre"></param>
        internal static void LoadGenres(ComboBox CbGenre)
        {
            var genres = DB.LibraryEntities.GetContext().Genre.ToList();

            // Создание элемента "По умолчанию"
            var defaultGenre = new DB.Genre { IdGenre = -1, Name = "Все" };

            // Добавление элемента "По умолчанию" в начало списка
            genres.Insert(0, defaultGenre);

            CbGenre.ItemsSource = genres;

            // Установка "По умолчанию" как текущий выбор
            CbGenre.SelectedIndex = 0;
        }
    }
}
