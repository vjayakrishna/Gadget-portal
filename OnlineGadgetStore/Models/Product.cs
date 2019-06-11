using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGadgetStore.Models
{
    public class Product
    {
        
        public int ID { get; set; }

        public string Name { get; set; }

        public  List<Image> ImageURLs { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public ProductInfo ProductInfo { get; set; }
    }
}
