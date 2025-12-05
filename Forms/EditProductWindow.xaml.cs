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
using System.Globalization;

namespace FlowerShopApp.Forms
{
    public partial class EditProductWindow : Window
    {
        private readonly Product _product;

        public EditProductWindow(Product product)
        {
            InitializeComponent();
            _product = product;

            // Заповнення полів
            NameTextBox.Text = product.Name;
            TypeTextBox.Text = product.Type;
            PriceTextBox.Text = product.Price.ToString(CultureInfo.InvariantCulture);
            DescriptionTextBox.Text = product.Description;
            ExpiryDatePicker.SelectedDate = product.ExpiryDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _product.Name = NameTextBox.Text.Trim();
                _product.Type = TypeTextBox.Text.Trim();
                _product.Description = DescriptionTextBox.Text.Trim();
                _product.Price = decimal.Parse(PriceTextBox.Text, CultureInfo.InvariantCulture);
                _product.ExpiryDate = ExpiryDatePicker.SelectedDate;

                ProductRepository.UpdateProduct(_product);
                MessageBox.Show("Товар оновлено успішно!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

