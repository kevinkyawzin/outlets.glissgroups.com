using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Common_m_Message
    {
        public int id { get; set; }
        public string messagecode { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
        public string type { get; set; }
    }
 
}