using DocumentFormat.OpenXml.Wordprocessing;
using LibraryApp.BL;
using LibraryApp.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace LibraryApp.UI.Pgs.LibrarianModule
{
    /// <summary>
    /// Логика взаимодействия для AddEditReaderPage.xaml
    /// </summary>

    public partial class AddEditReaderPage : Page
    {
        private DB.Reader originalReader;
        private DB.Reader currentReader = new DB.Reader();

        public AddEditReaderPage(DB.Reader selectedReader)
        {
            InitializeComponent();

            if (selectedReader != null)
            {
                originalReader = selectedReader.Clone();
                currentReader = selectedReader;
            }
            else
            {
                currentReader = new DB.Reader();
            }

            BL.TReader.CloneReader(selectedReader, originalReader, ref currentReader);

            DataContext = currentReader;

            if (selectedReader == null)
            {
                currentReader.RegistrationDate = DateTime.Now;
                currentReader.Birthday = DateTime.Now.AddYears(-10);

                // Обновляем привязки
                BindingOperations.GetBindingExpression(DpRegDate, TextBox.TextProperty)?.UpdateTarget();
                BindingOperations.GetBindingExpression(DpBirthday, TextBox.TextProperty)?.UpdateTarget();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Если страница становится невидимой
            if (!(bool)e.NewValue)
            {
                // Отменяются изменения, восстанавливаются изначальные значения
                currentReader.CopyFrom(originalReader);
                DataContext = null;
                DataContext = currentReader;
                
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите сохранить читателя?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (BL.TReader.IsReaderCorrect(currentReader))
                {
                    BL.TReader.AddReader(currentReader);

                    try
                    {
                        DB.LibraryEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        originalReader = currentReader.Clone();
                        DataContext = null;
                        DataContext = currentReader;
                        TFrame.mainFrame.Navigate(new ReadersPage());

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }
    }
}

