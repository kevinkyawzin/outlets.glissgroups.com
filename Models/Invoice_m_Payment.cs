using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Invoice_m_Payment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Invoice ID")]
        public int invoiceid { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Payment Mode")]
        public string paymentmode { get; set; }
        [Display(Name = "Received Amount")]
        public double amountrecd { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [Display(Name = "Received By")]
        public int receivedid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Cheque/Credit Card")]
        public string chequecreditcard { get; set; }
        [Display(Name = "Remarks")]
        public string remarks { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double exchangerate { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }
        //[ForeignKey("createid")]
        //public virtual Common_m_Staff Create { get; set; }
        //public IEnumerable<Invoice_m_Invoice_Items> items { get; set; }
    }


    //Kyaw on 20240729
    public class Daily_Closing
    {
        // i.id, DATE(i.createdate) createdate, i.branchcode, i.resourcecode, i.cussupname, paymentmode, sales, discount, gst, nett, total_amountreceived
        public string id { get; set; }
        public string createdate { get; set; }
        public string branchcode { get; set; }
        public string resourcecode { get; set; }
        public string cussupname { get; set; }
        public string paymentmode { get; set; }
        public string sales { get; set; }
        public string discount { get; set; }
        public string gst { get; set; }
        public string nett { get; set; }
        public string total_amountreceived { get; set; }

    }
}