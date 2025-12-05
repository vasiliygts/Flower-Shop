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
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        private Customer _customer;

        public EditCustomerWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            // Заповнення полів
            FullNameTextBox.Text = customer.FullName;
            PhoneTextBox.Text = customer.Phone;
            EmailTextBox.Text = customer.Email;
            AddressTextBox.Text = customer.Address;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _customer.FullName = FullNameTextBox.Text.Trim();
                _customer.Phone = PhoneTextBox.Text.Trim();
                _customer.Email = EmailTextBox.Text.Trim();
                _customer.Address = AddressTextBox.Text.Trim();

                CustomerRepository.Update(_customer);
                MessageBox.Show("Клієнта оновлено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
