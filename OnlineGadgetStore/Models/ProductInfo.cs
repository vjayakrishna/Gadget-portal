using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineGadgetStore.Models
{
    public class ProductInfo
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }

        [Display(Name ="Model Name")]
        public String ModelName { get; set; }

        public String Color { get; set; }

        public String Touchscreen { get; set; }

        [Display(Name ="Display Size")]
        public String DisplaySize { get; set; }

        public String Resolution { get; set; }

        [Display(Name ="Resolution Type")]
        public String ResolutionType { get; set; }

        [Display(Name ="Display Type")]
        public String DisplayType { get; set; }

        [Display(Name ="Operating System")]
        public String OperatingSystem { get; set; }

        public String Processor { get; set; }

        [Display(Name ="Internal Storage")]
        public String InternalStorage { get; set; }

        [Display(Name ="Primary Camera")]
        public String PrimaryCamera { get; set; }

        [Display(Name ="Secondary Camera")]
        public String SecondaryCamera { get; set; }

        [Display(Name ="Network Type")]
        public String NetworkType { get; set; }

        [Display(Name ="Supported Networks")]
        public String SupportedNetworks { get; set; }

        [Display(Name ="Internet Connectivity")]
        public String InternetConnectivity { get; set; }

        [Display(Name ="Wi-Fi")]
        public String WiFi { get; set; }

        [Display(Name ="Wi-Fi Version")]
        public String WiFiVersion { get; set; }

        [Display(Name ="Map Support")]
        public String MapSupport { get; set; }

        [Display(Name ="GPS Support")]
        public String GPSSupport { get; set; }

        [Display(Name ="Sensors")]
        public String Sensors { get; set; }

        [Display(Name ="Warranty Details")]
        public String Warranty { get; set; }

    }
}
