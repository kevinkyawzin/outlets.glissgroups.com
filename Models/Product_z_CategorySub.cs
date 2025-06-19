using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Product_z_CategorySub
    {
        public int id { get; set; }
        public string cocode { get; set; }
        public string prefix { get; set; }
        public string Category { get; set; }
        public string categorycode { get; set; }
        public int lastnumber { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }     
    }
}