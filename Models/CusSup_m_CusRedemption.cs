using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class CusSup_m_CusRedemption
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "CussupId")]
        public int cussupid { get; set; }
        [Display(Name = "Product Id")]
        public int productid { get; set; }
        [Display(Name = "Product Description")]
        public string productdesc { get; set; }
        [Display(Name = "Credit")]
        public double? credit { get; set; }
        [Display(Name = "debit")]
        public double? debit { get; set; }
        [Display(Name = "Balance")]
        public double? balance { get; set; }
        [Display(Name = "Create ID")]
        public int createid { get; set; }
        [Display(Name = "Remark")]
        public string remark { get; set; }
        [Display(Name = "Resource")]
        public string resource { get; set; }
        [Display(Name = "Ref")]
        public string RefNo { get; set; }
        [Display(Name = "Company ID")]
        public string cocode { get; set; }
        [Display(Name = "Branch ID")]
        public string branchcode { get; set; }
        [Display(Name = "Redemption Types")]
        public string redemptiontype { get; set; }
        public string uom { get; set; }
        public int? invoiceitemid { get; set; }
        //Added by ZawZO on 1 Dec 2015
        [Display(Name = "Package Code")]
        public string packagecode { get; set; }
        [Display(Name = "Package Description")]
        public string packagedesc { get; set; }

        [ForeignKey("createid")]
        public virtual Access_m_Users Create { get; set; }
        //[ForeignKey("productid")]
        //public virtual Product_m_Product Product { get; set; }

        public string cname = "";
        
    }

    public class CusSup_m_CusRedemptionDtl
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Ref.")]
        public string resourcecode { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Product Id")]
        public int productid { get; set; }
        [Display(Name = "Product Code")]
        public string productcode { get; set; }
        //Added by ZawZO on 24 Feb 2015
        [Display(Name = "Remark")]
        public string remark { get; set; }
        [Display(Name = "Product Description")]
        public string productdesc { get; set; }
        [Display(Name = "Type")]
        public string category { get; set; }
        [Display(Name = "Category")]
        public string categorysub { get; set; }
        [Display(Name = "Brand")]
        public string brand { get; set; }
        [Display(Name = "RangesNSeries")]
        public string RangesNSeries { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Quantity")]
        public int qty { get; set; }
        [Display(Name = "Redeem Citi$")]
        public double redeemdollars { get; set; }
        [Display(Name = "Redeem Bonus$")]
        public double redeembonus { get; set; }
        [Display(Name = "Redeem Citi$")]
        public double awarddollars { get; set; }
        [Display(Name = "Redeem Bonus$")]
        public double awardbonus { get; set; }
        public string status { get; set; }
        public string redemptionType { get; set; }
        public double sellprice { get; set; }
        [Display(Name = "Create ID")]
        public int createid { get; set; }
        //[Display(Name = "Remark")]
        //public string remark { get; set; }
        public int TotalPages;
     

    }

   


    //Created by ZawZO on 2 Dec 2015
    public class Redempt_Package
    {
        [Display(Name = "Package Code")]
        public string packagecode { get; set; }
        [Display(Name = "Package Description")]
        public string packagedesc { get; set; }
        [Display(Name = "Product Id")]
        public int productid { get; set; }
        [Display(Name = "Product Description")]
        public string productdesc { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Credit")]
        public double? credit { get; set; }
        [Display(Name = "debit")]
        public double? debit { get; set; }
        [Display(Name = "balance")]
        public double? balance { get; set; }
        [Display(Name = "maxRedeemAmt")]
        public double? maxredeemamt { get; set; }
    }

    public class Package_Summary
    {
        [Display(Name = "Package Code")]
        public string packagecode { get; set; }
        [Display(Name = "Package Description")]
        public string packagedesc { get; set; }
        [Display(Name = "Product Id")]
        public int productid { get; set; }
        [Display(Name = "Product Description")]
        public string productdesc { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Credit")]
        public double? credit { get; set; }
        [Display(Name = "debit")]
        public double? debit { get; set; }
        [Display(Name = "balance")]
        public double? balance { get; set; }
        public string receipt { get; set; }
        public DateTime? date { get; set; }
        public string remarks { get; set; }
    }
    //Created by ZawZO on 2 Dec 2015
    public class Incompleted_Package_Item
    {
        [Display(Name = "Member ID")]
        public int cussupid { get; set; }
        [Display(Name = "Package Code")]
        public string packagecode { get; set; }
        [Display(Name = "Package Description")]
        public string packagedesc { get; set; }
        [Display(Name = "Product Id")]
        public int productid { get; set; }
        [Display(Name = "Product Description")]
        public string productdesc { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Credit")]
        public double? credit { get; set; }
        [Display(Name = "debit")]
        public double? debit { get; set; }
        [Display(Name = "balance")]
        public double? balance { get; set; }
    }
}
