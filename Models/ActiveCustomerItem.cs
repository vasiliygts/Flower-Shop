using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class ActiveCustomerItem
    {
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProductsBought { get; set; }
    }
}
