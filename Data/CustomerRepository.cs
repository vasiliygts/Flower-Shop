using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;

namespace FlowerShopApp.Data
{
    public class CustomerRepository
    {
        public static List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"SELECT customerid, fullname, phone, email, address, registeredat FROM customers";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerId = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Phone = reader.GetString(2),
                    Email = reader.GetString(3),
                    Address = reader.GetString(4),
                    RegisteredAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5)

                });
            }

            return customers;
        }

        public static void Add(Customer customer)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        INSERT INTO customers (fullname, phone, email, address)
        VALUES (@fullname, @phone, @email, @address)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("fullname", customer.FullName);
            cmd.Parameters.AddWithValue("phone", customer.Phone);
            cmd.Parameters.AddWithValue("email", customer.Email);
            cmd.Parameters.AddWithValue("address", customer.Address);
            cmd.ExecuteNonQuery();
        }

        public static void Update(Customer customer)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        UPDATE customers
        SET fullname = @fullname,
            phone = @phone,
            email = @email,
            address = @address
        WHERE customerid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("fullname", customer.FullName);
            cmd.Parameters.AddWithValue("phone", customer.Phone);
            cmd.Parameters.AddWithValue("email", customer.Email);
            cmd.Parameters.AddWithValue("address", customer.Address);
            cmd.Parameters.AddWithValue("id", customer.CustomerId);
            cmd.ExecuteNonQuery();
        }

        public static void Delete(int customerId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM customers WHERE customerid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", customerId);
            cmd.ExecuteNonQuery();
        }

        public static List<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT * FROM customers";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerId = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Phone = reader.GetString(2),
                    Email = reader.GetString(3),
                    Address = reader.GetString(4)
                });
            }

            return customers;
        }


    }
}

