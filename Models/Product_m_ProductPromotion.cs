using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Product_m_ProductPromotion
    {
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime? startdate  { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "End Date")]
        public DateTime? enddate { get; set; }
        [Display(Name = "Price")]
        public double price { get; set; }
        [Display(Name = "Redeem")]
        public double redeemdollars { get; set; }
        [Display(Name = "Redeem")]
        public double redeembonus { get; set; }
        [Display(Name = "Award")]
        public double awarddollars { get; set; }
        [Display(Name = "Award")]
        public double awardbonus { get; set; }
        [Display(Name = "Service Commission")]
        public double servicecommission { get; set; }

        [ForeignKey("productid")]
        public virtual Product_m_Product product { get; set; }
    }
}