using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using MySql.Data.MySqlClient;

namespace BigMac.Controllers
{
    public class GeneralController : Controller
    {
        //
        // GET: /General/
        private BigMacEntities db = new BigMacEntities();

        public static string getGeneratedNewID(string tblname = "CusSup_m_CusSup", string colname = "inhousecode", string prekeyvalue = "MEMPREFIX", string startnokeyvalue = "MEMNO")
        {
            string returnvalue = "";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {

                    MySqlCommand cmd = new MySqlCommand("getGenerateNewID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("tblname", tblname));
                    cmd.Parameters.Add(new MySqlParameter("colname", colname));
                    cmd.Parameters.Add(new MySqlParameter("prekeyvalue", prekeyvalue));
                    cmd.Parameters.Add(new MySqlParameter("startnokeyvalue", startnokeyvalue));
                    var Result = new MySqlParameter();
                    Result.ParameterName = "newcode";
                    Result.MySqlDbType = MySqlDbType.VarChar;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Result);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    returnvalue = Result.Value.ToString();
                    //cmd.Connection.Close();
                    //var Result = new Object();
                    //var val = context.Database.ExecuteSqlCommand("call getGenerateNewID('" + tblname + "','" + colname + "','" + prekeyvalue + "','" + startnokeyvalue + "'," + Result + ")");

                }
                catch (Exception e)
                { }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
            return returnvalue;
        }

        public static string generateNewID(string BranchCode, string ResourceCode,string TBLName)
        {
            string returnValue = "";
            return returnValue;
        }
        public ActionResult AccessTypeIndex()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult getCampaignDetailGrid(int campaignid, string branchcode, int groupid, int t1,int t2, int t3)
        public JsonResult getAccessType()
        {
            //var returnHtml = "<table class='table table-striped table-bordered bootstrap-datatable datatable'><thead>";
            try {
                ICollection<Access_z_AccessTypes> ac = db.AccessTypes.ToList();
                return Json(ac, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return Content(returnHtml);
        }

        [HttpPost]
        public ActionResult AccessTypeSave(string uname, string at,int id=0)
        {
            string returnStr = "";
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(user).State = EntityState.Modified;
                    //sid = Convert.Toint(Request.Params["staffid"].ToString());
                    if (id == 0)
                    {
                        Access_z_AccessTypes accesstype = new Access_z_AccessTypes();
                        accesstype.accesstype = at;
                        accesstype.createdate = DateTime.Now;
                        //accesstype.lastmodifieddate = DateTime.Now;
                        db.AccessTypes.Add(accesstype);
                        db.SaveChanges();
                        returnStr = "SUCCESS";                        
                    }
                    else
                    {
                        Access_z_AccessTypes accesstype = db.AccessTypes.Find(id);
                        accesstype.accesstype = at;
                        //accesstype.createdate = DateTime.Now;
                        //accesstype.lastmodifieddate = DateTime.Now;
                        db.SaveChanges(); 
                        returnStr = "SUCCESS";

                    }
                }

            }
            catch (Exception e) { returnStr = e.Message.ToString(); }
            return Content(returnStr);
        }

        [HttpPost]
        public ActionResult AccessTypeDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Access_z_AccessTypes accesstype = db.AccessTypes.Find(id);
                db.AccessTypes.Remove(accesstype);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }
        //Added by ZawZO on 4 Jun 2015
        public int getResourceID(string rcode)
        {
            int returnValue = 0;
            try
            {
                var rgcode = "";
                if (rcode.IndexOf("_") >= 0)
                {
                    rgcode = rcode.Substring(0, rcode.IndexOf("_"));
                    rcode = rcode.Substring(rcode.IndexOf("_") + 1);
                }
                //rcode = rcode.IndexOf("_");
                var ridtmp = db.Resources.Where(x => x.resource.ToUpper() == rcode.ToUpper() && x.resourcegroupcode.ToUpper() == rgcode.ToUpper()).FirstOrDefault().id;

                if (ridtmp != null) returnValue = Convert.ToInt32(ridtmp);

            }
            catch (Exception e)
            {

            }
            return returnValue;
        }
        public Boolean SaveToLog(int uid, string cocode, string sessionnid, int resourceid, int resourcecode, string logtype, string description, string from = "", string to = "", string ip = "")
        {
            Boolean returnValue = false;
            try
            {
                Logs_m_Logs log = new Logs_m_Logs();
                log.userid = uid; //Convert.ToInt32(Session["userid"].ToString());
                log.cocode = cocode;
                log.createdate = DateTime.Now;
                log.lastmodifieddate = DateTime.Now;
                log.logtype = logtype;
                log.resourcecode = resourcecode;
                log.resourceid = resourceid;
                log.description = description;
                log.ipaddress = ip;
                log.from = from;
                log.to = to;
                log.session = sessionnid ;
                db.logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception e)
            { }

            return returnValue;
        }

    }
}
