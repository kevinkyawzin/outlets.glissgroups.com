using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class CusSup_m_Cards
    {
        public int id { get; set; }        
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
         [Display(Name = "ID")]
        public int cussupid { get; set; }
         [Display(Name = "Card No")]
        public Int32 cardno { get; set; }        
         [Display(Name = "Card Sr.No.")]
        public string cardserialno { get; set; }
        
         [Display(Name = "Card Type")]
        public string cardtype { get; set; }
         [Display(Name = "Expiry Date")]
        public DateTime? expirydate { get; set; }

         [Display(Name = "Print Date")]
         public DateTime? printeddate { get; set; }
         [Display(Name = "Print By")]
         public int? printedbyid { get; set; }

         [Display(Name = "Collection Date")]
         public DateTime? collectiondate { get; set; }
         [Display(Name = "Collected By")]
         public int? collectionhandlebyid { get; set; }
         [Display(Name = "Status.")]
         public string status { get; set; }

         [ForeignKey("cussupid")]
         public virtual CusSup_m_CusSup Member { get; set; }
    }
}