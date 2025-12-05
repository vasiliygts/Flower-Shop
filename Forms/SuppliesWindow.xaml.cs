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
    public partial class SuppliesWindow : Window
    {
        public SuppliesWindow()
        {
            InitializeComponent();
            LoadSupplies();
        }

        private void LoadSupplies()
        {
            try
            {
                var supplies = SupplyRepository.GetAllSupplies();
                SuppliesDataGrid.ItemsSource = supplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження закупок:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SuppliesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SuppliesDataGrid.SelectedItem is SupplyView selected)
            {
                try
                {
                    var details = SupplyRepository.GetSupplyDetails(selected.SupplyId);
                    SupplyDetailsDataGrid.ItemsSource = details;

                    // Розрахунок суми
                    decimal total = details.Sum(d => d.Total);
                    TotalAmountText.Text = $"{total:0.00} грн";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завантаження деталей:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddSupply_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddSupplyWindow { Owner = this };
            if (window.ShowDialog() == true)
                LoadSupplies(); // оновити таблицю після додавання
        }

        private void EditSupply_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliesDataGrid.SelectedItem is SupplyView selectedSupply)
            {
                var supply = new Supply
                {
                    SupplyId = selectedSupply.SupplyId,
                    SupplierId = selectedSupply.SupplierId,
                    SupplyDate = selectedSupply.SupplyDate
                };

                var window = new EditSupplyWindow(supply) { Owner = this };
                if (window.ShowDialog() == true)
                    LoadSupplies(); // оновити таблицю після редагування
            }
            else
                MessageBox.Show("Оберіть поставку для редагування.");
        }

        private void DeleteSupply_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliesDataGrid.SelectedItem is SupplyView selectedSupply)
            {
                if (MessageBox.Show("Видалити обрану поставку?", "Підтвердження",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SupplyDetailRepository.DeleteBySupplyId(selectedSupply.SupplyId);
                    SupplyRepository.Delete(selectedSupply.SupplyId);
                    LoadSupplies(); // оновити таблицю після видалення
                }
            }
            else
                MessageBox.Show("Оберіть поставку для видалення.");
        }


    }
}


