using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_m_Category
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Campaign")]
        public int campaignid { get; set; }
        [Display(Name = "Category")]
        public int campaigncategoryid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Type")]
        public int categorytypeid { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }

        [ForeignKey("campaignid")]
        public virtual Campaign_m_Campaign Campaign { get; set; }
        [ForeignKey("campaigncategoryid")]
        public virtual Campaign_z_Category Category { get; set; }
        [ForeignKey("categorytypeid")]
        public virtual Campaign_z_CategoryType CategoryType { get; set; }

        public string gameDesc;

    }
}