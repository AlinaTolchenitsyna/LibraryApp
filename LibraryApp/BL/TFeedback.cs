using LibraryApp.DB;
using LibraryApp.UI.Pgs.ReaderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TFeedback
    {
        /// <summary>
        /// Метод удаления отзыва читателем
        /// </summary>
        /// <param name="feedbackForReceiving"></param>
        /// <param name="LbxFeedbacks"></param>
        /// <param name="currentCheckout"></param>
        internal static void DeleteMyFeedback(DB.Feedback feedbackForReceiving, ListBox LbxFeedbacks, DB.Checkout currentCheckout)
        {
            if (MessageBox.Show("Вы действительно хотите удалить отзыв?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    DB.LibraryEntities.GetContext().Feedback.Remove(feedbackForReceiving);
                    DB.LibraryEntities.GetContext().SaveChanges();
                    MessageBox.Show("Отзыв успешно удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    LbxFeedbacks.ItemsSource = DB.LibraryEntities.GetContext().Feedback.Where(f => f.IdBook == currentCheckout.IdBook && f.IdReader == TUser.userId).ToList();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Метод удаления отзыва библиотекарем
        /// </summary>
        /// <param name="feedbackForReceiving"></param>
        /// <param name="LbxFeedbacks"></param>
        internal static void DeleteFeedback(DB.Feedback feedbackForReceiving, ListBox LbxFeedbacks)
        {
            

            if (MessageBox.Show("Вы действительно хотите удалить этот отзыв?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    DB.LibraryEntities.GetContext().Feedback.Remove(feedbackForReceiving);
                    DB.LibraryEntities.GetContext().SaveChanges();
                    MessageBox.Show("Отзыв успешно удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    LbxFeedbacks.ItemsSource = DB.LibraryEntities.GetContext().Feedback.Where(b => b.IdBook == TBook.bookId).ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Метод проверки отзыва на корректность введенных значений
        /// </summary>
        /// <param name="currentFeedback"></param>
        /// <returns></returns>
        internal static bool IsFeedbackCorrect(DB.Feedback currentFeedback)
        {
            StringBuilder errors = new StringBuilder();

            if (currentFeedback.Rating < 1 || currentFeedback.Rating > 5)
            {
                errors.AppendLine("Оценка - число от 1 до 5");
            }
            if (currentFeedback.FeedbackText.Length < 3 || currentFeedback.FeedbackText.Length > 200)
            {
                errors.AppendLine("Длина отзыва должна быть от 3 до 200 символов");
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
        /// Метод сохранения отзыва
        /// </summary>
        /// <param name="currentFeedback"></param>
        internal static void SaveFeedback(DB.Feedback currentFeedback)
        {
            if (currentFeedback.IdFeedback == 0)
            {
                DB.LibraryEntities.GetContext().Feedback.Add(currentFeedback);
            }
            if (currentFeedback.IdReader == 0)
            {
                currentFeedback.IdReader = TUser.userId;
            }

            currentFeedback.FeedbackDate = DateTime.Now;

            if (currentFeedback.IdBook == 0)
            {
                currentFeedback.IdBook = TBook.bookId;
            }
            try
            {
                var currentCheckoutQuery = DB.LibraryEntities.GetContext().Checkout.Where(c => c.IdCheckout == TCheckout.checkoutId);
                var currentCheckout = currentCheckoutQuery.FirstOrDefault();
                DB.LibraryEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                TFrame.mainFrame.Navigate(new FeedbackReaderPage(currentCheckout));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
