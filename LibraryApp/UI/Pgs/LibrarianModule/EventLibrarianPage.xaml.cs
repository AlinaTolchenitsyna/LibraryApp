using LibraryApp.BL;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EventLibrarianPage.xaml
    /// </summary>
    public partial class EventLibrarianPage : Page
    {
        public EventLibrarianPage()
        {
            InitializeComponent();
            var currentEvents = DB.LibraryEntities.GetContext().Event.OrderByDescending(e => e.EventDateTime).ToList();
            LbxEvents.ItemsSource = currentEvents;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BL.TEvent.UpdateEventInterface(this, LbxEvents, LblNoResults);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditEventPage((sender as Button)
                .DataContext as DB.Event));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.Navigate(new AddEditEventPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            BL.TEvent.DeleteEvent(LbxEvents);
        }

    }
}
