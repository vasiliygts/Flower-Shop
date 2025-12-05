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
    /// Interaction logic for SuppliersWindow.xaml
    /// </summary>
    public partial class SuppliersWindow : Window
    {
        public SuppliersWindow()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = SupplierRepository.GetAll();
                SuppliersDataGrid.ItemsSource = suppliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження постачальників:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddSupplierWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadSuppliers(); // оновити таблицю після додавання
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem is Supplier selected)
            {
                var editWindow = new EditSupplierWindow(selected);
                if (editWindow.ShowDialog() == true)
                {
                    LoadSuppliers();
                }
            }
            else
            {
                MessageBox.Show("Оберіть постачальника для редагування.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem is Supplier selected)
            {
                var result = MessageBox.Show(
                    $"Ви впевнені, що хочете видалити постачальника \"{selected.CompanyName}\"?",
                    "Підтвердження",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        SupplierRepository.Delete(selected.SupplierId);
                        LoadSuppliers();
                        MessageBox.Show("Постачальника видалено.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при видаленні:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Оберіть постачальника.");
            }
        }
    }
}
