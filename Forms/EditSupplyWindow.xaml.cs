using FlowerShopApp.Data;
using FlowerShopApp.Models;
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

namespace FlowerShopApp.Forms
{
    /// <summary>
    /// Interaction logic for EditSupplyWindow.xaml
    /// </summary>
    public partial class EditSupplyWindow : Window
    {
        private Supply _supply;
        private List<SupplyItem> supplyItems = new();

        public EditSupplyWindow(Supply supply)
        {
            InitializeComponent();
            _supply = supply;

            //SupplierComboBox.ItemsSource = SupplierRepository.GetAll();
            //ProductComboBox.ItemsSource = ProductRepository.GetAllProducts();

            //SupplierComboBox.SelectedValue = _supply.SupplierId;

            // Завантажити постачальників у ComboBox
            SupplierComboBox.ItemsSource = SupplierRepository.GetAll();
            SupplierComboBox.DisplayMemberPath = "CompanyName";
            SupplierComboBox.SelectedValuePath = "SupplierId";

            // Завантажити продукти
            ProductComboBox.ItemsSource = ProductRepository.GetAllProducts();
            ProductComboBox.DisplayMemberPath = "Name";
            ProductComboBox.SelectedValuePath = "ProductId";

            // Встановити обраного постачальника
            SupplierComboBox.SelectedValue = _supply.SupplierId;


            SupplyDatePicker.SelectedDate = _supply.SupplyDate;

            LoadSupplyDetails();
        }

        private void LoadSupplyDetails()
        {
            var details = SupplyDetailRepository.GetBySupplyId(_supply.SupplyId);

            supplyItems = details.Select(d => new SupplyItem
            {
                ProductId = d.ProductId,
                ProductName = ProductRepository.GetById(d.ProductId)?.Name ?? "Невідомо",
                Quantity = d.Quantity,
                PurchasePrice = d.PurchasePrice
            }).ToList();

            SupplyItemsDataGrid.ItemsSource = supplyItems;
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
                MessageBox.Show("Перевірте введення товару, кількості і ціни.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _supply.SupplierId = (int)SupplierComboBox.SelectedValue;
            _supply.SupplyDate = SupplyDatePicker.SelectedDate ?? DateTime.Today;

            SupplyRepository.Update(_supply);

            SupplyDetailRepository.DeleteBySupplyId(_supply.SupplyId);
            foreach (var item in supplyItems)
            {
                SupplyDetailRepository.Add(new SupplyDetail
                {
                    SupplyId = _supply.SupplyId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PurchasePrice = item.PurchasePrice
                });
            }

            DialogResult = true;
        }

        private void RemoveProductFromSupply_Click(object sender, RoutedEventArgs e)
        {
            if (SupplyItemsDataGrid.SelectedItem is SupplyItem selectedItem)
            {
                if (MessageBox.Show($"Видалити {selectedItem.ProductName} з поставки?", "Підтвердження",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    supplyItems.Remove(selectedItem);
                    SupplyItemsDataGrid.ItemsSource = null;
                    SupplyItemsDataGrid.ItemsSource = supplyItems;
                }
            }
            else
            {
                MessageBox.Show("Оберіть товар для видалення.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
