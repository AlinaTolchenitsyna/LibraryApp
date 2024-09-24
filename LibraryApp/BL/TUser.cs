using LibraryApp.UI.Wnds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TUser
    {
        public static string userType { get; set; }
        public static int userId { get; set; }

        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="TbxLogin"></param>
        /// <param name="PswBox"></param>
        /// <param name="window"></param>
        internal static void Authorize(TextBox TbxLogin, PasswordBox PswBox, Window window)
        {
            bool isAuth = false;
            try
            {
                foreach (DB.Librarian librarian in
                    DB.LibraryEntities.GetContext().Librarian.Where
                    (c => c.Login.Equals(TbxLogin.Text)
                    && c.Password.Equals(PswBox.Password)))
                {
                    isAuth = true;
                    TUser.userType = "Библиотекарь";
                    TUser.userId = librarian.IdLibrarian;
                }
                foreach (DB.Reader reader in
                    DB.LibraryEntities.GetContext().Reader.Where
                    (c => c.Login.Equals(TbxLogin.Text)
                    && c.Password.Equals(PswBox.Password)))
                {
                    isAuth = true;
                    TUser.userType = "Читатель";
                    TUser.userId = reader.IdReader;
                }
                if (isAuth == true)
                {
                    MessageBox.Show("Успешная авторизация", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (TUser.userType == "Библиотекарь")
                    {
                        LibrarianMainWindow librarianMainWindow =
                            new LibrarianMainWindow();
                        librarianMainWindow.Show();
                    }
                    else if (TUser.userType == "Читатель")
                    {
                        ReaderMainWindow readerMainWindow = new ReaderMainWindow();
                        readerMainWindow.Show();
                    }
                    window.Close();

                }
                else
                {

                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться по техническим причинам. Обратитесь к системному администратору.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
