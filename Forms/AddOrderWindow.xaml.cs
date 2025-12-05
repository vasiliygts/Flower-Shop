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
using FlowerShopApp.Models;
using System;

namespace FlowerShopApp.Forms
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        private List<OrderItem> orderItems = new();

        public AddOrderWindow()
        {
            InitializeComponent();
            LoadCustomers();
            OrderDatePicker.SelectedDate = DateTime.Now;
        }

        private void AddProductToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is Product selectedProduct &&
                int.TryParse(QuantityTextBox.Text, out int quantity) &&
                quantity > 0)
            {
                var item = new OrderItem
                {
                    ProductId = selectedProduct.ProductId,
                    ProductName = selectedProduct.Name,
                    Quantity = quantity,
                    PriceAtOrderTime = selectedProduct.Price
                };

                orderItems.Add(item);

                // Оновлення DataGrid
                OrderItemsDataGrid.ItemsSource = null;
                OrderItemsDataGrid.ItemsSource = orderItems;
            }
            else
            {
                MessageBox.Show("Оберіть товар та введіть коректну кількість.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadCustomers()
        {
            var customers = CustomerRepository.GetAllCustomers();
            CustomerComboBox.ItemsSource = customers;
        }

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var order = new Order
        //        {
        //            CustomerId = (int)CustomerComboBox.SelectedValue,
        //            OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now,
        //            PaymentMethod = PaymentMethodTextBox.Text.Trim(),
        //            DeliveryAddress = DeliveryAddressTextBox.Text.Trim(),
        //            Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "нове"
        //        };

        //        OrderRepository.Add(order);
        //        MessageBox.Show("Замовлення створено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        //        DialogResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Помилка:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = (int)CustomerComboBox.SelectedValue,
                    OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now,
                    PaymentMethod = PaymentMethodTextBox.Text.Trim(),
                    DeliveryAddress = DeliveryAddressTextBox.Text.Trim(),
                    Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "нове"
                };

                // Зберігаємо замовлення і отримуємо його ID
                int orderId = OrderRepository.AddWithReturn(order);

                // Зберігаємо деталі замовлення
                foreach (var item in orderItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = item.PriceAtOrderTime,
                        PurchasePrice = item.PriceAtOrderTime
                    };

                    OrderDetailRepository.Add(detail);
                }

                MessageBox.Show("Замовлення створено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var customers = CustomerRepository.GetAllCustomers();
            CustomerComboBox.ItemsSource = customers;

            var products = ProductRepository.GetAllProducts();
            ProductComboBox.ItemsSource = products;
        }

    }
}
