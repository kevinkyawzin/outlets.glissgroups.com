using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
//Created by ZawZO on 21 May 2015
namespace BigMac.Controllers
{
    public class AttendanceController : Controller
    {
        private BigMacEntities db = new BigMacEntities();
        //Added by ZawZO on 22 May 2015
        [Authorize]
        public ActionResult AttdLogIn(string uname, string pwd)
        {
            var returnStr = "0";
            ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x => x.status.ToUpper() == "ACTIVE").ToList();
            Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname.ToUpper() && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();
            if (u != null)
            {
                Common_m_Staff s = db.Staffs.Where(x => x.userid == u.id).FirstOrDefault();
                
                if (s != null)
                {
                    var bcode = Session["branchcode"];
                    if (IsLogInExist(bcode.ToString(), s.id.ToString()))
                    {
                        returnStr = "-2";
                    }
                    else
                    {
                        using (var context = new BigMacEntities())
                        {
                            context.Database.ExecuteSqlCommand("call AttendanceLogIn('" + bcode + "'," + s.id.ToString() + ",'" + s.name + "');");
                            returnStr = "1";
                        }
                    }
                    
                }
                else
                {
                    returnStr = "-1";
                }
                
            }
            else
            {
                returnStr = "-1";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 22 May 2015
        [Authorize]
        public ActionResult AttdLogOut(string uname, string pwd)
        {
            var returnStr = "0";
            ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x => x.status.ToUpper() == "ACTIVE").ToList();
            Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname.ToUpper() && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();
            if (u != null)
            {
                Common_m_Staff s = db.Staffs.Where(x => x.userid == u.id).FirstOrDefault();

                if (s != null)
                {
                    using (var context = new BigMacEntities())
                    {
                        var bcode = Session["branchcode"];

                        if (IsLogInExist(bcode.ToString(),s.id.ToString()))
                        {
                            context.Database.ExecuteSqlCommand("call AttendanceLogOut('" + bcode + "'," + s.id.ToString() + ",'" + s.name + "');");
                            returnStr = "1";
                        }
                        else 
                        { 
                            returnStr = "-2"; 
                        }
                    }
                }
                else
                {
                    returnStr = "-1";
                }

            }
            else
            {
                returnStr = "-1";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 22 May 2015
        public Boolean IsLogInExist( string bcode,string sID )
        {
            Boolean returnvalue = false;
            try
            {
                using (var context = new BigMacEntities())
                {
                    var strSQL ="SELECT * FROM attendance_m_attendancedetail WHERE YEAR(attendancein)=YEAR(NOW()) AND MONTH(attendancein)=MONTH(NOW()) AND DAY(attendancein)=DAY(NOW()) AND staffid=" + sID.ToString() + " ORDER BY CreateDate DESC";
                    attendance_m_attendancedetail attn = context.Database.SqlQuery<attendance_m_attendancedetail>(strSQL).FirstOrDefault();
                    if (attn != null)
                    {
                        if (attn.attendanceout == null)
                        {
                            returnvalue = true;  
                        }
                        else
                        {
                            returnvalue = false;
                        }
                    }
                    else
                    {
                        returnvalue = false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return returnvalue;
        }
    }
}