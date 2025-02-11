using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TCheckout
    {
        internal static int checkoutId { get; set; }

        /// <summary>
        /// Метод выдачи книги
        /// </summary>
        /// <param name="TbxNumberOfBook"></param>
        /// <param name="currentReader"></param>
        /// <exception cref="Exception"></exception>
        internal static void CreateCheckout(TextBox TbxNumberOfBook, DB.Reader currentReader)
        {
            var context = DB.LibraryEntities.GetContext();

            var newCheckout = new DB.Checkout
            {
                IdBook = int.Parse(TbxNumberOfBook.Text),
                IdReader = currentReader.IdReader,
                IdLibrarianCheckout = TUser.userId,
                DateCheckout = DateTime.Now
            };
            int bookId = int.Parse(TbxNumberOfBook.Text);
            var book = context.Book.FirstOrDefault(b => b.IdBook == bookId);

            if (book != null)
            {
                int booksCheckedOut = context.Checkout.Count(c => c.IdReader == currentReader.IdReader && c.DateReturn == null);

                if (booksCheckedOut >= 10)
                {
                    MessageBox.Show("Нельзя выдать больше 10 книг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var issuedStatus = context.Status.FirstOrDefault(s => s.Name == "Выдана");
                if (book.IdStatus == issuedStatus.IdStatus)
                {
                    MessageBox.Show("Указанная книга уже выдана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                context.Checkout.Add(newCheckout);

                if (issuedStatus == null)
                {
                    throw new Exception("Статус 'Выдана' не найден в таблице Status.");
                }

                book.IdStatus = issuedStatus.IdStatus;
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Указанной книги не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("Книга успешно выдана", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Метод выдачи книги, выбранной в списке
        /// </summary>
        /// <param name="currentBook"></param>
        /// <param name="currentReader"></param>
        internal static void CreateCheckoutFromLB(DB.Book currentBook, DB.Reader currentReader)
        {
            var context = DB.LibraryEntities.GetContext();

            var newCheckout = new DB.Checkout
            {
                IdBook = currentBook.IdBook,
                IdReader = currentReader.IdReader,
                IdLibrarianCheckout = TUser.userId,
                DateCheckout = DateTime.Now
            };
            var book = context.Book.FirstOrDefault(b => b.IdBook == currentBook.IdBook);
            int booksCheckedOut = context.Checkout.Count(c => c.IdReader == currentReader.IdReader && c.DateReturn == null);

            if (booksCheckedOut >= 10)
            {
                MessageBox.Show("Нельзя выдать больше 10 книг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var issuedStatus = context.Status.FirstOrDefault(s => s.Name == "Выдана");
                
                context.Checkout.Add(newCheckout);


                book.IdStatus = issuedStatus.IdStatus;
                context.SaveChanges();
            
            

            MessageBox.Show("Книга успешно выдана", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Метод вывода уведомления о сроках сдачи
        /// </summary>
        internal static void DisplayCheckoutNotification()
        {
            var context = DB.LibraryEntities.GetContext();
            DateTime currentDate = DateTime.Now;
            DateTime alertDate = currentDate.AddDays(3); // Дата через 3 дня
            // Получение несданных книг для конкретного читателя
            var overdueCheckouts = context.Checkout
                .Where(c => c.IdReader == TUser.userId && c.DateReturn == null)
                .ToList();
            // Есть ли книги, которые нужно сдать в ближайшие 3 дня
            bool hasOverdueBooks = false;
            string message = "Внимание, вы должны сдать следующие книги:\n";
            foreach (var checkout in overdueCheckouts)
            {
                // Вычисление срока сдачи книги и количество оставшихся дней
                DateTime dueDate = checkout.DateCheckout.AddDays(14);
                int daysLeft = (dueDate - currentDate).Days;
                // Проверка на то, что осталось 3 или меньше дней до срока сдачи
                if (dueDate <= alertDate)
                {
                    hasOverdueBooks = true;
                    var bookName = context.Book
                        .Where(b => b.IdBook == checkout.IdBook)
                        .Select(b => b.Name)
                        .FirstOrDefault();
                    if (daysLeft >= 0)
                    {
                        message += $"Книга: {bookName}, " +
                            $"Срок сдачи: {dueDate.ToShortDateString()} " +
                            $"(осталось {daysLeft} дней)\n";
                    }
                    else
                    {
                        int daysOverdue = Math.Abs(daysLeft);
                        message += $"Книга: {bookName}, " +
                            $"Просрочена ({daysOverdue} " +
                            $"{(daysOverdue == 1 ? "день" : "дней")})\n";
                    }
                }
            }
            if (hasOverdueBooks)
            {
                MessageBox.Show(message, "Срок сдачи книг", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
