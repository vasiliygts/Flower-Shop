using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;

namespace FlowerShopApp.Data
{
    public class SupplyRepository
    {
        public static List<SupplyView> GetAllSupplies()
        {
            var supplies = new List<SupplyView>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                SELECT s.supplyid, s.supplierid, s.supplydate, sp.companyname
                FROM supplies s
                JOIN suppliers sp ON s.supplierid = sp.supplierid
                ORDER BY s.supplydate DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                supplies.Add(new SupplyView
                {
                    SupplyId = reader.GetInt32(0),
                    SupplierId = reader.GetInt32(1),
                    SupplyDate = reader.GetDateTime(2),
                    CompanyName = reader.GetString(3)
                });
            }

            return supplies;
        }

        public static List<SupplyDetailView> GetSupplyDetails(int supplyId)
        {
            var details = new List<SupplyDetailView>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
        SELECT p.name AS product_name, sd.quantity, sd.purchaseprice
        FROM supplydetails sd
        JOIN products p ON sd.productid = p.productid
        WHERE sd.supplyid = @supplyId";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplyId", supplyId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                details.Add(new SupplyDetailView
                {
                    ProductName = reader.GetString(0),
                    Quantity = reader.GetInt32(1),
                    PurchasePrice = reader.GetDecimal(2)
                });
            }

            return details;
        }

        public static int AddWithReturn(Supply supply)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO supplies (supplierid, supplydate)
                VALUES (@supplierid, @supplydate)
                RETURNING supplyid;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplierid", supply.SupplierId);
            cmd.Parameters.AddWithValue("supplydate", supply.SupplyDate);

            return Convert.ToInt32(cmd.ExecuteScalar());


        }

        public static void Delete(int supplyId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            var cmd = new NpgsqlCommand("DELETE FROM supplies WHERE supplyid = @id", conn);
            cmd.Parameters.AddWithValue("id", supplyId);
            cmd.ExecuteNonQuery();
        }

        public static void Update(Supply supply)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "UPDATE supplies SET supplierid=@supplierid, supplydate=@supplydate WHERE supplyid=@supplyid";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplierid", supply.SupplierId);
            cmd.Parameters.AddWithValue("supplydate", supply.SupplyDate);
            cmd.Parameters.AddWithValue("supplyid", supply.SupplyId);
            cmd.ExecuteNonQuery();
        }


    }

}

