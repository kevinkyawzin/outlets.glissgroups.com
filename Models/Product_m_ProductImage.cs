using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Product_m_ProductImage
    {
        public int id { get; set; }
        public int productid { get; set; }
        public string image { get; set; }
        public int defaultflag { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }       
    }
}