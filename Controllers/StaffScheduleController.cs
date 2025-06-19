using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;

namespace BigMac.Controllers
{
    public class StaffScheduleController : Controller
    {
        private BigMacEntities db = new BigMacEntities();
        
        public ActionResult Index()
        {   
            return View();
        }

        public ActionResult Listing()
        {
            if (Session["userid"] != null)
            {
                string rcode = "STAFFSCHED";
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
                    ViewBag.StaffOptions = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == deptid).Select(x => new { x.Staffs.id, x.Staffs.name }).OrderBy(x => x.name).ToList();
                }
                else
                    ViewBag.StaffOptions = null;

                ViewBag.Rcode = rcode;
                ViewBag.Bcode = bcode;
                ViewBag.UserID = Session["userid"].ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Access");
            }

        }

        [Authorize]
        public ActionResult LoginToStaffSchedule(string uname, string pwd)
        {
            var returnStr = "false";
            ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x => x.status.ToUpper() == "ACTIVE").ToList();
            Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname.ToUpper() && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();
            if (u != null)
            {
                if (u.roldid == 4 || u.roldid == 1)
                {
                    returnStr = "true";
                }
         
            }
            else
            {
                returnStr = "invalid";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }



        #region Holiday
        public JsonResult GetPublicHoliday()
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Schedule_m_Holiday> holiday = db.holiday.ToList();

                    var holidayList = (from hol in holiday
                                      select new
                                      {
                                          id = hol.id,
                                          name = hol.holidayName +" ("+hol.holidayDate.ToString("d MMM yyyy")+")",
                                          shortName = hol.holidayName,
                                          date = hol.holidayDate.ToString("MM/dd/yyyy"),
                                          holidaydate = hol.holidayDate
                                    }).ToList();

                    return Json(holidayList.OrderBy(x=>x.date).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddNewHoliday(Schedule_m_Holiday publicHoliday)
        {
            var returnStr = "FAIL";

            
            try
            {
                publicHoliday.createdDate = DateTime.Now;
                publicHoliday.modifiedDate = publicHoliday.createdDate;
                db.holiday.Add(publicHoliday);
                db.SaveChanges();

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
          

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateHoliday(Schedule_m_Holiday publicHoliday)
        {
            var returnStr = "FAIL";

           
            try
            {
                Schedule_m_Holiday h = db.holiday.FirstOrDefault(x => x.id == publicHoliday.id);

                if (h != null)
                {
                    h.modifiedDate = DateTime.Now;
                    h.holidayName = publicHoliday.holidayName;
                    h.holidayDate = publicHoliday.holidayDate;
                    db.SaveChanges(); 
                        
                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
           

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveHoliday(int id = 0)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_holiday where id=" + id);
                }
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region Leave
        public JsonResult GetLeave()
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Schedule_m_Leave> leaveList = db.leave.OrderBy(x => x.leaveName).ToList();

                    return Json(leaveList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddNewLeave(Schedule_m_Leave leave)
        {
            var returnStr = "FAIL";

            try
            {
                leave.createdDate = DateTime.Now;
                leave.modifiedDate = leave.createdDate;
                db.leave.Add(leave);
                db.SaveChanges();

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
           

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateLeave(Schedule_m_Leave leave)
        {
            var returnStr = "FAIL";

           
            try
            {
                Schedule_m_Leave l = db.leave.FirstOrDefault(x => x.id == leave.id);

                if (l != null)
                {
                    l.modifiedDate = DateTime.Now;
                    l.leaveName = leave.leaveName;
                    l.textColor = leave.textColor;
                    l.backgroundColor = leave.backgroundColor;
                    l.start = leave.start;
                    l.end = leave.end;
                    l.isWholeDay = leave.isWholeDay;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
         

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveLeave(int id = 0)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_leave where id=" + id);
                }
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region Staff Schedule
        public JsonResult GetStaffSchedule(int id,string bcode)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x=> x.staffId == id && x.branchcode == bcode).OrderBy(x => x.startDate).ToList();
                    ICollection<Schedule_m_Holiday> holiday = db.holiday.OrderBy(x => x.holidayDate).ToList();
                    ICollection<Schedule_m_StaffWork> staffwork = db.staffWorkSchedule.Where(x => x.staffid == id && x.branchcode == bcode).OrderBy(x => x.startdate).ToList();

                    var staffSched = (from sched in staffLeave join leave in db.leave on sched.leaveId equals leave.id 
                                      select new {
                                          id = sched.id,
                                          text = leave.leaveName,
                                          type = sched.leaveId.ToString(),
                                          start_date = sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                                          end_date = sched.endDate.ToString("yyyy-MM-dd HH:mm"),
                                          color = leave.backgroundColor,
                                          textColor = leave.textColor,
                                          isWholeDay = leave.isWholeDay.ToString(),
                                          remarks = sched.remarks ?? ""
                                       }).ToList();

                    staffSched.AddRange((from h in holiday
                                           select new
                                           {
                                               id = 0,
                                               text = h.holidayName,
                                               type = "holiday",
                                               start_date = h.holidayDate.ToString("yyyy-MM-dd HH:mm"),
                                               end_date = h.holidayDate.ToString("yyyy-MM-dd HH:mm"),
                                               color = "",
                                               textColor = "",
                                               isWholeDay = "",
                                               remarks = ""
                                           }).ToList());

                    staffSched.AddRange((from sw in staffwork 
                                         join work in db.workSchedule on sw.workid equals work.id
                                         where work.type == "Work"
                                         select new
                                         {
                                             id = 0,
                                             text = work.description,
                                             type = "work",
                                             start_date = sw.startdate.ToString("yyyy-MM-dd HH:mm"),
                                             end_date = sw.enddate.ToString("yyyy-MM-dd HH:mm"),
                                             color = "",
                                             textColor = "",
                                             isWholeDay = "",
                                             remarks = ""
                                         }).ToList());

                    staffSched.AddRange((from sw in staffwork
                                         join work in db.workSchedule on sw.workid equals work.id
                                         where work.type == "Off"
                                         select new
                                         {
                                             id = 0,
                                             text = work.description,
                                             type = "off",
                                             start_date = sw.startdate.ToString("yyyy-MM-dd HH:mm"),
                                             end_date = sw.enddate.ToString("yyyy-MM-dd HH:mm"),
                                             color = "",
                                             textColor = "",
                                             isWholeDay = "",
                                             remarks = ""
                                         }).ToList());

                    return Json(staffSched.OrderBy(x=>x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult GetStaffScheduleByDate(int id,string date)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x => x.staffId == id).OrderBy(x => x.startDate).ToList();
                    staffLeave = staffLeave.Where(x => x.startDate.Date == Convert.ToDateTime(date)).ToList();
              
                    var staffSched = (from sched in staffLeave
                                      join leave in db.leave on sched.leaveId equals leave.id
                                      select new
                                      {
                                          id = sched.id,
                                          name = leave.leaveName,
                                          type = sched.leaveId.ToString(),
                                          start_date = sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                                          end_date = sched.endDate.ToString("yyyy-MM-dd HH:mm"),
                                          backgroundcolor = leave.backgroundColor,
                                          textcolor = leave.textColor,
                                          isWholeDay = leave.isWholeDay.ToString(),
                                          remarks = sched.remarks ?? ""
                                      }).ToList();

                    return Json(staffSched, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddNewStaffSchedule(Schedule_m_Staff staffSched,string bcode)
        {
            var returnStr = "FAIL";

           
                try
                {
                    Schedule_m_Leave leave = db.leave.Where(x => x.id == staffSched.leaveId).FirstOrDefault();
                    if (leave != null)
                    {
                        if (leave.isWholeDay == 1)
                        {
                            staffSched.endDate = staffSched.endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                        else
                        {
                            string[] strt = leave.start.Split(':');
                            string[] end = leave.end.Split(':');

                            staffSched.startDate = staffSched.startDate.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                            staffSched.endDate = staffSched.endDate.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);
                        }

                        Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.branchcode == bcode && x.staffid == staffSched.staffId);
                       
                        int deptId = 0;
                        if (staffbranch != null)
                            deptId = staffbranch.departmentid;

                        staffSched.createdDate = DateTime.Now;
                        staffSched.modifiedDate = staffSched.createdDate;
                        staffSched.branchcode = bcode;
                        staffSched.departmentid = deptId;
                        db.staffSchedule.Add(staffSched);
                        db.SaveChanges();

                        returnStr = "SUCCESS";
                    }
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
          

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveStaffSchedule(int id = 0)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_staff where id=" + id);
                }
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        #endregion

        #region Summary Schedule

         public JsonResult GetStaff(string bcode,int departmentid)
         {
            try
            {
                using (var context = new BigMacEntities())
                {

                    var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                    return Json(staffList.OrderBy(x=>x.name).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
         }


         public JsonResult GetStaffByDepartment(string bcode,int departmentid)
         {
             try
             {
                 using (var context = new BigMacEntities())
                 {

                     var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                     return Json(staffList.OrderBy(x => x.name).ToList(), JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception e)
             { return Json("Failed", JsonRequestBehavior.AllowGet); }
         }

         public JsonResult GetAllStaffSchedule(string bcode,int departmentid){
         {
            try
            {
                using (var context = new BigMacEntities())
                {
                  
                    var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();


                    ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x=>x.branchcode==bcode && x.departmentid == departmentid).ToList();
                    ICollection<Schedule_m_Holiday> holiday = db.holiday.OrderBy(x => x.holidayDate).ToList();
                    ICollection<Schedule_m_StaffWork> staffWork = db.staffWorkSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid).ToList();
                  
                    var staffSched = (from sched in staffLeave join leave in db.leave on sched.leaveId equals leave.id 
                                      select new {
                                          id = sched.id,
                                          text = leave.leaveName,
                                          type = sched.leaveId.ToString(),
                                          section_id = sched.staffId.ToString(),
                                          start_date = sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                                          end_date = sched.endDate.ToString("yyyy-MM-dd HH:mm"),
                                          color = leave.backgroundColor,
                                          textColor = leave.backgroundColor,
                                          isWholeDay = leave.isWholeDay.ToString(),
                                          remarks = sched.remarks ?? ""
                                       }).ToList();


                    foreach (Schedule_m_StaffWork sw in staffWork)
                    {
                        Schedule_m_Work work = db.workSchedule.FirstOrDefault(x => x.id == sw.workid);
                        staffSched.Add(new
                                      {
                                          id = 0,
                                          text = work.description,
                                          type = (sw.type=="Work") ? "work" : "off",
                                          section_id = sw.staffid.ToString(),
                                          start_date = sw.startdate.ToString("yyyy-MM-dd HH:mm"),
                                          end_date = sw.enddate.ToString("yyyy-MM-dd HH:mm"),
                                          color = "",
                                          textColor = "",
                                          isWholeDay = "",
                                          remarks = ""

                                      });
                    }
                    
                    foreach (var staff in staffList)
                    {
                        staffSched.AddRange((from h in holiday
                                             select new
                                             {
                                                 id = 0,
                                                 text = h.holidayName,
                                                 type = "holiday",
                                                 section_id = staff.id.ToString(),
                                                 start_date = h.holidayDate.ToString("yyyy-MM-dd HH:mm"),
                                                 end_date = h.holidayDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm"),
                                                 color = "",
                                                 textColor = "",
                                                 isWholeDay = "",
                                                 remarks = ""

                                             }).ToList());
                    }



                    return Json(staffSched.OrderBy(x=>x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
           }
          }

         [HttpPost]
         public JsonResult UpdateStaffSchedule(Schedule_m_Staff staffSched)
         {
             var returnStr = "FAIL";

                 try
                 {
                     Schedule_m_Staff staffSchedule = db.staffSchedule.FirstOrDefault(x => x.id == staffSched.id);
                     
                     Schedule_m_Leave leave = db.leave.Where(x => x.id == staffSched.leaveId).FirstOrDefault();
                     if (leave != null)
                     {
                         if (leave.isWholeDay == 1)
                         {
                             staffSchedule.startDate = staffSched.startDate;
                             staffSchedule.endDate = staffSched.endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                         }
                         else
                         {
                             string[] strt = leave.start.Split(':');
                             string[] end = leave.end.Split(':');

                             staffSchedule.startDate = staffSched.endDate.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                             staffSchedule.endDate = staffSched.endDate.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);
                         }

                         Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.branchcode == staffSchedule.branchcode && x.staffid == staffSched.staffId);

                         int deptId = 0;
                         if (staffbranch != null)
                             deptId = staffbranch.departmentid;

                         staffSchedule.departmentid = deptId;
                         staffSchedule.staffId = staffSched.staffId;
                         staffSchedule.modifiedDate = DateTime.Now;
                         db.SaveChanges();

                         returnStr = "SUCCESS";
                     }
                 }
                 catch (Exception e)
                 { returnStr = e.Message.ToString(); }
          

             return Json(returnStr, JsonRequestBehavior.AllowGet);
         }


     #endregion

        #region Work Schedule
         [HttpPost]
         public JsonResult AddNewWorkSchedule(Schedule_m_Work work,string datefrom,string dateto)
         {
             var returnStr = "FAIL";
       
             try
             {
               

                 DateTime df = DateTime.ParseExact(datefrom, "dd/MM/yyyy", null);
                 var dateFrom = df.Month + "/" + df.Day + "/" + df.Year;

                 DateTime dt = DateTime.ParseExact(dateto, "dd/MM/yyyy", null);
                 var dateTo = dt.Month + "/" + dt.Day + "/" + dt.Year;

                 

                 work.createddate = DateTime.Now;
                 work.datefrom = Convert.ToDateTime(dateFrom);
                 work.dateto = Convert.ToDateTime(dateTo);

                 //ICollection<Schedule_m_Work> sw = db.workSchedule.Where(x => x.branchcode == work.branchcode && x.staffid == work.staffid).ToList();

                 //foreach (Schedule_m_Work i in sw)
                 //{
                 //    if ((work.datefrom >= i.datefrom && work.datefrom <= i.dateto) || (work.dateto >= i.datefrom && work.dateto <= i.dateto))
                 //    {
                 //        if (TimeSpan.Parse(work.starttime) >= TimeSpan.Parse(i.starttime) && TimeSpan.Parse(work.starttime) <= TimeSpan.Parse(i.endtime) || (TimeSpan.Parse(work.starttime) >= TimeSpan.Parse(i.starttime) && TimeSpan.Parse(work.endtime) <= TimeSpan.Parse(i.endtime)))
                 //        {
                 //            continueProcess = false;
                 //        }
               
                    
                 //    }
                 //}

                 //if (continueProcess)
                 //{

                     work.lastmodifieddate = work.createddate;
                     db.workSchedule.Add(work);
                     db.SaveChanges();


                     for (DateTime date = work.datefrom; date.Date <= work.dateto.Date; date = date.AddDays(1))
                     {
                         if (work.days.Contains(date.DayOfWeek.ToString()))
                         {
                             Schedule_m_StaffWork staffwork = new Schedule_m_StaffWork();

                             string[] strt = work.starttime.Split(':');
                             string[] end = work.endtime.Split(':');

                             staffwork.startdate = date.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                             staffwork.enddate = date.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);

                             staffwork.branchcode = work.branchcode;
                             staffwork.departmentid = work.departmentid;
                             staffwork.createddate = DateTime.Now;
                             staffwork.lastmodifieddate = staffwork.createddate;
                             staffwork.workid = work.id;
                             staffwork.staffid = work.staffid;
                             staffwork.type = work.type;
                             db.staffWorkSchedule.Add(staffwork);
                             db.SaveChanges();
                         }
                     }
                     returnStr = "SUCCESS";
                 
             }
             catch (Exception e)
             { returnStr = e.Message.ToString(); }


             return Json(returnStr, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult UpdateWorkSchedule(Schedule_m_Work work, string datefrom, string dateto)
         {
             var returnStr = "FAIL";
             var recreate = true;

             try
             {
                 Schedule_m_Work oldwork = db.workSchedule.FirstOrDefault(x => x.id == work.id);

                 if (oldwork != null)
                 {
                     if (oldwork.days == work.days && oldwork.datefrom == work.datefrom && oldwork.dateto == work.dateto && oldwork.starttime == work.starttime && oldwork.endtime == work.endtime)
                     {
                         recreate = false;
                     }

                     DateTime df = DateTime.ParseExact(datefrom, "dd/MM/yyyy", null);
                     var dateFrom = df.Month + "/" + df.Day + "/" + df.Year;

                     DateTime dt = DateTime.ParseExact(dateto, "dd/MM/yyyy", null);
                     var dateTo = dt.Month + "/" + dt.Day + "/" + dt.Year;

                     oldwork.datefrom = Convert.ToDateTime(dateFrom);
                     oldwork.dateto = Convert.ToDateTime(dateTo);
                     oldwork.description = work.description;
                     oldwork.starttime = work.starttime;
                     oldwork.endtime = work.endtime;
                     oldwork.days = work.days;
                     oldwork.type = work.type;
                     oldwork.lastmodifieddate = work.createddate;
                     db.SaveChanges();

                     if (recreate)
                     {
                         using (var context = new BigMacEntities())
                         {
                             var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_staffwork where workid=" + work.id);
                         }

                         for (DateTime date = oldwork.datefrom; date.Date <= oldwork.dateto.Date; date = date.AddDays(1))
                         {
                             if (work.days.Contains(date.DayOfWeek.ToString()))
                             {
                                 Schedule_m_StaffWork staffwork = new Schedule_m_StaffWork();

                                 string[] strt = work.starttime.Split(':');
                                 string[] end = work.endtime.Split(':');

                                 staffwork.startdate = date.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                                 staffwork.enddate = date.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);

                                 staffwork.branchcode = work.branchcode;
                                 staffwork.departmentid = work.departmentid;
                                 staffwork.createddate = DateTime.Now;
                                 staffwork.lastmodifieddate = staffwork.createddate;
                                 staffwork.workid = work.id;
                                 staffwork.staffid = work.staffid;
                                 staffwork.type = work.type;
                                 db.staffWorkSchedule.Add(staffwork);
                                 db.SaveChanges();
                             }
                         }

                     }
                     

                     
                    


                     returnStr = "SUCCESS";
                 }
             }
             catch (Exception e)
             { returnStr = e.Message.ToString(); }


             return Json(returnStr, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult RemoveWorkSchedule(int id)
         {
             var returnStr = "FAIL";

             try
             {
                 using (var context = new BigMacEntities())
                 {
                     var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_work where id=" + id);
                 }

                 using (var context = new BigMacEntities())
                 {
                     var value = context.Database.ExecuteSqlCommand("Delete from schedule_m_staffwork where workid=" + id);
                 }

                 returnStr = "SUCCESS";
                 
             }
             catch (Exception e)
             { returnStr = e.Message.ToString(); }


             return Json(returnStr, JsonRequestBehavior.AllowGet);
         }

         [Authorize]
         public ActionResult GetWorkScheduleList(jQueryDataTableParamModel param,string staffid)
         {
             try
             {
                 using (var context = new BigMacEntities())
                 {
                     string bcode = Session["branchcode"].ToString();
                     int staff = Convert.ToInt32(staffid);
                     ICollection<Schedule_m_Work> work = db.workSchedule.Where(x => x.branchcode == bcode && x.staffid == staff).ToList();
                     var pList = work.OrderBy(x => x.createddate).ToList();

                     var searchValue = "";
                     if (param.sSearch == null) searchValue = "";
                     else searchValue = param.sSearch;
                     var filterPList = pList.Where(x => string.Format("dd/MM/yyyy", x.createddate).Contains(searchValue) || x.description.ToUpper().Contains(searchValue.ToUpper())).OrderByDescending(x => x.createddate).ToList();

                     var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                     var sortDirection = Request["sSortDir_0"]; //asc or desc
                     if (sortDirection == "asc")
                     {
                         if (sortColumnIndex == 0)
                             filterPList = filterPList.OrderBy(x => x.description).ToList();
                         else if (sortColumnIndex == 1)
                             filterPList = filterPList.OrderBy(x => x.type).ToList();
                         else if (sortColumnIndex == 2)
                             filterPList = filterPList.OrderBy(x => x.starttime).ToList();
                         else if (sortColumnIndex == 3)
                             filterPList = filterPList.OrderBy(x => x.endtime).ToList();
                         else if (sortColumnIndex == 4)
                             filterPList = filterPList.OrderBy(x => x.datefrom).ToList();
                         else if (sortColumnIndex == 5)
                             filterPList = filterPList.OrderBy(x => x.dateto).ToList();
                         else if (sortColumnIndex == 6)
                             filterPList = filterPList.OrderBy(x => x.days).ToList();
                     }
                     else
                     {
                         if (sortColumnIndex == 0)
                             filterPList = filterPList.OrderByDescending(x => x.description).ToList();
                         else if (sortColumnIndex == 1)
                             filterPList = filterPList.OrderBy(x => x.type).ToList();
                         else if (sortColumnIndex == 2)
                             filterPList = filterPList.OrderByDescending(x => x.starttime).ToList();
                         else if (sortColumnIndex == 3)
                             filterPList = filterPList.OrderByDescending(x => x.endtime).ToList();
                         else if (sortColumnIndex == 4)
                             filterPList = filterPList.OrderByDescending(x => x.datefrom).ToList();
                         else if (sortColumnIndex == 5)
                             filterPList = filterPList.OrderByDescending(x => x.dateto).ToList();
                         else if (sortColumnIndex == 6)
                             filterPList = filterPList.OrderByDescending(x => x.days).ToList();
                     }

                     var paginatedPList = filterPList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                     return Json(new
                     {
                         sEcho = param.sEcho,
                         iTotalRecords = pList.Count, //paginatedQPList.TotalCount,
                         iTotalDisplayRecords = filterPList.Count, //paginatedQPList.TotalCount,
                         aaData = paginatedPList
                     },
                     JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception e)
             { return Json("Failed", JsonRequestBehavior.AllowGet); }
         }

         [Authorize]
         public ActionResult GetStaffWorkScheduleListByDay(string bcode, int departmentid)
         {
             try
             {
                 using (var context = new BigMacEntities())
                 {
                    ICollection<Schedule_m_StaffWork> staffwork = db.staffWorkSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid).OrderBy(x => x.createddate).ToList();
                    return Json(staffwork, JsonRequestBehavior.AllowGet);
                 
                 }
             }
             catch (Exception e)
             { return Json("Failed", JsonRequestBehavior.AllowGet); }
         }

         [Authorize]
         public ActionResult GetStaffWorkScheduleListByStaff(string bcode, int departmentid,int staffid)
         {
             try
             {
                 using (var context = new BigMacEntities())
                 {
                     ICollection<Schedule_m_StaffWork> staffwork = db.staffWorkSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid && x.staffid == staffid).OrderBy(x => x.createddate).ToList();
                     return Json(staffwork, JsonRequestBehavior.AllowGet);

                 }
             }
             catch (Exception e)
             { return Json("Failed", JsonRequestBehavior.AllowGet); }
         }

        #endregion

    }
}
