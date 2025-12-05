using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;
using System;
using System.Windows;
using System.Diagnostics;




namespace FlowerShopApp.Data
{
    public class ReportRepository
    {
        //public static List<SalesReportItem> GetSalesReport(DateTime from, DateTime to)
        //{
        //    var report = new List<SalesReportItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    //string query = @"
        //    //    SELECT product_name,
        //    //           SUM(quantity) AS total_quantity,
        //    //           SUM(price_at_order_time * quantity) AS total_revenue,
        //    //           SUM(purchase_price * quantity) AS total_cost
        //    //    FROM vw_orderdetailsfull
        //    //    WHERE order_date BETWEEN @from AND @to
        //    //    GROUP BY product_name
        //    //    ORDER BY total_revenue DESC";

        //    string query = @"
        //SELECT c.fullname, p.name, od.quantity, od.purchaseprice, od.priceatordertime
        //FROM orderdetails od
        //JOIN orders o ON o.orderid = od.orderid
        //JOIN products p ON p.productid = od.productid
        //JOIN customers c ON c.customerid = o.customerid
        //WHERE o.orderdate BETWEEN @from AND @to
        //ORDER BY c.fullname, p.name";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("from", from);
        //    cmd.Parameters.AddWithValue("to", to);

        //    using var reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        report.Add(new SalesReportItem
        //        {
        //            //ProductName = reader.GetString(0),
        //            //TotalQuantity = reader.GetInt32(1),
        //            //TotalRevenue = reader.GetDecimal(2),
        //            //TotalCost = reader.GetDecimal(3)

        //            //ProductName = reader.GetString(0),
        //            //Quantity = reader.GetInt32(1),
        //            //SalePrice = reader.GetDecimal(2),
        //            //PurchasePrice = reader.GetDecimal(3)

        //            CustomerName = reader.GetString(0),
        //            ProductName = reader.GetString(1),
        //            Quantity = reader.GetInt32(2),
        //            PurchasePrice = reader.GetDecimal(3),
        //            SalePrice = reader.GetDecimal(4)
        //        });
        //    }

        //    return report;
        //}

        //public static List<SalesReportItem> GetSalesReport(DateTime from, DateTime to)
        //{
        //    var report = new List<SalesReportItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    string query = @"
        //SELECT 
        //    c.fullname AS customer_name,
        //    p.name AS product_name,
        //    od.quantity,
        //    od.purchaseprice,
        //    od.priceatordertime
        //FROM orderdetails od
        //JOIN orders o ON o.orderid = od.orderid
        //JOIN products p ON p.productid = od.productid
        //JOIN customers c ON c.customerid = o.customerid
        //WHERE o.orderdate BETWEEN @from AND @to
        //ORDER BY c.fullname, p.name";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("from", from);
        //    cmd.Parameters.AddWithValue("to", to);

        //    using var reader = cmd.ExecuteReader();

        //    //while (reader.Read())
        //    //{
        //    //    report.Add(new SalesReportItem
        //    //    {
        //    //        CustomerName = reader.GetString(0),
        //    //        ProductName = reader.GetString(1),
        //    //        Quantity = reader.GetInt32(2),
        //    //        PurchasePrice = reader.GetDecimal(3),
        //    //        SalePrice = reader.GetDecimal(4)
        //    //    });
        //    //}


        //    for (int i = 0; i < reader.FieldCount; i++)
        //    {
        //        Debug.WriteLine($"{i}: {reader.GetName(i)} ({reader.GetDataTypeName(i)})");
        //    }

        //    while (reader.Read())
        //    {
        //        string customer = reader.GetString(0);
        //        string product = reader.GetString(1);
        //        int quantity = reader.GetInt32(2);
        //        decimal purchase = reader.GetDecimal(3);
        //        decimal sale = reader.GetDecimal(4);

        //        //  Виводимо вивід у вікно Output
        //        System.Diagnostics.Debug.WriteLine($"Customer: {customer}, Product: {product}, Qty: {quantity}, Purchase: {purchase}, Sale: {sale}");
        //        MessageBox.Show($"Customer: {customer}\nProduct: {product}\nQty: {quantity}\nPurchase: {purchase}\nSale: {sale}");

        //        report.Add(new SalesReportItem
        //        {
        //            CustomerName = customer,
        //            ProductName = product,
        //            Quantity = quantity,
        //            PurchasePrice = purchase,
        //            SalePrice = sale
        //        });
        //    }

        //    return report;
        //}

        public static List<SalesReportItem> GetSalesReport(DateTime from, DateTime to)
        {
            var report = new List<SalesReportItem>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        SELECT 
            c.fullname AS customer_name,
            p.name AS product_name,
            od.quantity,
            od.purchaseprice,
            od.priceatordertime
        FROM orderdetails od
        JOIN orders o ON o.orderid = od.orderid
        JOIN products p ON p.productid = od.productid
        JOIN customers c ON c.customerid = o.customerid
        WHERE o.orderdate BETWEEN @from AND @to
        ORDER BY c.fullname, p.name";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("to", to);

            using var reader = cmd.ExecuteReader();

            // Визначаємо індекси стовпчиків по імені
            int idxCustomer = reader.GetOrdinal("customer_name");
            int idxProduct = reader.GetOrdinal("product_name");
            int idxQuantity = reader.GetOrdinal("quantity");
            int idxPurchasePrice = reader.GetOrdinal("purchaseprice");
            int idxSalePrice = reader.GetOrdinal("priceatordertime");

            while (reader.Read())
            {
                report.Add(new SalesReportItem
                {
                    CustomerName = reader.GetString(idxCustomer),
                    ProductName = reader.GetString(idxProduct),
                    Quantity = reader.GetInt32(idxQuantity),
                    PurchasePrice = reader.GetDecimal(idxPurchasePrice),
                    SalePrice = reader.GetDecimal(idxSalePrice)
                });
            }

            return report;
        }

