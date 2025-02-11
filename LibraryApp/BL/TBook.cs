using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.DB;
using System.Windows;
using LibraryApp.UI.Pgs.LibrarianModule;
using System.Data.SqlClient;
using System.Data;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
using LiveCharts.Wpf.Charts.Base;

namespace LibraryApp.BL
{
    public class TBook
    {
        
        internal static int bookId { get; set; }

        /// <summary>
        /// Метод клонирования книги для изменения ее копии
        /// </summary>
        /// <param name="selectedBook"></param>
        /// <param name="originalBook"></param>
        /// <param name="currentBook"></param>
        /// <param name="originalImageData"></param>
        /// <param name="BookImage"></param>
        internal static void CloneBook(Book selectedBook, Book originalBook, ref Book currentBook, byte[] originalImageData,Image BookImage)
        {
            if (selectedBook != null)
            {
                originalBook = selectedBook.Clone();
                currentBook = selectedBook;
                originalImageData = selectedBook.Photo;
                BookImage.Source = TImage.LoadBitmapImage(selectedBook.Photo);
            }
        }
        
        /// <summary>
        /// Проверка книги на корректность заполненных значений
        /// </summary>
        /// <param name="currentBook"></param>
        /// <param name="CbPublishingHouse"></param>
        /// <param name="CbGenre"></param>
        /// <param name="CbStatus"></param>
        /// <returns></returns>
        internal static bool IsBookCorrect (DB.Book currentBook, ComboBox CbPublishingHouse, ComboBox CbGenre)
        {
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(currentBook.Name) || currentBook.Name.Length > 50 || currentBook.Name.Trim().Length == 0)
            {
                errors.AppendLine("Название книги должно содержать от 1 до 50 символов");
            }
            if (String.IsNullOrWhiteSpace(currentBook.Author) || currentBook.Author.Length > 50 || currentBook.Author.Trim().Length == 0)
            {
                errors.AppendLine("ФИО автор(-ов) должны содержать от 1 до 50 символов");
            }
            if (CbPublishingHouse.SelectedItem == null)
            {
                errors.AppendLine("Укажите издательство");
            }
            if (currentBook.PublishingYear < 1500 || currentBook.PublishingYear > DateTime.Now.Year || currentBook.PublishingYear.ToString().Trim().Length == 0)
            {
                errors.AppendLine($"Год издательства должен быть от 1500 до {DateTime.Now.Year}");
            }
            if (currentBook.NumberOfPages < 1 || currentBook.NumberOfPages > 10000 || currentBook.NumberOfPages.ToString().Trim().Length == 0)
            {
                errors.AppendLine("Количество страниц должно быть от 1 до 10000");
            }
            if (CbGenre.SelectedItem == null)
            {
                errors.AppendLine("Укажите жанр книги");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Метод сохранения книги вместе с изображением
        /// </summary>
        /// <param name="currentImageData"></param>
        /// <param name="currentBook"></param>
        internal static void SaveBook(byte[] currentImageData, Book currentBook)
        {

            var context = DB.LibraryEntities.GetContext();

            if (currentBook.IdBook == 0)
            {
                context.Book.Add(currentBook);
                var issuedStatus = context.Status.FirstOrDefault(s => s.Name == "Доступна");
                currentBook.IdStatus = issuedStatus.IdStatus;
                context.SaveChanges();  
            }

            try
            {
                context.SaveChanges();

                if (currentImageData != null)
                {
                    using (SqlConnection conn = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                    {
                        string query = "UPDATE Book SET Photo = @Photo WHERE IdBook = @BookId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = currentImageData ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@BookId", SqlDbType.Int).Value = currentBook.IdBook;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                else if (currentBook.Photo == null) 
                {
                    using (SqlConnection conn = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                    {
                        string query = "UPDATE Book SET Photo = NULL WHERE IdBook = @BookId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@BookId", SqlDbType.Int).Value = currentBook.IdBook;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

                MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        
        /// <summary>
        /// Метод удаления книги
        /// </summary>
        /// <param name="LbxBooks"></param>
        internal static void DeleteBook(ListBox LbxBooks)
        {
            var bookForReceiving = LbxBooks.SelectedItems.Cast<DB.Book>().ToList();
            var bookIds = bookForReceiving.Select(book => book.IdBook).ToList();
            int bookId = 0; 
            foreach (var id in bookIds)
            {
                bookId = id;
            }
            var foundBook = DB.LibraryEntities.GetContext().Book.FirstOrDefault(book => book.IdBook == bookId);
            if (bookForReceiving.Count() > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить выбранную книгу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var issuedStatus = DB.LibraryEntities.GetContext().Status.FirstOrDefault(s => s.Name == "Доступна");
                        if (foundBook.IdStatus == issuedStatus.IdStatus)
                        {
                            DB.LibraryEntities.GetContext().Book.RemoveRange(bookForReceiving);
                            DB.LibraryEntities.GetContext().SaveChanges();
                            MessageBox.Show("Книга успешно удалена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Эту книгу нельзя удалить, так как она выдана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Метод загрузки книг 
        /// </summary>
        /// <param name="currentReader"></param>
        /// <param name="LbxBooks"></param>
        /// <param name="LblNoResults"></param>
        internal static List<DB.Checkout> UploadBooksOnHands(DB.Reader currentReader, ListBox LbxBooks, Label LblNoResults)
        {
            var context = DB.LibraryEntities.GetContext();

            var booksOnHands = context.Checkout
    .Where(c => c.IdReader == currentReader.IdReader && c.DateReturn == null)
    .ToList();

            

            int contentCount = booksOnHands.Count();
            TLabel.UpdateLabel(contentCount, LblNoResults);
            return booksOnHands;
        }

        /// <summary>
        /// Метод возвращения книги
        /// </summary>
        /// <param name="selectedCheckout"></param>
        /// <exception cref="Exception"></exception>
        internal static void ReturnBook(DB.Checkout selectedCheckout)
        {
            
            TCheckout.checkoutId = selectedCheckout.IdCheckout;
            bookId = selectedCheckout.IdBook;

            var context = DB.LibraryEntities.GetContext();

            // Получение активной записи о выдаче этой книги, в которой она еще не возвращена
            var checkoutRecord = context.Checkout.FirstOrDefault(c => c.IdBook == selectedCheckout.IdBook && c.DateReturn == null);

            if (checkoutRecord != null)
            {
                checkoutRecord.DateReturn = DateTime.Now;
                checkoutRecord.IdLibrarianReturn = TUser.userId;
                int bookId = selectedCheckout.IdBook;
                var book = context.Book.FirstOrDefault(b => b.IdBook == bookId);
                var issuedStatus = context.Status.FirstOrDefault(s => s.Name == "Доступна");
                if (issuedStatus == null)
                {
                    throw new Exception("Статус 'Доступна' не найден в таблице Status.");
                }
                book.IdStatus = issuedStatus.IdStatus;
                context.SaveChanges();

                
                MessageBox.Show("Книга успешно возвращена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Метод отмены изменений
        /// </summary>
        /// <param name="e"></param>
        /// <param name="currentBook"></param>
        /// <param name="originalBook"></param>
        /// <param name="currentImageData"></param>
        /// <param name="originalImageData"></param>
        /// <param name="BookImage"></param>
        internal static void CancelBookChanges(DependencyPropertyChangedEventArgs e, DB.Book currentBook, DB.Book originalBook, byte[] currentImageData, byte[] originalImageData, Image BookImage)
        {
            // Если страница становится невидимой
            if (!(bool)e.NewValue)
            {
                // Отменяются изменения, восстанавливаются изначальные значения
                currentBook.CopyFrom(originalBook);
                currentImageData = originalImageData;
                BookImage.Source = BL.TImage.LoadBitmapImage(originalImageData);
            }
        }

        /// <summary>
        /// Метод удаления изображения книги
        /// </summary>
        /// <param name="currentBook"></param>
        /// <param name="BookImage"></param>
        internal static void DeleteBookImage(DB.Book currentBook, Image BookImage)
        {
            
            if (currentBook != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить изображение?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    currentBook.Photo = null; // Скрытие изображения
                    BookImage.Source = null;
                }
            }
        }

        /// <summary>
        /// Метод рекомендации книг
        /// </summary>
        /// <param name="readerId"></param>
        /// <returns></returns>
        internal static List<Book> RecommendBooks(int readerId)
        {
            var _context = DB.LibraryEntities.GetContext();

            // Получение списка книг, которые читал данный читатель
            var booksReadByCurrentReader = _context.Checkout
                .Where(c => c.IdReader == readerId)
                .Select(c => c.IdBook)
                .ToList();

            // Получение списка книг, которые положительно оценивал данный читатель
            var positivelyRatedBooksByCurrentReader = _context.Feedback
                .Where(f => f.IdReader == readerId && f.Rating >= 4 && f.Rating <= 5)
                .Select(f => f.IdBook)
                .ToList();

            // Поиск других читателей, которые читали и положительно оценивали те же книги
            var otherReaders = _context.Feedback
                .Where(f => positivelyRatedBooksByCurrentReader.Contains(f.IdBook) && f.IdReader != readerId)
                .Select(f => f.IdReader)
                .Distinct()
                .ToList();

            // Поиск книг, которые читали и положительно оценили другие читатели, но не читал текущий читатель
            var recommendedBooks = _context.Feedback
                .Where(f => otherReaders.Contains(f.IdReader) && !booksReadByCurrentReader.Contains(f.IdBook))
                .GroupBy(f => f.IdBook)
                .Select(group => new
                {
                    BookId = group.Key,
                    AverageRating = group.Average(f => f.Rating),
                    TotalReaders = group.Count()
                })
                .OrderByDescending(result => result.TotalReaders)
                .ThenByDescending(result => result.AverageRating)
                .Take(10) // Получение максимум 10 книг
                .Select(result => result.BookId)
                .ToList();

            // Получение деталей рекомендованных книг
            var recommendedBooksDetails = _context.Book
                .Where(book => recommendedBooks.Contains(book.IdBook))
                .ToList();

            // Возвращение списка рекомендованных книг
            return recommendedBooksDetails;
        }



    }
}
