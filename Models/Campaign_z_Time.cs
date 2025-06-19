using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Campaign_z_Time
    {
        public int id { get; set; }
        public TimeSpan? timevalue { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}