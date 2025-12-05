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
using System.Globalization;


namespace FlowerShopApp.Forms
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = new Product
                {
                    Name = NameTextBox.Text.Trim(),
                    Type = TypeTextBox.Text.Trim(),
                    Description = DescriptionTextBox.Text.Trim(),
                    Price = decimal.Parse(PriceTextBox.Text, CultureInfo.InvariantCulture),
                    ExpiryDate = ExpiryDatePicker.SelectedDate
                };

                ProductRepository.AddProduct(product);
                MessageBox.Show("Товар успішно додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
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

