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

namespace FlowerShopApp.Forms
{
    /// <summary>
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = CustomerRepository.GetAllCustomers();
            CustomersDataGrid.ItemsSource = customers;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddCustomerWindow();
            if (window.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem is Customer selected)
            {
                var window = new EditCustomerWindow(selected);
                if (window.ShowDialog() == true)
                {
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Оберіть клієнта для редагування.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem is Customer selected)
            {
                var confirm = MessageBox.Show($"Видалити клієнта \"{selected.FullName}\"?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    CustomerRepository.Delete(selected.CustomerId);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Оберіть клієнта для видалення.");
            }
        }

    }
}
