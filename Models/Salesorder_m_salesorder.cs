using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Salesorder_m_salesorder
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Unique Code")]
        public string uniquecode { get; set; }
        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }
        [Display(Name = "Co Code")]
        public string cocode { get; set; }
        [Display(Name = "Create ID")]
        public int createid { get; set; }
        [Display(Name = "Customer ID")]
        public int cussupid { get; set; }
        [Display(Name = "Customer Name")]
        public string cussupname { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchage Rate")]
        public double exchangerate { get; set; }
        [Display(Name = "Sub Total")]
        public double total_subtotal { get; set; }
        [Display(Name = "Total Sales Tax")]
        public double total_salestax { get; set; }
        [Display(Name = "Total Discount")]
        public double total_discount { get; set; }
        [Display(Name = "Total")]
        public double total_total { get; set; }
        [Display(Name = "Total Amount Received")]
        public double total_amountreceived { get; set; }
        [Display(Name = "Total Amount Refund")]
        public double total_amountrefund { get; set; }
        [Display(Name = "Total Amount Void")]
        public double total_amountvoid { get; set; }
        [Display(Name = "AR Acct ID")]
        public string aracctid { get; set; }
        [Display(Name = "Sales Tax Acct ID")]
        public string salestaxacctid { get; set; }
        [Display(Name = "Discount Acct ID")]
        public string discountacctid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        public DateTime? resourcedate { get; set; }
        [Display(Name = "Delivery Address")]
        public string deliveryaddress { get; set; }
        [Display(Name = "Contact Person")]
        public string contactperson { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }
        [Display(Name = "Ref No")]
        public string refno { get; set; }
        [Display(Name = "Remark")]
        public string remark { get; set; }
        //Added by ZawZO on 14 Jan 2015
        [Display(Name = "Branch Asset ID")]
        public int? branchassetid { get; set; }
        [Display(Name = "Staff ID")]
        public string staffid { get; set; }
        [Display(Name = "Invoice ID")]
        public int? invoiceid { get; set; }
        //Added by ZawZO on 19 Jan 2015
        [Display(Name = "Start Time")]
        public string starttime { get; set; }
        [Display(Name = "End Time")]
        public string endtime { get; set; }
        public string queuenumber { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

        public IEnumerable<salesorder_m_detail> details { get; set; }

    }
    //Added by ZawZO on 19 Jan 2016
    public class facility_booking
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }
        [Display(Name = "Co Code")]
        public string cocode { get; set; }
        [Display(Name = "Customer ID")]
        public int cussupid { get; set; }
        [Display(Name = "Customer Name")]
        public string cussupname { get; set; }
        [Display(Name = "Facility")]
        public string facility { get; set; }
        [Display(Name = "Staff Name")]
        public string staffname { get; set; }
        [Display(Name = "Start Time")]
        public string starttime { get; set; }
        [Display(Name = "End Time")]
        public string endtime { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        public string total_total { get; set; }
        public string total_amountrefund { get; set; }
        public string queuenumber { get; set; }
        public int statusorder { get; set; }
        public string type { get; set; }


    }

}
