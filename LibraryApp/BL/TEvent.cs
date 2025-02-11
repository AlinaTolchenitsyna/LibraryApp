using LibraryApp.UI.Pgs.LibrarianModule;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TEvent
    {
        /// <summary>
        /// Метод клонирования мероприятия для изменения его копии
        /// </summary>
        /// <param name="selectedEvent"></param>
        /// <param name="originalEvent"></param>
        /// <param name="currentEvent"></param>
        /// <param name="originalImageData"></param>
        /// <param name="EventImage"></param>
        internal static void CloneEvent(DB.Event selectedEvent, DB.Event originalEvent, ref DB.Event currentEvent, byte[] originalImageData, System.Windows.Controls.Image EventImage)
        {
            if (selectedEvent != null)
            {
                originalEvent = selectedEvent.Clone();
                currentEvent = selectedEvent;
                originalImageData = selectedEvent.Photo;
                EventImage.Source = TImage.LoadBitmapImage(selectedEvent.Photo);
            }
        }

        /// <summary>
        /// Проверка мероприятия на корректность заполненных значений
        /// </summary>
        /// <param name="currentEvent"></param>
        /// <param name="CbPublishingHouse"></param>
        /// <param name="CbGenre"></param>
        /// <param name="CbStatus"></param>
        /// <returns></returns>
        internal static bool IsEventCorrect(DB.Event currentEvent)
        {
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(currentEvent.Name) || currentEvent.Name.Length > 100 || currentEvent.Name.Trim().Length == 0)
            {
                errors.AppendLine("Название мероприятия должно содержать от 1 до 100 символов");
            }
            if (currentEvent.EventDateTime == null)
            {
                errors.AppendLine("Укажите дату и время мероприятия");
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
        /// Метод сохранения мероприятия
        /// </summary>
        /// <param name="currentImageData"></param>
        /// <param name="currentBook"></param>
        internal static void SaveEvent(byte[] currentImageData, DB.Event currentEvent)
        {
            var context = DB.LibraryEntities.GetContext();

            if (currentEvent.IdEvent == 0)
            {
                context.Event.Add(currentEvent);
                context.SaveChanges();  
            }

            try
            {
                context.SaveChanges();

                if (currentImageData != null)
                {
                    using (SqlConnection conn = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                    {
                        string query = "UPDATE Event SET Photo = @Photo WHERE IdEvent = @EventId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = currentImageData ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@EventId", SqlDbType.Int).Value = currentEvent.IdEvent;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                else if (currentEvent.Photo == null)  
                {
                    using (SqlConnection conn = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                    {
                        string query = "UPDATE Event SET Photo = NULL WHERE IdEvent = @EventId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@EventId", SqlDbType.Int).Value = currentEvent.IdEvent;

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
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Метод отмены изменений
        /// </summary>
        /// <param name="e"></param>
        /// <param name="currentEvent"></param>
        /// <param name="originalEvent"></param>
        /// <param name="currentImageData"></param>
        /// <param name="originalImageData"></param>
        /// <param name="EventImage"></param>
        internal static void CancelEventChanges(DependencyPropertyChangedEventArgs e, DB.Event currentEvent, DB.Event originalEvent, byte[] currentImageData, byte[] originalImageData, Image EventImage)
        {
            // Если страница становится невидимой
            if (!(bool)e.NewValue)
            {
                // Отменяются изменения, восстанавливаются изначальные значения
                currentEvent.CopyFrom(originalEvent);
                currentImageData = originalImageData;
                EventImage.Source = BL.TImage.LoadBitmapImage(originalImageData);
            }
        }

        /// <summary>
        /// Метод удаления изображения мероприятия
        /// </summary>
        /// <param name="currentEvent"></param>
        /// <param name="EventImage"></param>
        internal static void DeleteEventImage(DB.Event currentEvent, Image EventImage)
        {
            
            if (currentEvent != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить изображение?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    currentEvent.Photo = null;
                    EventImage.Source = null;
                }
            }
        }

        
        /// <summary>
        /// Метод обновления страницы мероприятий
        /// </summary>
        /// <param name="page"></param>
        /// <param name="LbxEvents"></param>
        /// <param name="LblNoResults"></param>
        internal static void UpdateEventInterface(Page page, ListBox LbxEvents, Label LblNoResults)
        {
            if (page.Visibility == Visibility.Visible)
            {
                DB.LibraryEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                var currentEvents = DB.LibraryEntities.GetContext().Event.ToList();
                LbxEvents.ItemsSource = currentEvents;
                int contentCount = currentEvents.Count();
                TLabel.UpdateLabel(contentCount, LblNoResults);
            }
        }

        /// <summary>
        /// Метод удаления мероприятия
        /// </summary>
        /// <param name="LbxEvents"></param>
        internal static void DeleteEvent(ListBox LbxEvents)
        {
            var eventForReceiving = LbxEvents.SelectedItems.Cast<DB.Event>().ToList();
            if (eventForReceiving.Count() > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить выбранное мероприятие?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DB.LibraryEntities.GetContext().Event.RemoveRange(eventForReceiving);
                        DB.LibraryEntities.GetContext().SaveChanges();
                        MessageBox.Show("Мероприятие успешно удалено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        LbxEvents.ItemsSource = DB.LibraryEntities.GetContext().Event.ToList();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Проверка, зарегистрирован ли читатель на мероприятие
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="readerId"></param>
        /// <returns></returns>
        internal static bool IsRegistered(int eventId, int readerId)
        {
            string query = "SELECT COUNT(*) FROM EventRegistration WHERE IdEvent = @EventId AND IdReader = @ReaderId";
            int count = DB.LibraryEntities.GetContext().Database.SqlQuery<int>(query,
                new SqlParameter("EventId", eventId),
                new SqlParameter("ReaderId", readerId))
                .FirstOrDefault();
            return count > 0;
        }

        /// <summary>
        /// Метод регистрации читателя на мероприятие
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="readerId"></param>
        /// <returns></returns>
        internal static bool Register(int eventId, int readerId)
        {
            string query = "INSERT INTO EventRegistration (IdEvent, IdReader) VALUES (@EventId, @ReaderId)";
            int rowsAffected = DB.LibraryEntities.GetContext().Database.ExecuteSqlCommand(query,
                new SqlParameter("EventId", eventId),
                new SqlParameter("ReaderId", readerId));
            return rowsAffected > 0;
        }
    }
}
