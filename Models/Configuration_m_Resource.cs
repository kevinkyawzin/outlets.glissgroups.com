using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Configuration_m_Resource
    {
        public int id { get; set; }
        public string resource { get; set; }
        public string resourcegroupcode { get; set; }
        public string prefix { get; set; }
        public string postfix { get; set; }
        public string enabled { get; set; }
        public string trial { get; set; }
        public string trialenddate { get; set; }
        public DateTime?  createdate { get; set; }
        public string version { get; set; }

    }
}
