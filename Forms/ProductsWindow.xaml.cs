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
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        //private void LoadProducts()
        //{
        //    List<Product> products = ProductRepository.GetAllProducts();
        //    ProductsDataGrid.ItemsSource = products;
        //}

        private void LoadProducts()
        {
            try
            {
                var products = ProductRepository.GetAllProducts();
                ProductsDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження товарів:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadProducts(); // Перезавантажити таблицю
            }
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selected)
            {
                var editWindow = new EditProductWindow(selected);
                if (editWindow.ShowDialog() == true)
                {
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Оберіть товар для редагування.");
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selected)
            {
                var result = MessageBox.Show(
                    $"Ви дійсно хочете видалити товар \"{selected.Name}\"?",
                    "Підтвердження видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ProductRepository.DeleteProduct(selected.ProductId);
                        LoadProducts();
                        MessageBox.Show("Товар успішно видалено.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при видаленні:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Оберіть товар для видалення.");
            }
        }




    }
}
