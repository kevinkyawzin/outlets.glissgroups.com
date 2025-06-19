using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Product_m_ProductAltCode
    {
        public int id { get; set; }
        public int productid { get; set; }
        public string altcode { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }  
    }
}