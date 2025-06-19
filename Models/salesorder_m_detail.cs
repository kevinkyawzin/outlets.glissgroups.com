using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class salesorder_m_detail
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Sales Order ID")]
        public int salesorderid { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Resource ID")]
        public int resourceid { get; set; }
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
        [Display(Name = "Redeem Dollars")]
        public double redeemdollars { get; set; }
        [Display(Name = "Redeem Bonus")]
        public double redeembonus { get; set; }
        [Display(Name = "Award Dollars")]
        public double awarddollars { get; set; }
        [Display(Name = "Award Bonus")]
        public double awardbonus { get; set; }
        [Display(Name = "Service Commission")]
        public double? servicecommission { get; set; }
        [Display(Name = "Staff Service ID")]
        public int? staffserviceid { get; set; }
        public string packagecode { get; set; }
        public int foc { get; set; }
        public int rdm { get; set; }
        public int pos { get; set; }
        public int citi { get; set; }
        public int rwd { get; set; }
        public int pts { get; set; }
        public string status { get; set; }
        public string packagelineno { get; set; }
        public int ispackageselected { get; set; }
        public int redeemedqty { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public int staffid { get; set; }
        public int branchassetid { get; set; }
        public double? serviceallowance { get; set; }
        public int detailid { get; set; }
        public IEnumerable<cussup_m_treatment_Ops> treatment { get; set; }

    }
}