using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;
using MySql.Data.MySqlClient;
using Microsoft.Web.Helpers;
using System.Web.Configuration;
using System.Dynamic;
using System.Globalization;

namespace BigMac.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BranchStaffListing(string rcode)
        {
            ViewBag.RCode = rcode;
            string bcode = Session["branchcode"].ToString();
            var branchdeptList = db.BranchDepartment.Where(x => x.branchcode == bcode).ToList();
            var departmentList = (from brdept in branchdeptList
                                  join dept in db.Department on brdept.departmentid equals dept.id
                                  select new
                                  {
                                      id = dept.id,
                                      departmentname = dept.departmentname,
                                  }).ToList();
            ViewBag.DepartmentOptions = departmentList.OrderBy(x => x.departmentname).ToList();

            if (departmentList.Count > 0)
            {
                int deptid = departmentList[0].id;
                ViewBag.StaffOptions = db.Staffs.Select(x => new { x.name, x.id }).OrderBy(x => x.name).ToList();
            }
            else
                ViewBag.StaffOptions = db.Staffs.Select(x => new { x.name, x.id }).OrderBy(x => x.name).ToList();
            
            return View();
        }

        public ActionResult getBranchStaffListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                ICollection<Common_m_StaffBranchdtl> sList;
                
                using (var context = new BigMacEntities())
                {
                    sList = context.Database.SqlQuery<Common_m_StaffBranchdtl>("call getBranchStaffList('" + Session["branchcode"] + "')").ToList();                   
                }                

                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterSList = sList.Where(x => x.name.ToUpper().Contains(searchValue.ToUpper()) || x.position.ToUpper().Contains(searchValue.ToUpper()) || x.stafftype.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterSList = filterSList.OrderBy(x => x.name).ToList();
                    else if (sortColumnIndex == 1)
                        filterSList = filterSList.OrderBy(x => x.position).ToList();
                    else if (sortColumnIndex == 2)
                        filterSList = filterSList.OrderBy(x => x.departmentname).ToList();
                    else if (sortColumnIndex == 3)
                        filterSList = filterSList.OrderBy(x => x.stafftype).ToList();
                    else if (sortColumnIndex == 4)
                        filterSList = filterSList.OrderBy(x => x.createdate).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterSList = filterSList.OrderByDescending(x => x.name).ToList();
                    else if (sortColumnIndex == 1)
                        filterSList = filterSList.OrderByDescending(x => x.position).ToList();
                    else if (sortColumnIndex == 2)
                        filterSList = filterSList.OrderByDescending(x => x.departmentname).ToList();
                    else if (sortColumnIndex == 3)
                        filterSList = filterSList.OrderByDescending(x => x.stafftype).ToList();
                    else if (sortColumnIndex == 4)
                        filterSList = filterSList.OrderByDescending(x => x.createdate).ToList();
                }

                var paginatedQSList = filterSList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = sList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterSList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQSList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult BranchStaffRemove(int sid,string rcode ="BRANCHSTAFF")
        {
            string returnvalue = "FAIL";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    Common_m_StaffBranch bs= db.BranchStaff.Find(sid);
                    db.BranchStaff.Remove(bs);
                    db.SaveChanges();
                    returnvalue = "SUCCESS";

                    var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                    saveToLog(rid, bs.id, "CREATE", "Remove Branch Staff, ID -" + bs.id.ToString() + ", Staff ID -" + bs.staffid.ToString() + ", Branch Code -" + bs.branchcode);

                }
                catch (Exception e)
                {
                    returnvalue = "FAIL";
                }

            }
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BranchStaffAdd(int sid=0,string rcode="BRANCHSTAFF",int departmentid=0)
        {
            string returnvalue = "FAIL";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    Common_m_StaffBranch bs = new Common_m_StaffBranch();
                    bs.createdate = DateTime.Now;
                    bs.lastmodifieddate = DateTime.Now;
                    bs.staffid = sid;
                    bs.departmentid = departmentid;
                    bs.branchcode = Session["branchcode"].ToString();
                    db.BranchStaff.Add(bs);
                    db.SaveChanges();
                    returnvalue = "SUCCESS";

                    var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                    saveToLog(rid, bs.id, "CREATE", "Add New Branch Staff ID -" + bs.id.ToString() + ", Staff ID -" + bs.staffid.ToString() + ", Branch Code -" + bs.branchcode );
                }
                catch (Exception e)
                {
                    returnvalue = "FAIL";
                }

            }
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BranchStaffUpdate(int sid = 0, string rcode = "BRANCHSTAFF", int departmentid = 0)
        {
            string returnvalue = "FAIL";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    var bcode = Session["branchcode"].ToString();
                    Common_m_StaffBranch bs = db.BranchStaff.FirstOrDefault(x => x.staffid == sid && x.branchcode == bcode);

                    if (bs != null)
                    {
                       
                        bs.lastmodifieddate = DateTime.Now;
                        bs.departmentid = departmentid;
                        db.SaveChanges();

                        ICollection<Schedule_m_Appointment> appointmentList = db.appointment.Where(x => x.staffid == sid && x.branchcode == bcode).ToList();
                        foreach (Schedule_m_Appointment app in appointmentList)
                        {
                            app.departmentid = departmentid;
                            db.SaveChanges();
                        }
                        
                        ICollection<Schedule_m_StaffWork> staffWorkList = db.staffWorkSchedule.Where(x => x.staffid == sid && x.branchcode == bcode).ToList();
                        foreach (Schedule_m_StaffWork sw in staffWorkList)
                        {
                            sw.departmentid = departmentid;
                            db.SaveChanges();
                        }

                        ICollection<Schedule_m_Work> workList = db.workSchedule.Where(x => x.staffid == sid && x.branchcode == bcode).ToList();
                        foreach (Schedule_m_Work w in workList)
                        {
                            w.departmentid = departmentid;
                            db.SaveChanges();
                        }

                        ICollection<Schedule_m_Staff> staffSchedList = db.staffSchedule.Where(x => x.staffId == sid && x.branchcode == bcode).ToList();
                        foreach (Schedule_m_Staff l in staffSchedList)
                        {
                            l.departmentid = departmentid;
                            db.SaveChanges();
                        }

                        returnvalue = "SUCCESS";
                    }
                  

                }
                catch (Exception e)
                {
                    returnvalue = "FAIL";
                }

            }
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public Boolean saveToLog(int resourceid, int resourcecode, string logtype, string description, string from = "", string to = "", string ip = "")
        {
            GeneralController gc = new GeneralController();
            int uid = Convert.ToInt32(Session["userid"].ToString());
            string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            return gc.SaveToLog(uid, Session["cocode"].ToString(), Session.SessionID, resourceid, resourcecode, logtype, description, from, to, visitorIPAddress);
        }

    }
}
