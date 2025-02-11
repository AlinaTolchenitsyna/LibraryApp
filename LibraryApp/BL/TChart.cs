using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts.Wpf.Charts.Base;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp.BL
{
    internal class TChart
    {
        /// <summary>
        /// Метод формирования диаграммы «Самые популярные книги»
        /// </summary>
        /// <param name="PopularBooksChart"></param>
        internal static void LoadPopularBooksChart(Chart PopularBooksChart)
        {
            string query = @"
                SELECT 
                    b.Name, 
                    COUNT(c.IdBook) AS CheckoutCount
                FROM 
                    Book b
                JOIN 
                    Checkout c ON b.IdBook = c.IdBook
                GROUP BY 
                    b.Name
                ORDER BY 
                    CheckoutCount DESC;";
            using (SqlConnection connection = new SqlConnection
                ("data source=DESKTOP-91F36SH;initial catalog=Library;" +
                "integrated security=True;MultipleActiveResultSets=True;" +
                "App=EntityFramework"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter
                    (query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                List<string> bookNames = new List<string>();
                ChartValues<int> values = new ChartValues<int>();

                foreach (DataRow row in dataTable.Rows)
                {
                    bookNames.Add(row["Name"].ToString());
                    values.Add(Convert.ToInt32(row["CheckoutCount"]));
                }

                PopularBooksChart.AxisX[0].Labels = bookNames;
                PopularBooksChart.AxisX[0].Title = string.Empty; 
                PopularBooksChart.AxisY[0].Title = string.Empty; 
                PopularBooksChart.LegendLocation = LegendLocation.None;

                PopularBooksChart.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Количество прочтений",
                        Values = values,
                        Fill = new SolidColorBrush(Color.FromRgb(26, 166, 183)) 
                    }
                };
            }
        }

        /// <summary>
        /// Метод формирования диаграммы «Самые активные читатели»
        /// </summary>
        /// <param name="ActiveReadersChart"></param>
        internal static void LoadActiveReadersChart(Chart ActiveReadersChart)
        {
            string query = @"
        SELECT 
            r.FirstName + ' ' + r.LastName AS ReaderName, 
            COUNT(c.IdReader) AS BooksTaken
        FROM 
            Reader r
        JOIN 
            Checkout c ON r.IdReader = c.IdReader
        GROUP BY 
            r.FirstName, r.LastName
        ORDER BY 
            BooksTaken DESC;";

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Создание коллекций для хранения имен читателей и количества взятых ими книг
                List<string> readerNames = new List<string>();
                ChartValues<int> booksTaken = new ChartValues<int>();

                // Заполнение коллекций данными из DataTable
                foreach (DataRow row in dataTable.Rows)
                {
                    readerNames.Add(row["ReaderName"].ToString());
                    booksTaken.Add(Convert.ToInt32(row["BooksTaken"]));
                }

                // Создание столбчатой диаграммы
                ActiveReadersChart.Series = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Количество прочтений",
                Values = booksTaken,
                Fill = new SolidColorBrush(Color.FromRgb(26, 166, 183)) // Устанавливаем цвет столбцов
            }
        };

                // Установка меток для оси X
                ActiveReadersChart.AxisX.First().Labels = readerNames;
                ActiveReadersChart.AxisX[0].Title = string.Empty; // Устанавливаем пустое значение для заголовка оси X
                ActiveReadersChart.AxisY[0].Title = string.Empty; // Устанавливаем пустое значение для заголовка оси Y
            }
        }

        /// <summary>
        /// Метод формирования диаграммы «Посещение мероприятий»
        /// </summary>
        /// <param name="EventAttendanceChart"></param>
        internal static void LoadEventAttendanceChart(Chart EventAttendanceChart)
        {
            string query = @"
        SELECT 
            e.Name AS Name, 
            COUNT(r.IdReader) AS AttendanceCount
        FROM 
            Event e
        JOIN 
            EventRegistration r ON e.IdEvent = r.IdEvent
        GROUP BY 
            e.Name
        ORDER BY 
            AttendanceCount DESC;";

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Создание коллекций для хранения названий мероприятий и количества записей на них
                List<string> eventNames = new List<string>();
                ChartValues<int> attendanceCounts = new ChartValues<int>();

                // Заполнение коллекций данными из DataTable
                foreach (DataRow row in dataTable.Rows)
                {
                    eventNames.Add(row["Name"].ToString());
                    attendanceCounts.Add(Convert.ToInt32(row["AttendanceCount"]));
                }

                // Создание столбчатой диаграммы
                EventAttendanceChart.Series = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Количество посещений",
                Values = attendanceCounts,
                Fill = new SolidColorBrush(Color.FromRgb(26, 166, 183)) // Устанавливаем цвет столбцов
            }
        };

                // Установка меток для оси X
                EventAttendanceChart.AxisX.First().Labels = eventNames;
                EventAttendanceChart.AxisX[0].Title = string.Empty; // Устанавливаем пустое значение для заголовка оси X
                EventAttendanceChart.AxisY[0].Title = string.Empty; // Устанавливаем пустое значение для заголовка оси Y
            }
        }

        /// <summary>
        /// Метод экспорта в Excel данных из диаграммы «Самые популярные книги»
        /// </summary>
        internal static void ExportToExcelPopularBooks()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Сохранить как файл Excel"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string query = @"
                    SELECT 
                        b.Name, 
                        COUNT(c.IdBook) AS CheckoutCount
                    FROM 
                        Book b
                    JOIN 
                        Checkout c ON b.IdBook = c.IdBook
                    GROUP BY 
                        b.Name
                    ORDER BY 
                        CheckoutCount DESC;";
                using (SqlConnection connection = new SqlConnection
                    ("data source=DESKTOP-91F36SH;initial catalog=Library;" +
                    "integrated security=True;MultipleActiveResultSets=True;" +
                    "App=EntityFramework"))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter
                        (query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets
                            .Add("Популярные книги");
                        worksheet.Cell(1, 1).Value = "Название";
                        worksheet.Cell(1, 2).Value = "Количество прочтений";
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = 
                                dataTable.Rows[i]["Name"].ToString();
                            worksheet.Cell(i + 2, 2).Value = Convert.ToInt32
                                (dataTable.Rows[i]["CheckoutCount"]);
                        }
                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                }
                MessageBox.Show($"Отчет успешно сохранен в " +
                    $"{saveFileDialog.FileName}", "Успешно", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Метод экспорта в Excel данных из диаграммы «Самые активные читатели»
        /// </summary>
        internal static void ExportToExcelActiveReaders()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Сохранить как файл Excel"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string query = @"
            SELECT 
                r.FirstName + ' ' + r.LastName AS ReaderName, 
                COUNT(c.IdReader) AS BooksTaken
            FROM 
                Reader r
            JOIN 
                Checkout c ON r.IdReader = c.IdReader
            GROUP BY 
                r.FirstName, r.LastName
            ORDER BY 
                BooksTaken DESC;";

                using (SqlConnection connection = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Самые активные читатели");
                        worksheet.Cell(1, 1).Value = "Имя читателя";
                        worksheet.Cell(1, 2).Value = "Количество прочтений";

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = dataTable.Rows[i]["ReaderName"].ToString();
                            worksheet.Cell(i + 2, 2).Value = Convert.ToInt32(dataTable.Rows[i]["BooksTaken"]);
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                }

                MessageBox.Show($"Отчет успешно сохранен в {saveFileDialog.FileName}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Метод экспорта в Excel данных из диаграммы «Посещение мероприятий»
        /// </summary>
        internal static void ExportToExcelEventAttendance()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Сохранить как файл Excel"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string query = @"
            SELECT 
                e.Name AS EventName, 
                COUNT(r.IdReader) AS AttendanceCount
            FROM 
                Event e
            JOIN 
                EventRegistration r ON e.IdEvent = r.IdEvent
            GROUP BY 
                e.Name
            ORDER BY 
                AttendanceCount DESC;";

                using (SqlConnection connection = new SqlConnection("data source=DESKTOP-91F36SH;initial catalog=Library;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Посещения");
                        worksheet.Cell(1, 1).Value = "Название мероприятия";
                        worksheet.Cell(1, 2).Value = "Количество посещений";

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = dataTable.Rows[i]["EventName"].ToString();
                            worksheet.Cell(i + 2, 2).Value = Convert.ToInt32(dataTable.Rows[i]["AttendanceCount"]);
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                }

                MessageBox.Show($"Отчет успешно сохранен в {saveFileDialog.FileName}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Метод отображения диаграмм
        /// </summary>
        /// <param name="CbChartType"></param>
        /// <param name="PopularBooksChart"></param>
        /// <param name="ActiveReadersChart"></param>
        /// <param name="EventAttendanceChart"></param>
        internal static void DisplayChart(ComboBox CbChartType, Chart PopularBooksChart, Chart ActiveReadersChart, Chart EventAttendanceChart)
        {
            if (CbChartType.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedTag = selectedItem.Tag.ToString();
                if (PopularBooksChart != null)
                {
                    PopularBooksChart.Visibility = selectedTag == "PopularBooksChart" ? Visibility.Visible : Visibility.Collapsed;
                }
                if (ActiveReadersChart != null)
                {
                    ActiveReadersChart.Visibility = selectedTag == "ActiveReadersChart" ? Visibility.Visible : Visibility.Collapsed;
                }
                if (EventAttendanceChart != null)
                {
                    EventAttendanceChart.Visibility = selectedTag == "EventAttendanceChart" ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}
