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
using System.Windows.Shapes;
using FlowerShopApp.Data;
using System;
using FlowerShopApp.Models;

namespace FlowerShopApp.Forms
{
    /// <summary>
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            FromDatePicker.SelectedDate = DateTime.Today.AddDays(-30);
            ToDatePicker.SelectedDate = DateTime.Today;
        }

        //private void GenerateReport_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (FromDatePicker.SelectedDate == null || ToDatePicker.SelectedDate == null)
        //    //{
        //    //    MessageBox.Show("Будь ласка, оберіть обидві дати.");
        //    //    return;
        //    //}

        //    //var from = FromDatePicker.SelectedDate.Value;
        //    //var to = ToDatePicker.SelectedDate.Value;

        //    //var report = ReportRepository.GetSalesReport(from, to);
        //    //ReportDataGrid.ItemsSource = report;

        //    try
        //    {
        //        DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
        //        DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

        //        //    var orders = OrderRepository.GetOrdersInRange(from, to); // наш метод
        //        //    var details = OrderDetailRepository.GetAll();

        //        //    var report = (from o in orders
        //        //                  join d in details on o.OrderId equals d.OrderId
        //        //                  join p in ProductRepository.GetAllProducts() on d.ProductId equals p.ProductId
        //        //                  join c in CustomerRepository.GetAll() on o.CustomerId equals c.CustomerId
        //        //                  select new SalesReportItem
        //        //                  {
        //        //                      CustomerName = c.FullName,
        //        //                      ProductName = p.Name,
        //        //                      Quantity = d.Quantity,
        //        //                      PurchasePrice = d.PurchasePrice,
        //        //                      SalePrice = d.PriceAtOrderTime
        //        //                  }).ToList();

        //        //    ReportDataGrid.ItemsSource = report;
        //        //    TotalProfitText.Text = report.Sum(r => r.Profit).ToString("F2") + " грн";
        //        //}


        //        var orders = OrderRepository.GetOrdersInRange(from, to);
        //        var orderDetails = OrderDetailRepository.GetAll();
        //        var products = ProductRepository.GetAllProducts();
        //        var customers = CustomerRepository.GetAll();

        //        var reportItems = (from o in orders
        //                           join d in orderDetails on o.OrderId equals d.OrderId
        //                           join p in products on d.ProductId equals p.ProductId
        //                           join c in customers on o.CustomerId equals c.CustomerId
        //                           select new SalesReportItem
        //                           {
        //                               CustomerName = c.FullName,
        //                               ProductName = p.Name,
        //                               Quantity = d.Quantity,
        //                               SalePrice = d.PriceAtOrderTime,
        //                               PurchasePrice = d.PurchasePrice
        //                           }).ToList();

        //        ReportDataGrid.ItemsSource = reportItems;
        //        TotalProfitText.Text = reportItems.Sum(i => i.Profit).ToString("F2") + " грн";
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Помилка генерації звіту:\n" + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
                DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

                var report = ReportRepository.GetSalesReport(from, to);

                ReportDataGrid.Columns.Clear();

                ReportDataGrid.ItemsSource = report;

                TotalProfitText.Text = report.Sum(i => i.Profit).ToString("F2") + " грн";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка генерації звіту:\n" + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport_Click(sender, e);
        }

        //private void btnPopular_Click(object sender, RoutedEventArgs e)
        //{
        //    DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
        //    DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

        //    var items = ReportRepository.GetPopularProducts(from, to);
        //    ReportDataGrid.ItemsSource = items;
        //    TotalProfitText.Text = "";
        //}

        private void btnPopular_Click(object sender, RoutedEventArgs e)
        {
            DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

            var items = ReportRepository.GetPopularProducts(from, to);
            ReportDataGrid.Columns.Clear();

            ReportDataGrid.ItemsSource = items;

            ReportDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Продукт",
                Binding = new Binding("ProductName")
            });

            ReportDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Кількість продажів",
                Binding = new Binding("TotalQuantitySold")
            });

            ReportDataGrid.ItemsSource = items;
            TotalProfitText.Text = "";
        }


        //private void btnCustomers_Click(object sender, RoutedEventArgs e)
        //{
        //    DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
        //    DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

        //    var items = ReportRepository.GetCustomerStats(from, to);
        //    ReportDataGrid.ItemsSource = items;
        //    TotalProfitText.Text = "";
        //}

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

            var items = ReportRepository.GetCustomerStats(from, to);
            ReportDataGrid.Columns.Clear();

            ReportDataGrid.ItemsSource = items;

            ReportDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Клієнт",
                Binding = new Binding("CustomerName")
            });

            ReportDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Кількість замовлень",
                Binding = new Binding("TotalOrders")
            });

            ReportDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Кількість придбаних товарів",
                Binding = new Binding("TotalProductsBought")
            });

            ReportDataGrid.ItemsSource = items;
            TotalProfitText.Text = "";
        }


        //private void btnInventory_Click(object sender, RoutedEventArgs e)
        //{
        //    var items = ReportRepository.GetInventoryStatus();
        //    ReportDataGrid.ItemsSource = items;
        //    TotalProfitText.Text = "";
        //}

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var items = ReportRepository.GetInventoryStatus();

                // Оновлення колонок вручну
                ReportDataGrid.Columns.Clear();
                ReportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Товар", Binding = new Binding("ProductName") });
                ReportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Залишок на складі", Binding = new Binding("TotalInStock") });

                ReportDataGrid.ItemsSource = items;
                TotalProfitText.Text = ""; // очищуємо прибуток
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при формуванні звіту залишків:\n" + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //private void btnInventory_Click(object sender, RoutedEventArgs e)
        //{
        //    var items = ReportRepository.GetInventoryStatus();
        //    ReportDataGrid.Columns.Clear();

        //    ReportDataGrid.Columns.Add(new DataGridTextColumn
        //    {
        //        Header = "Товар",
        //        Binding = new Binding("ProductName")
        //    });

        //    ReportDataGrid.Columns.Add(new DataGridTextColumn
        //    {
        //        Header = "Залишок",
        //        Binding = new Binding("TotalInStock")
        //    });

        //    ReportDataGrid.ItemsSource = items;
        //    TotalProfitText.Text = "";
        //}

    }
}
