using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using System.Globalization;
using System.Net.Mail;
using System.Net;

namespace BigMac.Controllers
{
    public class AppointmentController : Controller
    {
        //
        // GET: /Appointment/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult Index()
        {

            if (Session["userid"] != null)
            {
                int sid = 0;
                int.TryParse(Session["staffid"].ToString(), out sid);
                string bcode = Session["branchcode"].ToString();

                ViewBag.StaffID = sid;
                ViewBag.FacilityOptions = db.BranchAsset.Where(x => x.branchcode == bcode).OrderBy(x => x.name).ToList();
                //ViewBag.DepartmentOptions = db.bra.OrderBy(x => x.departmentname).ToList();
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
                    ViewBag.StaffOptions = string.Empty;

                ViewBag.AllStaffOptions = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode).Select(x => new { x.Staffs.id, x.Staffs.name }).OrderBy(x => x.name).ToList();
                ViewBag.Rcode = "APPOINTMENTDAILY";
                ViewBag.Bcode = bcode;
                ViewBag.UserID = Session["userid"].ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Access");

            }

        }



        #region ByDay
        public JsonResult GetAllStaffScheduleDay(string bcode, int departmentid, int day, int month, int year)
        {
            DateTime datevalue = (Convert.ToDateTime(DateTime.Now.ToString()));
            //int dy = Int32.Parse(datevalue.Day.ToString());
            //int mn = Int32.Parse(datevalue.Month.ToString());
            //int yy = Int32.Parse(datevalue.Year.ToString());

            datevalue = new DateTime(year, month, day) + datevalue.TimeOfDay;


            return GetAllStaffScheduleByDate(bcode, departmentid, datevalue);
        }

        public JsonResult GetAllStaffSchedule(string bcode, int departmentid)
        {
            return GetAllStaffScheduleByDate(bcode, departmentid, DateTime.Now);
        }

