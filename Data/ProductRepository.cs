using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;
using System.Collections.Generic;

namespace FlowerShopApp.Data
{
    public class ProductRepository
    {
        public static List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT productid, name, type, price, description, expirydate  FROM products";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //products.Add(new Product
                //{
                //    Id = reader.GetInt32(0),
                //    Name = reader.GetString(1),
                //    Category = reader.GetString(2),
                //    Price = reader.GetDecimal(3),
                //    Description = reader.GetString(4)
                //});

                products.Add(new Product
                {
                    ProductId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Type = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Price = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                    Description = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    ExpiryDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5)

                });
            }

            return products;
        }

        public static void AddProduct(Product product)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        INSERT INTO products (name, type, price, description, expirydate)
        VALUES (@name, @type, @price, @description, @expirydate)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("name", product.Name);
            cmd.Parameters.AddWithValue("type", product.Type);
            cmd.Parameters.AddWithValue("price", product.Price);
            cmd.Parameters.AddWithValue("description", product.Description ?? "");
            cmd.Parameters.AddWithValue("expirydate", (object?)product.ExpiryDate ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateProduct(Product product)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        UPDATE products
        SET name = @name,
            type = @type,
            price = @price,
            description = @description,
            expirydate = @expirydate
        WHERE productid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("name", product.Name);
            cmd.Parameters.AddWithValue("type", product.Type);
            cmd.Parameters.AddWithValue("price", product.Price);
            cmd.Parameters.AddWithValue("description", product.Description ?? "");
            cmd.Parameters.AddWithValue("expirydate", (object?)product.ExpiryDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id", product.ProductId);

            cmd.ExecuteNonQuery();
        }

        public static void DeleteProduct(int productId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM products WHERE productid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", productId);
            cmd.ExecuteNonQuery();
        }

        public static Product? GetById(int productId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM products WHERE productid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", productId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("productid")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Type = reader.GetString(reader.GetOrdinal("type")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                    ExpiryDate = reader.IsDBNull(reader.GetOrdinal("expirydate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("expirydate"))
                };
            }

            return null;
        }




    }
}
