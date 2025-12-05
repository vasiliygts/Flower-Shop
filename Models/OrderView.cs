using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class OrderView
    {
        public int OrderId { get; set; }
        public string Customer { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
    }
}

