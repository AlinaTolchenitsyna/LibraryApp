using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using LibraryApp.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
using LibraryApp.BL;

namespace LibraryApp.UI.Pgs.ReaderModule
{
    /// <summary>
    /// Логика взаимодействия для EventReaderPage.xaml
    /// </summary>
    public partial class EventReaderPage : Page
    {
        public EventReaderPage()
        {
            InitializeComponent();
            var currentEvents = DB.LibraryEntities.GetContext().Event.OrderByDescending(e => e.EventDateTime).ToList();
            LbxEvents.ItemsSource = currentEvents;
            if (currentEvents.Count() == 0)
            {
                LblNoResults.Visibility = Visibility.Visible;
            }
        }

        

        
        

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            DB.Event selectedEvent = (sender as Button).DataContext as DB.Event;
            int readerId = TUser.userId;

            if (selectedEvent != null && readerId != -1)
            {
                if (BL.TEvent.IsRegistered(selectedEvent.IdEvent, readerId))
                {
                    MessageBox.Show("Вы уже записаны на это мероприятие.");
                }
                else if (selectedEvent.EventDateTime <= DateTime.Now)
                {
                    MessageBox.Show("Мероприятие уже прошло.");
                }
                else
                {
                    BL.TEvent.Register(selectedEvent.IdEvent, readerId);
                    MessageBox.Show("Вы успешно записаны на мероприятие!");
                }
            }
            else
            {
                MessageBox.Show("Ошибка при записи на мероприятие.");
            }

        }
    }
}
