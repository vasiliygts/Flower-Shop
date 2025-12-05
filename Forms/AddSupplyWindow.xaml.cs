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
    /// Interaction logic for AddSupplyWindow.xaml
    /// </summary>
    public partial class AddSupplyWindow : Window
    {
        private List<SupplyItem> supplyItems = new();

        public AddSupplyWindow()
        {
            InitializeComponent();
            SupplierComboBox.ItemsSource = SupplierRepository.GetAll();

            SupplierComboBox.DisplayMemberPath = "CompanyName";
            SupplierComboBox.SelectedValuePath = "SupplierId";

            ProductComboBox.ItemsSource = ProductRepository.GetAllProducts();
            SupplyDatePicker.SelectedDate = DateTime.Today;
        }

        private void AddProductToSupply_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is Product product &&
                int.TryParse(QuantityTextBox.Text, out int quantity) &&
                decimal.TryParse(PriceTextBox.Text, out decimal price) &&
                quantity > 0 && price > 0)
            {
                supplyItems.Add(new SupplyItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Quantity = quantity,
                    PurchasePrice = price
                });

                SupplyItemsDataGrid.ItemsSource = null;
                SupplyItemsDataGrid.ItemsSource = supplyItems;
            }
            else
            {
                MessageBox.Show("Перевірте введення товару, кількості і ціни.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SupplierComboBox.SelectedValue == null || !supplyItems.Any())
                {
                    MessageBox.Show("Оберіть постачальника і додайте хоча б один товар.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var supply = new Supply
                {
                    SupplierId = (int)SupplierComboBox.SelectedValue,
                    SupplyDate = SupplyDatePicker.SelectedDate ?? DateTime.Today
                };

                int supplyId = SupplyRepository.AddWithReturn(supply);

                foreach (var item in supplyItems)
                {
                    SupplyDetailRepository.Add(new SupplyDetail
                    {
                        SupplyId = supplyId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PurchasePrice = item.PurchasePrice
                    });
                }

                MessageBox.Show("Поставка збережена успішно.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}
