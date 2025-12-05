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
using FlowerShopApp.Forms;





namespace FlowerShopApp.Forms
{
    /// <summary>
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private Order _order;
        private List<OrderItem> orderItems = new();


        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            _order = order;

            //  Завантаження списку товарів у ComboBox
            ProductComboBox.ItemsSource = ProductRepository.GetAllProducts();

            OrderDatePicker.SelectedDate = _order.OrderDate;
            PaymentMethodTextBox.Text = _order.PaymentMethod;
            DeliveryAddressTextBox.Text = _order.DeliveryAddress;

            // Встановлюємо статус
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == _order.Status)
                {
                    item.IsSelected = true;
                    break;
                }
            }


            LoadOrderItems(); // 
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

                OrderItemsDataGrid.ItemsSource = null;
                OrderItemsDataGrid.ItemsSource = orderItems;
            }
            else
            {
                MessageBox.Show("Оберіть товар та введіть коректну кількість.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _order.OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now;
                _order.PaymentMethod = PaymentMethodTextBox.Text.Trim();
                _order.DeliveryAddress = DeliveryAddressTextBox.Text.Trim();
                _order.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "нове";

                OrderRepository.Update(_order);

                //  Видаляємо старі записи товарів з orderdetails
                OrderDetailRepository.DeleteByOrderId(_order.OrderId);

                //  Додаємо нові товари
                foreach (var item in orderItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = _order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = item.PriceAtOrderTime,
                        PurchasePrice = item.PriceAtOrderTime // або з партій у майбутньому
                    };

                    OrderDetailRepository.Add(detail);
                }

                MessageBox.Show("Замовлення оновлено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadOrderItems()
        {
            var details = OrderDetailRepository.GetByOrderId(_order.OrderId);

            orderItems = details.Select(d => new OrderItem
            {
                ProductId = d.ProductId,
                ProductName = ProductRepository.GetById(d.ProductId)?.Name ?? "Невідомо",
                Quantity = d.Quantity,
                PriceAtOrderTime = d.PriceAtOrderTime
            }).ToList();

            OrderItemsDataGrid.ItemsSource = orderItems;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
