using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Message_m_Chats
    {
        public int id { get; set; }        
        public DateTime? datecreated { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int senderid { get; set; }
        public int receiverid { get; set; }
        public string message { get; set; }
        public int groupid { get; set; }

        [ForeignKey("groupid")]
        public virtual Message_m_ChatsGroups Group { get; set; }

        [ForeignKey("senderid")]
        public virtual Access_m_Users Sender { get; set; }

        [ForeignKey("receiverid")]
        public virtual Access_m_Users Receiver { get; set; }

        public string groupName;
        public string ctime;

    }
}