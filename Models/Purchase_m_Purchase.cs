using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Purchase_m_Purchase
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
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
        [Display(Name = "Paid Amount")]
        public double total_amountpaid { get; set; }
        [Display(Name = "Refund Amount")]
        public double total_amountrefund { get; set; }
        [Display(Name = "Void Amount")]
        public double total_amountvoid { get; set; }
        [Display(Name = "ApAcctID")]
        public string apacctid { get; set; }
        [Display(Name = "purchasetaxacctid")]
        public string purchasetaxacctid { get; set; }
        [Display(Name = "discountacctid")]
        public string discountacctid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }

        [ForeignKey("createid")]
        public virtual Common_m_Staff Create { get; set; }

        public IEnumerable<Purchase_m_Purchase_Items> items { get; set; }
    }
}