using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Product_m_Citibella
    {
        [Key]
        [Display(Name = "Item No")]
        public string Item_no { get; set; }
        [Display(Name = "Inhouse Code")]
        public string Rpt_Code { get; set; }
        [Display(Name = "Des")]
        public string Item_Desc { get; set; }
        [Display(Name = "Class")]
        public string Class { get; set; }
        [Display(Name = "Div")]
        public string Div { get; set; }
        [Display(Name = "Brand")]
        public string Brand { get; set; }
        [Display(Name = "Dept")]
        public string Dept { get; set; }
        [Display(Name = "UOM")]
        public string uom { get; set; }
        [Display(Name = "Active")]
        public int item_isactive { get; set; }
        [Display(Name = "post")]
        public int post { get; set; }
        public DateTime? postdatetime { get; set; }
        public Int32? postby { get; set; }
        [Display(Name = "ItemNo")]
        public double? redeemciti { get; set; }
        public double? redeembonus { get; set; }
        public double? awardciti { get; set; }
        public double? awardbonus { get; set; }
        public double? servicecomm { get; set; }
        public double? sellingprice { get; set; }
        [Display(Name = "New ID")]
        public int? newItemid { get; set; }
        [Display(Name = "ItemCode")]
        public string newitemcode { get; set; }
        [Display(Name = "New Type")]
        public string type { get; set; }
        [Display(Name = "New Category")]
        public string category { get; set; }
        [Display(Name = "New Brand")]
        public string brandnew { get; set; }
        [Display(Name = "New Ranges")]
        public string ranges { get; set; }

        public Int32 TotalPages;
    }
}