using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class stock_m_stocktransfer_items
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Stock Transfer ID")]
        public int stocktransferid { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [Display(Name = "Line no")]
        public int lineno { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }
        [Display(Name = "Product Code")]
        public string productcode { get; set; }
        [Display(Name = "Product")]
        public string productdesc { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Qty")]
        public double? qty { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double? exchangerate { get; set; }
        [Display(Name = "Unit Price")]
        public double? unitprice { get; set; }
        [Display(Name = "Discount")]
        public double? discountamount { get; set; }
        [Display(Name = "Tax")]
        public double? taxamount { get; set; }
        [Display(Name = "Amount")]
        public double? lineamount { get; set; }
        public int TotalPages;
    }
}
