using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Product_z_Category
    {
        public int id { get; set; }
        public string cocode { get; set; }
        public string Prefix { get; set; }
        public string Category { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}