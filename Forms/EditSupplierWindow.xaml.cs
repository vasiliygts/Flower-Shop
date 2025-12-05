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
    /// Interaction logic for EditSupplierWindow.xaml
    /// </summary>
    public partial class EditSupplierWindow : Window
    {
        private Supplier _supplier;

        public EditSupplierWindow(Supplier supplier)
        {
            InitializeComponent();
            _supplier = supplier;

            // Заповнення полів
            CompanyNameTextBox.Text = _supplier.CompanyName;
            ContactPersonTextBox.Text = _supplier.ContactPerson;
            PhoneTextBox.Text = _supplier.Phone;
            EmailTextBox.Text = _supplier.Email;
            AddressTextBox.Text = _supplier.Address;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _supplier.CompanyName = CompanyNameTextBox.Text.Trim();
                _supplier.ContactPerson = ContactPersonTextBox.Text.Trim();
                _supplier.Phone = PhoneTextBox.Text.Trim();
                _supplier.Email = EmailTextBox.Text.Trim();
                _supplier.Address = AddressTextBox.Text.Trim();

                SupplierRepository.Update(_supplier);
                MessageBox.Show("Дані постачальника оновлено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
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
