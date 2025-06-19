using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Message_m_ChatsUsers
    {
        public int id { get; set; }
        public DateTime? datecreated { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int userid { get; set; }
        public string  status { get; set; }
    }
}