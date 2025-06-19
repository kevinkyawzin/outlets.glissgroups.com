using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Invoice_m_Invoice_SalesStaff
    {
        public int id { get; set; }
        public int createid { get; set; }
        public int invoiceid { get; set; }
        public string resourcecode { get; set; }
        public int? resourcedetailid { get; set; } 
        public int lineno { get; set; }
        public int staffid { get; set; }
        public int percent { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}