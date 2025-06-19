using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Product_z_ProductPriceType
    {
        public int id { get; set; }
        public string pricetype { get; set; }        
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }     
    }
}