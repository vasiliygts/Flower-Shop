using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShopApp.Models;
using Npgsql;



namespace FlowerShopApp.Data
{
    public static class SupplyDetailRepository
    {
        public static void Add(SupplyDetail detail)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO supplydetails (supplyid, productid, quantity, purchaseprice)
                VALUES (@supplyid, @productid, @quantity, @purchaseprice)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplyid", detail.SupplyId);
            cmd.Parameters.AddWithValue("productid", detail.ProductId);
            cmd.Parameters.AddWithValue("quantity", detail.Quantity);
            cmd.Parameters.AddWithValue("purchaseprice", detail.PurchasePrice);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteBySupplyId(int supplyId)
        {
            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM supplydetails WHERE supplyid = @supplyid";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplyid", supplyId);
            cmd.ExecuteNonQuery();
        }

        public static List<SupplyDetail> GetBySupplyId(int supplyId)
        {
            var details = new List<SupplyDetail>();

            using var conn = DbConnectionHelper.GetConnection();
            conn.Open();

            string query = "SELECT supplydetailid, supplyid, productid, quantity, purchaseprice FROM supplydetails WHERE supplyid = @supplyid";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("supplyid", supplyId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                details.Add(new SupplyDetail
                {
                    SupplyDetailId = reader.GetInt32(0),
                    SupplyId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3),
                    PurchasePrice = reader.GetDecimal(4)
                });
            }

            return details;
        }
    }
}

