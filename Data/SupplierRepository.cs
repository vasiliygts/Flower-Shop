using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;

namespace FlowerShopApp.Data
{
    public class SupplierRepository
    {
        public static List<Supplier> GetAll()
        {
            var list = new List<Supplier>();
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT supplierid, companyname, contactperson, phone, email, address FROM suppliers";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Supplier
                {
                    SupplierId = reader.GetInt32(0),
                    CompanyName = reader.GetString(1),
                    ContactPerson = reader.GetString(2),
                    Phone = reader.GetString(3),
                    Email = reader.GetString(4),
                    Address = reader.GetString(5)
                });
            }

            return list;
        }

        public static void Add(Supplier supplier)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO suppliers (companyname, contactperson, phone, email, address)
                VALUES (@companyname, @contactperson, @phone, @email, @address)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("companyname", supplier.CompanyName);
            cmd.Parameters.AddWithValue("contactperson", supplier.ContactPerson);
            cmd.Parameters.AddWithValue("phone", supplier.Phone);
            cmd.Parameters.AddWithValue("email", supplier.Email);
            cmd.Parameters.AddWithValue("address", supplier.Address);

            cmd.ExecuteNonQuery();
        }

        public static void Update(Supplier supplier)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                UPDATE suppliers
                SET companyname = @companyname,
                    contactperson = @contactperson,
                    phone = @phone,
                    email = @email,
                    address = @address
                WHERE supplierid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("companyname", supplier.CompanyName);
            cmd.Parameters.AddWithValue("contactperson", supplier.ContactPerson);
            cmd.Parameters.AddWithValue("phone", supplier.Phone);
            cmd.Parameters.AddWithValue("email", supplier.Email);
            cmd.Parameters.AddWithValue("address", supplier.Address);
            cmd.Parameters.AddWithValue("id", supplier.SupplierId);

            cmd.ExecuteNonQuery();
        }

        public static void Delete(int id)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM suppliers WHERE supplierid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
