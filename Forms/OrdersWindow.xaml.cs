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
using System.Linq;
using FlowerShopApp.Forms;



namespace FlowerShopApp.Forms
{
    public partial class OrdersWindow : Window
    {
        public OrdersWindow()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = OrderRepository.GetAllOrders();
                OrdersDataGrid.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження замовлень:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderView selected)
            {
                try
                {
                    var details = OrderRepository.GetOrderDetails(selected.OrderId);
                    OrderDetailsDataGrid.ItemsSource = details;

                    decimal total = details.Sum(d => d.Total);
                    TotalAmountText.Text = $"{total:0.00} грн";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження деталей замовлення:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddOrderWindow();
            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderView selectedView)
            {
                // потрібно перетворити OrderView на Order
                var order = new Order
                {
                    OrderId = selectedView.OrderId,
                    OrderDate = selectedView.OrderDate,
                    PaymentMethod = selectedView.PaymentMethod,
                    DeliveryAddress = selectedView.DeliveryAddress,
                    Status = selectedView.Status
                };

                var window = new EditOrderWindow(order);
                if (window.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Оберіть замовлення для редагування.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is OrderView selected)
            {
                var result = MessageBox.Show(
                    $"Ви дійсно хочете видалити замовлення №{selected.OrderId} клієнта {selected.Customer}?",
                    "Підтвердження видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        OrderRepository.Delete(selected.OrderId);
                        MessageBox.Show("Замовлення успішно видалено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при видаленні замовлення:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Оберіть замовлення для видалення.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}


