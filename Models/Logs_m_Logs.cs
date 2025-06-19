using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Logs_m_Logs
    {
        public int id { get; set; }
        public int resourceid { get; set; }
        public int resourcecode { get; set; }
        public int userid { get; set; }
        public string logtype { get; set; }
        public string description { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string ipaddress { get; set; }
        public string session { get; set; }
        public string cocode { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }        
    }
}