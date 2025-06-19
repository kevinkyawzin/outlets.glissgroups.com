using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Invoice_m_Invoice_Items
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Invoice ID")]
        public int invoiceid { get; set; }
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
        public double qty { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "Unit Price")]
        public double unitprice { get; set; }
        //Added by ZawZO on 17 Dec 2015
        [Display(Name = "Discount Percent")]
        public int? discountpercent { get; set; }
        [Display(Name = "Discount")]
        public double discountamount { get; set; }
        [Display(Name = "Tax")]
        public double taxamount { get; set; }
        [Display(Name = "Amount")]
        public double lineamount { get; set; }
        [Display(Name = "GST Code")]
        public string gstcode { get; set; }
        [Display(Name = "Redeem Citi Dollar")]
        public double? redeemdollars { get; set; }
        [Display(Name = "Redeem Bonus Dollar")]
        public double? redeembonus { get; set; }
        [Display(Name = "Award Citi Dollar")]
        public double? awarddollars { get; set; }
        [Display(Name = "Award Bonus Dollar")]
        public double? awardbonus { get; set; }
        [Display(Name = "Service Commission")]
        public double? servicecommission { get; set; }
        public int? staffserviceid { get; set; }
        public int TotalPages;
        public int? detailid { get; set; }

        // Kyaw
        public double? balance;
        [Display(Name = "Redemption Type")]
        public string redemptiontype { get; set; }


        public IEnumerable<Invoice_m_Invoice_SalesStaff> salestaffs { get; set; }
        public IEnumerable<cussup_m_treatment_Ops> treatment { get; set; }
    }

    public class Invoice_m_Invoice_Itemsdtl
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Invoice ID")]
        public int invoiceid { get; set; }
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
        public double qty { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "Unit Price")]
        public double unitprice { get; set; }
        [Display(Name = "Discount")]
        public double discountamount { get; set; }
        [Display(Name = "Tax")]
        public double taxamount { get; set; }
        [Display(Name = "Amount")]
        public double lineamount { get; set; }
        [Display(Name = "GST Code")]
        public string gstcode { get; set; }
        [Display(Name = "Redeem Citi Dollar")]
        public double redeemdollars { get; set; }
        [Display(Name = "Redeem Bonus Dollar")]
        public double redeembonus { get; set; }
        [Display(Name = "Award Citi Dollar")]
        public double awarddollars { get; set; }
        [Display(Name = "Award Bonus Dollar")]
        public double awardbonus { get; set; }
        [Display(Name = "Service Commission")]
        public double servicecommission { get; set; }
        public int? staffserviceid { get; set; }
        public int detailid { get; set; }
        public int TotalPages;
    }
    //Added by ZawZO on 22 Jan 2016
    public class Invoice_Itemsdtl_fromSOdetl
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Line no")]
        public int lineno { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }
        [Display(Name = "Product Code")]
        public string productcode { get; set; }
        [Display(Name = "Product")]
        public string productdesc { get; set; }
        [Display(Name = "Service Commission")]
        public double servicecommission { get; set; }
        [Display(Name = "Unit Price")]
        public double unitprice { get; set; }
        [Display(Name = "Qty")]
        public double qty { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "Amount")]
        public double lineamount { get; set; }
        [Display(Name = "Discount")]
        public double discountamount { get; set; }
        public double awardbonus { get; set; }
        public double awarddollars { get; set; }
        public int redeemedqty { get; set; }
        public int detailid { get; set; }

   
    }
}
