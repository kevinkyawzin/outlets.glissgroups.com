using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class stock_m_stockmovement
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string stockmoduletype { get; set; }
        public int stockrefid { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
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
        public double? lastbalance { get; set; }
        public double? productbalance { get; set; }
        public string cocode { get; set; }
        public string branchcode { get; set; }
        //public int TotalPages;
    }

    public class stock_m_stockmovementlist
    {
        public string branchcode { get; set; }
        public string productid { get; set; }
        public string productcode { get; set; }
        public string productdesc { get; set; }
        public string productbalance { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
        public string categorysub { get; set; }
        public string b1 { get; set; }
        public string b2 { get; set; }
        public string b3 { get; set; }
        public string b4 { get; set; }
        public string b5 { get; set; }
        public string b6 { get; set; }
        public string b7 { get; set; }
        public string b8 { get; set; }
        public string b9 { get; set; }
        public string b10 { get; set; }
        public string b11 { get; set; }
        public string b12 { get; set; }
        public string b13 { get; set; }
        public string b14 { get; set; }
        public string b15 { get; set; }
        public string b16 { get; set; }
        public string uom1 { get; set; }
        public string uom2 { get; set; }
        public string uom3 { get; set; }
        public string uom4 { get; set; }
        public string uom5 { get; set; }
        public string uom6 { get; set; }
        public string uom7 { get; set; }
        public string uom8 { get; set; }
        public string uom9 { get; set; }
        public string uom10 { get; set; }
        public string uom11 { get; set; }
        public string uom12 { get; set; }
        public string uom13 { get; set; }
        public string uom14 { get; set; }
        public string uom15 { get; set; }
        public string uom16 { get; set; }


    }
}