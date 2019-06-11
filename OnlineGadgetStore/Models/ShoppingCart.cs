using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGadgetStore.Models
{
    public class ShoppingCart
    {
        public int ID { get; set; }

        public decimal TotalPrice { get; set; }

        public String ApplicationUserID { get; set; } 

        public ApplicationUser ApplicationUser { get; set; }

        public List<LineProduct> LineProducts { get; set; }
    }
}