        //public static List<SalesReportItem> GetPopularProducts(DateTime from, DateTime to)
        //{
        //    var list = new List<SalesReportItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    string query = @"
        //        SELECT p.name, SUM(od.quantity) AS total_quantity
        //        FROM orderdetails od
        //        JOIN orders o ON o.orderid = od.orderid
        //        JOIN products p ON p.productid = od.productid
        //        WHERE o.orderdate BETWEEN @from AND @to
        //        GROUP BY p.name
        //        ORDER BY total_quantity DESC";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("from", from);
        //    cmd.Parameters.AddWithValue("to", to);

        //    using var reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        list.Add(new SalesReportItem
        //        {
        //            ProductName = reader.GetString(0),
        //            Quantity = reader.GetInt32(1)
        //        });
        //    }
        //    return list;
        //}

        //public static List<SalesReportItem> GetCustomerStats(DateTime from, DateTime to)
        //{
        //    var list = new List<SalesReportItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    string query = @"
        //        SELECT c.fullname, SUM(od.quantity), SUM(od.priceatordertime * od.quantity)
        //        FROM orderdetails od
        //        JOIN orders o ON o.orderid = od.orderid
        //        JOIN customers c ON c.customerid = o.customerid
        //        WHERE o.orderdate BETWEEN @from AND @to
        //        GROUP BY c.fullname
        //        ORDER BY SUM(od.priceatordertime * od.quantity) DESC";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("from", from);
        //    cmd.Parameters.AddWithValue("to", to);

        //    using var reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        list.Add(new SalesReportItem
        //        {
        //            CustomerName = reader.GetString(0),
        //            Quantity = reader.GetInt32(1),
        //            SalePrice = reader.GetDecimal(2),
        //            PurchasePrice = 0 // не рахуємо прибуток у цьому звіті
        //        });
        //    }
        //    return list;
        //}

        //public static List<SalesReportItem> GetInventoryStatus()
        //{
        //    var list = new List<SalesReportItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    string query = @"
        //        SELECT p.name, i.quantity
        //        FROM inventory i
        //        JOIN products p ON p.productid = i.productid
        //        ORDER BY p.name";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    using var reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        list.Add(new SalesReportItem
        //        {
        //            ProductName = reader.GetString(0),
        //            Quantity = reader.GetInt32(1)
        //        });
        //    }
        //    return list;
        //}

        public static List<PopularProductItem> GetPopularProducts(DateTime from, DateTime to)
        {
            var result = new List<PopularProductItem>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        SELECT p.name, SUM(od.quantity) AS total_sold
        FROM orderdetails od
        JOIN products p ON p.productid = od.productid
        JOIN orders o ON o.orderid = od.orderid
        WHERE o.orderdate BETWEEN @from AND @to
        GROUP BY p.name
        ORDER BY total_sold DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("to", to);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new PopularProductItem
                {
                    ProductName = reader.GetString(0),
                    TotalQuantitySold = reader.GetInt32(1)
                });
            }

            return result;
        }

        public static List<ActiveCustomerItem> GetCustomerStats(DateTime from, DateTime to)
        {
            var result = new List<ActiveCustomerItem>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        SELECT c.fullname,
               COUNT(DISTINCT o.orderid) AS total_orders,
               SUM(od.quantity) AS total_products
        FROM orders o
        JOIN customers c ON c.customerid = o.customerid
        JOIN orderdetails od ON od.orderid = o.orderid
        WHERE o.orderdate BETWEEN @from AND @to
        GROUP BY c.fullname
        ORDER BY total_products DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("to", to);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new ActiveCustomerItem
                {
                    CustomerName = reader.GetString(0),
                    TotalOrders = reader.GetInt32(1),
                    TotalProductsBought = reader.GetInt32(2)
                });
            }

            return result;
        }

        //public static List<InventoryStatusItem> GetInventoryStatus()
        //{
        //    var result = new List<InventoryStatusItem>();

        //    using var conn = DbConnectionHelper.GetConnection();
        //    conn.Open();

        //    string query = @"
        //SELECT p.name, SUM(i.quantity)
        //FROM inventory i
        //JOIN products p ON p.productid = i.productid
        //GROUP BY p.name
        //ORDER BY p.name";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    using var reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        result.Add(new InventoryStatusItem
        //        {
        //            ProductName = reader.GetString(0),
        //            TotalInStock = reader.GetInt32(1)
        //        });
        //    }

        //    System.Diagnostics.Debug.WriteLine(">>> Отримано: " + reader.GetString(0) + " — " + reader.GetInt32(1));

        //    return result;
        //}


        public static List<SalesReportItem> GetInventoryStatus()
        {
            var result = new List<SalesReportItem>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        SELECT p.name AS product_name, 
               SUM(i.quantity) AS total_in_stock
        FROM inventory i
        JOIN products p ON i.productid = p.productid
        GROUP BY p.name
        ORDER BY p.name";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new SalesReportItem
                {
                    ProductName = reader["product_name"].ToString(),
                    TotalInStock = Convert.ToInt32(reader["total_in_stock"])
                });
            }

            return result;
        }







    }
}

