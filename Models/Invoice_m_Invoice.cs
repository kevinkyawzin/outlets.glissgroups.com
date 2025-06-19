using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Invoice_m_Invoice
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [DisplayFormat(NullDisplayText = "", DataFormatString = " {0:dd/MM/yyyy}")]
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [Display(Name = "Supplier")]
        public int cussupid { get; set; }
        [Display(Name = "Supplier")]
        public string cussupname { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "Sub Total")]
        public double total_subtotal { get; set; }
        [Display(Name = "Tax")]
        public double total_salestax { get; set; }
        [Display(Name = "Discount")]
        public double total_discount { get; set; }
        [Display(Name = "Total")]
        public double total_total { get; set; }
        //Added by ZawZO on 18 Sep 2015
        [Display(Name = "Changes")]
        public double changes { get; set; }

        [Display(Name = "Received Amount")]
        public double total_amountreceived { get; set; }
        [Display(Name = "Refund Amount")]
        public double total_amountrefund { get; set; }
        [Display(Name = "Void Amount")]
        public double total_amountvoid { get; set; }
        [Display(Name = "ArAcctID")]
        public string aracctid { get; set; }
        [Display(Name = "salestaxacctid")]
        public string salestaxacctid { get; set; }
        [Display(Name = "discountacctid")]
        public string discountacctid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }
        [Display(Name = "refno")]
        public string refno { get; set; }
        public int staffid { get; set; }
        public int? printcount { get; set; }
        [Display(Name = "Post")]
        public int? post { get; set; }
        [Display(Name = "Post ID")]
        public int? postid { get; set; }
        //Added by ZawZO on 10 Jul 2015
        [Display(Name = "Resource Date")]
        public DateTime? resourcedate { get; set; }
        [Display(Name = "Post Date")]
        public DateTime? postdate { get; set; }
        //Added by ZawZO on 25 Feb 2015
        [Display(Name = "Remark")]
        public string remark { get; set; }
        //Added by ZawZO on 22 Sep 2015
        [Display(Name = "Signature File")]
        public string signaturefile { get; set; }
        //Added by ZawZO on 24 Dec 2015
        [Display(Name = "sales order ID")]
        public int? salesorderid { get; set; }
        public int? salesorderdetailid { get; set; }
        public int? discountpercent { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }
        [ForeignKey("createid")]
        public virtual Access_m_Users Create { get; set; }

        public IEnumerable<Invoice_m_Invoice_Items> items { get; set; }
        public IEnumerable<Invoice_m_Payment> payments { get; set; }
        public IEnumerable<Invoice_m_Invoice_SalesStaff> salestaffs { get; set; }
        public IEnumerable<Product_m_Product> TopUpItems { get; set; }
        //public Invoice_m_Invoice_Items topup;
    }

    public class ReservedList
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
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
        public double qty { get; set; }
        [Display(Name = "Redeem Citi Dollar")]
        public double redeemdollars { get; set; }
        [Display(Name = "Redeem Bonus Dollar")]
        public double redeembonus { get; set; }
        [Display(Name = "Award Citi Dollar")]
        public double awarddollars { get; set; }
        [Display(Name = "Award Bonus Dollar")]
        public double awardbonus { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
    }

}
