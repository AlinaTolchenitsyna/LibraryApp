using LibraryApp.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Data.Entity;
using LibraryApp.BL;

namespace LibraryApp.UI.Pgs.LibrarianModule
{
    /// <summary>
    /// Логика взаимодействия для AddEditEventPage.xaml
    /// </summary>
    public partial class AddEditEventPage : Page
    {
        private DB.Event originalEvent;
        private DB.Event currentEvent = new DB.Event();
        private byte[] originalImageData;
        private byte[] currentImageData;

        public AddEditEventPage(DB.Event selectedEvent)
        {
            InitializeComponent();

            if (selectedEvent != null)
            {
                originalEvent = selectedEvent.Clone();
                currentEvent = selectedEvent;
                originalImageData = selectedEvent.Photo;
                
            }
            else
            {
                currentEvent = new DB.Event();
                currentEvent.EventDateTime = DateTime.Now;
            }

            BL.TEvent.CloneEvent(selectedEvent, originalEvent, ref currentEvent, originalImageData, EventImage);

            DataContext = currentEvent;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BL.TEvent.CancelEventChanges(e, currentEvent, originalEvent, currentImageData, originalImageData, EventImage);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите сохранить мероприятие?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (BL.TEvent.IsEventCorrect(currentEvent))
                {

                    currentEvent.IdLibrarian = TUser.userId;
                    BL.TEvent.SaveEvent(currentImageData, currentEvent);
                    TFrame.mainFrame.Navigate(new EventLibrarianPage());
                }
            }
        }

        

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            BL.TImage.SelectImage(ref currentImageData, EventImage);
        }

        private void BtnDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            var currentEvent = DataContext as DB.Event;
            if (currentEvent.Photo != null)
            {
                BL.TEvent.DeleteEventImage(currentEvent, EventImage);
            }
            else
            {
                MessageBox.Show("У мероприятия отсутствует изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }



            private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }
    }
}
