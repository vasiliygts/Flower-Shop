using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;

namespace FlowerShopApp.Data
{
    public static class OrderDetailRepository
    {
        public static void Add(OrderDetail detail)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO orderdetails (orderid, productid, quantity, priceatordertime, purchaseprice)
                VALUES (@orderid, @productid, @quantity, @price, @purchase)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("orderid", detail.OrderId);
            cmd.Parameters.AddWithValue("productid", detail.ProductId);
            cmd.Parameters.AddWithValue("quantity", detail.Quantity);
            cmd.Parameters.AddWithValue("price", detail.PriceAtOrderTime);
            cmd.Parameters.AddWithValue("purchase", detail.PurchasePrice);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteByOrderId(int orderId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM orderdetails WHERE orderid = @orderid";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("orderid", orderId);
            cmd.ExecuteNonQuery();
        }

        public static List<OrderDetail> GetByOrderId(int orderId)
        {
            var list = new List<OrderDetail>();
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM orderdetails WHERE orderid = @orderid";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@orderid", orderId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new OrderDetail
                {
                    OrderDetailId = reader.GetInt32(reader.GetOrdinal("orderdetailid")),
                    OrderId = reader.GetInt32(reader.GetOrdinal("orderid")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("productid")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    PriceAtOrderTime = reader.GetDecimal(reader.GetOrdinal("priceatordertime")),
                    PurchasePrice = reader.GetDecimal(reader.GetOrdinal("purchaseprice"))
                });
            }

            return list;
        }

        public static List<OrderDetail> GetAll()
        {
            var list = new List<OrderDetail>();
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT * FROM orderdetails";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new OrderDetail
                {
                    OrderId = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    PriceAtOrderTime = reader.GetDecimal(3),
                    PurchasePrice = reader.GetDecimal(4)
                });
            }

            return list;
        }



    }
}

