using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Product_m_ProductPrice
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }
        [Display(Name = "Type")]
        public string pricetype { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Sell Price")]
        public double? sellprice { get; set; }
        [Display(Name = "Redeem")]
        public double? redeemdollars { get; set; }
        [Display(Name = "Redeem")]
        public double? redeembonus { get; set; }
        [Display(Name = "Award")]
        public double? awarddollars { get; set; }
        [Display(Name = "Award")]
        public double? awardbonus { get; set; }
        [Display(Name = "Service Commission")]
        public double? servicecommission { get; set; }
        [Display(Name = "Service Allowance")]
        public double? serviceallowance { get; set; }
        [Display(Name = "Stock Received Ref:")]
        public int stockreceivedref { get; set; }

        [ForeignKey("productid")]
        public virtual Product_m_Product product { get; set; }
    }
}