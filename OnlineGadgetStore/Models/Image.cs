using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGadgetStore.Models
{
    public class Image
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }
    }
}
