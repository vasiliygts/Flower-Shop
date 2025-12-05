using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class SupplyDetailView
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Total => Quantity * PurchasePrice;
    }
}

