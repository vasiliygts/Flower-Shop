using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public DateTime? ExpiryDate { get; set; } // дозволяє null, якщо поле необов’язкове

    }
}

