using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BigMac.Models
{
    public class Message_m_ChatsGroups
    {
        public int id { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? datecreated { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string groupname { get; set; }
        public int createdbyid { get; set; }
        public int discontinued { get; set; }
        public int discontinuedbyid { get; set; }
        public DateTime? discontinueddate { get; set; }

        [ForeignKey("createdbyid")]
        public virtual Access_m_Users CreateBy { get; set; }

        [ForeignKey("discontinuedbyid")]
        public virtual Access_m_Users DiscontinuedBy { get; set; }

    }
}