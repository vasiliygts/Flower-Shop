using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class OrderDetailView
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal PriceAtOrderTime { get; set; }
        public decimal Total => Quantity * PriceAtOrderTime;
    }
}

