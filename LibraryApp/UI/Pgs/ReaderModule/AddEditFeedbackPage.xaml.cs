
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для AddEditFeedbackPage.xaml
    /// </summary>
    public partial class AddEditFeedbackPage : Page
    {
        DB.Feedback currentFeedback = new DB.Feedback();
        public AddEditFeedbackPage(DB.Feedback selectedFeedback)
        {
            InitializeComponent();
            if (selectedFeedback != null)
            {
                currentFeedback = selectedFeedback;
            }
            DataContext = currentFeedback;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            TFrame.mainFrame.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (BL.TFeedback.IsFeedbackCorrect(currentFeedback))
            {
                BL.TFeedback.SaveFeedback(currentFeedback);

            }
        }

        
    }
}
