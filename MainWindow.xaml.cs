using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Configuration;
using FlowerShopApp.Data;



namespace FlowerShopApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();
            //MessageBox.Show("✅ Підключення до БД успішне!", "Стан БД", MessageBoxButton.OK, MessageBoxImage.Information);
            DbStatusText.Text = "✅ Підключено до БД";
            DbStatusText.Foreground = Brushes.Green;
        }
        catch (Exception ex)
        {
            DbStatusText.Text = "🔴 Немає підключення до БД";
            DbStatusText.Foreground = Brushes.Red;
            MessageBox.Show($"❌ Помилка підключення до БД:\n{ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            //Close(); // — якщо хочем закрити, залишаєм, інакше можна просто залишити повідомлення
        }
    }

    private void ProductsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new FlowerShopApp.Forms.ProductsWindow();
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка відкриття форми Товари:\n{ex.Message}");
        }
    }


    //private void OrdersButton_Click(object sender, RoutedEventArgs e)
    //{
    //    var window = new FlowerShopApp.Forms.OrdersWindow();
    //    window.ShowDialog();
    //}

    private void OrdersButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new FlowerShopApp.Forms.OrdersWindow();
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка відкриття форми Замовлення:\n{ex.Message}");
        }
    }


    //private void CustomersButton_Click(object sender, RoutedEventArgs e)
    //{
    //    var window = new FlowerShopApp.Forms.CustomersWindow();
    //    window.ShowDialog();
    //}

    private void CustomersButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new FlowerShopApp.Forms.CustomersWindow();
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка відкриття форми Споживачі:\n{ex.Message}");
        }
    }


    //private void SuppliesButton_Click(object sender, RoutedEventArgs e)
    //{
    //    var window = new FlowerShopApp.Forms.SuppliesWindow();
    //    window.ShowDialog();
    //}

    private void SuppliesButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new FlowerShopApp.Forms.SuppliesWindow();
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка відкриття форми Постачання:\n{ex.Message}");
        }
    }


    //private void ReportsButton_Click(object sender, RoutedEventArgs e)
    //{
    //    var window = new FlowerShopApp.Forms.ReportsWindow();
    //    window.ShowDialog();
    //}
    private void ReportsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new FlowerShopApp.Forms.ReportsWindow();
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка відкриття форми Звіти:\n{ex.Message}");
        }
    }

    private void SuppliersButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new FlowerShopApp.Forms.SuppliersWindow();
        window.ShowDialog();
    }



    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }


}