using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class CusSup_z_CitibellaConversionRate
    {
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string description { get; set; }
        public double fromamount { get; set; }
        public double tocitidollar { get; set; }
        public double torewarddollar { get; set; }
        public double admissionfees { get; set; }
        public string desc = "";
    }
}
