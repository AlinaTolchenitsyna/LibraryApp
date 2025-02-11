using LibraryApp.DB;
using LibraryApp.UI.Pgs.LibrarianModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;

namespace LibraryApp.BL
{
    public class TReader
    {

        /// <summary>
        /// Метод клонирования читателя
        /// </summary>
        /// <param name="selectedReader"></param>
        /// <param name="originalReader"></param>
        /// <param name="currentReader"></param>
        internal static void CloneReader(DB.Reader selectedReader, DB.Reader originalReader, ref DB.Reader currentReader)
        {
            if (selectedReader != null)
            {
                originalReader = selectedReader.Clone();
                currentReader = selectedReader;
            }
        }

        /// <summary>
        /// Проверка читателя на корректность заполненных значений
        /// </summary>
        /// <param name="currentReader"></param>
        /// <returns></returns>
        public static bool IsReaderCorrect(DB.Reader currentReader)
        {
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(currentReader.LastName) || currentReader.LastName.Trim().Length == 0 || currentReader.LastName.Trim().Length > 50)
            {
                errors.AppendLine("Фамилия должна содержать от 1 до 50 символов");
            }
            if (String.IsNullOrWhiteSpace(currentReader.FirstName) || currentReader.FirstName.Trim().Length == 0 || currentReader.FirstName.Trim().Length > 50)
            {
                errors.AppendLine("Имя должно содержать от 1 до 50 символов");
            }
            if (String.IsNullOrWhiteSpace(currentReader.Patronymic) || currentReader.Patronymic.Trim().Length == 0 || currentReader.Patronymic.Trim().Length > 50)
            {
                errors.AppendLine("Отчество должно содержать от 1 до 50 символов");
            }
            if (currentReader.Birthday == null || DateTime.Now.Year - currentReader.Birthday.Year < 7 || DateTime.Now.Year - currentReader.Birthday.Year > 18)
            {
                errors.AppendLine("Укажите дату рождения (возраст должен быть от 7 до 18 лет)");
            }
            
                if (String.IsNullOrWhiteSpace(currentReader.Phone) || currentReader.Phone.Trim().Length != 11)
                {
                    errors.AppendLine("Укажите корректный номер телефона без дефисов, скобок и плюса");
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
        /// Метод добавления читателя
        /// </summary>
        /// <param name="currentReader"></param>
        internal static void AddReader(DB.Reader currentReader)
        {
            if (currentReader.IdReader == 0)
            {
                DB.LibraryEntities.GetContext().Reader.Add(currentReader);
                currentReader.Login = GenerateRandomString(10);
                currentReader.Password = GenerateRandomString(15);
                MessageBox.Show($"Сообщите читателю его логин и пароль: {currentReader.Login}, {currentReader.Password}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    result.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }
            return result.ToString();
        }



        /// <summary>
        /// Метод обновления списка читателей
        /// </summary>
        /// <param name="TbxSearchName"></param>
        /// <param name="LbxReaders"></param>
        /// <param name="LblNoResults"></param>
        internal static void UpdateReaders(TextBox TbxSearchName, ListBox LbxReaders, Label LblNoResults)
        {
            var currentReaders = DB.LibraryEntities.GetContext().Reader.ToList();

            if (!string.IsNullOrWhiteSpace(TbxSearchName.Text))
            {
                currentReaders = currentReaders.Where(r => r.FirstName.ToLower().Contains(TbxSearchName.Text.ToLower()) || r.LastName.ToLower().Contains(TbxSearchName.Text.ToLower()) || r.Patronymic.Contains(TbxSearchName.Text.ToLower())).ToList();
            }

            LbxReaders.ItemsSource = currentReaders;
            int contentCount = currentReaders.Count;
            BL.TLabel.UpdateLabel(contentCount, LblNoResults);
        }

        /// <summary>
        /// Метод удаления читателя
        /// </summary>
        /// <param name="LbxReaders"></param>
        /// <param name="TbxSearchName"></param>
        /// <param name="LblNoResults"></param>
        internal static void DeleteReader(ListBox LbxReaders, TextBox TbxSearchName, Label LblNoResults)
        {
            var readerForReceiving = LbxReaders.SelectedItems.Cast<DB.Reader>().ToList();
            var readerIds = readerForReceiving.Select(reader => reader.IdReader).ToList();
            int readerId = 0;
            foreach (var id in readerIds)
            {
                readerId = id;
            }
            var foundReader = DB.LibraryEntities.GetContext().Reader.FirstOrDefault(reader => reader.IdReader == readerId);
            if (readerForReceiving.Count() > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить выбранного читателя?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var hasBooksOnHand = DB.LibraryEntities.GetContext().Checkout.Any(c => c.IdReader == foundReader.IdReader);

                        if (hasBooksOnHand)
                        {

                            MessageBox.Show("Нельзя удалить читателя, так как у него есть выданные книги", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DB.LibraryEntities.GetContext().Reader.RemoveRange(readerForReceiving);
                        DB.LibraryEntities.GetContext().SaveChanges();
                        MessageBox.Show("Читатель успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateReaders(TbxSearchName, LbxReaders, LblNoResults);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

    }
}
