using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;
using System.Data.Objects;


namespace BigMac.Controllers
{
    public class CampaignController : Controller
    {
        private BigMacEntities db = new BigMacEntities();
        // GET: /Campaign/

        public ActionResult Index()
        {
            return View(db.Campaigns.ToList());
        }
        // GET: /Access/Create

        [HttpPost]
        public ActionResult CampaignDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Campaign_m_Campaign campaign = db.Campaigns.Find(id);
                db.Campaigns.Remove(campaign);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult Step1(int id = 0)
        {
            ViewBag.StatusOptions = db.CampaignStatus.AsEnumerable();            
            //var campaign = new Campaign_m_Campaign();
            //user.accessRights = new List<Access_m_AccessRights>();
            if (id > 0)
            {
                Campaign_m_Campaign campaign = db.Campaigns.Find(id);
                return View(campaign);
            }
            else
            {
                Campaign_m_Campaign campaign = new Campaign_m_Campaign();
                return View(campaign);
            }
        }

        [HttpPost]
        public ActionResult Step1(Campaign_m_Campaign campaign)
        {

            //dbtmp.ope
            //var userid = db.Users.Where(x =>x.username == User.Identity.Name).AsEnumerable().First().id;
            try
            {
                //if (campaign.id)
                //{
                    if (campaign.id > 0)
                    {
                        db.Entry(campaign).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                        SaveNewCampaign(campaign);
                //}
                //else
                //{
                //    SaveNewCampaign(campaign);
                //}
            }
            catch (Exception e)
            { 
                
            }
            return RedirectToAction("Step2", new { id = campaign.id });
            //ViewBag.StatusOptions = db.CampaignStatus.AsEnumerable(); 
            //return View(campaign);
        }

        public void SaveNewCampaign(Campaign_m_Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.createdbyid = Convert.ToInt32(Session["userid"].ToString());   //userid;
                campaign.createdate = DateTime.Now;
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                //return RedirectToAction("CreateStep2", new { id = campaign.id });
            }
        }

        public void SaveCampaign(Campaign_m_Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.createdbyid = Convert.ToInt32(Session["userid"].ToString());   //userid;
                campaign.createdate = DateTime.Now;
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                //return RedirectToAction("CreateStep2", new { id = campaign.id });
            }
        }

