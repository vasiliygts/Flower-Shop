using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Models
{
    public class SupplyView
    {
        public int SupplyId { get; set; }
        public int SupplierId { get; set; }
        public DateTime SupplyDate { get; set; }
        public string CompanyName { get; set; } = string.Empty; // назва має відповідати alias у SELECT
        //public string Supplier { get; set; } = "";


    }
}

