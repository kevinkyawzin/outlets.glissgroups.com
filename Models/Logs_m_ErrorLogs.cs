using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Logs_m_ErrorLogs
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string errortype { get; set; }
        public string description { get; set; }
        public string ipaddress { get; set; }
        public int resourceid { get; set; }
        public string cocode { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}