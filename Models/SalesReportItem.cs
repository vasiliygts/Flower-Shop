using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class SalesReportItem
    {
        //public string Product { get; set; } = "";
        //public int TotalQuantity { get; set; }
        //public decimal TotalRevenue { get; set; }
        //public decimal TotalCost { get; set; }
        //public decimal Profit => TotalRevenue - TotalCost;

        //public string CustomerName { get; set; }
        //public string ProductName { get; set; }
        //public int Quantity { get; set; }
        //public decimal PurchasePrice { get; set; }
        //public decimal SalePrice { get; set; }
        //public decimal Profit => (SalePrice - PurchasePrice) * Quantity;

        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        //public int OrderId { get; set; } // ➕ нове поле
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int TotalInStock { get; set; }  // для залишків на складі
        public decimal Profit => (SalePrice - PurchasePrice) * Quantity;
    }
}

