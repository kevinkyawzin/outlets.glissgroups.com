using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class DetailEntry
    {
        public string branchcode { get; set; }
        public int groupid { get; set; }
        public int campaignid { get; set; }
        public int staffid { get; set; }
        public TimeSpan? time { get; set; }
        public DateTime? cdate { get; set; }
        //public int t2 { get; set; }
        //public int t3 { get; set; }        
        //public double actual { get; set; }
        //public double forecast { get; set; }
        public int categorytype { get; set; }
        public double? salesactual { get; set; }
        public double? salesforecast { get; set; }
        public double? productactual { get; set; }
        public double? productforecast { get; set; }
        public int categoryid { get; set; }
    }

    public class MultiPayment
    {
        public string paymentmode { get; set; }
        public double amountrecd { get; set; }
    }
}