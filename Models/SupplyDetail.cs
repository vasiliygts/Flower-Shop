using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class SupplyDetail
    {
        public int SupplyDetailId { get; set; }
        public int SupplyId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}

