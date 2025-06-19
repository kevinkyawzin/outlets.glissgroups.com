using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Product_m_Product
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Proudct Code")]
        public string productcode { get; set; }
        [Display(Name = "Type")]
        public string category { get; set; }
        [Display(Name = "Category")]
        public string categorysub { get; set; }
        [Display(Name = "Brand")]
        public string brand { get; set; }
        [Display(Name = "Ranges & Series")]
        public string RangesNSeries { get; set; }
        [Display(Name = "Description")]
        public string desc { get; set; }
        //////[Display(Name = "Quantity")]
        //////public int? qty { get; set; }
        [Display(Name = "UOM 1")]
        public string uom { get; set; }
        [Display(Name = "UOM 2")]
        public string uom2 { get; set; }
        [Display(Name = "Factor")]
        public int uomfactor2 { get; set; }
        [Display(Name = "UOM 3")]
        public string uom3 { get; set; }
        [Display(Name = "Factor")]
        public int uomfactor3 { get; set; }
        [Display(Name = "Remark")]
        public string remark { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        public int? stock { get; set; }

        public double redeemdollars = 0;
        public double redeembonus = 0;
        public double sellprice = 0;
        //public string tmpAltCodes;

        public IEnumerable<Product_m_ProductPrice> prices { get; set; }
        public IEnumerable<Product_m_ProductBundles> bundleItems { get; set; }
        public IEnumerable<Product_m_ProductAltCode> AltCodes { get; set; }
        public IEnumerable<Product_m_ProductImage> Images { get; set; }
        public Product_m_ProductPrice priceItem;
    }

    public class Product_m_Productdtl
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Proudct Code")]
        public string productcode { get; set; }
        [Display(Name = "Category")]
        public string category { get; set; }
        [Display(Name = "Sub Category")]
        public string categorysub { get; set; }
        [Display(Name = "Brand")]
        public string brand { get; set; }
        [Display(Name = "Description")]
        public string desc { get; set; }
        [Display(Name = "UOM 1")]
        public string uom { get; set; }
        [Display(Name = "UOM 2")]
        public string uom2 { get; set; }
        [Display(Name = "Factor")]
        public int uomfactor2 { get; set; }
        [Display(Name = "UOM 3")]
        public string uom3 { get; set; }
        [Display(Name = "Factor")]
        public int uomfactor3 { get; set; }
        [Display(Name = "Remark")]
        public string remark { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }

         [Display(Name = "Balance")]
        public double serviceqty { get; set; }

        public double redeemdollars { get; set; }
        public double redeembonus { get; set; }
        public double awarddollars { get; set; }
        public double awardbonus { get; set; }
        public double servicecommission { get; set; }
        public double sellprice { get; set; }
       
        public double stockqty { get; set; }
        public int productid { get; set; }


        public IEnumerable<Product_m_ProductPrice> prices { get; set; }

        public Product_m_ProductPrice priceItem;
        public int TotalPages;
    }

    public class pkgRedeemProduct
    {
        public int id { get; set; }
        public int productid { get; set; }
        public string productcode { get; set; }
        public string productdesc { get; set; }
        public string uom { get; set; }
        public double balance { get; set; }
        public double sellprice { get; set; }
        public double servicecommission { get; set; }
    }

    public class Product_Sales_Kits
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Description")]
        public string desc { get; set; }
    }

}