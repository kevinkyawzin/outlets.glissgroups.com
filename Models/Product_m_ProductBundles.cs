using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Product_m_ProductBundles
    {
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int productid { get; set; }
        public int lineno { get; set; }
        public int itemid { get; set; }
        public double qty { get; set; }        

        [ForeignKey("itemid")]
        public virtual Product_m_Product Product { get; set; }
    }
}