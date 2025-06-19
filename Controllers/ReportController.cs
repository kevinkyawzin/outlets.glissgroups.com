using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;
using System.Data.Entity;
using RazorPDF;
using MySql.Data.MySqlClient;
using System.Dynamic;
using System.IO;
namespace BigMac.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private BigMacEntities db = new BigMacEntities();

        //public ActionResult RedemptionReceipt1(int id)
        //{
        //    return View();
        //}

        //public ActionResult RedemptionReceipt(int id)
        //{
        //    return View();
        //}

        //public ActionResult WebForm1()
        //{
        //    return View();
        //}
        //Added by Alexis on 26 Feb 2015
        [HttpPost]
        public JsonResult PrintMobile(DateTime df, DateTime dt, string b, string uname = "")
        {
            var returnStr = "FAIL";
            try
            {
                //var cusinv = db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount });
                using (var context = new BigMacEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from rptEncryptValues where uname='" + uname + "' and rptname='NewMember'");
                }
                var cuscol = db.CusSup.Where(x => (x.branchcode == b || b == "ALL") && (x.createdate >= df) && (x.createdate <= dt)).ToList();
                for (int i = 0; i < cuscol.Count; i++)
                {
                    rptEncryptValues te = new rptEncryptValues();
                    te.rptname = "NewMember";
                    te.uname = uname;
                    te.tblid = cuscol.ElementAt(i).id;
                    te.value1 = cAESEncryption.getDecryptedString(cuscol.ElementAt(i).mobile);
                    db.encryptdata.Add(te);
                    db.SaveChanges();
                }
                returnStr = "SUCCESS";

            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 29 Sep 2015
        [HttpPost]
        public ActionResult UploadSignImage(string fileData, string fileName)
        {
            string dataWithoutJpegMarker = fileData.Replace("data:image/jpeg;base64,", String.Empty);
            byte[] filebytes = Convert.FromBase64String(dataWithoutJpegMarker);
            string writePath = Path.Combine(Server.MapPath("~/Reports"), fileName + ".jpg");
            using (FileStream fs = new FileStream(writePath,
                                            FileMode.OpenOrCreate,
                                            FileAccess.Write,
                                            FileShare.None))
            {
                fs.Write(filebytes, 0, filebytes.Length);
            }
            return new EmptyResult();
        }
        //Added by ZawZO on 27 Feb 2015
        [HttpPost]
        public JsonResult PrintMembersListByDOB( string b, string mth,string uname = "")
        {
            var returnStr = "FAIL";
            try
            {
                //var cusinv = db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount });
                using (var context = new BigMacEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from rptEncryptValues where uname='" + uname + "' and rptname='NewMember'");
                }

                

                if (mth=="0")
                {
                    var cuscol = db.CusSup.Where(x => (x.branchcode == b || b == "ALL") ).ToList();
                    for (int i = 0; i < cuscol.Count; i++)
                    {
                        rptEncryptValues te = new rptEncryptValues();
                        te.rptname = "MembersListByDOB";
                        te.uname = uname;
                        te.tblid = cuscol.ElementAt(i).id;
                        te.value1 = cAESEncryption.getDecryptedString(cuscol.ElementAt(i).mobile);
                        db.encryptdata.Add(te);
                        db.SaveChanges();
                    }
                }
                else
                {
                    int intmth = Convert.ToInt32(mth);
                    var cuscol = db.CusSup.Where(x => (x.branchcode == b || b == "ALL") && (x.dateofbirth.Value.Month == intmth)).ToList();
                    for (int i = 0; i < cuscol.Count; i++)
                    {
                        rptEncryptValues te = new rptEncryptValues();
                        te.rptname = "MembersListByDOB";
                        te.uname = uname;
                        te.tblid = cuscol.ElementAt(i).id;
                        te.value1 = cAESEncryption.getDecryptedString(cuscol.ElementAt(i).mobile);
                        db.encryptdata.Add(te);
                        db.SaveChanges();
                    }
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PrintcardNRIC(string uname="")
        {
            var returnStr = "FAIL";
            try
            {
                //var cusinv = db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount });
                using (var context = new BigMacEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from rptEncryptValues where uname='" + uname + "' and rptname='printcard'");                       
                }
                var cuscol = db.MemberCard.Where(x => x.printedbyid == null || x.printeddate == null || x.printedbyid == 0).ToList().Join(db.CusSup, card => card.cussupid, cus => cus.id, (card, cus) => new { MemberCard = card, CusSup = cus }).Select(x => new { x.CusSup.id, x.CusSup.nric }).ToList();
                for (int i = 0; i < cuscol.Count; i++)
                {
                    rptEncryptValues te = new rptEncryptValues();
                    te.rptname = "printcard";
                    te.uname = uname;
                    te.tblid = cuscol.ElementAt(i).id;
                    te.value1 = cAESEncryption.getDecryptedString(cuscol.ElementAt(i).nric);                    
                    db.encryptdata.Add(te);
                    db.SaveChanges();
                }
                returnStr = "SUCCESS";

            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProductListingReport(string rcode)
        {
            var c = ""; var b = "";
            try
            {
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }
            return View();
        }

        public ActionResult VoidTransactionReport(string rcode)
        {
            var c = ""; var b = "";
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();
                
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }

            return View();
        }

        public ActionResult MemberTransactionReport(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                {}
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                ICollection<CusSup_m_CusSup> mlist = db.CusSup.Where(x => x.status == "Active" || x.status == "ACTIVE").ToList();
                ViewBag.MemberOptions = mlist.Select(x => new { x.id, x.cussupname }).OrderBy(x => x.cussupname).ToList();
            }
            catch (Exception e)
            {
            }
            return View();
        }


        public ActionResult ServiceReportByBranch(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }

            return View();
        }

        public ActionResult ServiceCommissionReport(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Common_m_Staff> bcol = db.Staffs.ToList();
                Common_m_Staff btmp = new Common_m_Staff();
                btmp.id = 0;
                btmp.name = "ALL";
                bcol.Add(btmp);
                ViewBag.staffOptions = bcol.OrderBy(x => x.id).ToList();
            }
            catch (Exception e)
            {

            }
            return View();
        }
        //Added by ZawZO on 10 Feb 2016
        public ActionResult CustomerAttendanceList(string rcode)
        {
            try
            {
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();
            }
            catch (Exception e)
            {

            }
            return View();
        }

 

        //Added by ZawZO on 10 Feb 2016
        public ActionResult CustomerTransferList(string rcode)
        {
            try
            {
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();
            }
            catch (Exception e)
            {

            }
            return View();
        }
        //Added by alexis 18072016
        public ActionResult ReportMileage(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }

            return View();
        }

        public ActionResult DailySalesByBranch(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            { 
            }

            return View();
        }
        //Added by ZawZO on 24 Mar 2016
        public ActionResult SalesCommissionByBranch(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }

            return View();
        }
        public ActionResult DailyRedemptionByBranch(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;

                string cocode = Session["cocode"].ToString();
                string userrole = Session["userrole"].ToString();
               
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();

                if(userrole=="PARTNERS OUTLET")
                {
                   bcol = bcol.Where(x => x.cocode == cocode).ToList();
                }
              
               
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                ViewBag.CoCode = cocode;
                ViewBag.Role = userrole;
            }
            catch (Exception e)
            {
            }
            return View();
        }
        //Added by Alexies on 26 Feb 2015
        public ActionResult NewMember(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();


                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            }
            catch (Exception e)
            {
            }

            return View();
        }
        //Added by ZawZO on 26 Feb 2015
        public ActionResult MembersListByDOB(string rcode)
        {
            try
            {
                ViewBag.Rcode = rcode;
                ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
                Configuration_m_Branches btmp = new Configuration_m_Branches();
                foreach (Configuration_m_Branches br in bcol)
                {
                    br.tel = "B";
                }

                btmp.id = 0;
                btmp.tel = "A";
                btmp.branchcode = "ALL";
                bcol.Add(btmp);
                ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();


                var c = ""; var b = "";
                try
                {
                    c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                    b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                }
                catch (Exception e)
                { }
            }
            catch (Exception e)
            {
            }

            return View();
        }
        public ActionResult ReportGroupSummaryToExcel()
        {
            int campaignid = 1;
            string branchcode = "";
            int groupid = 1;
            TimeSpan time = new TimeSpan(10, 0, 0);
            DateTime cdate = new DateTime(2013, 10, 30);
            int ctype = 1;
            ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.Category.Category).ToList();
            //ViewBag.branches = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.groupid == groupid).Include("Branch").Select(x => new { x.branchcode }).Distinct().AsEnumerable();
            //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == 2).Include("Category").ToList();
            ICollection<CampaignBranchSales> details;
            //db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == 2).GroupBy(x => new { x.branchcode, x.categoryid }).Select(x => new {  x.Key.branchcode, x.Key.categoryid, actual = x.Sum(b => b.actual), forecast = x.Sum(b => b.forecast)}).
            ICollection<Campaign_m_CampaignDetails> Daydetails = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1).ToList();
            ICollection<Configuration_m_Branches> branches;
            using (var context = new BigMacEntities())
            {

                branches = context.Database.SqlQuery<Configuration_m_Branches>("call getCampaignBranches (" + campaignid + "," + groupid + ")").ToList();
                details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales (" + campaignid + "," + groupid + ",'" + time + "','2013-10-30'," + 2 + ")").ToList();    
            }
            //return flag;
            ViewBag.categories = categories;
            ViewBag.branches = branches;
            ViewBag.details = details;
            ViewBag.Daydetails = Daydetails;
            //return new RazorPDF.PdfResult(tmp, "ReportGroupSummary");            
            Response.AddHeader("Content-Disposition", "filename=thefilename.xls");
            Response.ContentType = "application/vnd.ms-excel";
            return View();
        }

        public ActionResult ReportGroupSummaryPDF()
        {
            var pdf = new PdfResult(null, "ReportGroupSummaryPDF");
            var tmp = new DetailEntry();
            int campaignid = 1;
            string branchcode = "";
            int groupid = 1;
            TimeSpan time = new TimeSpan(10, 0, 0);
            DateTime cdate = new DateTime(2013, 10, 30);
            int ctype = 1;
            ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == ctype).Include("Category").OrderBy(x => x.Category.Category).ToList();
            ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
            ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
            ICollection<Campaign_m_CampaignDetails> Daydetails = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
            pdf.ViewBag.categories = categories;
            pdf.ViewBag.staffs = staffs;
            pdf.ViewBag.details = details;
            pdf.ViewBag.Daydetails = Daydetails;
            //return new RazorPDF.PdfResult(tmp, "ReportGroupSummary");
            return pdf;
            //return View();
        }
        //public ActionResult ReportGroupSummary()
        //{
        //    ViewBag.GroupOptions = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).AsEnumerable();
        //    ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).AsEnumerable();
        //    ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).AsEnumerable();
        //    return View();
        //}
        public ActionResult TopUpPrint(int id =0)
        {
            try
            {
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;

                Invoice_m_Invoice inv = db.sales.Where(x => x.id == id).Include("Create").FirstOrDefault();
                inv.items = db.saleItems.Where(x => x.invoiceid == id).ToList();
                return View(inv);

            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult RedeemPrint(int id = 0)
        {
            try
            {
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;

                Invoice_m_Invoice inv = db.sales.Where(x => x.id == id).Include("Create").FirstOrDefault();
                inv.items = db.saleItems.Where(x => x.invoiceid == id).ToList();
                return View(inv);

            }
            catch (Exception e)
            {
                return View();
            }
        }

        //public ActionResult getCampaignDetailGrid(int campaignid, string branchcode, int groupid, TimeSpan time, DateTime cdate, int ctype)
        //{
        //    var returnHtml = "<table class='table table-striped table-bordered bootstrap-datatable datatable'><thead>";
        //    var closeHtml = "</tbody></table>";
        //    int r = 0; int c = 0; int dtlcount = 0;
        //    var tmpHtml = "";
        //    //TimeSpan time = new TimeSpan();

        //    try
        //    {
        //        ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == ctype).Include("Category").OrderBy(x => x.Category.Category).ToList();
        //        ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
        //        ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
        //        returnHtml = returnHtml + "<tr><th style='width:100px;'>Staff Name</th>";
        //        returnHtml = returnHtml + "<th style='width:100px;'>Daily Sales<br/>Target</th>";
        //        returnHtml = returnHtml + "<th style='width:100px;'>Sales<br/>Till Now</th>";
        //        returnHtml = returnHtml + "<th style='width:100px;'>Hourly Goals</th>";
        //        returnHtml = returnHtml + "<th style='width:100px;'>Shortage</th>";

        //        foreach (var category in categories)
        //        {
        //            returnHtml = returnHtml + "<th >" + category.Category.Category + "</th>";
        //        }
        //        returnHtml = returnHtml + "</tr></thead><tbody>";

        //        foreach (var staff in staffs)
        //        {
        //            returnHtml = returnHtml + "<tr id='" + staff.staffid + "'>";
        //            returnHtml = returnHtml + "<td rowspan='2'>" + staff.Staff.name + "</td>";
        //            ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.staffid == staff.staffid).OrderBy(x => x.Category.Category).ToList();

        //            //details.Where(x=>x.staffid == staff.staffid).Sum(x=>x.actual)
        //            double hourlyforcest = details.Where(x => x.staffid == staff.staffid).Sum(x => (double?)x.forecast) ?? 0d;
        //            double saleForcest = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.staffid == staff.staffid && x.createdate == cdate && x.categorytypeid == ctype).Sum(x => (double?)x.forecast) ?? 0d;
        //            double saleActual = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.staffid == staff.staffid && x.createdate == cdate && x.categorytypeid == ctype).Sum(x => (double?)x.actual) ?? 0d;

        //            returnHtml = returnHtml + "<td >" + saleForcest + "</td>";
        //            returnHtml = returnHtml + "<td >" + saleActual + "</td>";
        //            returnHtml = returnHtml + "<td >" + hourlyforcest + "</td>";
        //            returnHtml = returnHtml + "<td >" + (saleForcest - saleActual) + "</td>";

        //            dtlcount = dtl.Count;
        //            c = 0;
        //            tmpHtml = "";

        //            foreach (var category in categories)
        //            {
        //                var id = Convert.ToString(staff.staffid) + "_" + Convert.ToString(category.campaigncategoryid);
        //                if (c < dtl.Count)
        //                {
        //                    if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
        //                    {
        //                        //returnHtml = returnHtml + "<td id='td_" + id + "'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='A' id='" + id + "' value ='" + dtl.ElementAt(c).actual + "'><br/><input style ='background-color:#EED8EE' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='F' id='F" + id + "' value ='" + dtl.ElementAt(c).forecast + "'></td>";                              
        //                        returnHtml = returnHtml + "<td id='td_" + id + "' style ='padding:0px;'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='padding-top:0px;padding-right:0px;max-height:20px;' af ='A' id='" + id + "' value ='" + dtl.ElementAt(c).actual + "'></td>";
        //                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' style ='padding-top:0px;padding-right:0px;max-height:20px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='" + dtl.ElementAt(c).forecast + "'></td>";
        //                        //returnHtml = returnHtml + "<td id='tdf_" + id + "'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='a' id=F'" + id + "' value ='" + dtl.ElementAt(c).salesamt + "'></td>";
        //                        c = c + 1;
        //                    }
        //                    else
        //                    {
        //                        //returnHtml = returnHtml + "<td id='td_" + id + "'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='A' id='" + id + "' value ='0'><br/><input style ='background-color:#EED8EE' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='F' id='F" + id + "' value ='0'></td>";
        //                        returnHtml = returnHtml + "<td id='td_" + id + "' style ='padding:0px;'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='padding-top:0px;padding-right:0px;max-height:20px;' af ='A' id='" + id + "' value ='0'></td>";
        //                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;max-height:20px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
        //                    }
        //                }
        //                else
        //                {
        //                    //returnHtml = returnHtml + "<td id='td_" + id + "'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='A' id='" + id + "' value ='0'><br/><input style ='background-color:#EED8EE' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' af ='F' id='F" + id + "' value ='0'></td>";
        //                    returnHtml = returnHtml + "<td id='td_" + id + "' style ='padding:0px;'><input style ='background-color:#4EE7FF' type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='padding-top:0px;padding-right:0px;max-height:20px;' af ='A' id='" + id + "' value ='0'></td>";
        //                    tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;max-height:20px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
        //                }

        //            }
        //            //returnHtml = returnHtml + "</tr>";
        //            returnHtml = returnHtml + "</tr>";
        //            returnHtml = returnHtml + "<tr>" + tmpHtml + "</tr>";
        //            r = r + 1;
        //        }
        //        returnHtml = returnHtml + closeHtml;
        //    }
        //    catch (Exception e)
        //    { returnHtml = e.Message.ToString(); }

        //    return Content(returnHtml);
        //}

        [Authorize]
        public ActionResult BranchSalesReport(string rcode ="")
        {
            ViewBag.rcode = rcode;
            var glist = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
            var result = new List<SelectListItem>();
            foreach (var itm in glist)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = itm.group.ToString();
                selectItem.Value = itm.groupid.ToString();
                result.Add(selectItem);
            }
            var tmpitem = new SelectListItem();
            tmpitem.Text = "-ALL-";
            tmpitem.Value = "0";
            result.Insert(0, tmpitem);
            ViewBag.GroupOptions = result;
            ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).ToList();
            //ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            var tlist = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            var tresult = new List<SelectListItem>();
            foreach (var itm in tlist)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = itm.timevalue.ToString();
                selectItem.Value = itm.timevalue.ToString();
                tresult.Add(selectItem);
            }
            var tmptitem = new SelectListItem();
            tmptitem.Text = "-ALL-";
            tmptitem.Value = "ALL";
            tresult.Insert(0, tmptitem);
            ViewBag.TimeOptions = tresult;
            return View();
            //dynamic tmpitem = new ExpandoObject();
            //tmpitem.groupid = 0;
            //tmpitem.group = "-All-";
            //glist.Insert(0, new { groupid = 0, Group.group = "-All-" }); 
            //ViewBag.GroupOptions = glist;
        }



        [Authorize]
        [HttpPost]
        public ActionResult getBranchSalesReport(int campaignid, int groupid, string t, DateTime cdate)
        {
            //string returnHtml = "";
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;' id ='tblbranchSales'><thead>";
            try
            {
                //TimeSpan time = new TimeSpan();                
                var headerHtml = "";
                var bodyHtml = "";
                var closeHtml = "</tbody></table>";
                int r = 0; int c = 0; int dtlcount = 0;
                int catcount = 0;
                TimeSpan time = new TimeSpan();
                var tmpHtml = "";
                if (t != "ALL")
                    time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));

                string cocode = Session["cocode"].ToString();
                try
                {
                    ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                    headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:120px;'>Sales Target</th><th style='width:120px;'>Sales Total</th><th style='width:120px;'>Hourly Goals</th><th style='width:120px;'>Shortage</th>";

                    foreach (var category in categories)
                    {
                        headerHtml = headerHtml + "<th style='text-align:center;width:70px;'>" + category.Category.Category + "</th>";
                        catcount++;
                    }

                    returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                    ICollection<Campaign_m_CampaignDetails> details;
                    ICollection<Campaign_m_CampaignDetails> sadetails;
                    ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).ToList();
                    //string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                    //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid) && x.createdate == cdate && x.time == time && x.staffid == 0).ToList();
                    using (var context = new BigMacEntities())
                    {
                        string strsql="";
                        if (t == "ALL")
                        {
                            strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                            details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                        }
                        else
                        {
                            strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2 and time ='" + time.ToString() + "'";
                            details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                        }
                        //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time && x.staffid == 0).ToList();
                        strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  and categorytypeid =1 ";
                        sadetails = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }

                    var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                    foreach (var g in groups)
                    {
                        var branches = db.BranchGroupLinks.Include("Branch").Where(x => x.groupid == g.groupid).ToList();
                        bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                        foreach (var branch in branches)
                        {                            
                            ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.staffid == 0).ToList();
                            bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.Branch.getBranchName() + "</td>";
                            double hourlyforcest = 0; //details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            //hourlyforcest = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid == 0).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            TimeSpan? timetmp = details.Max(x=> x.time);
                            hourlyforcest = sadetails.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.staffid == 0 && x.time == timetmp).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == g.groupid).Sum(x => (double?)x.salesforecast) ?? 0d;                            
                            //double? saleActual = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == g.groupid).Sum(x => (double?)x.salesactual) ?? 0d;
                            var pstmp = db.ConfigDefault.Where(x => x.key == "POSSALE").FirstOrDefault();
                            int psFlag = 0;
                            if (pstmp != null)
                            {
                                psFlag = Convert.ToInt32(pstmp.value);
                            }
                            double? saleActual = 0;
                            if (psFlag ==1)
                                saleActual = sadetails.Where(x => x.branchcode == branch.branchcode && x.groupid == g.groupid && x.categoryid == 0 && x.staffid > 0).Sum(x => (double?)x.salesactual) ?? 0d;
                            else
                                saleActual = sadetails.Where(x => x.branchcode == branch.branchcode && x.groupid == g.groupid && x.categoryid == 0 && x.staffid == 0 && x.time == timetmp).Sum(x => (double?)x.salesactual) ?? 0d;
                            
                            bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
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
                                //bodyHtml = bodyHtml + "<td style ='padding:0px;'><input type='text' onchange='txtonChanged(this);' ch ='0' class='form-control' style ='max-height:24px;' af ='PA' id='" + id + "' value ='" + Aactual.ToString() + "'></td>";
                                //tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE;padding:0px;'><input class='form-control'  style ='background-color:#EEE;max-height:24px;' type='text' onchange='txtonChanged(this);' ch ='0' af ='PF' id='" + id + "' value ='" + Aforecast.ToString() + "'></td>";
                                bodyHtml = bodyHtml + "<td style ='padding:0px;'>" + Aactual.ToString() + "</td>";
                                tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE;padding:0px;'>" + Aforecast.ToString() + "</td>";
                            }
                            bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                            r = r + 1;
                        }

                        returnHtml = returnHtml + bodyHtml;
                    }

                    returnHtml = returnHtml + closeHtml;
                    //returnHtml = returnHtml + closeHtml;
                }
                catch (Exception e)
                { returnHtml = e.Message.ToString(); }
                //return returnHtml;
            }
            catch (Exception e)
            {
            }
            return Content(returnHtml);
        }


        [Authorize]
        [HttpPost]
        public ActionResult getGroupSalesReport(int campaignid, int groupid, string t, DateTime cdate)
        {
            string returnHtml = "";
            try
            {
                TimeSpan time = new TimeSpan(); int c = 0;
                returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;'><thead>";
                var headerHtml = "";
                var bodyHtml = "";
                var closeHtml = "</tbody></table>";
                //int r = 0;  int dtlcount = 0;
                int catcount = 0;                
                var tmpHtml = "";
                double? totalst; double? totalsale; double? totalhg; double? totalsh;
                totalst = totalsale = totalhg = totalsh = 0;

                if (t != "ALL")
                    time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));
                string cocode = Session["cocode"].ToString();
                try
                {

                    ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                    headerHtml = "<tr style ='background-color:#F2E886;' ><th style='width:150px;'>Group/Outlet</th><th style='width:120px;'>Sales Target</th><th style='width:120px;'>Sales Total</th><th style='width:120px;'>Hourly Goals</th><th style='width:120px;'>Shortage</th>";

                    foreach (var category in categories)
                    {
                        headerHtml = headerHtml + "<th style='text-align:center;width:70px;'>" + category.Category.Category + "</th>";
                        catcount++;
                    }
                    returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";
                    var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();                    
                    ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).ToList();
                    ICollection<Campaign_m_CampaignDetails> details;
                    ICollection<Campaign_m_CampaignDetails> sadetails;
                    string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                    //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate).ToList();
                    using (var context = new BigMacEntities())
                    {
                        string strsql = "";
                        if (t == "ALL")
                        {
                            strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                            details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                        }
                        else
                        {
                            strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2 and time ='" + time.ToString() + "'";
                            details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                        }                        
                        strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0)  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  and categorytypeid =1 ";
                        sadetails = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }

                    foreach (var g in groups)
                    {
                        groupid = g.groupid;
                        bodyHtml = "<tr><td rowspan ='2' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td>";

                        ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.categorytypeid == 2 && x.groupid == groupid).ToList();
                        TimeSpan? timetmp = details.Max(x => x.time);
                        double? hourlyforcest = 0;
                        double? saleForcest = 0;
                        double? saleActual = 0;
                        saleForcest = bsales.Where(x => x.groupid == groupid).Sum(x => (double?)x.salesforecast) ?? 0d;
                        //saleActual = bsales.Where(x => x.groupid == groupid).Sum(x => (double?)x.salesactual) ?? 0d;
                        //saleActual = details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                        var pstmp = db.ConfigDefault.Where(x => x.key == "POSSALE").FirstOrDefault();
                        int psFlag = 0;
                        if (pstmp != null)
                        {
                            psFlag = Convert.ToInt32(pstmp.value);
                        }                                     
                        if (psFlag == 1)
                            saleActual = sadetails.Where(x => x.groupid == g.groupid && x.categoryid == 0 && x.staffid > 0).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                        else
                            saleActual = sadetails.Where(x => x.groupid == g.groupid && x.categoryid == 0 && x.staffid == 0 & x.time == timetmp).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                            
                        //saleActual = sadetails.Where(x => x.groupid == g.groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                        hourlyforcest = sadetails.Where(x => x.groupid == g.groupid && x.categorytypeid == 1 && x.staffid == 0 && x.time == timetmp).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        //hourlyforcest = details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;                        
                        totalst += saleForcest;
                        totalsale += saleActual;
                        totalhg += hourlyforcest;
                        bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2'>" + hourlyforcest + "</td><td rowspan ='2'>" + (saleActual - saleForcest) + "</td>";
                        //dtlcount = dtl.Count;
                        c = 0;
                        tmpHtml = "";
                        foreach (var category in categories)
                        {
                            var id = Convert.ToString(groupid) + "_" + Convert.ToString(category.campaigncategoryid);
                            var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                            double sforecast = 0; double sactual = 0;
                            if (tmpobj != null)
                            {
                                sactual = tmpobj.Sum(x => (double?)x.productactual) ?? 0d;
                                sforecast = tmpobj.Sum(x => (double?)x.productforecast) ?? 0d;
                                c = c + 1;
                            }
                            bodyHtml = bodyHtml + "<td >" + sactual.ToString() + "</td>";
                            tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + sforecast.ToString() + "</td>";

                        }
                        //returnHtml = returnHtml + "</tr>";
                        bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                        returnHtml = returnHtml + bodyHtml;
                        //r = r + 1;
                    }

                    string cattotalhtml = "";
                    foreach (var category in categories)
                    {
                        double? totalcat = details.Where(x => x.categorytypeid == 2 && x.categoryid == category.campaigncategoryid).Sum(x => (double?)x.productactual) ?? 0d;
                        cattotalhtml = cattotalhtml + "<td>" + totalcat + "</td>";
                    }
                    totalsh = totalsale - totalst;
                    string gtotalhtml = "<tr style='background-color:#F2E886;'><td style='text-align:center;'>Total</td><td>" + totalst + "</td><td>" + totalsale + "</td><td>" + totalhg + "</td><td>" + totalsh + "</td>" + cattotalhtml + "</tr>";
                    returnHtml = returnHtml + gtotalhtml + closeHtml;
                }
                catch (Exception e)
                { returnHtml = e.Message.ToString(); }

                //return returnHtml;
                
            }
            catch (Exception e)
            {
            }
            return Content(returnHtml);
        }

        [Authorize]
        public ActionResult ReportGroupSummary(string rcode ="")
        {
            ViewBag.rcode = rcode;
            var glist = db.BranchGroupLinks.Include("Group").Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
            var result = new List<SelectListItem>();
            foreach (var itm in glist)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = itm.group.ToString();
                selectItem.Value = itm.groupid.ToString();
                result.Add(selectItem);
            }
            var tmpitem = new SelectListItem();
            tmpitem.Text = "-ALL-";
            tmpitem.Value = "0";
            result.Insert(0, tmpitem);
            ViewBag.GroupOptions = result;
            ViewBag.CampaignOptions = db.Campaigns.OrderBy(x => x.gamedesc).ToList();
            //ViewBag.TimeOptions = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            var tlist = db.CampaignTimes.OrderBy(x => x.timevalue).ToList();
            var tresult = new List<SelectListItem>();
            foreach (var itm in tlist)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = itm.timevalue.ToString();
                selectItem.Value = itm.timevalue.ToString();
                tresult.Add(selectItem);
            }
            var tmptitem = new SelectListItem();
            tmptitem.Text = "-ALL-";
            tmptitem.Value = "ALL";
            tresult.Insert(0, tmptitem);
            ViewBag.TimeOptions = tresult;
            return View();
            //dynamic tmpitem = new ExpandoObject();
            //tmpitem.groupid = 0;
            //tmpitem.group = "-All-";
            //glist.Insert(0, new { groupid = 0, Group.group = "-All-" }); 
            //ViewBag.GroupOptions = glist;
        }

        [Authorize]
        [HttpPost]
        public ActionResult getGroupSalesReportGrid(int campaignid, int groupid, string time, DateTime cdate)
        {
            string returnHtml = "";
            try
            {
                string aflag ="0";
                var tmpobj= db.ConfigDefault.Where(x=>x.key=="ACMVALUE").FirstOrDefault();
                if (tmpobj!=null)
                    aflag = tmpobj.value;
                 if (aflag == "1") 
                    returnHtml = getGroupSalesReportGridAccumulative(campaignid, groupid, time, cdate);
                
            }
            catch (Exception e)
            { 
            }
            return Content(returnHtml);
        }


        public string getGroupSalesReportGridAccumulative(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan  time=new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; //int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            double? totalst; double? totalsale; double? totalhg; double? totalsh;
            totalst = totalsale = totalhg = totalsh = 0;

            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0,2)),Convert.ToInt16(t.Substring(3,2)),Convert.ToInt16(t.Substring(6,2)));
            string cocode = Session["cocode"].ToString();
            try
            {

                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                headerHtml = "<tr style ='background-color:#F2E886;' ><th style='width:150px;'>Group/Outlet</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;width:70px;'>" + category.Category.Category + "</th>";
                    catcount++;
                }                
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";
                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                //ICollection<Configuration_m_Branches> branches;
                ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode ).ToList();
                //ICollection<CampaignBranchSales> details;
                //ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<Campaign_m_CampaignDetails> sadetails;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate).ToList();
                using (var context = new BigMacEntities())
                {
                    string strsql = "";
                    if (t == "ALL")
                    {
                        strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }
                    else
                    {
                        strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  and time ='" + time.ToString() + "'";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }
                    //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time && x.staffid == 0).ToList();
                    strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  and categorytypeid =1 ";
                    sadetails = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();

                    //string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  GROUP BY cocode, campaignid, createdate, branchcode, groupid, categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                    ////summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    ////branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    //if (t == "ALL")
                    //    details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    //else
                    //    details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }

                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    bodyHtml = "<tr><td rowspan ='2' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td>";

                    ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.categorytypeid == 2 && x.groupid == groupid).ToList();
                    double? hourlyforcest = 0;
                    double? saleForcest = 0;
                    double? saleActual = 0;

                    saleForcest = bsales.Where(x => x.groupid == groupid && x.staffid == 0).Sum(x => (double?)x.salesforecast ) ?? 0d;
                    saleActual = sadetails.Where(x => x.groupid == g.groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                    //saleActual = bsales.Where(x => x.groupid == groupid && x.staffid > 0).Sum(x => (double?)x.salesactual) ?? 0d;
                    //commented by HninwY on 20150129 - change with target table
                    //saleActual = details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                    if (t != "ALL")
                    {
                        hourlyforcest = details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                    }
                    totalst += saleForcest;
                    totalsale += saleActual;
                    totalhg += hourlyforcest;
                    bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2'>" + hourlyforcest + "</td><td rowspan ='2'>" + (saleActual - saleForcest) + "</td>";
                    //dtlcount = dtl.Count;
                    c = 0;
                    tmpHtml = "";
                    foreach (var category in categories)
                    {                        
                        var id = Convert.ToString(groupid) + "_" + Convert.ToString(category.campaigncategoryid);
                        var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();                        
                        double sforecast = 0; double sactual = 0;
                        if (tmpobj != null)
                        {
                            sactual = tmpobj.Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            sforecast = tmpobj.Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            c = c + 1;
                        }
                        bodyHtml = bodyHtml + "<td >" + sactual.ToString() + "</td>";
                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + sforecast.ToString() + "</td>";

                    }
                    //returnHtml = returnHtml + "</tr>";
                    bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                    returnHtml = returnHtml + bodyHtml;
                    //r = r + 1;
                }                
                
                string cattotalhtml = "";
                foreach (var category in categories)
                {
                    double? totalcat =  details.Where(x => x.categorytypeid == 2 && x.categoryid == category.campaigncategoryid).Sum(x => (double?)x.salesactual) ?? 0d;
                    cattotalhtml = cattotalhtml + "<td>" + totalcat + "</td>";
                }
                totalsh = totalsale - totalst;
                string gtotalhtml = "<tr style='background-color:#F2E886;'><td style='text-align:center;'>Total</td><td>" + totalst + "</td><td>" + totalsale + "</td><td>" + totalhg + "</td><td>" + totalsh + "</td>" + cattotalhtml  + "</tr>";
                returnHtml = returnHtml + gtotalhtml + closeHtml;
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
            //return Content(returnHtml);
        }



        [Authorize]
        [HttpPost]
        public ActionResult getBranchSalesReportGrid(int campaignid, int groupid, string time, DateTime cdate)
        {
            string returnHtml = "";
            try
            {
                string aflag = "0";
                var tmpobj = db.ConfigDefault.Where(x => x.key == "ACMVALUE").FirstOrDefault();
                if (tmpobj != null)
                    aflag = tmpobj.value;
                if (aflag == "1")
                    returnHtml = getBranchSalesReportGridAccumulative(campaignid, groupid, time, cdate);

            }
            catch (Exception e)
            {
            }
            return Content(returnHtml);
        }

        public string getBranchSalesReportGridAccumulative(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));

            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;width:70px;'>" + category.Category.Category + "</th>";
                    catcount++;
                }
                
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x=>x.groupid == groupid || groupid == 0).Select(x=>new {x.groupid,x.Group.group}).Distinct().OrderBy(x=>x.group).ToList();
                ICollection<Configuration_m_Branches> branches;                
                //ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<Campaign_m_CampaignDetails> sadetails;
                ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode ).ToList();
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate).ToList();
                using (var context = new BigMacEntities())
                {
                    string strsql = "";
                    if (t == "ALL")
                    {
                        strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }
                    else
                    {
                        strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2 and time ='" + time.ToString() + "'";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }                    
                    strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  and categorytypeid =1 ";
                    sadetails = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();

                    ////string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 1 GROUP BY cocode, campaignid, createdate, branchcode, groupid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.maxtimeid = t2.time";
                    //string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time";
                    ////summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    ////branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    //if (t == "ALL")
                    //    details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    //else
                    //    details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                        //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate).ToList(); 
                    }
                    
                    bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                    foreach (var branch in branches)
                    {                       
                        //TimeSpan? maxtime = details.Where(x => x.groupid == groupid && x.categorytypeid == 1 && x.branchcode == branch.branchcode).Max(x => x.time);
                        bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.getBranchName() + "</td>";
                        ICollection<Campaign_m_CampaignDetails> dtl=details.Where(x => x.groupid == groupid &&  x.branchcode == branch.branchcode && x.categorytypeid == 2).ToList();
                        double hourlyforcest = 0; //details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        if (t != "ALL")
                            hourlyforcest = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        double? saleActual = sadetails.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //double saleActual = bsales.Where(x => x.branchcode == branch.branchcode && x.groupid == groupid && x.staffid > 0).Sum(x => (double?)x.salesactual) ?? 0d;
                        //double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate &&  x.cocode== cocode && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        //commented by HninWY on 20150129 - take from target table
                        //double? saleActual = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                        dtlcount = dtl.Count;
                        c = 0;
                        tmpHtml = "";
                        foreach (var category in categories)
                        {                            
                            var id = Convert.ToString(branch.id) + "_" + Convert.ToString(category.campaigncategoryid);
                            double? Aactual; double? Aforecast;
                            Aactual = Aforecast = 0;
                            //if (c < dtl.Count)
                            //{
                                var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                                //if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
                                if (tmpobj != null)
                                {
                                    Aactual = tmpobj.Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                                    Aforecast = tmpobj.Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                                    c = c + 1;
                                }
                                bodyHtml = bodyHtml + "<td style =''>" + Aactual +"</td>";
                                tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + Aforecast + "</td>";

                        }
                        //returnHtml = returnHtml + "</tr>";
                        bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                        r = r + 1;
                    }
                    returnHtml = returnHtml + bodyHtml;
                }
                returnHtml = returnHtml + closeHtml;
                
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }

        [Authorize]
        [HttpPost]
        public ActionResult getBranchStaffReportGrid(int campaignid, int groupid, string time, DateTime cdate)
        {
            string returnHtml = "";
            try
            {
                string aflag = "0";
                var tmpobj = db.ConfigDefault.Where(x => x.key == "ACMVALUE").FirstOrDefault();
                if (tmpobj != null)
                    aflag = tmpobj.value;
                if (aflag == "1")
                    returnHtml = getBranchStaffReportGridAccumulative(campaignid, groupid, time, cdate);

            }
            catch (Exception e)
            {
            }
            return Content(returnHtml);
        }

        
        public string getBranchStaffReportGridAccumulative(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:small;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));
            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Staff</th><th style='max-width:100px;' >Attendance</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;width:70px;'>" + category.Category.Category + "</th>";
                    catcount++;
                }
                //headerHtml = headerHtml + "</tr>";
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                ICollection<Configuration_m_Branches> branches;
                ICollection<campaign_m_branchsalestarget> bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode).ToList();
                //ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<Campaign_m_CampaignDetails> sadetails;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                using (var context = new BigMacEntities())
                {
                    string strsql = "";
                    if (t == "ALL")
                    {
                        strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid,categorytypeid,  MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time ";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }
                    else
                    {
                        strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' and categorytypeid = 2 and time ='" + time.ToString() + "'";
                        details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    }                    
                    strsql = "SELECT * FROM campaign_m_campaigndetails where campaignid = " + campaignid + " and (groupid = " + groupid + " or " + groupid + " = 0) and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  and categorytypeid =1 ";
                    sadetails = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();

                    //string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, categorytypeid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "'  GROUP BY cocode, campaignid, createdate, branchcode, groupid,categorytypeid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.categorytypeid = t2.categorytypeid and t1.maxtimeid = t2.time";
                    ////summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    ////branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    //if (t == "ALL")
                    //    details = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    //else
                    //    details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {                        
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    }
                    bodyHtml = "<tr ><td colspan ='" + ((categories.Count *2)+6) + "' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td></tr>";
                    foreach (var branch in branches)
                    {
                        //returnHtml = returnHtml + "<tr>";
                        bodyHtml = bodyHtml + "<tr ><td colspan ='" + ((categories.Count * 2) + 6) + "' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left'>" + branch.getBranchName() + "</td></tr>";
                        ICollection<Campaign_m_Attendance_Staff> slist = staffs.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode).ToList();
                        foreach (var staff in slist)
                        {                                                       
                            ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.staffid == staff.staffid).ToList();
                            double hourlyforcest =0;
                            if (t != "ALL")
                                hourlyforcest = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.categorytypeid == 1).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode).Sum(x => (double?)x.salesforecast) ?? 0d;
                            double? saleActual = sadetails.Where(x => x.branchcode == branch.branchcode && x.groupid == g.groupid && x.staffid == staff.staffid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;        
                            //double saleActual = bsales.Where(x => x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode).Sum(x => (double?)x.salesactual) ?? 0d;
                            //double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode ).Sum(x => (double?)x.salesforecast) ?? 0d;
                            //commented by HninWY on 20150129 
                            //double? saleActual = details.Where(x => x.groupid == g.groupid && x.categorytypeid == 1 && x.staffid == staff.staffid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;                            
                            var dobj = bsales.Where(x=> x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid).FirstOrDefault();
                            string dopt = "Attend";
                            if (dobj != null)
                                dopt = dobj.staffdaytype;
                            bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='padding:0 0 0 5px;text-align:left'>" + staff.Staff.name + "</td><td rowspan ='2' style='width:100px;text-align:center;'>" + dopt + "</td>"; 
                            //bodyHtml = bodyHtml + "<td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleForcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleActual + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + hourlyforcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + (saleForcest - saleActual) + "</td>";
                            bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                            dtlcount = dtl.Count;
                            c = 0;
                            tmpHtml = "";
                            foreach (var category in categories)
                            {                                
                                var id = Convert.ToString(branch.id) + "_" + Convert.ToString(category.campaigncategoryid);
                                double? Aactual; double? Aforecast;
                                Aactual = Aforecast = 0;
                                var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).FirstOrDefault();                                
                                if (tmpobj != null)
                                {
                                    //Aactual = tmpobj.salesactual;
                                    //Aforecast = tmpobj.salesforecast;
                                    Aactual = tmpobj.productactual;
                                    Aforecast = tmpobj.productforecast;
                                    c = c + 1;
                                }
                                bodyHtml = bodyHtml + "<td >" + Aactual + "</td>";
                                tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + Aforecast + "</td>";
                            }                            
                            bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                            r = r + 1;
                        }

                    }
                    returnHtml = returnHtml + bodyHtml;
                }
                returnHtml = returnHtml + closeHtml;

            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }

        [Authorize]
        [HttpPost]
        public string getGroupSalesReportGridAccumulative_old(int campaignid, int groupid, TimeSpan time, DateTime cdate)
        {
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:smaller;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            double? totalst; double? totalsale; double? totalhg; double? totalsh;
            totalst = totalsale = totalhg = totalsh = 0;

            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;' ><th style='width:150px;'>Group/Outlet</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th>" + category.Category.Category + "</th>";
                    catcount++;
                }
                //headerHtml = headerHtml + "</tr>";
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                ICollection<Configuration_m_Branches> branches;
                //ICollection<CampaignBranchSales> details;
                ICollection<Campaign_m_CampaignDetails> details;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                        //branches = context.Database.SqlQuery<Configuration_m_Branches>("call getCampaignBranches (" + campaignid + "," + groupid + ")").ToList();
                        //details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales (" + campaignid + "," + groupid + ",'" + time + "','2013-10-30'," + 2 + ")").ToList();
                        //details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales(" + campaignid + "," + groupid + ",'" + time + "','" + cd + "'," + 2 + ")").ToList();
                        details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate).ToList();
                    }
                    bodyHtml = "<tr><td rowspan ='2' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td>";
                    //bodyHtml = "<tr><td colspan ='50'>" + g.group + "</td></tr>";
                    ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.categorytypeid == 2 && x.time == time).ToList();
                    double? hourlyforcest = 0;
                    double? saleForcest = 0;
                    double? saleActual = 0;

                    foreach (var branch in branches)
                    {
                        //returnHtml = returnHtml + "<tr>";
                        //bodyHtml = bodyHtml + "<tr><td rowspan ='2'>" + branch.getBranchName() + "</td>";
                        //ICollection<CampaignBranchSales> dtl = details.Where(x => x.groupid == groupid).OrderBy(x => x.category).ToList();                        
                        //double hourlyforcest = db.CampaignDetails.Where(x => x.campaignid == campaignid  && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        hourlyforcest += details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        saleForcest += db.CampaignBranchSalesTarget.Where(x => x.branchcode == branch.branchcode && x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        TimeSpan? maxtime = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1).Max(x => x.time);
                        saleActual += details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.time == maxtime).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                    }

                    totalst += saleForcest;
                    totalsale += saleActual;
                    totalhg += hourlyforcest;

                    bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2'>" + hourlyforcest + "</td><td rowspan ='2'>" + (saleActual - saleForcest) + "</td>";
                    dtlcount = dtl.Count;
                    c = 0;
                    tmpHtml = "";
                    foreach (var category in categories)
                    {
                        //var id = Convert.ToString(staff.staffid) + "_" + Convert.ToString(category.campaigncategoryid);
                        var id = Convert.ToString(groupid) + "_" + Convert.ToString(category.campaigncategoryid);
                        //if (c < dtl.Count)
                        //{
                        var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                        //if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
                        double sforecast = 0; double sactual = 0;
                        if (tmpobj != null)
                        {
                            //returnHtml = returnHtml + "<td ><table style='width:100%'><tr style='width:100%'><td style ='background-color:#4EE7FF;width:100%'>" + dtl.ElementAt(c).actual + "</td></tr><tr><td style ='background-color:#EED8EE'>" + dtl.ElementAt(c).forecast + "</td></tr></table></td>";
                            //returnHtml = returnHtml + "<td><input style ='background-color:#4EE7FF;width:100%' class='form-control' value ='" + dtl.ElementAt(c).actual + "' /><br/><input disabled='disabled' style ='background-color:#EED8EE' class='form-control' value ='" + dtl.ElementAt(c).forecast + "'/></td>";
                            sactual = tmpobj.Sum(x => (double?)x.salesactual) ?? 0d;
                            sforecast = tmpobj.Sum(x => (double?)x.salesforecast) ?? 0d;
                            c = c + 1;
                        }
                        else
                        {
                            //returnHtml = returnHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;width:100%;' class='form-control' value ='0' /><br/><input disabled='disabled' style ='background-color:#EED8EE' class='form-control' value ='0'/></td>";
                            //bodyHtml = bodyHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;padding-top:0px;padding-right:0px;' class='form-control' value ='0' /></td>";
                            //tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
                        }
                        bodyHtml = bodyHtml + "<td >" + sactual.ToString() + "</td>";
                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + sforecast.ToString() + "</td>";

                    }
                    //returnHtml = returnHtml + "</tr>";
                    bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                    returnHtml = returnHtml + bodyHtml;
                    //r = r + 1;
                }

                string cattotalhtml = "";
                foreach (var category in categories)
                {
                    double? totalcat = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.categorytypeid == 2 && x.time == time && x.categoryid == category.campaigncategoryid).Sum(x => (double?)x.salesactual) ?? 0d;
                    cattotalhtml = cattotalhtml + "<td>" + totalcat + "</td>";
                }
                totalsh = totalsale - totalst;
                string gtotalhtml = "<tr style='background-color:#F2E886;'><td style='text-align:center;'>Total</td><td>" + totalst + "</td><td>" + totalsale + "</td><td>" + totalhg + "</td><td>" + totalsh + "</td>" + cattotalhtml + "</tr>";
                returnHtml = returnHtml + gtotalhtml + closeHtml;
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
            //return Content(returnHtml);
        }

        public ActionResult getBranchSalesReportGrid_old(int campaignid, int groupid, TimeSpan time, DateTime cdate)
        {
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:smaller;'><thead>";
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
                //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;'>" + category.Category.Category + "</th>";
                    catcount++;
                }
                //headerHtml = headerHtml + "</tr>";
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                ICollection<Configuration_m_Branches> branches;
                //ICollection<CampaignBranchSales> details;
                ICollection<Campaign_m_CampaignDetails> details;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                        //branches = context.Database.SqlQuery<Configuration_m_Branches>("call getCampaignBranches (" + campaignid + "," + groupid + ")").ToList();
                        //details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales (" + campaignid + "," + groupid + ",'" + time + "','2013-10-30'," + 2 + ")").ToList();
                        //details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales(" + campaignid + "," + groupid + ",'" + time + "','" + cd + "'," + 2 + ")").ToList();
                        details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate).ToList();
                    }

                    bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                    foreach (var branch in branches)
                    {
                        //returnHtml = returnHtml + "<tr>";
                        TimeSpan? maxtime = details.Where(x => x.categorytypeid == 1 && x.branchcode == branch.branchcode).Max(x => x.time);
                        bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.getBranchName() + "</td>";
                        //ICollection<CampaignBranchSales> dtl = details.Where(x => x.branchcode == branch.branchcode).OrderBy(x => x.category).ToList();
                        ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.time == time).ToList();
                        //double hourlyforcest = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        double hourlyforcest = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.time == time).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        //double saleForcest = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1).Sum(x => (double?)x.forecast) ?? 0d;
                        double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        double? saleActual = details.Where(x => x.branchcode == branch.branchcode && x.categorytypeid == 1 && x.time == maxtime).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //double saleActual = details.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        //bodyHtml = bodyHtml + "<td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleForcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleActual + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + hourlyforcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + (saleForcest - saleActual) + "</td>";
                        bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                        dtlcount = dtl.Count;
                        c = 0;
                        tmpHtml = "";
                        foreach (var category in categories)
                        {
                            //var id = Convert.ToString(staff.staffid) + "_" + Convert.ToString(category.campaigncategoryid);
                            var id = Convert.ToString(branch.id) + "_" + Convert.ToString(category.campaigncategoryid);
                            double? Aactual; double? Aforecast;
                            Aactual = Aforecast = 0;
                            //if (c < dtl.Count)
                            //{
                            var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                            //if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
                            if (tmpobj != null)
                            {
                                Aactual = tmpobj.Sum(x => (double?)x.salesactual) ?? 0d;
                                Aforecast = tmpobj.Sum(x => (double?)x.salesforecast) ?? 0d;
                                //Aactual = tmpobj.salesactual;
                                //Aforecast = tmpobj.salesforecast;
                                c = c + 1;
                            }
                            else
                            {
                                //bodyHtml = bodyHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;padding-top:0px;padding-right:0px;' class='form-control' value ='0' /></td>";
                                //tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
                            }
                            bodyHtml = bodyHtml + "<td style =''>" + Aactual + "</td>";
                            tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + Aforecast + "</td>";

                        }
                        //returnHtml = returnHtml + "</tr>";
                        bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                        r = r + 1;
                    }
                    returnHtml = returnHtml + bodyHtml;
                }
                returnHtml = returnHtml + closeHtml;

            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return Content(returnHtml);
        }

        public string getGroupSalesReportGridAccumulativeUpdated(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:smaller;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            double? totalst; double? totalsale; double? totalhg; double? totalsh;
            totalst = totalsale = totalhg = totalsh = 0;

            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));
            string cocode = Session["cocode"].ToString();
            try
            {

                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;' ><th style='width:150px;'>Group/Outlet</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th>" + category.Category.Category + "</th>";
                    catcount++;
                }
                //headerHtml = headerHtml + "</tr>";
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                //ICollection<Configuration_m_Branches> branches;
                //ICollection<CampaignBranchSales> details;

                ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                using (var context = new BigMacEntities())
                {
                    string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' GROUP BY cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.staffid  = t2.staffid and t1.categorytypeid = t2.categorytypeid and t1.categoryid = t2.categoryid and t1.maxtimeid = t2.time";
                    summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    //branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    if (t == "ALL")
                        details = summarydtl;
                    else
                        details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }
                foreach (var g in groups)
                {
                    groupid = g.groupid;

                    bodyHtml = "<tr><td rowspan ='2' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td>";
                    //bodyHtml = "<tr><td colspan ='50'>" + g.group + "</td></tr>";
                    //ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.categorytypeid == 2 && x.time == time).ToList();
                    ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.categorytypeid == 2 && x.groupid == groupid).ToList();
                    double? hourlyforcest = 0;
                    double? saleForcest = 0;
                    double? saleActual = 0;

                    saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                    saleActual += details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0;
                    if (t != "ALL")
                    {
                        hourlyforcest = details.Where(x => x.categorytypeid == 1 && x.groupid == groupid).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;

                    }

                    totalst += saleForcest;
                    totalsale += saleActual;
                    totalhg += hourlyforcest;

                    bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2'>" + hourlyforcest + "</td><td rowspan ='2'>" + (saleActual - saleForcest) + "</td>";
                    dtlcount = dtl.Count;
                    c = 0;
                    tmpHtml = "";
                    foreach (var category in categories)
                    {
                        //var id = Convert.ToString(staff.staffid) + "_" + Convert.ToString(category.campaigncategoryid);
                        var id = Convert.ToString(groupid) + "_" + Convert.ToString(category.campaigncategoryid);
                        //if (c < dtl.Count)
                        //{
                        var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                        //if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
                        double sforecast = 0; double sactual = 0;
                        if (tmpobj != null)
                        {
                            //returnHtml = returnHtml + "<td ><table style='width:100%'><tr style='width:100%'><td style ='background-color:#4EE7FF;width:100%'>" + dtl.ElementAt(c).actual + "</td></tr><tr><td style ='background-color:#EED8EE'>" + dtl.ElementAt(c).forecast + "</td></tr></table></td>";
                            //returnHtml = returnHtml + "<td><input style ='background-color:#4EE7FF;width:100%' class='form-control' value ='" + dtl.ElementAt(c).actual + "' /><br/><input disabled='disabled' style ='background-color:#EED8EE' class='form-control' value ='" + dtl.ElementAt(c).forecast + "'/></td>";
                            sactual = tmpobj.Sum(x => (double?)x.salesactual) ?? 0d;
                            sforecast = tmpobj.Sum(x => (double?)x.salesforecast) ?? 0d;
                            c = c + 1;
                        }
                        else
                        {
                            //returnHtml = returnHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;width:100%;' class='form-control' value ='0' /><br/><input disabled='disabled' style ='background-color:#EED8EE' class='form-control' value ='0'/></td>";
                            //bodyHtml = bodyHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;padding-top:0px;padding-right:0px;' class='form-control' value ='0' /></td>";
                            //tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
                        }
                        bodyHtml = bodyHtml + "<td >" + sactual.ToString() + "</td>";
                        tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + sforecast.ToString() + "</td>";

                    }
                    //returnHtml = returnHtml + "</tr>";
                    bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                    returnHtml = returnHtml + bodyHtml;
                    //r = r + 1;
                }

                string cattotalhtml = "";
                foreach (var category in categories)
                {
                    double? totalcat = details.Where(x => x.categorytypeid == 2 && x.categoryid == category.campaigncategoryid).Sum(x => (double?)x.salesactual) ?? 0d;
                    cattotalhtml = cattotalhtml + "<td>" + totalcat + "</td>";
                }
                totalsh = totalsale - totalst;
                string gtotalhtml = "<tr style='background-color:#F2E886;'><td style='text-align:center;'>Total</td><td>" + totalst + "</td><td>" + totalsale + "</td><td>" + totalhg + "</td><td>" + totalsh + "</td>" + cattotalhtml + "</tr>";
                returnHtml = returnHtml + gtotalhtml + closeHtml;
            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
            //return Content(returnHtml);
        }

        public string getBranchSalesReportGridAccumulativeUpdated(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:smaller;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));

            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Branch</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th style='text-align:center;'>" + category.Category.Category + "</th>";
                    catcount++;
                }

                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                ICollection<Configuration_m_Branches> branches;
                ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                using (var context = new BigMacEntities())
                {
                    string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' GROUP BY cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.staffid  = t2.staffid and t1.categorytypeid = t2.categorytypeid and t1.categoryid = t2.categoryid and t1.maxtimeid = t2.time";
                    summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    //branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    if (t == "ALL")
                        details = summarydtl;
                    else
                        details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                        //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate).ToList(); 
                    }

                    bodyHtml = "<tr s><td colspan ='50' style='background-color:yellow;padding:0 0 0 5px;text-align:left;'>" + g.group + "</td></tr>";
                    foreach (var branch in branches)
                    {
                        //TimeSpan? maxtime = details.Where(x => x.groupid == groupid && x.categorytypeid == 1 && x.branchcode == branch.branchcode).Max(x => x.time);
                        bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left;'>" + branch.getBranchName() + "</td>";
                        ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2).ToList();
                        double hourlyforcest = 0; //details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1 ).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        if (t != "ALL")
                            hourlyforcest = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                        double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode && x.staffid == 0).Sum(x => (double?)x.salesforecast) ?? 0d;
                        double? saleActual = details.Where(x => x.groupid == groupid && x.branchcode == branch.branchcode && x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                        bodyHtml = bodyHtml + "<td rowspan ='2' >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                        dtlcount = dtl.Count;
                        c = 0;
                        tmpHtml = "";
                        foreach (var category in categories)
                        {
                            var id = Convert.ToString(branch.id) + "_" + Convert.ToString(category.campaigncategoryid);
                            double? Aactual; double? Aforecast;
                            Aactual = Aforecast = 0;
                            //if (c < dtl.Count)
                            //{
                            var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).ToList();
                            //if (dtl.ElementAt(c).categoryid == category.campaigncategoryid)
                            if (tmpobj != null)
                            {
                                Aactual = tmpobj.Sum(x => (double?)x.salesactual) ?? 0d;
                                Aforecast = tmpobj.Sum(x => (double?)x.salesforecast) ?? 0d;
                                //Aactual = tmpobj.salesactual;
                                //Aforecast = tmpobj.salesforecast;
                                c = c + 1;
                            }
                            else
                            {
                                //bodyHtml = bodyHtml + "<td style ='padding:0px;'><input  disabled='disabled' style ='background-color:#4EE7FF;padding-top:0px;padding-right:0px;' class='form-control' value ='0' /></td>";
                                //tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='padding:0px;'><input class='form-control' style ='background-color:#EED8EE' type='text' style ='padding-top:0px;padding-right:0px;' onchange='txtonChanged(this);' ch ='0' af ='F' id='F" + id + "' value ='0'></td>";
                            }
                            bodyHtml = bodyHtml + "<td style =''>" + Aactual + "</td>";
                            tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + Aforecast + "</td>";

                        }
                        //returnHtml = returnHtml + "</tr>";
                        bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                        r = r + 1;
                    }
                    returnHtml = returnHtml + bodyHtml;
                }
                returnHtml = returnHtml + closeHtml;

            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }

        public string getBranchStaffReportGridAccumulativeUpdated(int campaignid, int groupid, string t, DateTime cdate)
        {
            TimeSpan time = new TimeSpan();
            var returnHtml = "<table class='table table-bordered bootstrap-datatable datatable' style='font-size:smaller;'><thead>";
            var headerHtml = "";
            var bodyHtml = "";
            var closeHtml = "</tbody></table>";
            int r = 0; int c = 0; int dtlcount = 0;
            int catcount = 0;
            //TimeSpan time = new TimeSpan();
            var tmpHtml = "";
            if (t != "ALL")
                time = new TimeSpan(Convert.ToInt16(t.Substring(0, 2)), Convert.ToInt16(t.Substring(3, 2)), Convert.ToInt16(t.Substring(6, 2)));
            string cocode = Session["cocode"].ToString();
            try
            {
                ICollection<Campaign_m_Category> categories = db.CampaignCategories.Where(x => x.campaignid == campaignid && x.categorytypeid == 2).Include("Category").OrderBy(x => x.id).ToList();
                ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).Include("Staff").OrderBy(x => x.Staff.name).ToList();
                //ICollection<Campaign_m_CampaignDetails> details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.branchcode == branchcode && x.groupid == groupid && x.time == time && x.createdate == cdate && x.categorytypeid == ctype).Include("Category").ToList();
                headerHtml = "<tr style ='background-color:#F2E886;'><th style='width:150px;'>Staff</th><th style='max-width:100px;' >Attendance</th><th style='width:100px;'>Sales Target</th><th style='width:100px;'>Sales Total</th><th style='width:100px;'>Hourly Goals</th><th style='width:100px;'>Shortage</th>";

                foreach (var category in categories)
                {
                    headerHtml = headerHtml + "<th >" + category.Category.Category + "</th>";
                    catcount++;
                }
                //headerHtml = headerHtml + "</tr>";
                returnHtml = returnHtml + headerHtml + "</tr></thead><tbody>";

                var groups = db.BranchGroupLinks.Include("Group").Where(x => x.groupid == groupid || groupid == 0).Select(x => new { x.groupid, x.Group.group }).Distinct().OrderBy(x => x.group).ToList();
                ICollection<Configuration_m_Branches> branches;
                //ICollection<CampaignBranchSales> details;
                ICollection<Campaign_m_CampaignDetails> summarydtl;
                ICollection<Campaign_m_CampaignDetails> details;
                ICollection<campaign_m_branchsalestarget> bsales;
                string cd = string.Format("{0:yyyy-MM-dd}", cdate);
                using (var context = new BigMacEntities())
                {
                    string strsql = "SELECT t2.* FROM ( SELECT cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid, MAX(time) AS maxtimeid FROM campaign_m_campaigndetails where campaignid = " + campaignid + "  and createdate = '" + cdate.ToString("yyyy-MM-dd") + "' GROUP BY cocode, campaignid, createdate, branchcode, groupid, staffid , categorytypeid,categoryid ) as t1 INNER JOIN campaign_m_campaigndetails t2 ON t1.cocode = t2.cocode and t1.campaignid = t2.campaignid and t1.createdate = t2.createdate and t1. groupid = t2.groupid and t1.branchcode = t2.branchcode and t1.staffid  = t2.staffid and t1.categorytypeid = t2.categorytypeid and t1.categoryid = t2.categoryid and t1.maxtimeid = t2.time";
                    summarydtl = context.Database.SqlQuery<Campaign_m_CampaignDetails>(strsql).ToList();
                    bsales = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.createdate == cdate && x.cocode == cocode).ToList();
                    //branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                    if (t == "ALL")
                        details = summarydtl;
                    else
                        details = db.CampaignDetails.Where(x => x.campaignid == campaignid && (x.groupid == groupid || groupid == 0) && x.createdate == cdate && x.time == time).ToList();
                }
                foreach (var g in groups)
                {
                    groupid = g.groupid;
                    using (var context = new BigMacEntities())
                    {
                        //branches = context.Database.SqlQuery<Configuration_m_Branches>("call getCampaignBranches (" + campaignid + "," + groupid + ")").ToList();
                        branches = context.Database.SqlQuery<Configuration_m_Branches>("select distinctrow branch.* from Configuration_m_Branches branch inner join (select branchcode from Campaign_m_Attendance_Staff where campaignid =" + campaignid + " and (groupid = " + groupid + " or " + groupid + "=0)) staff on branch.branchcode = staff.branchcode;").ToList();
                        //details = context.Database.SqlQuery<CampaignBranchSales>("call getCampaignBranchSales(" + campaignid + "," + groupid + ",'" + time + "','" + cd + "'," + 2 + ")").ToList();
                        //details = db.CampaignDetails.Where(x => x.campaignid == campaignid && x.groupid == groupid && x.createdate == cdate).ToList();
                    }
                    bodyHtml = "<tr ><td colspan ='" + ((categories.Count * 2) + 6) + "' style='background-color:yellow;padding:0 0 0 5px;text-align:left'>" + g.group + "</td></tr>";
                    foreach (var branch in branches)
                    {
                        //returnHtml = returnHtml + "<tr>";
                        bodyHtml = bodyHtml + "<tr ><td colspan ='" + ((categories.Count * 2) + 6) + "' style='background-color:#ADC17D;padding:0 0 0 5px;text-align:left'>" + branch.getBranchName() + "</td></tr>";
                        ICollection<Campaign_m_Attendance_Staff> slist = staffs.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode).ToList();
                        foreach (var staff in slist)
                        {

                            ICollection<Campaign_m_CampaignDetails> dtl = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.categorytypeid == 2 && x.staffid == staff.staffid).ToList();
                            double hourlyforcest = 0;
                            if (t != "ALL")
                                hourlyforcest = details.Where(x => x.groupid == g.groupid && x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.categorytypeid == 1).Sum(x => (double?)x.salesforecast + x.productforecast) ?? 0d;
                            double saleForcest = bsales.Where(x => x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode).Sum(x => (double?)x.salesforecast) ?? 0d;
                            //double saleForcest = db.CampaignBranchSalesTarget.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid && x.createdate == cdate && x.cocode == cocode ).Sum(x => (double?)x.salesforecast) ?? 0d;
                            double? saleActual = details.Where(x => x.groupid == g.groupid && x.categorytypeid == 1 && x.staffid == staff.staffid).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            //double saleActual = details.Where(x => x.campaignid == campaignid && x.branchcode == branch.branchcode && x.groupid == groupid && x.createdate == cdate && x.categorytypeid == 1).Sum(x => (double?)x.salesactual + x.productactual) ?? 0d;
                            var dobj = bsales.Where(x => x.branchcode == branch.branchcode && x.staffid == staff.staffid && x.groupid == groupid).FirstOrDefault();
                            string dopt = "Attend";
                            if (dobj != null)
                                dopt = dobj.staffdaytype;
                            bodyHtml = bodyHtml + "<tr><td rowspan ='2' style='padding:0 0 0 5px;text-align:left'>" + staff.Staff.name + "</td><td rowspan ='2' style='width:100px;text-align:center;'>" + dopt + "</td>";
                            //bodyHtml = bodyHtml + "<td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleForcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + saleActual + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + hourlyforcest + "</td><td rowspan ='2' style='padding:0px 8px 0px 0px;'>" + (saleForcest - saleActual) + "</td>";
                            bodyHtml = bodyHtml + "<td rowspan ='2'  >" + saleForcest + "</td><td rowspan ='2' >" + saleActual + "</td><td rowspan ='2' >" + hourlyforcest + "</td><td rowspan ='2' >" + (saleActual - saleForcest) + "</td>";
                            dtlcount = dtl.Count;
                            c = 0;
                            tmpHtml = "";
                            foreach (var category in categories)
                            {
                                var id = Convert.ToString(branch.id) + "_" + Convert.ToString(category.campaigncategoryid);
                                double? Aactual; double? Aforecast;
                                Aactual = Aforecast = 0;
                                var tmpobj = dtl.Where(x => x.categoryid == category.Category.id).FirstOrDefault();
                                if (tmpobj != null)
                                {
                                    Aactual = tmpobj.salesactual;
                                    Aforecast = tmpobj.salesforecast;
                                    c = c + 1;
                                }
                                bodyHtml = bodyHtml + "<td >" + Aactual + "</td>";
                                tmpHtml = tmpHtml + "<td id='td_" + id + "' style ='background-color:#EEE'>" + Aforecast + "</td>";
                            }
                            bodyHtml = bodyHtml + "</tr><tr>" + tmpHtml + "</tr>";
                            r = r + 1;
                        }

                    }
                    returnHtml = returnHtml + bodyHtml;
                }
                returnHtml = returnHtml + closeHtml;

            }
            catch (Exception e)
            { returnHtml = e.Message.ToString(); }

            return returnHtml;
        }
    }
}
