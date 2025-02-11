using LiveCharts.Wpf;
using LiveCharts;
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
using ClosedXML.Excel;
using Microsoft.Win32;

namespace LibraryApp.UI.Pgs.LibrarianModule
{
    /// <summary>
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            BL.TChart.LoadPopularBooksChart(PopularBooksChart);
            BL.TChart.LoadActiveReadersChart(ActiveReadersChart);
            BL.TChart.LoadEventAttendanceChart(EventAttendanceChart);
        }

        

        

        

        

        private void BtnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (PopularBooksChart.Visibility == Visibility.Visible)
            {
                BL.TChart.ExportToExcelPopularBooks();
            }
            else if (ActiveReadersChart.Visibility == Visibility.Visible)
            {
                BL.TChart.ExportToExcelActiveReaders();
            }
            else if (EventAttendanceChart.Visibility == Visibility.Visible)
            {
                BL.TChart.ExportToExcelEventAttendance();
            }
        }

        private void CbChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BL.TChart.DisplayChart(CbChartType, PopularBooksChart, ActiveReadersChart, EventAttendanceChart);
        }
    }
}
