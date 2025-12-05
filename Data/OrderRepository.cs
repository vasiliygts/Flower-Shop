using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;

namespace FlowerShopApp.Data
{
    public class OrderRepository
    {
        public static List<OrderView> GetAllOrders()
        {
            var orders = new List<OrderView>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                SELECT o.orderid, c.fullname, o.orderdate, o.status, o.paymentmethod, o.deliveryaddress
                FROM orders o
                JOIN customers c ON o.customerid = c.customerid
                ORDER BY o.orderdate DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new OrderView
                {
                    OrderId = reader.GetInt32(0),
                    Customer = reader.GetString(1),
                    OrderDate = reader.GetDateTime(2),
                    Status = reader.GetString(3),
                    PaymentMethod = reader.GetString(4),
                    DeliveryAddress = reader.GetString(5)
                });
            }

            return orders;
        }

        public static List<OrderDetailView> GetOrderDetails(int orderId)
        {
            var details = new List<OrderDetailView>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                SELECT p.name AS product_name, od.quantity, od.priceatordertime
                FROM orderdetails od
                JOIN products p ON od.productid = p.productid
                WHERE od.orderid = @orderId";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("orderId", orderId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                details.Add(new OrderDetailView
                {
                    ProductName = reader.GetString(0),
                    Quantity = reader.GetInt32(1),
                    PriceAtOrderTime = reader.GetDecimal(2)
                });
            }

            return details;
        }

        public static void Add(Order order)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        INSERT INTO orders (customerid, orderdate, status, paymentmethod, deliveryaddress)
        VALUES (@customerid, @orderdate, @status, @paymentmethod, @deliveryaddress)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("customerid", order.CustomerId);
            cmd.Parameters.AddWithValue("orderdate", order.OrderDate);
            cmd.Parameters.AddWithValue("status", order.Status);
            cmd.Parameters.AddWithValue("paymentmethod", order.PaymentMethod);
            cmd.Parameters.AddWithValue("deliveryaddress", order.DeliveryAddress);

            cmd.ExecuteNonQuery();
        }

        public static void Update(Order order)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        UPDATE orders
        SET orderdate = @orderdate,
            status = @status,
            paymentmethod = @paymentmethod,
            deliveryaddress = @deliveryaddress
        WHERE orderid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("orderdate", order.OrderDate);
            cmd.Parameters.AddWithValue("status", order.Status);
            cmd.Parameters.AddWithValue("paymentmethod", order.PaymentMethod);
            cmd.Parameters.AddWithValue("deliveryaddress", order.DeliveryAddress);
            cmd.Parameters.AddWithValue("id", order.OrderId);
            cmd.ExecuteNonQuery();
        }

        public static void Delete(int orderId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            // Видалити спочатку orderdetails, потім замовлення (через FK)
            using var tran = conn.BeginTransaction();

            try
            {
                string deleteDetails = "DELETE FROM orderdetails WHERE orderid = @orderid";
                using (var cmdDetails = new NpgsqlCommand(deleteDetails, conn, tran))
                {
                    cmdDetails.Parameters.AddWithValue("orderid", orderId);
                    cmdDetails.ExecuteNonQuery();
                }

                string deleteOrder = "DELETE FROM orders WHERE orderid = @orderid";
                using (var cmdOrder = new NpgsqlCommand(deleteOrder, conn, tran))
                {
                    cmdOrder.Parameters.AddWithValue("orderid", orderId);
                    cmdOrder.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static int AddWithReturn(Order order)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        INSERT INTO orders (customerid, orderdate, status, paymentmethod, deliveryaddress)
        VALUES (@customerid, @orderdate, @status, @paymentmethod, @deliveryaddress)
        RETURNING orderid";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("customerid", order.CustomerId);
            cmd.Parameters.AddWithValue("orderdate", order.OrderDate);
            cmd.Parameters.AddWithValue("status", order.Status);
            cmd.Parameters.AddWithValue("paymentmethod", order.PaymentMethod);
            cmd.Parameters.AddWithValue("deliveryaddress", order.DeliveryAddress);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static List<Order> GetOrdersInRange(DateTime from, DateTime to)
        {
            var orders = new List<Order>();
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT * FROM orders WHERE orderdate BETWEEN @from AND @to";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("to", to);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    OrderDate = reader.GetDateTime(2),
                    Status = reader.GetString(3),
                    PaymentMethod = reader.GetString(4),
                    DeliveryAddress = reader.GetString(5)
                });
            }

            return orders;
        }





    }
}