        public JsonResult GetAllStaffScheduleByDate(string bcode, int departmentid, DateTime date)
        {
            {
                try
                {
                    using (var context = new BigMacEntities())
                    {

                        // DateTime now = DateTime.Now;
                        var sDate = date.AddDays(-1);
                        var eDate = date.AddDays(0);

                        //var sDate = date;
                        //var eDate = date;

                        var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                        ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid).ToList();
                        //   ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.departmentid == departmentid && (x.transactiontype == null || x.transactiontype == String.Empty) && (x.appointmentdate >= sDate && x.appointmentdate <= eDate)).ToList();
                        ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.departmentid == departmentid && (x.transactiontype == null || x.transactiontype == String.Empty) && (x.appointmentdate >= sDate && x.appointmentdate <= eDate)).ToList();

                        ICollection<Salesorder_m_salesorder> salesOrder = db.saleorders.Where(x => x.type == "ORDER TAKING" && x.branchcode == bcode).OrderBy(x => x.createdate).ToList();


                        var staffSched = (from ssched in staffLeave
                                          join leave in db.leave on ssched.leaveId equals leave.id
                                          select new
                                          {
                                              id = ssched.id,
                                              text = leave.leaveName,
                                              type = "leave",
                                              unit_id = ssched.staffId.ToString(),
                                              start_date = (leave.isWholeDay.ToString() == "1") ? ssched.startDate.AddHours(8).AddMinutes(00).AddSeconds(00).ToString("yyyy-MM-dd h:mm tt") : ssched.startDate.ToString("yyyy-MM-dd h:mm tt"),
                                              end_date = ssched.endDate.ToString("yyyy-MM-dd h:mm tt"),
                                              isWholeDay = leave.isWholeDay.ToString(),
                                              color = leave.backgroundColor,
                                              textColor = leave.textColor,
                                              remarks = ssched.remarks ?? "",
                                              queueno = "",
                                              sono = "",
                                              status = "",
                                              customername = "",
                                              leaveId = ssched.leaveId.ToString(),
                                              staffname = "",
                                              balance = ""
                                          }).ToList();



                        foreach (Schedule_m_Appointment sp in staffAppointment)
                        {

                            //i++;
                            //if (i == 5) break;

                            string staffname = "";
                            string balance = "0";
                            DateTime startDate = sp.appointmentdate;
                            DateTime endDate = sp.appointmentdate;
                            TimeSpan sTime = DateTime.ParseExact(sp.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                            TimeSpan eTime = DateTime.ParseExact(sp.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                            var time = eTime - sTime;

                            startDate = startDate.Date.Add(sTime);
                            endDate = endDate.Date.Add(eTime);

                            Common_m_Staff staff = db.Staffs.Where(x => x.id == sp.staffid).FirstOrDefault();

                            if (staff != null)
                            {
                                staffname = staff.name;
                            }

                            if (sp.customercode != "" && sp.customercode != null)
                            {
                                balance = getOutstandingBalance(sp.customercode);
                            }

                            string text = "";
                            if (time.TotalHours < 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "," + sp.description + "," + sp.starttime + "-" + sp.endtime + "," + balance + "</p>";
                            }
                            else if (time.TotalHours >= 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.description + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.starttime + "-" + sp.endtime + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + balance + "</p>";
                            }

                            string color = "blue";

                            if (sp.status == "Cancelled")
                                color = "red";
                            else if (sp.status == "Confirmed")
                                color = "#00cc00";
                            else if (sp.status == "Arrived")
                                color = "#007f00";
                            else if (sp.status == "Done")
                                color = "#004C00";
                            else if (sp.status == "Wait List")
                                color = "#ffa500";
                            else if (sp.status == "No Show")
                                color = "#a6a6a6";
                            else
                                color = "blue";

                            staffSched.Add(new
                            {
                                id = sp.id,
                                text = text,
                                type = "appointment",
                                unit_id = sp.staffid.ToString(),
                                start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                isWholeDay = "",
                                color = color,
                                textColor = "white",
                                remarks = "",
                                queueno = sp.queuenumber ?? "",
                                sono = sp.sonumber ?? "",
                                status = sp.status,
                                customername = sp.customername,
                                leaveId = "",
                                staffname = staffname,
                                balance = balance
                            });
                        }


                        foreach (Salesorder_m_salesorder so in salesOrder)
                        {
                            int sostaffid = Convert.ToInt32(so.staffid);
                            Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.departmentid == departmentid && x.staffid == sostaffid);
                            if (staffbranch != null)
                            {
                                string staffname = "";
                                DateTime startDate = so.createdate ?? DateTime.Now;
                                DateTime endDate = so.createdate ?? DateTime.Now;
                                TimeSpan sTime = DateTime.ParseExact(so.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                TimeSpan eTime = DateTime.ParseExact(so.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                                var time = eTime - sTime;

                                startDate = startDate.Date.Add(sTime);
                                endDate = endDate.Date.Add(eTime);

                                Common_m_Staff staff = db.Staffs.Where(x => x.id == sostaffid).FirstOrDefault();
                                if (staff != null)
                                {
                                    staffname = staff.name;
                                }


                                string text = "";

                                if (time.TotalHours < 2)
                                {
                                    text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "," + so.resourcecode + "," + so.starttime + "-" + so.endtime + "</p>";
                                }
                                else if (time.TotalHours >= 2)
                                {
                                    text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.resourcecode + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.starttime + "-" + so.endtime + "</p>";
                                }


                                staffSched.Add(new
                                {
                                    id = so.id,
                                    text = text,
                                    type = "salesorder",
                                    unit_id = so.staffid.ToString(),
                                    start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                    end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                    isWholeDay = "",
                                    color = "white",
                                    textColor = "black",
                                    remarks = "",
                                    queueno = so.queuenumber ?? "",
                                    sono = so.resourcecode ?? "",
                                    status = so.status,
                                    customername = so.cussupname,
                                    leaveId = "",
                                    staffname = staffname,
                                    balance = ""
                                });
                            }
                        }

                        return Json(staffSched.OrderBy(x => x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                { return Json("Failed", JsonRequestBehavior.AllowGet); }
            }
        }

        public JsonResult GetAllStaffScheduleByWeek(string bcode, int departmentid)
        {

            {
                try
                {
                    using (var context = new BigMacEntities())
                    {

                        var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                        ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid).ToList();
                        ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.departmentid == departmentid && (x.transactiontype == null || x.transactiontype == String.Empty)).ToList();

                        ICollection<Salesorder_m_salesorder> salesOrder = db.saleorders.Where(x => x.type == "ORDER TAKING" && x.branchcode == bcode).OrderBy(x => x.createdate).ToList();

                        var staffSched = (from ssched in staffLeave
                                          join leave in db.leave on ssched.leaveId equals leave.id
                                          join staff in db.Staffs on ssched.staffId equals staff.id
                                          select new
                                          {
                                              id = ssched.id,
                                              text = "",
                                              name = leave.leaveName,
                                              staff_name = staff.name,
                                              type = "leave",
                                              unit_id = ssched.staffId.ToString(),
                                              start_date = (leave.isWholeDay.ToString() == "1") ? ssched.startDate.AddHours(8).AddMinutes(00).AddSeconds(00).ToString("yyyy-MM-dd h:mm tt") : ssched.startDate.ToString("yyyy-MM-dd h:mm tt"),
                                              end_date = ssched.endDate.ToString("yyyy-MM-dd h:mm tt"),
                                              isWholeDay = leave.isWholeDay.ToString(),
                                              color = leave.backgroundColor,
                                              textColor = leave.textColor,
                                              remarks = ssched.remarks ?? "",
                                              queueno = "",
                                              sono = "",
                                              status = "",
                                              customername = "",
                                              leaveId = ssched.leaveId.ToString(),
                                              staffname = "",
                                              balance = ""
                                          }).ToList();



                        foreach (Schedule_m_Appointment sp in staffAppointment)
                        {
                            string balance = "";

                            DateTime startDate = sp.appointmentdate;
                            DateTime endDate = sp.appointmentdate;
                            TimeSpan sTime = DateTime.ParseExact(sp.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                            TimeSpan eTime = DateTime.ParseExact(sp.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                            var time = eTime - sTime;

                            startDate = startDate.Date.Add(sTime);
                            endDate = endDate.Date.Add(eTime);
                            Common_m_Staff staffdetails = db.Staffs.FirstOrDefault(x => x.id == sp.staffid);

                            if (sp.customercode != "" && sp.customercode != null)
                            {
                                balance = getOutstandingBalance(sp.customercode);
                            }

                            string text = "";
                            if (time.TotalHours < 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "," + sp.description + "," + sp.starttime + "-" + sp.endtime + "," + balance + "</p>";
                            }
                            else if (time.TotalHours >= 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.description + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.starttime + "-" + sp.endtime + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + balance + "</p>";
                            }

                            string color = "blue";

                            if (sp.status == "Cancelled")
                                color = "red";
                            else if (sp.status == "Confirmed")
                                color = "#00cc00";
                            else if (sp.status == "Arrived")
                                color = "#007f00";
                            else if (sp.status == "Done")
                                color = "#004C00";
                            else if (sp.status == "Wait List")
                                color = "#ffa500";
                            else if (sp.status == "No Show")
                                color = "#a6a6a6";
                            else
                                color = "blue";

                            staffSched.Add(new
                            {
                                id = sp.id,
                                text = text,
                                name = "Appointment",
                                staff_name = staffdetails.name,
                                type = "appointment",
                                unit_id = sp.staffid.ToString(),
                                start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                isWholeDay = "",
                                color = color,
                                textColor = "white",
                                remarks = "",
                                queueno = sp.queuenumber ?? "",
                                sono = sp.sonumber ?? "",
                                status = sp.status,
                                customername = sp.customername,
                                leaveId = "",
                                staffname = "",
                                balance = balance
                            });
                        }


                        foreach (Salesorder_m_salesorder so in salesOrder)
                        {
                            int sostaffid = Convert.ToInt32(so.staffid);
                            Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.departmentid == departmentid && x.staffid == sostaffid);
                            if (staffbranch != null)
                            {
                                DateTime startDate = so.createdate ?? DateTime.Now;
                                DateTime endDate = so.createdate ?? DateTime.Now;
                                TimeSpan sTime = DateTime.ParseExact(so.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                TimeSpan eTime = DateTime.ParseExact(so.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                                var time = eTime - sTime;

                                startDate = startDate.Date.Add(sTime);
                                endDate = endDate.Date.Add(eTime);
                                int staffid = Convert.ToInt32(so.staffid);
                                Common_m_Staff staffdetails = db.Staffs.FirstOrDefault(x => x.id == staffid);

                                string text = "";

                                if (time.TotalHours < 2)
                                {
                                    text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "," + so.resourcecode + "," + so.starttime + "-" + so.endtime + "</p>";
                                }
                                else if (time.TotalHours >= 2)
                                {
                                    text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.resourcecode + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.starttime + "-" + so.endtime + "</p>";
                                }

                                staffSched.Add(new
                                {
                                    id = so.id,
                                    text = text,
                                    name = "Queued Order",
                                    staff_name = staffdetails.name,
                                    type = "salesorder",
                                    unit_id = so.staffid.ToString(),
                                    start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                    end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                    isWholeDay = "",
                                    color = "white",
                                    textColor = "black",
                                    remarks = "",
                                    queueno = so.queuenumber ?? "",
                                    sono = so.resourcecode ?? "",
                                    status = so.status,
                                    customername = so.cussupname,
                                    leaveId = "",
                                    staffname = "",
                                    balance = ""
                                });
                            }
                        }

                        return Json(staffSched.OrderBy(x => x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                { return Json("Failed", JsonRequestBehavior.AllowGet); }
            }
        }

        public JsonResult UpdateStaffLeaveByDay(int id, int staffId)
        {
            var returnStr = "FAIL";


            try
            {
                Schedule_m_Staff staffSchedule = db.staffSchedule.FirstOrDefault(x => x.id == id);

                if (staffSchedule != null)
                {

                    staffSchedule.staffId = staffId;
                    staffSchedule.modifiedDate = DateTime.Now;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStaffAppointmentByDay(int id, int staffId, string starttime, string endtime)
        {
            var returnStr = "FAIL";


            try
            {
                Schedule_m_Appointment staffAppointment = db.appointment.FirstOrDefault(x => x.id == id);

                if (staffAppointment != null)
                {

                    staffAppointment.staffid = staffId;
                    staffAppointment.modifieddate = DateTime.Now;

                    TimeSpan sTime = DateTime.ParseExact(starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan eTime = DateTime.ParseExact(endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                    if (sTime.Minutes >= 1 && sTime.Minutes < 15)
                    {
                        int timeToAdd = 15 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 16 && sTime.Minutes < 29)
                    {
                        int timeToAdd = 30 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 31 && sTime.Minutes < 44)
                    {
                        int timeToAdd = 45 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));
                    }
                    else if (sTime.Minutes >= 46 && sTime.Minutes < 59)
                    {
                        int timeToAdd = 60 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }

                    DateTime dateStartTime = DateTime.Today.Add(sTime);
                    string displayStartTime = dateStartTime.ToString("hh:mm tt");

                    DateTime dateEndTime = DateTime.Today.Add(eTime);
                    string displayEndTime = dateEndTime.ToString("hh:mm tt");

                    staffAppointment.starttime = displayStartTime;
                    staffAppointment.endtime = displayEndTime;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStaffLeaveByWeek(int id, int staffId, DateTime date)
        {
            var returnStr = "FAIL";


            try
            {
                Schedule_m_Staff staffSchedule = db.staffSchedule.FirstOrDefault(x => x.id == id);
                Schedule_m_Leave leave = db.leave.Where(x => x.id == staffSchedule.leaveId).FirstOrDefault();

                if (staffSchedule != null)
                {
                    if (leave != null)
                    {

                        if (leave.isWholeDay == 1)
                        {
                            staffSchedule.startDate = date;
                            staffSchedule.endDate = date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                        else
                        {
                            string[] strt = leave.start.Split(':');
                            string[] end = leave.end.Split(':');

                            staffSchedule.startDate = date.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                            staffSchedule.endDate = date.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);
                        }

                        staffSchedule.staffId = staffId;
                        staffSchedule.modifiedDate = DateTime.Now;
                        db.SaveChanges();

                        returnStr = "SUCCESS";
                    }


                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStaffAppointmentByWeek(int id, int staffId, DateTime date, string starttime, string endtime)
        {
            var returnStr = "FAIL";


            try
            {
                Schedule_m_Appointment staffAppointment = db.appointment.FirstOrDefault(x => x.id == id);

                if (staffAppointment != null)
                {
                    staffAppointment.appointmentdate = date;
                    staffAppointment.staffid = staffId;
                    staffAppointment.modifieddate = DateTime.Now;

                    TimeSpan sTime = DateTime.ParseExact(starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan eTime = DateTime.ParseExact(endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                    if (sTime.Minutes >= 1 && sTime.Minutes < 15)
                    {
                        int timeToAdd = 15 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 16 && sTime.Minutes < 29)
                    {
                        int timeToAdd = 30 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 31 && sTime.Minutes < 44)
                    {
                        int timeToAdd = 45 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));
                    }
                    else if (sTime.Minutes >= 46 && sTime.Minutes < 59)
                    {
                        int timeToAdd = 60 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }

                    DateTime dateStartTime = DateTime.Today.Add(sTime);
                    string displayStartTime = dateStartTime.ToString("hh:mm tt");

                    DateTime dateEndTime = DateTime.Today.Add(eTime);
                    string displayEndTime = dateEndTime.ToString("hh:mm tt");

                    staffAppointment.starttime = displayStartTime;
                    staffAppointment.endtime = displayEndTime;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ByStaff
        public JsonResult GetScheduleByStaff(int id, string bcode)
        {
            {
                try
                {
                    using (var context = new BigMacEntities())
                    {

                        ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x => x.branchcode == bcode && x.staffId == id).ToList();
                        ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.staffid == id && (x.transactiontype == null || x.transactiontype == String.Empty)).ToList();

                        ICollection<Schedule_m_Holiday> holiday = db.holiday.OrderBy(x => x.holidayDate).ToList();
                        string staffid = id.ToString();
                        ICollection<Salesorder_m_salesorder> salesOrder = db.saleorders.Where(x => x.type == "ORDER TAKING" && x.branchcode == bcode && x.staffid == staffid).OrderBy(x => x.createdate).ToList();

                        var staffSched = (from ssched in staffLeave
                                          join leave in db.leave on ssched.leaveId equals leave.id
                                          select new
                                          {
                                              id = ssched.id,
                                              text = leave.leaveName,
                                              type = "leave",
                                              unit_id = ssched.staffId.ToString(),
                                              start_date = ssched.startDate.ToString("yyyy-MM-dd h:mm tt"),
                                              end_date = ssched.endDate.ToString("yyyy-MM-dd h:mm tt"),
                                              isWholeDay = leave.isWholeDay.ToString(),
                                              color = leave.backgroundColor,
                                              textColor = leave.textColor,
                                              remarks = ssched.remarks ?? "",
                                              queueno = "",
                                              sono = "",
                                              status = "",
                                              customername = "",
                                              leaveId = ssched.leaveId.ToString(),
                                              staffname = "",
                                              balance = ""
                                          }).ToList();


                        foreach (Schedule_m_Appointment sp in staffAppointment)
                        {
                            string staffname = "";
                            string balance = "";

                            DateTime startDate = sp.appointmentdate;
                            DateTime endDate = sp.appointmentdate;
                            TimeSpan sTime = DateTime.ParseExact(sp.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                            TimeSpan eTime = DateTime.ParseExact(sp.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                            var time = eTime - sTime;

                            startDate = startDate.Date.Add(sTime);
                            endDate = endDate.Date.Add(eTime);

                            Common_m_Staff staff = db.Staffs.Where(x => x.id == sp.staffid).FirstOrDefault();
                            if (staff != null)
                            {
                                staffname = staff.name;
                            }

                            if (sp.customercode != "" && sp.customercode != null)
                            {
                                balance = getOutstandingBalance(sp.customercode);
                            }

                            string text = "";
                            if (time.TotalHours < 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "," + sp.description + "," + sp.starttime + "-" + sp.endtime + "," + balance + "</p>";
                            }
                            else if (time.TotalHours >= 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + sp.customername + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.description + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + sp.starttime + "-" + sp.endtime + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + balance + "</p>";
                            }

                            string color = "blue";

                            if (sp.status == "Cancelled")
                                color = "red";
                            else if (sp.status == "Confirmed")
                                color = "#00cc00";
                            else if (sp.status == "Arrived")
                                color = "#007f00";
                            else if (sp.status == "Done")
                                color = "#004C00";
                            else if (sp.status == "Wait List")
                                color = "#ffa500";
                            else if (sp.status == "No Show")
                                color = "#a6a6a6";
                            else
                                color = "blue";

                            staffSched.Add(new
                            {
                                id = sp.id,
                                text = text,
                                type = "appointment",
                                unit_id = sp.staffid.ToString(),
                                start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                isWholeDay = "",
                                color = color,
                                textColor = "white",
                                remarks = "",
                                queueno = sp.queuenumber ?? "",
                                sono = sp.sonumber ?? "",
                                status = sp.status,
                                customername = sp.customername,
                                leaveId = "",
                                staffname = staffname,
                                balance = balance

                            });
                        }


                        foreach (Salesorder_m_salesorder so in salesOrder)
                        {
                            string staffname = "";
                            DateTime startDate = so.createdate ?? DateTime.Now;
                            DateTime endDate = so.createdate ?? DateTime.Now;
                            TimeSpan sTime = DateTime.ParseExact(so.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                            TimeSpan eTime = DateTime.ParseExact(so.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                            var time = eTime - sTime;

                            startDate = startDate.Date.Add(sTime);
                            endDate = endDate.Date.Add(eTime);
                            int sostaffid = Convert.ToInt32(so.staffid);
                            Common_m_Staff staff = db.Staffs.Where(x => x.id == sostaffid).FirstOrDefault();
                            if (staff != null)
                            {
                                staffname = staff.name;
                            }

                            string text = "";

                            if (time.TotalHours < 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "," + so.resourcecode + "," + so.starttime + "-" + so.endtime + "</p>";
                            }
                            else if (time.TotalHours >= 2)
                            {
                                text = "<p style='white-space: nowrap;margin-bottom:0px;'>" + so.cussupname + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.resourcecode + "</p><p style='white-space: nowrap;margin-bottom:0px;'>" + so.starttime + "-" + so.endtime + "</p>";
                            }

                            staffSched.Add(new
                            {
                                id = so.id,
                                text = text,
                                type = "salesorder",
                                unit_id = so.staffid.ToString(),
                                start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                isWholeDay = "",
                                color = "white",
                                textColor = "black",
                                remarks = "",
                                queueno = so.queuenumber ?? "",
                                sono = so.resourcecode ?? "",
                                status = so.status,
                                customername = so.cussupname,
                                leaveId = "",
                                staffname = staffname,
                                balance = ""
                            });
                        }

                        return Json(staffSched.OrderBy(x => x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                { return Json("Failed", JsonRequestBehavior.AllowGet); }
            }
        }

        public JsonResult UpdateStaffLeaveByStaff(int id, DateTime date)
        {
            var returnStr = "FAIL";

            try
            {
                Schedule_m_Staff staffSchedule = db.staffSchedule.FirstOrDefault(x => x.id == id);

                Schedule_m_Leave leave = db.leave.Where(x => x.id == staffSchedule.leaveId).FirstOrDefault();

                if (staffSchedule != null)
                {

                    if (leave != null)
                    {

                        if (leave.isWholeDay == 1)
                        {
                            staffSchedule.startDate = date;
                            staffSchedule.endDate = date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                        else
                        {
                            string[] strt = leave.start.Split(':');
                            string[] end = leave.end.Split(':');

                            staffSchedule.startDate = date.AddHours(Convert.ToDouble(strt[0])).AddMinutes(Convert.ToDouble(strt[1])).AddSeconds(00);
                            staffSchedule.endDate = date.AddHours(Convert.ToDouble(end[0])).AddMinutes(Convert.ToDouble(end[1])).AddSeconds(00);
                        }

                        staffSchedule.modifiedDate = DateTime.Now;
                        db.SaveChanges();

                        returnStr = "SUCCESS";
                    }
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStaffAppointmentByStaff(int id, DateTime date, string starttime, string endtime)
        {
            var returnStr = "FAIL";

            try
            {
                Schedule_m_Appointment staffAppointment = db.appointment.FirstOrDefault(x => x.id == id);

                if (staffAppointment != null)
                {
                    staffAppointment.appointmentdate = date;
                    staffAppointment.modifieddate = DateTime.Now;

                    TimeSpan sTime = DateTime.ParseExact(starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan eTime = DateTime.ParseExact(endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                    if (sTime.Minutes >= 1 && sTime.Minutes < 15)
                    {
                        int timeToAdd = 15 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 16 && sTime.Minutes < 29)
                    {
                        int timeToAdd = 30 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }
                    else if (sTime.Minutes >= 31 && sTime.Minutes < 44)
                    {
                        int timeToAdd = 45 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));
                    }
                    else if (sTime.Minutes >= 46 && sTime.Minutes < 59)
                    {
                        int timeToAdd = 60 - sTime.Minutes;
                        sTime = sTime.Add(new TimeSpan(0, timeToAdd, 0));
                        eTime = eTime.Add(new TimeSpan(0, timeToAdd, 0));

                    }

                    DateTime dateStartTime = DateTime.Today.Add(sTime);
                    string displayStartTime = dateStartTime.ToString("hh:mm tt");

                    DateTime dateEndTime = DateTime.Today.Add(eTime);
                    string displayEndTime = dateEndTime.ToString("hh:mm tt");

                    staffAppointment.starttime = displayStartTime;
                    staffAppointment.endtime = displayEndTime;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ByMonth
        public JsonResult GetAllStaffScheduleByMonth(string bcode, int departmentid)
        {
            {
                try
                {
                    using (var context = new BigMacEntities())
                    {

                        var staffList = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode && x.BranchStaff.departmentid == departmentid).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                        ICollection<Schedule_m_Staff> staffLeave = db.staffSchedule.Where(x => x.branchcode == bcode && x.departmentid == departmentid).ToList();
                        ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.departmentid == departmentid && (x.transactiontype == null || x.transactiontype == String.Empty)).ToList();
                        ICollection<Salesorder_m_salesorder> salesOrder = db.saleorders.Where(x => x.type == "ORDER TAKING" && x.branchcode == bcode).OrderBy(x => x.createdate).ToList();

                        var staffSched = (from ssched in staffLeave
                                          join leave in db.leave on ssched.leaveId equals leave.id
                                          join staff in db.Staffs on ssched.staffId equals staff.id
                                          select new
                                          {
                                              id = ssched.id,
                                              text = staff.name,
                                              staffName = staff.name,
                                              eventType = leave.leaveName,
                                              type = "leave",
                                              unit_id = ssched.staffId.ToString(),
                                              start_date = ssched.startDate.ToString("yyyy-MM-dd h:mm tt"),
                                              end_date = ssched.endDate.ToString("yyyy-MM-dd h:mm tt"),
                                              isWholeDay = leave.isWholeDay.ToString(),
                                              color = leave.backgroundColor,
                                              textColor = leave.textColor,
                                              remarks = ssched.remarks ?? "",
                                              queueno = "",
                                              sono = "",
                                              status = "",
                                              customername = "",
                                              leaveId = ssched.leaveId.ToString(),
                                          }).ToList();


                        foreach (Schedule_m_Appointment sp in staffAppointment)
                        {
                            DateTime startDate = sp.appointmentdate;
                            DateTime endDate = sp.appointmentdate;
                            TimeSpan sTime = DateTime.ParseExact(sp.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                            TimeSpan eTime = DateTime.ParseExact(sp.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                            startDate = startDate.Date.Add(sTime);
                            endDate = endDate.Date.Add(eTime);
                            Common_m_Staff staffdetails = db.Staffs.FirstOrDefault(x => x.id == sp.staffid);

                            string color = "blue";

                            if (sp.status == "Cancelled")
                                color = "red";
                            else if (sp.status == "Confirmed")
                                color = "#00cc00";
                            else if (sp.status == "Arrived")
                                color = "#007f00";
                            else if (sp.status == "Done")
                                color = "#004C00";
                            else if (sp.status == "Wait List")
                                color = "#ffa500";
                            else if (sp.status == "No Show")
                                color = "#a6a6a6";
                            else
                                color = "blue";

                            staffSched.Add(new
                            {
                                id = sp.id,
                                text = staffdetails.name,
                                staffName = staffdetails.name,
                                eventType = "Appointment",
                                type = "appointment",
                                unit_id = sp.staffid.ToString(),
                                start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                isWholeDay = "",
                                color = color,
                                textColor = "white",
                                remarks = "",
                                queueno = sp.queuenumber ?? "",
                                sono = sp.sonumber ?? "",
                                status = sp.status,
                                customername = sp.customername,
                                leaveId = "",
                            });
                        }

                        foreach (Salesorder_m_salesorder so in salesOrder)
                        {
                            int sostaffid = Convert.ToInt32(so.staffid);
                            Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.departmentid == departmentid && x.staffid == sostaffid);
                            if (staffbranch != null)
                            {
                                DateTime startDate = so.createdate ?? DateTime.Now;
                                DateTime endDate = so.createdate ?? DateTime.Now;
                                TimeSpan sTime = DateTime.ParseExact(so.starttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                                TimeSpan eTime = DateTime.ParseExact(so.endtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                                startDate = startDate.Date.Add(sTime);
                                endDate = endDate.Date.Add(eTime);
                                Common_m_Staff staffdetails = db.Staffs.FirstOrDefault(x => x.id == sostaffid);

                                staffSched.Add(new
                                {
                                    id = so.id,
                                    text = staffdetails.name,
                                    staffName = staffdetails.name,
                                    eventType = "Queued Order",
                                    type = "salesorder",
                                    unit_id = so.staffid.ToString(),
                                    start_date = startDate.ToString("yyyy-MM-dd h:mm tt"),
                                    end_date = endDate.ToString("yyyy-MM-dd h:mm tt"),
                                    isWholeDay = "",
                                    color = "white",
                                    textColor = "black",
                                    remarks = "",
                                    queueno = so.queuenumber ?? "",
                                    sono = so.resourcecode ?? "",
                                    status = so.status,
                                    customername = so.cussupname,
                                    leaveId = "",
                                });
                            }
                        }


                        return Json(staffSched.OrderBy(x => x.start_date).ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                { return Json("Failed", JsonRequestBehavior.AllowGet); }
            }
        }
        #endregion
        public JsonResult UpdateStaffAppointmentByMonth(int id, DateTime date)
        {
            var returnStr = "FAIL";


            try
            {
                Schedule_m_Appointment staffAppointment = db.appointment.FirstOrDefault(x => x.id == id);

                if (staffAppointment != null)
                {
                    staffAppointment.appointmentdate = date;
                    staffAppointment.modifieddate = DateTime.Now;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }


        #region Modal Methods
        [HttpPost]
        public JsonResult UpdateStaffSchedRemark(int id, string remarks)
        {
            var returnStr = "FAIL";

            try
            {
                Schedule_m_Staff staffSchedule = db.staffSchedule.FirstOrDefault(x => x.id == id);

                if (staffSchedule != null)
                {
                    staffSchedule.remarks = remarks;
                    staffSchedule.modifiedDate = DateTime.Now;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesOrderDetails(int id)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Salesorder_m_salesorder> salesorder = db.saleorders.Where(x => x.id == id).ToList();
                    int branchassetid = salesorder.ElementAt(0).branchassetid ?? 0;
                    Common_m_BranchAsset branchasset = db.BranchAsset.FirstOrDefault(x => x.id == branchassetid);
                    salesorder.ElementAt(0).contactperson = branchasset.name;
                    return Json(salesorder, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getPackageDetail(int mid)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Redempt_Package> pptmp = context.Database.SqlQuery<Redempt_Package>("call getCussupPackageItemList(" + mid + ")").ToList();
                    ICollection<Package_Summary> packages = new List<Package_Summary>();
                    foreach (Redempt_Package sp in pptmp)
                    {

                        CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == mid && x.packagecode == sp.packagecode && x.productid == sp.productid && x.uom == sp.uom).OrderBy(x => x.createdate).FirstOrDefault();
                        Package_Summary summary = new Package_Summary();
                        summary.date = redempt.lastmodifieddate;
                        summary.receipt = redempt.remark ?? redempt.RefNo;
                        summary.packagecode = sp.packagecode;
                        summary.packagedesc = sp.packagedesc;
                        summary.productid = sp.productid;
                        summary.productdesc = sp.productdesc;
                        summary.uom = sp.uom;
                        summary.credit = sp.credit;
                        summary.debit = sp.debit;
                        summary.balance = sp.balance;

                        packages.Add(summary);

                    }

                    var fList = (from product in packages
                                 join package in db.products on product.packagecode equals package.productcode
                                 select new
                                 {
                                     id = package.id,
                                     packagecode = product.packagecode,
                                     date = product.date,
                                     receipt = product.receipt,
                                     packagedesc = product.packagedesc,
                                     productdesc = product.productdesc,
                                     productid = product.productid,
                                     credit = product.credit,
                                     debit = product.debit,
                                     uom = product.uom,
                                     balance = product.balance
                                 }).ToList();



                    return Json(fList.Where(x => x.balance > 0).ToList(), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getAppointmentHistory(string code)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Schedule_m_Appointment> list = db.appointment.Where(x => x.customercode == code && (x.transactiontype == null || x.transactiontype == String.Empty)).OrderByDescending(x => x.appointmentdate).ToList();

                    var appHistoryList = (from hist in list
                                          select new
                                          {
                                              id = hist.id,
                                              description = hist.description,
                                              status = hist.status,
                                              appointmentdate = hist.appointmentdate.ToString("MM/dd/yyyy"),
                                              starttime = hist.starttime,
                                              endtime = hist.endtime,

                                          }).ToList();

                    return Json(appHistoryList, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult GetCustomerIdByCode(string code)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    int id = 0;

                    CusSup_m_CusSup cussup = db.CusSup.Where(x => x.inhousecode == code).FirstOrDefault();

                    if (cussup != null)
                    {
                        id = cussup.id;
                    }

                    return Json(id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }


        #region Appointment

        [HttpPost]
        public JsonResult AddNewAppointment(Schedule_m_Appointment app, string bcode)
        {
            var returnStr = "FAIL";

            try
            {
                Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.branchcode == bcode && x.staffid == app.staffid);

                int deptId = 0;
                if (staffbranch != null)
                    deptId = staffbranch.departmentid;

                app.createddate = DateTime.Now;
                app.modifieddate = app.createddate;
                app.branchcode = bcode;
                app.queuenumber = "";
                app.departmentid = deptId;
                db.appointment.Add(app);
                db.SaveChanges();

                SendSMS(app.customername, app.mobile, bcode, app.appointmentdate.ToString("dd/MM/yyyy"), app.starttime + "-" + app.endtime);

                if (app.email != "")
                    SendEmail(app);

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAppointment(Schedule_m_Appointment app)
        {
            var returnStr = "FAIL";

            try
            {


                Schedule_m_Appointment sp = db.appointment.FirstOrDefault(x => x.id == app.id);


                Common_m_StaffBranch staffbranch = db.BranchStaff.FirstOrDefault(x => x.branchcode == sp.branchcode && x.staffid == app.staffid);

                int deptId = 0;
                if (staffbranch != null)
                    deptId = staffbranch.departmentid;

                if (sp != null)
                {
                    sp.modifieddate = DateTime.Now;
                    sp.customername = app.customername;
                    sp.customercode = app.customercode;
                    sp.nric = app.nric;
                    sp.description = app.description;
                    sp.endtime = app.endtime;
                    sp.starttime = app.starttime;
                    sp.status = app.status;
                    sp.mobile = app.mobile;
                    sp.branchassetid = app.branchassetid;
                    sp.staffid = app.staffid;
                    sp.email = app.email;
                    sp.channel = app.channel;
                    sp.departmentid = deptId;
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }


            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAppointmentDetails(int id)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    int soid = 0;
                    ICollection<Schedule_m_Appointment> appointment = db.appointment.Where(x => x.id == id).ToList();
                    string sonumber = appointment.ElementAt(0).sonumber;

                    Salesorder_m_salesorder so = db.saleorders.FirstOrDefault(x => x.resourcecode == sonumber);
                    if (so != null)
                    {
                        soid = so.id;
                    }
                    var appointDetails = (from app in appointment
                                          select new
                                          {
                                              id = app.id,
                                              customername = app.customername,
                                              customercode = app.customercode,
                                              mobile = app.mobile,
                                              nric = app.nric,
                                              email = app.email,
                                              staffid = app.staffid,
                                              branchassetid = app.branchassetid,
                                              bookedby = app.bookedby,
                                              description = app.description,
                                              status = app.status,
                                              starttime = app.starttime,
                                              endtime = app.endtime,
                                              appointmentdate = app.appointmentdate.ToString("dd/MM/yyyy"),
                                              sonumber = app.sonumber,
                                              queuenumber = app.queuenumber,
                                              soid = soid,
                                              channel = app.channel
                                          }).ToList();

                    return Json(appointDetails, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult GetBranchDetails(string bcode)
        {
            try
            {
                if (bcode == null)
                {
                    bcode = Session["branchcode"].ToString();
                }

                using (var context = new BigMacEntities())
                {
                    ICollection<Configuration_m_Branches> branch = db.Branches.Where(x => x.branchcode == bcode).ToList();

                    var branchdetails = (from b in branch
                                         select new
                                         {
                                             id = b.id,
                                             branchname = cAESEncryption.getDecryptedString(b.branchname),
                                             email = cAESEncryption.getDecryptedString(b.email),
                                             fax = cAESEncryption.getDecryptedString(b.fax),
                                             tel = cAESEncryption.getDecryptedString(b.tel),
                                             address = cAESEncryption.getDecryptedString(b.address)

                                         }).ToList();

                    return Json(branchdetails, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }




        #endregion

        #endregion

        #region Autocomplete
        [Authorize]
        public JsonResult getMemberInfoByName(string custInfo = "", int count = 10)
        {
            try
            {
                var ctmplist = db.CusSup.Where(x => x.status == "Active").ToList();
                var clist = ctmplist.Where(x => x.cussupname.ToUpper().Contains(custInfo.ToUpper())).Select(x => new
                {
                    id = x.id,
                    inhousecode = x.inhousecode,
                    cussupname = x.cussupname,
                    nric = cAESEncryption.getDecryptedString(x.nric),
                    mobile = cAESEncryption.getDecryptedString(x.mobile),
                    email = x.email
                }).ToList();

                clist = clist.OrderBy(x => x.cussupname).Take(count).ToList();
                return Json(clist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public JsonResult getMemberInfoByCode(string custInfo = "", int count = 10)
        {
            try
            {
                var ctmplist = db.CusSup.Where(x => x.status == "Active").ToList();
                var clist = ctmplist.Where(x => x.inhousecode.ToUpper().Contains(custInfo.ToUpper())).Select(x => new
                {
                    id = x.id,
                    inhousecode = x.inhousecode,
                    cussupname = x.cussupname,
                    nric = cAESEncryption.getDecryptedString(x.nric),
                    mobile = cAESEncryption.getDecryptedString(x.mobile),
                    email = x.email
                }).ToList();


                clist = clist.OrderBy(x => x.cussupname).Take(count).ToList();
                return Json(clist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        #endregion


        #region Order Taking
        public JsonResult GetAllAppointmentByBranch()
        {
            {
                try
                {
                    using (var context = new BigMacEntities())
                    {
                        string bcode = Session["branchcode"].ToString();

                        ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.status == "Booking" || x.status == "Wait List" && (x.transactiontype == null || x.transactiontype == String.Empty)).OrderBy(x => x.appointmentdate).ThenBy(x => x.status).ToList();

                        var appointmentList = (from app in staffAppointment
                                               join staff in db.Staffs on app.staffid equals staff.id
                                               join asset in db.BranchAsset on app.branchassetid equals asset.id
                                               select new
                                               {
                                                   id = app.id,
                                                   appdate = app.appointmentdate,
                                                   time = app.starttime + " - " + app.endtime,
                                                   customername = app.customername,
                                                   mobile = app.mobile,
                                                   nric = app.nric,
                                                   staffname = staff.name,
                                                   room = asset.name,
                                                   description = app.description ?? "",
                                                   status = app.status
                                               }).ToList();

                        return Json(appointmentList, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                { return Json("Failed", JsonRequestBehavior.AllowGet); }
            }
        }
        #endregion

        #region Message

        public void SendSMS(string customername, string mobile, string bcode, string appdate, string time)
        {
            try
            {
                SMSWS.MsgSender objSMSGW = new SMSWS.MsgSender();

                Common_m_Message sms = getMessageByCode("SMS_4");
                var msg = "";

                Configuration_m_Branches branches = db.Branches.Where(x => x.branchcode == bcode).FirstOrDefault();

                msg = sms.message.Replace("XXX", customername);
                msg = (branches != null) ? msg.Replace("XbranchX", cAESEncryption.getDecryptedString(branches.branchname)) : msg.Replace("XbranchX", "");
                msg = msg.Replace("XappdateX", appdate);
                msg = msg.Replace("XtimeX", time);
                msg = msg.Replace("XContactX", cAESEncryption.getDecryptedString(branches.tel));

                var strSMS = mobile + "||" + msg;
                objSMSGW.SendMessage(sms.from, strSMS);

            }
            catch (Exception e)
            {

            }
        }

        public void SendEmail(Schedule_m_Appointment app)
        {
            try
            {
                string outlet = "";
                string booked = "";
                string staffname = "";
                string room = "";
                string msg = "";

                AppSettings appSettings = new AppSettings();
                Common_m_Message email = getMessageByCode("EMAIL_4");

                Configuration_m_Branches branches = db.Branches.Where(x => x.branchcode == app.branchcode).FirstOrDefault();

                if (branches != null)
                    outlet = cAESEncryption.getDecryptedString(branches.branchname);

                Common_m_Staff staff = db.Staffs.Where(x => x.id == app.staffid).FirstOrDefault();

                if (staff != null)
                    staffname = staff.name;

                Common_m_Staff bookedby = db.Staffs.Where(x => x.id == app.bookedby).FirstOrDefault();

                if (bookedby != null)
                    booked = bookedby.name;

                Common_m_BranchAsset branchasset = db.BranchAsset.Where(x => x.id == app.branchassetid).FirstOrDefault();

                if (branchasset != null)
                    room = branchasset.name;

                msg = "<html><body style='font-family: Helvetica,Arial,sans-serif;font-size: 1.1em'><br/>";
                msg = msg + "<table><tr>";
                msg = msg + "<td></td><td> </td><td style='font-size:30px;'><b>Your " + appSettings.appName + " Appointment</b></td></tr>";
                msg = msg + " <tr><td>Appointment Date</td><td>:</td><td><b>" + app.appointmentdate.ToString("dd/MM/yyyy") + "</b></td></tr>";
                msg = msg + " <tr><td>Time</td><td>:</td><td><b>" + app.starttime + " - " + app.endtime + "</b></td></tr>";
                msg = msg + " <tr><td>Room</td><td>:</td><td><b>" + room + "</b></td></tr>";
                msg = msg + " <tr><td>Outlet</td><td>:</td><td><b>" + outlet + "</b></td></tr>";
                msg = msg + " <tr><td>Name</td><td>:</td><td><b>" + app.customername + "</b></td></tr>";
                msg = msg + " <tr><td>Code</td><td>:</td><td><b>" + app.customercode + "</b></td></tr>";
                msg = msg + " <tr><td>HP</td><td>:</td><td><b>" + app.mobile + "</b></td></tr>";
                msg = msg + " <tr><td>NRIC</td><td>:</td><td><b>" + app.nric + "</b></td></tr>";
                msg = msg + " <tr><td>Description</td><td>:</td><td><b>" + app.description + "</b></td></tr>";
                msg = msg + " <tr><td>Staff Name</td><td>:</td><td><b>" + staffname + "</b></td></tr>";
                msg = msg + " <tr><td>Booked By</td><td>:</td><td><b>" + booked + "</b></td></tr>";

                msg = msg + "</table>";
                msg = msg + email.footer;
                msg = msg + "</body></html>";

                var strSubj = appSettings.appName + " Appointment";

                if (app.email != null)
                {
                    MailMessage mailmsg = new MailMessage();
                    mailmsg.From = new MailAddress(email.from);
                    mailmsg.To.Add(app.email);

                    if (email.bcc != null)
                    {
                        if (email.bcc.Trim() != "")
                        {
                            string[] bccList = email.bcc.Split(';');
                            foreach (string bcc in bccList)
                            {
                                mailmsg.Bcc.Add(bcc);
                            }
                        }
                    }

                    if (email.cc != null)
                    {
                        if (email.cc.Trim() != "")
                        {
                            string[] ccList = email.cc.Split(';');
                            foreach (string cc in ccList)
                            {
                                mailmsg.CC.Add(cc);
                            }
                        }
                    }

                    mailmsg.Subject = strSubj;
                    mailmsg.Body = msg;
                    mailmsg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.buy2go.com";
                    smtp.Port = 25;
                    smtp.Credentials = new NetworkCredential("Helpdesk@buy2go.com", "Info121@sa");
                    smtp.Send(mailmsg);
                }
            }
            catch (Exception e)
            {

            }
        }

        public Common_m_Message getMessageByCode(string messagecode)
        {
            Common_m_Message message = db.Message.Where(x => x.messagecode == messagecode).FirstOrDefault();

            return message;
        }
        #endregion


        public string getOutstandingBalance(string cussupcode)
        {
            try
            {
                string balance = "";
                double total_subtotal = 0.00;
                using (var context = new BigMacEntities())
                {

                    CusSup_m_CusSup cussup = db.CusSup.FirstOrDefault(x => x.inhousecode == cussupcode);

                    if (cussup != null)
                    {
                        var sList = context.Database.SqlQuery<Salesorder_m_salesorder>("call getActiveSOList()").ToList();
                        var obList = sList.Where(x => x.cussupid == cussup.id).ToList();
                        foreach (Salesorder_m_salesorder so in obList)
                        {
                            total_subtotal += so.total_subtotal;
                        }
                        balance = total_subtotal.ToString();
                    }

                    return balance;
                }
            }
            catch (Exception e)
            { return "0.00"; }
        }

    }
}