        public JsonResult getBranches(int groupid)
        {
            
            try
            {
                string dstr = "" ;
                //var MKey = "";
                //var IVKey = "";
                //var MKeyHex = "";
                //var IVKeyHex = "";
                var EncryptHex = "";
                ICollection<Campaign_z_BranchesGroups> bgroups = db.BranchGroupLinks.Where(x => x.groupid == groupid).Include("Branch").ToList();
                foreach (var tmp in bgroups)
                {
                    //cAESEncryption.AESDecryption(tmp.Branch.branchname , out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr); 
                    dstr = cAESEncryption.getDecryptedString(tmp.Branch.branchname);
                    tmp.Branch.branchname = dstr; 
                }

                return Json(bgroups.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }            

        }

        public JsonResult getBranchStaffs(string branchcode)
        {
            try
            {
                var tmp= db.BranchStaff.Where(x => x.branchcode == branchcode).Include("Staff").ToList();
                return Json(tmp.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public JsonResult getCampaigns()
        {
            try
            {
                var tmp = db.Campaigns.OrderByDescending(x=>x.createdate).ToList();
                return Json(tmp.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public ActionResult Step2(int id)
        {
            ViewBag.GroupOptions = db.Groups.AsEnumerable();
            //ViewBag.CampaignStaffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == id).Include("Group").Include("Branch").Include("Staff").AsEnumerable();
            //ViewBag.CampaignStaffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == id).AsEnumerable();
            
            //ViewBag.BranchOptions = db.CampaignStatus.AsEnumerable();
            //ViewBag.StaffOptions = db.CampaignStatus.AsEnumerable();
            //var campaign = new Campaign_m_Campaign();
            //user.accessRights = new List<Access_m_AccessRights>();
            Campaign_m_Attendance_Staff cstaff = new Campaign_m_Attendance_Staff();
            cstaff.campaignid = id;
            cstaff.gameDesc = db.Campaigns.Find(id).gamedesc;
            return View(cstaff);
        }

        [HttpPost]
        public ActionResult Step2(Campaign_m_Attendance_Staff staff)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        staff.createdate = DateTime.Today;
            //        db.CampaignAttendanceStaffs.Add(staff);
            //        db.SaveChanges();
            //        //.return RedirectToAction("Index");
            //    }
            //}
            //catch (Exception e)
            //{

            //}
            ViewBag.GroupOptions = db.Groups.AsEnumerable();
            //ViewBag.CampaignStaffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == staff.campaignid).Include("Group").Include("Branch").Include("Staff").AsEnumerable();
            staff.gameDesc = db.Campaigns.Find(staff.campaignid).gamedesc;
            return View(staff);
        }

        [HttpPost]
        public ActionResult Step2AddStaff(int campaignid, int groupid, string branchcode, int staffid)
        {
            string returnStr = "";
            try
            {
                Campaign_m_Attendance_Staff staff = new Campaign_m_Attendance_Staff();
                staff.campaignid = campaignid;
                staff.groupid = groupid;
                staff.branchcode = branchcode;
                staff.staffid = staffid;
                staff.createdate = DateTime.Now;
                staff.cocode = Session["cocode"].ToString(); 
                //staff.status = 
                db.CampaignAttendanceStaffs.Add(staff);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Content(returnStr);
        }


        [HttpPost]
        public JsonResult Step2StaffList(int campaignid)
        {
            //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).Include("Group").Include("Branch").Include("Staff").OrderBy(x => new { x.Group.group,x.branchcode,x.Staff.name}).ToList();
            ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).OrderBy(x => new { x.Group.group, x.branchcode, x.Staff.name }).ToList();
            int i = 0;

            for (i = 0; i < staffs.Count; i++)
            {
                staffs.ElementAt(i).DecryptedBranchName = cAESEncryption.getDecryptedString(staffs.ElementAt(i).Branch.branchname);
            }

            return Json(staffs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CampaignStaffDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Campaign_m_Attendance_Staff staff = db.CampaignAttendanceStaffs.Find(id);
                db.CampaignAttendanceStaffs.Remove(staff);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult Step3(int id)
        {
            ViewBag.StatusOptions = db.CampaignStatus.AsEnumerable();
            ViewBag.CategoryOptions = db.Categories.AsEnumerable();
            ViewBag.CategoryTypeOptions = db.CategoryTypes.AsEnumerable();
            ViewBag.CampaignCategories = db.CampaignCategories.Where(x => x.campaignid == id).OrderBy(x=>x.categorytypeid).AsEnumerable();
            
            //ViewBag.BranchOptions = db.CampaignStatus.AsEnumerable();
            //ViewBag.StaffOptions = db.CampaignStatus.AsEnumerable();
            //var campaign = new Campaign_m_Campaign();
            //user.accessRights = new List<Access_m_AccessRights>();
            Campaign_m_Category ccategory = new Campaign_m_Category();
            ccategory.campaignid = id;
            ccategory.gameDesc = db.Campaigns.Find(id).gamedesc;
            //ccategory.ca  
            return View(ccategory);
        }

        [HttpPost]
        public ActionResult Step3(Campaign_m_Category category)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        category.createdate = DateTime.Today;
            //        db.CampaignCategories.Add(category);
            //        db.SaveChanges();
            //        //return RedirectToAction("Index");
            //    }
            //}
            //catch (Exception e)
            //{

            //}
            ViewBag.StatusOptions = db.CampaignStatus.AsEnumerable();
            ViewBag.CategoryOptions = db.Categories.AsEnumerable();
            ViewBag.CategoryTypeOptions = db.CategoryTypes.AsEnumerable();
            //ViewBag.CampaignCategories = db.CampaignCategories.Where(x => x.campaignid == category.campaignid).AsEnumerable();
            category.gameDesc = db.Campaigns.Find(category.campaignid).gamedesc;
            return View(category);
        }

        [HttpPost]
        public ActionResult Step3AddCategory(int campaignid, int categoryid, int categorytypeid, string status)
        {
            string returnStr = "";
            try
            {
                Campaign_m_Category category = new Campaign_m_Category();
                category.campaignid = campaignid;
                category.campaigncategoryid = categoryid;
                category.categorytypeid = categorytypeid;
                category.status = status;
                category.createdate = DateTime.Now;
                //staff.status = 
                db.CampaignCategories.Add(category);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Content(returnStr);
        }


        [HttpPost]
        public JsonResult Step3CategoryList(int campaignid)
        {
            ICollection<Campaign_m_Category> category = db.CampaignCategories.Where(x => x.campaignid == campaignid).OrderBy(x => new { x.categorytypeid, x.Category.Category}).ToList();
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CampaignCategoryDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Campaign_m_Category category = db.CampaignCategories.Find(id);
                db.CampaignCategories.Remove(category);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult Step4(int id)
        {

            Campaign_m_Campaign campaign =db.Campaigns.Find(id);
            return View(campaign);
        }

        [Authorize]
        public ActionResult CampaignDetail(int id=0,string rcode ="")
        {
            string branchcode="";
            ViewBag.rcode = rcode;
            ViewBag.CampaignID = id;
            ViewBag.GroupOptions = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).AsEnumerable();
            ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).ToList();
            ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            ViewBag.DayTypeOptions = db.StaffDayType.OrderBy(x => x.value).ToList();
            ViewBag.gid = 0;
            if (Session["userid"] != null)
            {
                branchcode = Session["branchcode"].ToString();
                //ViewBag.bcode = branchcode;
                var gtmp = db.BranchGroupLinks.Where(x => x.branchcode == branchcode).FirstOrDefault();
                if (gtmp != null)
                    ViewBag.gid = gtmp.groupid;
                //ViewBag.GroupOptions = db.camp(x => x.group).AsEnumerable();
                //ViewBag.CategoryOptions = db.Categories.AsEnumerable();                 
            }
            else
            {                
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
            }
            ViewBag.bcode = branchcode;
            return View();
        }

        [Authorize]
        public ActionResult CampaignDetailManual(int id = 0, string rcode = "")
        {
            string branchcode = "";
            ViewBag.rcode = rcode;
            ViewBag.CampaignID = id;
            ViewBag.GroupOptions = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).AsEnumerable();
            ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).ToList();
            ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            ViewBag.DayTypeOptions = db.StaffDayType.OrderBy(x => x.value).ToList();
            ViewBag.gid = 0;
            if (Session["userid"] != null)
            {
                branchcode = Session["branchcode"].ToString();
                //ViewBag.bcode = branchcode;
                var gtmp = db.BranchGroupLinks.Where(x => x.branchcode == branchcode).FirstOrDefault();
                if (gtmp != null)
                    ViewBag.gid = gtmp.groupid;
                //ViewBag.GroupOptions = db.camp(x => x.group).AsEnumerable();
                //ViewBag.CategoryOptions = db.Categories.AsEnumerable();                 
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
            }
            ViewBag.bcode = branchcode;
            return View();
        }

        //[HttpPost]
        [Authorize]
        //public ActionResult getCampaignDetailGrid(int campaignid, string branchcode, int groupid, int t1,int t2, int t3)
        public JsonResult getCampaignDetailGrid(int campaignid, string branchcode, int groupid, TimeSpan time, DateTime? cdate, int ctype = 1,Boolean branchSale = true)
        {
            var returnHtml = "";
            if (Session["userid"] != null)
            {

                if (branchSale == true)
                {
                    returnHtml = branchSaleEntry(campaignid, branchcode, groupid, time, cdate);
                }
                else
                {
                    string tblname = "tblActivity";
                    string tblwidth = "";
                    string rspan = "";

                    if (ctype == 1)
                    {
                        tblname = "tblSale";
                        tblwidth = "width:100%;";
                    }
                    else if (ctype == 2) { rspan = "rowspan ='2'"; }

                    returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' id='" + tblname + "' name='" + tblname + "' " + tblwidth + "'><thead>";
                    var closeHtml = "</tbody></table>";
                    int r = 0; int c = 0; int dtlcount = 0;
                    //var tmpHtml = "";
                    double? sbtarget = 0;
                    double? abtarget = 0;
                    string cocode = Session["cocode"].ToString();
                    string saleahtml = ""; string salefhtml = "";
                    string prodahtml = ""; string prodfhtml = "";

                    //TimeSpan time = new TimeSpan();
                    if (cdate == null)
                    {
                        cdate = DateTime.Now;
                    }
                    try
                    {
                        ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode).ToList();
                        ICollection<campaign_m_branchactivitytarget> bactivity = db.CampaignBranchAcitivityTarget.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == ctype).ToList();
                        //var sbtargetObj = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.staffid == 0).FirstOrDefault();
                        var sbtargetObj = bsales.Where(x => x.staffid == 0).FirstOrDefault();
                        if (sbtargetObj != null)
                        {
                            sbtarget = sbtargetObj.salesforecast;
                        }
                        ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == ctype).Include("Category").OrderBy(x => x.id).ToList();
                        ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                        //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                        ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.time == time).Include("Category").ToList();
                        ICollection<Campaign_m_CampaignDetails> sadetails = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.time == time && x.categorytypeid == 1 ).ToList();
                        //ICollection<Campaign_m_CampaignDetails> summarydtl;
                        //using (var context = new BigMacEntities())
                        //{
                        //    //string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + Convert.ToDateTime(cdate).ToString("yyyy-MM-dd") + "' and groupid = " + groupid + " and branchcode='" + branchcode + "' and time <='" + time + "' GROUP BY cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.staffid  = t2.staffid and t1.categorytypeid = t2.categorytypeid and t1.categoryid = t2.categoryid and t1.maxtimeid = t2.time";
                        //    string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + Convert.ToDateTime(cdate).ToString("yyyy-MM-dd") + "' and groupid = " + groupid + " and branchcode='" + branchcode + "' GROUP BY cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.staffid  = t2.staffid and t1.categorytypeid = t2.categorytypeid and t1.categoryid = t2.categoryid and t1.maxtimeid = t2.time";
                        //    summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();                        
                        //}
                        //TimeSpan? maxtime = details.Where(x => x.categorytypeid == 1).Max(x => x.time);

                        if (ctype == 1)
                            returnHtml = returnHtml + "<tr><th style='width:100px;' rowspan='2' >Staff Name</th><th style='width:50px;' rowspan='2' ></th><th style='width:70px;' rowspan='2'>Daily Sales<br/>Target</th><th style='width:70px;' rowspan='2'>Sales<br/>Till Now</th><th style='width:70px;' rowspan='2'>Hourly Goals</th><th style='width:70px;' rowspan='2'>Shortage</th> ";
                        else
                            returnHtml = returnHtml + "<tr><th style='width:180px;' rowspan='2'>Staff Name</th><th style='width:50px;' rowspan='2'></th><th style='width:120px;' rowspan='2'>Daily Sales Target</th><th style='width:120px;' rowspan='2'>Sales Till Now</th><th style='width:120px;' rowspan='2'>Hourly Goals</th><th style='width:120px;' rowspan='2'>Shortage</th> ";
                        //returnHtml = returnHtml + "";
                        //returnHtml = returnHtml + "";
                        //returnHtml = returnHtml + "";
                        //returnHtml = returnHtml + "";
                        if (ctype == 1)
                            returnHtml = returnHtml + "<th colspan='" + categories.Count + "' style='text-align:center;'> M ($)</th> <th colspan='" + categories.Count + "' style='text-align:center;'> Product </th><tr >";

                        string txtdailyboxes = "";
                        string cathtml = "";
                        foreach (var category in categories)
                        {
                            cathtml = cathtml + "<th style='max-width:60px;padding:0;text-align:center;vertical-align:central;' " + rspan + ">" + category.Category.Category + "</th>";
                            if (ctype == 2)
                            {
                                //var abtargetObj = db.CampaignBranchAcitivityTarget.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate & x.categoryid == category.Category.id && x.categorytypeid == ctype).FirstOrDefault();
                                var abtargetObj = bactivity.Where(x => x.categoryid == category.Category.id).FirstOrDefault();
                                if (abtargetObj != null)
                                {
                                    abtarget = abtargetObj.forecast;
                                }
                                txtdailyboxes = txtdailyboxes + "<td style ='padding:0px;marign:0px;padding:0px;font-size:13px;'><input type='text' class='form-control' style ='background-color:#ADC17D;height:25px;' af ='T' id='A_" + category.Category.id + "' value ='" + abtarget + "'></td>";
                            }
                        }
                        if (ctype == 1)
                            returnHtml = returnHtml + cathtml + cathtml + "</tr></thead><tbody>";
                        else
                            returnHtml = returnHtml + cathtml + "</tr></thead><tbody>";

                        if (ctype == 1)
                        {
                            returnHtml = returnHtml + "<tr style='background-color:#ADC17D;height:25px;'><td colspan='2' style='background-color:#ADC17D;padding:0px 0px 0px 5px;text-align:left;'  data-sFlag = '0'>Branch Daily Target</td><td style ='padding:0px;marign:0px;padding:0px;'><input  type='text' class='form-control' style ='background-color:#ADC17D;height:25px;font-size:13px;' onchange='txtonChanged(this);' ch ='0' id='txtDailySales' value ='" + sbtarget + "' af ='T'></td><td style ='background-color:#ADC17D;' colspan ='" + ((categories.Count * 2) + 3).ToString() + "' ></td></tr>";
                        }
                        else
                        {
                            returnHtml = returnHtml + "<tr style='background-color:#ADC17D;height:25px;'><td colspan='6' style='background-color:#ADC17D;padding:0px 0px 0px 5px;text-align:left;' data-sFlag = '0'>Branch Daily Target</td>" + txtdailyboxes + "</tr>";
                        }

                        foreach (var staff in staffs)
                        {
                            returnHtml = returnHtml + "<tr id='" + staff.staffid + "'  data-sFlag = '1' >";
                            //returnHtml = returnHtml + "<td rowspan ='2' style='padding:0px 0px 0px 5px;text-align:left;'><a style='width:100%;height:100%;'  id='S_" + staff.staffid + "' onclick='OpenStaffDay(this);' data-day='ATTEND'>" + staff.Staff.name + "<a/></td>";

                            ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.staffid == staff.staffid && x.categorytypeid == ctype && x.time == time).OrderBy(x => x.categoryid).ToList();

                            //details.Where(x=>x.staffid == staff.staffid).Sum(x=>x.actual)
                            double hourlyforcest = details.Where(x => x.time == time && x.staffid == staff.staffid && x.categorytypeid == 1).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            //double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode && x.staffid == staff.staffid).Sum(x => (double?)x.salesforecast) ?? 0d;
                            double saleForcest = bsales.Where(x => x.staffid == staff.staffid).Sum(x => (double?)x.salesforecast) ?? 0d;
                            
                            //double? saleActual = summarydtl.Where(x => x.staffid == staff.staffid && x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;                        
                            //double? saleActual = details.Where(x => x.staffid == staff.staffid && x.categorytypeid == 1 && x.time == maxtime).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            //commented by Hninwy on 20150129 - take from branch target
                            //double? saleActual = details.Where(x => x.staffid == staff.staffid && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            //double saleActual = bsales.Where(x => x.staffid == staff.staffid).Sum(x => (double?)x.salesactual) ?? 0d;
                            double? saleActual = sadetails.Where(x => x.staffid == staff.staffid && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            var dtmpobj = bsales.Where(x => x.staffid == staff.staffid).FirstOrDefault();
                            string doptvalue = "";
                            if (dtmpobj != null)
                            {
                                doptvalue = dtmpobj.staffdaytype;
                            }
                            else
                                doptvalue = "Attend";

                            returnHtml = returnHtml + "<td rowspan ='2' style='padding:0px 0px 0px 5px;text-align:left;' data-day='" + doptvalue + "' title='" + doptvalue + "'>" + staff.Staff.name + "</td><td rowspan='2' style='padding:0;width:50px;'><select style='width:50px;' onchange='ChangeStaffDay(this)'></select></td>";
                            //returnHtml = returnHtml + "<td rowspan ='2' style='padding:0px 0px 0px 5px;text-align:left;' onclick='OpenStaffDay(this);' data-day='" + doptvalue + "' title='" + doptvalue + "'>" + staff.Staff.name + "</td><td style='padding:0px 0px 0px 5px;><select></select></td>";
                            //returnHtml = returnHtml + "<td rowspan =2>" + saleForcest + "</td>";
                            //<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' style ='padding-top:0px;padding-right:0px;max-height:20px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='SF' id='SF_" + id + "' value ='" + sforecast.ToString() + "'></td>
                            returnHtml = returnHtml + "<td rowspan ='2' style ='padding:0px;'><input class='form-control' style ='background-color:#EEE;min-height:49px;height:100%;'  type='text' onchange='txtonChanged(this);' ch ='0' af ='SS' id='SS_" + Convert.ToString(staff.staffid) + "' value ='" + saleForcest.ToString() + "'></td>";
                            returnHtml = returnHtml + "<td rowspan =2>" + saleActual + "</td><td rowspan =2>" + hourlyforcest + "</td><td rowspan =2>" + (saleActual - saleForcest) + "</td>";
                            dtlcount = dtl.Count;
                            c = 0;
                            saleahtml = salefhtml = prodahtml = prodfhtml = "";

                            foreach (var category in categories)
                            {
                                var id = Convert.ToString(staff.staffid) + "_" + Convert.ToString(category.campaigncategoryid);
                                double? sforecast, sactual, pforecast, pactual;
                                sforecast = sactual = pforecast = pactual = 0;
                                if (c < dtl.Count)
                                {
                                    var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).FirstOrDefault();
                                    if (tmpobj != null)
                                    {
                                        sforecast = tmpobj.salesforecast;
                                        sactual = tmpobj.salesactual;
                                        pforecast = tmpobj.productforecast;
                                        pactual = tmpobj.productactual;
                                        //returnHtml = returnHtml + "<td id='tdf_" + id + "'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='a' id=F'" + id + "' value ='" + dtl.ElementAt(c).salesamt + "'></td>";
                                        c = c + 1;
                                    }
                                }
                                saleahtml = saleahtml + "<td id='td_" + id + "' style ='padding:0px;'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' af ='SA' id='SA_" + id + "' value ='" + sactual.ToString() + "'></td>";
                                salefhtml = salefhtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EEE;max-height:24px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='SF' id='SF_" + id + "' value ='" + sforecast.ToString() + "'></td>";

                                if (ctype == 1)
                                {
                                    prodahtml = prodahtml + "<td id='td_" + id + "' style ='padding:0px;'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' af ='PA' id='PA_" + id + "' value ='" + pactual.ToString() + "'></td>";
                                    prodfhtml = prodfhtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control'  style ='background-color:#EEE;max-height:24px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='PF' id='PF_" + id + "' value ='" + pforecast.ToString() + "'></td>";
                                }

                            }
                            returnHtml = returnHtml + saleahtml + prodahtml + "</tr><tr>" + salefhtml + prodfhtml + "</tr>";
                            r = r + 1;
                        }
                        double? subtarget; double? subsales; double? subhgoal; double? subshortage;
                        subtarget = subsales = subhgoal = subshortage = 0;
                        string subcolspan = categories.Count.ToString();
                        subtarget = bsales.Where(x => x.staffid > 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        subsales = sadetails.Where(x => x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //subsales = bsales.Where(x => x.staffid > 0).Sum(x => (double?)x.salesactual) ?? 0d;
                        //subsales = summarydtl.Where(x => x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //subsales = details.Where(x => x.categorytypeid == 1 && x.time == maxtime).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //HninWY - 20150129 - to take from target table
                        //subsales = details.Where(x => x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        subhgoal = details.Where(x => x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        subshortage = subsales - subtarget;
                        if (ctype == 1)
                            subcolspan = (categories.Count * 2).ToString();

                        string subtotalhtml = "<tr style='background-color:#F2E886;' data-sFlag = '0'><td colspan='2' style='text-align:center'>Sub Total</td><td>" + subtarget + "</td><td>" + subsales + "</td><td>" + subhgoal + "</td><td>" + subshortage + "</td><td colspan='" + subcolspan + "'></td>";
                        returnHtml = returnHtml + subtotalhtml + closeHtml;
                    }
                    catch (Exception e)
                    { returnHtml = e.Message.ToString(); }
                }

            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnHtml = "SESSIONEXPIRED";
            }
            //return Content(returnHtml);
            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

        private string branchSaleEntry(int campaignid, string branchcode, int groupid, TimeSpan time, DateTime? cdate)
        {            
            //TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;' id ='tblbranchSales'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";

            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;'>" + category.Category.Category + "</th>";
                    catcount++;
                }

                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).ToList();
                //string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid) && x.createdate == cdate && x.time == time).ToList();
                var branches = db.BranchGroupLinks.Include("Branch").Where(x => x.groupid == groupid).ToList();
                //bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                foreach (var branch in branches)
                {
                    //TimeSpan? maxtime = details.Where(x => x.groupid == groupid && x.categorytypeid == 1 && x.branchcode == branch.branchcode).Max(x => x.time);
                    ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.staffid == 0).ToList();
                    bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.Branch.getBranchName() + "</td>";
                    
                    double hourlyforcest = 0; //details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                    hourlyforcest = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid == 0).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                    double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid).Sum(x => (double?)x.salesforecast) ?? 0d;                    
                    //double? saleActual = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid ==0).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                    //double? saleActual = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid).Sum(x => (double?)x.salesactual) ?? 0d;
                    double? saleActual = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.categoryid == 0 && x.staffid >0 ).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                    string saleForcesthtml = "";
                    saleForcesthtml = "<input type='text' onchange='changeShortage(this);' ch ='0' af ='SF' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SF' value ='" + saleForcest.ToString() + "'>";
                    string saleActualhtml = "";
                    saleActualhtml = "<input type='text' onchange='changeShortage(this);' ch ='0'  af ='SA' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SA' value ='" + saleActual.ToString() + "' disabled>";
                    string hourlyforcesthtml = "";
                    hourlyforcesthtml = "<input type='text' onchange='txtonChanged(this);' ch ='0'  af ='SFH' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SFH' value ='" + hourlyforcest.ToString() + "'>";
                    //tmpsaleshtml = "<input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' id='ST_" + branch.branchcode + "' value ='" + saleForcest.ToString() + "'>";
                    bodyHtml = bodyHtml + "<td rowspan ='2' style='padding:0px;'>" + saleForcesthtml + "</td><td rowspan ='2'  style='padding:0px;'>" + saleActualhtml + "</td><td rowspan ='2'  style='padding:0px;'>" + hourlyforcesthtml + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                    //bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                    dtlcount = dtl.Count;
                    c = 0;
                    tmpHtml = "";
                    foreach (var category in categories)
                    {
                        var id = Convert.ToString(branch.branchcode) + "_" + Convert.ToString(category.campaigncategoryid);
                        double? Aactual; double? Aforecast;
                        Aactual = Aforecast = 0;
                        var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                        if (tmpobj != null)
                        {
                            Aactual = tmpobj.Sum(x => (double?)x.productactual) ?? 0d;
                            Aforecast = tmpobj.Sum(x => (double?)x.productforecast) ?? 0d;
                            c = c + 1;
                        }
                        bodyHtml = bodyHtml + "<td style ='padding:0px;'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' af ='PA' id='" + id + "' value ='" + Aactual.ToString() + "'></td>";
                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE;padding:0px;'><input class='form-control'  style ='background-color:#EEE;max-height:24px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='PF' id='" + id + "' value ='" + Aforecast.ToString() + "'></td>";
                    }
                    bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                    r = r + 1;
                }
                returnHtml = returnHtml + bodyHtml + closeHtml;                
                //returnHtml = returnHtml + closeHtml;
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }

        [HttpPost]
        [Authorize]
        //public ActionResult SaveDetailSalesAmountEntry(int campaignid, string branchcode, int groupid, int t1,int t2, int t3,ICollection<DetailEntry> dtls)
        public JsonResult SaveCampaignBranchSales(ICollection<DetailEntry> dtls, ICollection<campaign_m_branchsalestarget> branchsales)
        {
            var returnStr = "FAIL";

            try
            {
                //int id = 0;
                string cocode = Session["cocode"].ToString();
                if (dtls != null)
                {
                    for (int i = 0; i < dtls.Count; i++)
                    {
                        DetailEntry dtmp = dtls.ElementAt(i);
                        saveToCampaignDetail(dtmp);
                    }
                }
                if (branchsales != null)
                {
                    for (int i = 0; i < branchsales.Count; i++)
                    {
                        saveToCampaignSalesTarget(branchsales.ElementAt(i));
                    }
                }
                
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        //public ActionResult SaveDetailSalesAmountEntry(int campaignid, string branchcode, int groupid, int t1,int t2, int t3,ICollection<DetailEntry> dtls)
        public JsonResult SaveCampaignDetailSales(ICollection<DetailEntry> dtls,ICollection<campaign_m_branchsalestarget> branchsales)
        {
            var returnStr = "FAIL";
            
            try
            {
                //int id = 0;
                string cocode = Session["cocode"].ToString();
                if (dtls != null)
                {
                    for (int i = 0; i < dtls.Count; i++)
                    {
                        DetailEntry dtmp = dtls.ElementAt(i);
                        saveToCampaignDetail(dtmp);
                        //Campaign_m_CampaignDetails cdtltmp = db.CampaignDetails.Where(x => x.campaignid == dtmp.campaignid && x.branchcode == dtmp.branchcode && x.groupid == dtmp.groupid && x.staffid == dtmp.staffid && x.categoryid == dtmp.categoryid && x.categorytypeid == dtmp.categorytype && x.time == dtmp.time && x.createdate == dtmp.cdate && x.cocode == cocode).FirstOrDefault();
                        //Campaign_m_CampaignDetails cdtltmp = db.CampaignDetails.Where(x => x.campaignid == dtls.ElementAt(i).campaignid && x.branchcode == dtls.ElementAt(i).branchcode && x.groupid == dtls.ElementAt(i).groupid && x.staffid == dtls.ElementAt(i).staffid &&c x.categoryid == dtls.ElementAt(i).categoryid && x.categorytypeid == dtls.ElementAt(i).categorytype && x.time == dtls.ElementAt(i).time && x.createdate == dtls.ElementAt(i).cdate && x.cocode == cocode).FirstOrDefault();
                        //Campaign_m_CampaignDetails cdtltmp=null;
                        //if (db.CampaignDetails.LongCount() > 0 )                    
                        //    cdtltmp = db.CampaignDetails.Where(x => x.campaignid == dtls.ElementAt(i).campaignid && x.branchcode == dtls.ElementAt(i).branchcode && x.time == dtls.ElementAt(i).time && x.createdate == dtls.ElementAt(i).cdate && x.cocode == cocode).FirstOrDefault();
                        //using (var context = new BigMacEntities())
                        //{
                        //    var blogNames = context.Database.ExecuteSqlCommand("call CampaignDetailsSave(" + dtls.ElementAt(i).campaignid + ",'" + dtls.ElementAt(i).branchcode + "'," + dtls.ElementAt(i).groupid + ",'" + dtls.ElementAt(i).time + "','" + string.Format("{0:yyyy-MM-dd}", dtls.ElementAt(i).cdate) + "'," + dtls.ElementAt(i).staffid + "," + dtls.ElementAt(i).categoryid + "," + dtls.ElementAt(i).categorytype + "," + dtls.ElementAt(i).actual + "," + dtls.ElementAt(i).forecast + ")");
                        //}
                    }
                }
                if (branchsales != null)
                {
                    for (int i = 0; i < branchsales.Count; i++)
                    {
                        saveToCampaignSalesTarget(branchsales.ElementAt(i));
                    }
                }
                    //saveToCampaignSalesTarget(st);
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        //public ActionResult SaveDetailSalesAmountEntry(int campaignid, string branchcode, int groupid, int t1,int t2, int t3,ICollection<DetailEntry> dtls)
        public JsonResult SaveCampaignDetailActivity(ICollection<DetailEntry> dtls, ICollection<campaign_m_branchactivitytarget> branchactivity, ICollection<campaign_m_branchsalestarget> branchsales)
        {
            var returnStr = "FAIL";

            try
            {   
                if (dtls != null)
                {
                    for (int i = 0; i < dtls.Count; i++)
                    {
                        DetailEntry dtmp = dtls.ElementAt(i);
                        saveToCampaignDetail(dtmp);
                    }
                }
                if (branchactivity != null)
                {
                    for (int i = 0; i < branchactivity.Count; i++)
                    {
                        saveToCampaignActivityTarget(branchactivity.ElementAt(i));
                    }
                }
                if (branchsales != null)
                {
                    for (int i = 0; i < branchsales.Count; i++)
                    {
                        saveToCampaignSalesTarget(branchsales.ElementAt(i));
                    }
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void saveToCampaignDetail(DetailEntry dtmp)
        {
            string cocode = Session["cocode"].ToString();
            Campaign_m_CampaignDetails cdtltmp = db.CampaignDetails.Where(x => x.campaignid == dtmp.campaignid && x.branchcode == dtmp.branchcode && x.groupid == dtmp.groupid && x.staffid == dtmp.staffid && x.categoryid == dtmp.categoryid && x.categorytypeid == dtmp.categorytype && x.time == dtmp.time && x.createdate == dtmp.cdate && x.cocode == cocode).FirstOrDefault();

            if (cdtltmp == null)
            {
                cdtltmp = new Campaign_m_CampaignDetails();
                cdtltmp.branchcode = dtmp.branchcode;
                cdtltmp.createdate = dtmp.cdate;
                cdtltmp.campaignid = dtmp.campaignid;
                cdtltmp.staffid = dtmp.staffid;
                cdtltmp.categoryid = dtmp.categoryid;
                cdtltmp.categorytypeid = dtmp.categorytype;
                cdtltmp.time = dtmp.time;
                cdtltmp.groupid = dtmp.groupid;
                cdtltmp.cocode = Session["cocode"].ToString();
                cdtltmp.userid = Convert.ToInt32(Session["userid"].ToString());
                cdtltmp.lastmodifieddate = DateTime.Now;
                if (dtmp.salesactual < 0)
                    cdtltmp.salesactual = 0;
                else
                    cdtltmp.salesactual = dtmp.salesactual;

                if (dtmp.salesforecast < 0)
                    cdtltmp.salesforecast = 0;
                else
                    cdtltmp.salesforecast = dtmp.salesforecast;

                if (dtmp.productactual < 0)
                    cdtltmp.productactual = 0;
                else
                    cdtltmp.productactual = dtmp.productactual;

                if (dtmp.productforecast < 0)
                    cdtltmp.productforecast = 0;
                else
                    cdtltmp.productforecast = dtmp.productforecast;

                db.CampaignDetails.Add(cdtltmp);
                db.SaveChanges();
            }
            else
            {
                if (dtmp.salesactual >= 0)
                    cdtltmp.salesactual = dtmp.salesactual;

                if (dtmp.salesforecast >= 0)
                    cdtltmp.salesforecast = dtmp.salesforecast;

                if (dtmp.productactual >= 0)
                    cdtltmp.productactual = dtmp.productactual;

                if (dtmp.productforecast >= 0)
                    cdtltmp.productforecast = dtmp.productforecast;

                cdtltmp.userid = Convert.ToInt32(Session["userid"].ToString());
                db.SaveChanges();
            }
        }

        [Authorize]
        public void saveToCampaignActivityTarget(campaign_m_branchactivitytarget objtmp)
        {
            string cocode = Session["cocode"].ToString();
            campaign_m_branchactivitytarget cdtltmp = db.CampaignBranchAcitivityTarget.Where(x => x.campaignid == objtmp.campaignid && x.branchcode == objtmp.branchcode && x.groupid == objtmp.groupid && x.categoryid == objtmp.categoryid && x.categorytypeid == objtmp.categorytypeid && x.createdate == objtmp.createdate && x.cocode == cocode).FirstOrDefault();

            if (cdtltmp == null)
            {
                objtmp.cocode = Session["cocode"].ToString();
                objtmp.lastmodifieddate = DateTime.Now;
                //objtmp.createdate = DateTime.Now;
                objtmp.userid = Convert.ToInt32(Session["userid"]);
                db.CampaignBranchAcitivityTarget.Add(objtmp);
                db.SaveChanges();
            }
            else
            {
                cdtltmp.forecast = objtmp.forecast;
                db.SaveChanges();
            }
        }

        [Authorize]
        public void saveToCampaignSalesTarget(campaign_m_branchsalestarget objtmp)
        {
            string cocode = Session["cocode"].ToString();
            campaign_m_branchsalestarget cdtltmp = db.CampaignBranchSalesTarget.Where(x => x.campaignid == objtmp.campaignid && x.branchcode == objtmp.branchcode && x.groupid == objtmp.groupid  && x.createdate == objtmp.createdate && x.cocode == cocode && x.staffid == objtmp.staffid).FirstOrDefault();

            if (cdtltmp == null)
            {
                objtmp.cocode = Session["cocode"].ToString();
                objtmp.userid = Convert.ToInt32(Session["userid"]);
                objtmp.lastmodifieddate = DateTime.Now;                
                db.CampaignBranchSalesTarget.Add(objtmp);
                db.SaveChanges();
            }
            else
            {
                if (objtmp.salesforecast != -1)
                {
                    cdtltmp.salesforecast = objtmp.salesforecast; 
                }
                if (objtmp.salesactual != -1)
                {
                    cdtltmp.salesactual = objtmp.salesactual;
                }
                //cdtltmp.salesforecast = objtmp.salesforecast;                
                cdtltmp.staffdaytype = objtmp.staffdaytype;
                db.SaveChanges();
            }
        }
        public ActionResult ReportGroupSummary()
        {
            ViewBag.GroupOptions = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).AsEnumerable();
            ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).AsEnumerable();
            ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).AsEnumerable();
            return View();
        }
        
        [Authorize]
        //public ActionResult SaveDetailSalesAmountEntry(int campaignid, string branchcode, int groupid, int t1,int t2, int t3,ICollection<DetailEntry> dtls)
        public JsonResult recalculateSaleFigure(DateTime campaigndate, string cocode="",int gid=0, string bcode ="ALL", int campaignid=0,int staffid =0,string resource ="")
        {
            var returnStr = "FAIL";

            try
            {
                //int id = 0;
                if (campaignid == 0) campaignid = 1;

                    
                var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                int rid = 0;
                int uid = 0;

                string sessionid = "";
                string visitorIPAddress="";
                try
                {
                    sessionid = Session.SessionID;
                    visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (String.IsNullOrEmpty(visitorIPAddress))
                        visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    if (string.IsNullOrEmpty(visitorIPAddress))
                        visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                catch (Exception ee)
                { }


                if (Session["userid"] != null)
                {
                    if (Session["userid"] != "")
                    {
                        uid = Convert.ToInt32(Session["userid"]);

                        if (cocode == "")
                        {
                            cocode = Session["cocode"].ToString();
                        }
                        var gblist = db.BranchGroupLinks.Where(x => (x.groupid == gid || gid == 0) && (x.branchcode == bcode || bcode == "ALL")).ToList();
                        for (int gcount = 0; gcount < gblist.Count; gcount++)
                        {
                            int gidtmp = gblist.ElementAt(gcount).groupid;
                            string bcodetmp = gblist.ElementAt(gcount).branchcode;                            
                            //string ipaddress = "";
                            var context = new BigMacEntities();
                            var value = context.Database.ExecuteSqlCommand("call ReCalculateSalesAcutal('" + cocode + "'," + campaignid.ToString() + "," + gidtmp.ToString() + ",'" + bcodetmp + "'," + staffid.ToString() + "," + uid.ToString() + ",'" + campaigndate.ToString("yyyy-MM-dd") + "','" + visitorIPAddress + "','" + sessionid + "'," + rid + ")");

                        }
                        returnStr = "SUCCESS";
                    }
                }               

            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //public ActionResult SaveDetailSalesAmountEntry(int campaignid, string branchcode, int groupid, int t1,int t2, int t3,ICollection<DetailEntry> dtls)
        public JsonResult SaveCampaignBranchSalesManual(ICollection<DetailEntry> dtls, ICollection<campaign_m_branchsalestarget> branchsales)
        {
            var returnStr = "FAIL";

            try
            {
                //int id = 0;
                string cocode = Session["cocode"].ToString();
                if (dtls != null)
                {
                    for (int i = 0; i < dtls.Count; i++)
                    {
                        DetailEntry dtmp = dtls.ElementAt(i);
                        saveToCampaignDetail(dtmp);
                    }
                }
                if (branchsales != null)
                {
                    for (int i = 0; i < branchsales.Count; i++)
                    {
                        saveToCampaignSalesTarget(branchsales.ElementAt(i));
                    }
                }

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
            
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public string branchSaleEntryManual(int campaignid, string branchcode, int groupid, TimeSpan time, DateTime? cdate)
        {
            //TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;' id ='tblbranchSales'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";

            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;'>" + category.Category.Category + "</th>";
                    catcount++;
                }

                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).ToList();
                //string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid) && x.createdate == cdate && x.time == time).ToList();
                var branches = db.BranchGroupLinks.Include("Branch").Where(x => x.groupid == groupid).ToList();
                //bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                foreach (var branch in branches)
                {
                    //TimeSpan? maxtime = details.Where(x => x.groupid == groupid && x.categorytypeid == 1 && x.branchcode == branch.branchcode).Max(x => x.time);
                    ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.staffid == 0).ToList();
                    bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.Branch.getBranchName() + "</td>";
                    double hourlyforcest = 0; //details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                    hourlyforcest = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid == 0).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                    double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid).Sum(x => (double?)x.salesforecast) ?? 0d;
                    //double? saleActual = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid ==0).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                    //double? saleActual = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid).Sum(x => (double?)x.salesactual) ?? 0d;
                    double? saleActual = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid == 0 && x.categoryid == 0).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                    string saleForcesthtml = "";
                    saleForcesthtml = "<input type='text' onchange='changeShortage(this);' ch ='0' af ='SF' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SF' value ='" + saleForcest.ToString() + "'>";
                    string saleActualhtml = "";
                    saleActualhtml = "<input type='text' onchange='changeShortage(this);' ch ='0'  af ='SA' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SA' value ='" + saleActual.ToString() + "' >";
                    string hourlyforcesthtml = "";
                    hourlyforcesthtml = "<input type='text' onchange='txtonChanged(this);' ch ='0'  af ='SFH' class='form-control' style ='min-height:49px;height:100%;' id='" + branch.branchcode + "_SFH' value ='" + hourlyforcest.ToString() + "'>";
                    //tmpsaleshtml = "<input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' id='ST_" + branch.branchcode + "' value ='" + saleForcest.ToString() + "'>";
                    bodyHtml = bodyHtml + "<td rowspan ='2' style='padding:0px;'>" + saleForcesthtml + "</td><td rowspan ='2'  style='padding:0px;'>" + saleActualhtml + "</td><td rowspan ='2'  style='padding:0px;'>" + hourlyforcesthtml + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                    //bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                    dtlcount = dtl.Count;
                    c = 0;
                    tmpHtml = "";
                    foreach (var category in categories)
                    {
                        var id = Convert.ToString(branch.branchcode) + "_" + Convert.ToString(category.campaigncategoryid);
                        double? Aactual; double? Aforecast;
                        Aactual = Aforecast = 0;
                        var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                        if (tmpobj != null)
                        {
                            Aactual = tmpobj.Sum(x => (double?)x.productactual) ?? 0d;
                            Aforecast = tmpobj.Sum(x => (double?)x.productforecast) ?? 0d;
                            c = c + 1;
                        }
                        bodyHtml = bodyHtml + "<td style ='padding:0px;'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' af ='PA' id='" + id + "' value ='" + Aactual.ToString() + "'></td>";
                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE;padding:0px;'><input class='form-control'  style ='background-color:#EEE;max-height:24px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='PF' id='" + id + "' value ='" + Aforecast.ToString() + "'></td>";
                    }
                    bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                    r = r + 1;
                }
                returnHtml = returnHtml + bodyHtml + closeHtml;
                //returnHtml = returnHtml + closeHtml;
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }
    } 
}
