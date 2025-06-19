using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using MySql.Data.MySqlClient; 
using Microsoft.Web.Helpers;
using System.Web.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;

namespace BigMac.Controllers
{
    public class CusSupController : Controller
    {
        //
        // GET: /CusSup/
        private BigMacEntities db = new BigMacEntities();

        [Authorize]
        public ActionResult getMemberProductListWithPaging(int cussupid, jQueryDataTableParamModel param)
        {
            try
            {
                //var pList = db.Treatments.Where(x => x.cussupid == cussupid).Join(db.Staffs, t => t.staffid, staff => staff.id, (t, staff) => new { Treatments = t, Staffs = staff }).Select(x => new { id = x.Treatments.id, createdate = x.Treatments.createdate, createby = x.Treatments.createby, createbyname = x.Staffs.name, resourcedetailid = x.Treatments.resourcedetailid, resourcecode = x.Treatments.resourcecode, description = x.Treatments.description, keyid = "0", statusfield = "E" }).ToList();
                var plist = db.sales.Where(x => x.cussupid == cussupid && x.type != "TOPUP" && x.status == "CLOSE").Join(db.saleItems, hdr => hdr.id, dtl => dtl.invoiceid, (hdr, dtl) => new { sales = hdr, saleItems = dtl }).Select(x => new { x.saleItems.invoiceid, x.saleItems.resourcecode, x.saleItems.createdate, x.saleItems.productcode, x.saleItems.productdesc, x.sales.remark, x.saleItems.qty, x.saleItems.unitprice, x.saleItems.lineamount }).ToList();
                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;

                searchValue = searchValue.ToUpper();
                var filterTList = plist.Where(x => x.resourcecode.ToUpper().Contains(searchValue) || x.createdate.ToString().Contains(searchValue) || x.productcode.ToUpper().Contains(searchValue) || x.productdesc.ToUpper().Contains(searchValue) || x.remark.ToUpper().Contains(searchValue)).ToList();
               
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc

                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderBy(x => x.productcode).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderBy(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderBy(x => x.qty).ToList();
                    else if (sortColumnIndex == 5)
                        filterTList = filterTList.OrderBy(x => x.unitprice).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderBy(x => x.lineamount).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderByDescending(x => x.productcode).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderByDescending(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderByDescending(x => x.qty).ToList();
                    else if (sortColumnIndex == 5)
                        filterTList = filterTList.OrderByDescending(x => x.unitprice).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderByDescending(x => x.lineamount).ToList();
                }

                var paginatedQPList = filterTList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = plist.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterTList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }


        [Authorize]
        public JsonResult getMemberTreatmentList(int cussupid = 0)
        {
            try
            {
                var tList = db.Treatments.Where(x => x.cussupid == cussupid).Join(db.Staffs, t => t.staffid, staff => staff.id, (t, staff) => new { Treatments = t, Staffs = staff }).Select(x => new { id = x.Treatments.id, createdate = x.Treatments.createdate, createby = x.Treatments.createby, createbyname = x.Staffs.name, resourcedetailid = x.Treatments.resourcedetailid, resourcecode = x.Treatments.resourcecode, description = x.Treatments.description, keyid = "0", statusfield = "E" }).ToList();
                return Json(tList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public ActionResult getMemberTreatmentListWithPaging(int cussupid,jQueryDataTableParamModel param)
        {
            try
            {
                ////var tList = db.Treatments.Where(x => x.cussupid == cussupid).Join(db.Staffs, t => t.staffid, staff => staff.id, (t, staff) => new { Treatments = t, Staffs = staff }).Select(x => new { id = x.Treatments.id, createdate = x.Treatments.createdate, createby = x.Treatments.createby, createbyname = x.Staffs.name, resourcedetailid = x.Treatments.resourcedetailid, resourcecode = x.Treatments.resourcecode, description = x.Treatments.description, keyid = "0", statusfield = "E" }).ToList();
                //var tList = (from t in db.Treatments join staff in db.Staffs on t.staffid equals staff.id join sale in db.sales on t.resourcecode equals sale.resourcecode where t.cussupid ==cussupid select new { id = t.id, createdate = t.createdate, createby = t.createby, createbyname = staff.name, resourcedetailid = t.resourcedetailid, resourcecode = t.resourcecode, description = t.description, remark = sale.remark, keyid = "0", statusfield = "E" }).ToList();

                var tList = (from t in db.Treatments
                             join staff in db.Staffs on t.staffid equals staff.id
                             where t.cussupid == cussupid
                             select new
                             {
                                 id = t.id,
                                 createdate = t.createdate,
                                 createby = t.createby,
                                 createbyname = staff.name,
                                 resourcedetailid = t.resourcedetailid,
                                 resourcecode = t.resourcecode,
                                 description = t.description ?? "",
                                 remarks2 = t.remarks2 ?? "",
                                 remarks3 = t.remarks3 ?? "",
                                 remarks4 = t.remarks4 ?? "",
                                 type = t.type ?? ""
                             }).ToList();
                
                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;

                searchValue = searchValue.ToUpper();
                var filterTList = tList.Where(x => x.createbyname.ToUpper().Contains(searchValue) || x.createdate.ToString().Contains(searchValue) || x.description.ToUpper().Contains(searchValue) || x.resourcecode.ToUpper().Contains(searchValue)).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
              
                var sortDirection = Request["sSortDir_0"]; // asc or desc

                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderBy(x => x.createbyname).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderBy(x => x.description).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderBy(x => x.type).ToList();

                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderByDescending(x => x.createbyname).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderByDescending(x => x.description).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderByDescending(x => x.type).ToList();
                }

                var paginatedQPList = filterTList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
   
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = tList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterTList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public JsonResult AddMemberTreatment(cussup_m_treatment t, string rcode = "")
        {
            try
            {
                //cussup_m_treatment t = new cussup_m_treatment();
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                int sid = 0;
                int.TryParse(Session["staffid"].ToString(), out sid);

                t.createby = Convert.ToInt32(Session["userid"]);
                t.staffid = sid;
                t.createdate = DateTime.Now;       
                db.Treatments.Add(t);
                db.SaveChanges();
                saveToLog(rid, t.id, "CREATE", "Add New Treatment - Member -" + t.cussupname, "RefNo no- " + t.resourcecode );
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public JsonResult RemoveMemberTreatment(int tid, string rcode = "")
        {
            string returnvalue = "FAIL";
            try
            {
                //cussup_m_treatment t = new cussup_m_treatment();
                cussup_m_treatment Ttmp = db.Treatments.Find(tid);
                if (Ttmp != null)
                {
                    var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                    int uid = Convert.ToInt32(Session["userid"]);
                    string uname = User.Identity.Name; //Session["username"].ToString();
                    db.Treatments.Remove(Ttmp);
                    db.SaveChanges();
                    saveToLog(rid, tid, "DELETE", "Remove New Treatment - Member -" + Ttmp.cussupname + ", UserID -" + uid.ToString() + ", User Name:" + uname, "RefNo no- " + Ttmp.resourcecode);
                    returnvalue = "SUCCESS";
                }

                return Json(returnvalue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public JsonResult getPkgRedeemMemberInfo(int  mid = 0)
        {
            try
            {
                CusSup_m_CusSupdtl c = new CusSup_m_CusSupdtl();
                c.id = 0;        
                ICollection<CusSup_m_CusSupdtl> mList = getMemberInfo("", "", "", mid, "", 0, "");
                if (mList != null)
                {
                    if (mList.Count > 0)
                    {
                        c = mList.ElementAt(0);
                        //db.MemberCard.Where(x=>x.cussupid == c.id && x.status)                          
                    }
                }
                return Json(c, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }


        [Authorize]
        public JsonResult getMemberInfoByDepartment(string custInfo = "", string department="ALL", int count = 10)
        {
            try
            {
                // Kyaw for the department filter
                var ctmplist = db.CusSup.Where(x => x.status == "Active").ToList();
                if(department.ToUpper() != "ALL")
                    ctmplist = db.CusSup.Where(x => x.status == "Active" && x.departmentname == department).ToList();

                var clist = ctmplist.Where(x => x.inhousecode.ToUpper().Contains(custInfo.ToUpper()) || cAESEncryption.getDecryptedString(x.mobile).ToUpper().Contains(custInfo.ToUpper()) || x.cussupname.ToUpper().Contains(custInfo.ToUpper())).Select(x => new { x.id, x.inhousecode, x.cussupname }).ToList();
                //var clist = db.CusSup.Where(x => x.status == "Active").Where(x => x.inhousecode.Contains(custInfo) || x.cussupname.Contains(custInfo)).Select(x => new { x.id, x.inhousecode, x.cussupname }).OrderBy(x => new { x.cussupname ,x.inhousecode}).ToList();
                clist = clist.OrderBy(x => x.cussupname).Take(count).ToList();
                return Json(clist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public JsonResult getMemberInfo(string custInfo = "", int count = 10)
        {
            try {
                var ctmplist = db.CusSup.Where(x => x.status == "Active").ToList();
                var clist = ctmplist.Where(x => x.inhousecode.ToUpper().Contains(custInfo.ToUpper()) || cAESEncryption.getDecryptedString(x.mobile).ToUpper().Contains(custInfo.ToUpper()) || x.cussupname.ToUpper().Contains(custInfo.ToUpper())).Select(x => new { x.id, x.inhousecode, x.cussupname }).ToList();
                //var clist = db.CusSup.Where(x => x.status == "Active").Where(x => x.inhousecode.Contains(custInfo) || x.cussupname.Contains(custInfo)).Select(x => new { x.id, x.inhousecode, x.cussupname }).OrderBy(x => new { x.cussupname ,x.inhousecode}).ToList();
                clist = clist.OrderBy(x=>x.cussupname).Take(count).ToList();
                return Json(clist,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message , JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 17 Feb 2016
        [Authorize]
        public JsonResult getMemberInfoForMileage(string custInfo = "", int count = 10)
        {
            try
            {
                var ctmplist = db.CusSup.Where(x => x.status == "Active").ToList();
                var clist = ctmplist.Where(x => x.cussupname.Contains(custInfo)).ToList();
                clist = clist.OrderBy(x => x.cussupname).Take(count).ToList();
                for (int i = 0; i < clist.Count; i++)
                {
                    clist.ElementAt(i).nric = cAESEncryption.getDecryptedString(clist.ElementAt(i).nric);
                    clist.ElementAt(i).Tel = cAESEncryption.getDecryptedString(clist.ElementAt(i).Tel);
                    clist.ElementAt(i).mobile= cAESEncryption.getDecryptedString(clist.ElementAt(i).mobile);
                }
                return Json(clist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        [Authorize]
        public JsonResult getReemMemberInfo(string custInfo = "", int count = 10)
        {
            try
            {
                int mid=0;
                //ICollection<CusSup_m_CusSup> clist;
                int cno=0; 
                if (custInfo.GetType() == typeof(int))
                {
                    cno = Convert.ToInt32(custInfo);
                }

                ICollection<CusSup_m_CusSup> ctmplist;
                if (cno > 0)
                {
                    var tmpcard = db.MemberCard.Where(x => x.cardno == cno && ( x.status != "VOID")).FirstOrDefault();
                    if (tmpcard != null)
                    { mid = tmpcard.cussupid; }
                }
                    
                ctmplist = db.CusSup.Where(x => x.status == "Active" && (x.id == mid || mid == 0 )).ToList();
                var clist = ctmplist.Where(x => x.inhousecode.ToUpper().Contains(custInfo.ToUpper()) || x.cussupname.ToUpper().Contains(custInfo.ToUpper()) || cAESEncryption.getDecryptedString(x.mobile).ToUpper().Contains(custInfo.ToUpper()) || cAESEncryption.getDecryptedString(x.nric).ToUpper().Contains(custInfo.ToUpper()) ).Select(x => new { x.id, x.inhousecode, x.cussupname }).ToList();
                //var clist = db.CusSup.Where(x => x.status == "Active").Where(x => x.inhousecode.Contains(custInfo) || x.cussupname.Contains(custInfo)).Select(x => new { x.id, x.inhousecode, x.cussupname }).OrderBy(x => new { x.cussupname ,x.inhousecode}).ToList();
                clist = clist.OrderBy(x => x.cussupname).Take(count).ToList();
                return Json(clist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        public int generateMemberCardNo()
        {
            int returnvalue = 0;
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    MySqlCommand cmd = new MySqlCommand("getMemberNewCardNo", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var Result = new MySqlParameter();
                    Result.ParameterName = "newNo";
                    Result.MySqlDbType = MySqlDbType.VarString;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Result);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    returnvalue = Convert.ToInt32(Result.Value);
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

        public JsonResult getMemberNewCardNo()
        {
            string returnvalue = "0";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    MySqlCommand cmd = new MySqlCommand("getMemberNewCardNo", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var Result = new MySqlParameter();
                    Result.ParameterName = "newNo";
                    Result.MySqlDbType = MySqlDbType.VarString;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    Result.Value = "";
                    cmd.Parameters.Add(Result);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    returnvalue = Result.Value.ToString();
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
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateMemberNewCardNo(String cardno="0",string rcode="Member")
        {
            string returnvalue = "FAIL";
            try { 
                //int cno = Convert.Toint(c);
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                using (var context = new BigMacEntities())
                {
                    context.Database.ExecuteSqlCommand("call updateMemberNewCardNo('" + cardno + "')");
                    returnvalue ="Success";
                }
                int rescode=0;
                bool isNum = Int32.TryParse(cardno, out rescode);
                saveToLog(rid, rescode, "Update", "Update card no to generate new card - " + cardno);
            }
            catch (Exception e) { }
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMemberTreatment(cussup_m_treatment treatment)
        {
            string returnvalue = "FAIL";
            try
            {
                cussup_m_treatment t = db.Treatments.FirstOrDefault(x => x.id == treatment.id);
                if (t != null)
                {
                    t.description = treatment.description ?? "";
                    t.remarks2 = treatment.remarks2 ?? "";
                    t.remarks3 = treatment.remarks3 ?? "";
                    t.remarks4 = treatment.remarks4 ?? "";
                    t.type = treatment.type;
                    t.resourcecode = treatment.resourcecode;
                    db.SaveChanges();
                    returnvalue = "Success";
                }
               
               
            }
            catch (Exception e) { }
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MemberCheckInOut(int cno, int inflag =0)
        {
            string returnvalue = "FAIL";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    string spname = "";
                    if (inflag == 1)    spname = "MemberCheckIn";
                    else
                    {
                        spname = "MemberCheckOut";
                    }
                    MySqlCommand cmd = new MySqlCommand(spname, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("cno", cno));
                    if (inflag == 1)
                    {
                        cmd.Parameters.Add(new MySqlParameter("bcode", Session["branchcode"]));
                        cmd.Parameters.Add(new MySqlParameter("ccode", Session["cocode"]));
                    }

                    var Result = new MySqlParameter();
                    Result.ParameterName = "returnvalue";
                    Result.MySqlDbType = MySqlDbType.VarChar;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Result);

                    var mName = new MySqlParameter();
                    mName.ParameterName = "mname";
                    mName.MySqlDbType = MySqlDbType.VarChar;
                    mName.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(mName);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    returnvalue = Result.Value.ToString();
                    if (inflag == 1)
                    {
                        if (returnvalue == "SUCCESS")
                        {
                            returnvalue = mName.Value.ToString() + " - Check In Successfully.";
                            Session["cardno"] = cno;
                        }
                    }
                    else
                    {
                        if (returnvalue == "SUCCESS")
                        {
                            returnvalue = mName.Value.ToString() + " - Check Out Successfully.";                            
                        }
                    }
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
            return Json(returnvalue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MemberListing(string filter="",string mobile="")
        {
            //string type = Request.QueryString["type"].ToString();
            //if (type.Length == 0) type = "Customer";

            // Kyaw , for department option
            List<Common_m_StaffDepartment> deptList = new List<Common_m_StaffDepartment>();
            var deptAll = new Common_m_StaffDepartment();

            deptAll.id = 0;
            deptAll.departmentname = "ALL";
            deptAll.description = "ALL";
            deptList.Add(deptAll);
            deptList.AddRange(db.Department.ToList());

            ViewBag.DepartmentOptions = deptList;

            ViewBag.Mobile = mobile;
            ViewBag.Filter = filter;
            string type = "Customer";
            var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
            var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
            ViewBag.CitiDesc = c;
            ViewBag.BonusDesc = b;
            ViewBag.custype = type;
            return View();
        }

        public static ICollection<CusSup_m_CusSupdtl> getMemberInfo(string filter = "%", string mobile = "", string name = "", int mid = 0, string mcode = "", Int32 cardno =0, string nric = "")
        {
            if (mobile != "") mobile = cAESEncryption.getEncryptedString(mobile);
            using (var context = new BigMacEntities())
            {
                var mList = context.Database.SqlQuery<CusSup_m_CusSupdtl>("call getMemberList('" + filter + "'," + mid + ",'" + mcode + "'," + cardno + ",'" + name + "','" + nric + "','" + mobile + "')").ToList();
                return mList;
            }            
            //return View(db.products.ToList());
        }
        //Added by ZawZO on 15 Sep 2015
        public static ICollection<CusSup_m_CusSupdtl> getMembers(string filter = "%", string mobile = "", string name = "", int mid = 0, string mcode = "", Int32 cardno = 0, string nric = "")
        {
            if (mobile != "") mobile = cAESEncryption.getEncryptedString(mobile);
            using (var context = new BigMacEntities())
            {
                var mList = context.Database.SqlQuery<CusSup_m_CusSupdtl>("call getMembers('" + filter + "'," + mid + ",'" + mcode + "'," + cardno + ",'" + name + "','" + nric + "','" + mobile + "')").ToList();
                return mList;
            }
            //return View(db.products.ToList());
        }
        public ActionResult getMemberListWithPaging(jQueryDataTableParamModel param,string filter = "%", string department="ALL", string mobile = "", string name = "", int mid = 0, string mcode = "", Int32 cardno = 0, string nric = "")
        {
            try
            {
                if (cardno == 0)
                {                      
                    int result;
                    if (int.TryParse(filter, out result))
                    {
                        cardno = Convert.ToInt32(filter);
                    }
                    else
                    {
                        cardno = 0;
                    }
                }
                //Changed by ZawZO on 15 Sep 2015
                //ICollection<CusSup_m_CusSupdtl> mList = getMemberInfo("", mobile, name, mid, mcode, 0, nric);
                ICollection<CusSup_m_CusSupdtl> mList = getMembers("", mobile, name, mid, mcode, 0, nric);
                
                mList = (department.ToUpper() == "ALL") ? mList : mList.Where(x => x.departmentname == department).ToList();

                if (filter != "" )
                    mList = mList.Where(x => x.cussupname.ToUpper().Contains(filter.ToUpper()) || x.inhousecode.ToUpper().Contains(filter.ToUpper()) || cAESEncryption.getDecryptedString(x.nric).ToUpper().Contains(filter.ToUpper()) || cAESEncryption.getDecryptedString(x.mobile).ToUpper()==filter.ToUpper()).ToList();

                if (cardno > 0)
                {
                    //Changed by ZawZO on 15 Sep 2015
                    //ICollection<CusSup_m_CusSupdtl> cList = getMemberInfo("", mobile, name, mid, mcode, cardno, nric);
                    ICollection<CusSup_m_CusSupdtl> cList = getMembers("", mobile, name, mid, mcode, cardno, nric);
                    if (cList != null)
                    {
                        if (cList.Count > 0)
                        {
                            //mList.
                            var t = mList.Where(x => x.id == cList.ElementAt(0).id).FirstOrDefault();
                            
                            if (t == null)
                            {
                                mList = mList.Union(cList).ToList();
                            }
                        }
                    }
                }               
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterMList = mList.Where(x => x.cussupname.ToUpper().Contains(searchValue.ToUpper()) || x.inhousecode.ToUpper().Contains(searchValue.ToUpper()) || cAESEncryption.getDecryptedString(x.nric).ToUpper().Contains(searchValue.ToUpper()) || cAESEncryption.getDecryptedString(x.mobile).ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderBy(x => x.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderBy(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 3)
                        filterMList = filterMList.OrderBy(x => x.CBalance).ToList();
                    else if (sortColumnIndex == 4)
                        filterMList = filterMList.OrderBy(x => x.BBalance).ToList();
                    else if (sortColumnIndex == 5)
                        filterMList = filterMList.OrderBy(x => x.CReserved).ToList();
                    else if (sortColumnIndex == 6)
                        filterMList = filterMList.OrderBy(x => x.BReserved).ToList();
                    else if (sortColumnIndex == 7)
                        filterMList = filterMList.OrderBy(x => x.CAvailable).ToList();
                    else if (sortColumnIndex == 8)
                        filterMList = filterMList.OrderBy(x => x.BAvailable).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderByDescending(x => x.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderByDescending(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 3)
                        filterMList = filterMList.OrderByDescending(x => x.CBalance).ToList();
                    else if (sortColumnIndex == 4)
                        filterMList = filterMList.OrderByDescending(x => x.BBalance).ToList();
                    else if (sortColumnIndex == 5)
                        filterMList = filterMList.OrderByDescending(x => x.CReserved).ToList();
                    else if (sortColumnIndex == 6)
                        filterMList = filterMList.OrderByDescending(x => x.BReserved).ToList();
                    else if (sortColumnIndex == 7)
                        filterMList = filterMList.OrderByDescending(x => x.CAvailable).ToList();
                    else if (sortColumnIndex == 8)
                        filterMList = filterMList.OrderByDescending(x => x.BAvailable).ToList();
                }

                var paginatedQMList = filterMList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                for (int i = 0; i < paginatedQMList.Count; i++)
                {
                    paginatedQMList.ElementAt(i).nric = cAESEncryption.getDecryptedString(paginatedQMList.ElementAt(i).nric);
                    paginatedQMList.ElementAt(i).mobile = cAESEncryption.getDecryptedString(paginatedQMList.ElementAt(i).mobile);
                }
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = mList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterMList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQMList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        
        public JsonResult getMemberList(string filter = "%", string mobile = "", string name = "", int mid = 0, string mcode = "", Int32 cardno = 0, string nric = "")
        {
            try
            {
                //ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                //ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                //if (mobile != "") mobile = cAESEncryption.getEncryptedString(mobile);
                //using (var context = new BigMacEntities())
                //{
                //    var mList = context.Database.SqlQuery<CusSup_m_CusSup>("call getMemberList('" + filter + "'," + mid + ",'" + mcode + "','" + name + "','" + nric + "','" + mobile + "')").ToList();
                //    return Json(mList, JsonRequestBehavior.AllowGet);
                //}
                var mList = getMemberInfo(filter, mobile, name, mid, mcode,cardno, nric);
                return Json(mList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public ActionResult CusSupIndex()
        {
            string type = "Customer";

            ViewBag.custype = type;
            ICollection<CusSup_m_CusSup> CusSupCol = db.CusSup.Where(x => x.cussuptype == type || x.cussuptype == "Customer/Supplier").ToList();
            int i = 0;
            for (i = 0; i < CusSupCol.Count; i++)
            {
                CusSupCol.ElementAt(i).address = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).address);
                CusSupCol.ElementAt(i).postalcode = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).postalcode);
                CusSupCol.ElementAt(i).nric = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).nric);
                CusSupCol.ElementAt(i).Tel = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).Tel);
                CusSupCol.ElementAt(i).mobile = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).mobile);
                CusSupCol.ElementAt(i).email = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).email);
                CusSupCol.ElementAt(i).userid = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).userid);
            }
            return View(CusSupCol.OrderBy(x => x.cussupname).ToList());
        }

        public ActionResult Index()
        {            
            string type = Request.QueryString["type"].ToString();

            if (type.Length == 0) type = "Customer";

            ViewBag.custype = type; 
            ICollection<CusSup_m_CusSup> CusSupCol = db.CusSup.Where(x => x.cussuptype == type || x.cussuptype == "Customer/Supplier").ToList();
            int i = 0;
            for (i = 0; i < CusSupCol.Count; i++)
            {
                CusSupCol.ElementAt(i).address = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).address);
                CusSupCol.ElementAt(i).postalcode = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).postalcode);
                CusSupCol.ElementAt(i).nric = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).nric);
                CusSupCol.ElementAt(i).Tel = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).Tel);
                CusSupCol.ElementAt(i).mobile = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).mobile);
                CusSupCol.ElementAt(i).email = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).email);
                CusSupCol.ElementAt(i).userid = cAESEncryption.getDecryptedString(CusSupCol.ElementAt(i).userid);
            }
            return View(CusSupCol.OrderBy(x => x.cussupname).ToList());
        }

        public ActionResult getCitiMemberListWithPaging(jQueryDataTableParamModel param, string filter = "%", string mobile = "", string name = "", int mid = 0, string mcode = "", Int32 cardno = 0, string nric = "")
        {
            try
            {
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterMList = db.CusSupCiti.Where(x => x.Status.ToUpper() != "INACTIVE" && x.Status.ToUpper() != "TRANSFER" && (x.Cust_name.ToUpper().Contains(searchValue.ToUpper()) || x.Cust_code.ToUpper().Contains(searchValue.ToUpper()))).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderBy(x => x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderBy(x => x.Cust_code).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderBy(x => x.Cust_name).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderByDescending(x => x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderByDescending(x => x.Cust_code).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderByDescending(x => x.Cust_name).ToList();
                }

                var paginatedQMList = filterMList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = db.CusSupCiti.Count(), //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterMList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQMList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult CitiMemberListing(string filter = "", string mobile = "")
        {
            //string type = Request.QueryString["type"].ToString();
            //if (type.Length == 0) type = "Customer";
            ViewBag.Mobile = mobile;
            ViewBag.Filter = filter;
            //string type = "Customer";
            var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
            var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
            ViewBag.CitiDesc = c;
            ViewBag.BonusDesc = b;
            //ViewBag.custype = type;

            ICollection<Configuration_m_Branches> branches = db.Branches.ToList();

            for (int i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }

            ViewBag.BranchOptions = branches;

            return View();
        }

        public ActionResult CitiMembershipEnquiry(string CustCode = "", int LiabilityID = 0, string rcode = "CustomerTransfer",int status = 0,string BranchCode = "")
        {
            //try
            //{
                ViewBag.DepartmentOptions = db.Department.AsEnumerable();    
                ViewBag.NationalityOptions = db.Nationality.AsEnumerable();
                ViewBag.RaceOptions = db.Race.AsEnumerable();
                ViewBag.NRICTypeOptions = db.NRICType.AsEnumerable();
                ViewBag.StatusOptions = db.CusSupStatus.AsEnumerable();
                ViewBag.CardTypeOptions = db.CardType.AsEnumerable();
                ViewBag.Type = "Customer";
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                ViewBag.RCode = rcode;
                ViewBag.Status = status;
                ViewBag.branchcode = BranchCode;
            
                ICollection<CusSup_z_CitibellaConversionRate> crate = db.CitiRate.ToList();
                for (int i = 0; i < crate.Count(); i++)
                {
                    crate.ElementAt(i).desc = "Redemption  -" + crate.ElementAt(i).tocitidollar.ToString() + ", Bonus Rate : " + crate.ElementAt(i).torewarddollar.ToString();
                }
                ViewBag.RateOptions = crate;
                CusSup_m_Citibella m = db.CusSupCiti.Where(x => x.Cust_code == CustCode).FirstOrDefault();
                //m.Liability = db.CitiLiability.Where(x => x.CustCode == m.Cust_code);
                if (m != null)
                {
                    m.Cust_email = m.Cust_email ?? "";
                    m.CustCode = db.CitiCustCode.Where(x => x.MainCustCode == m.Cust_code);
                    m.Liability = db.CitiLiability.Where(x => x.id == LiabilityID);
                    int staffid = Convert.ToInt32(m.Liability.FirstOrDefault().staff);
                    if (staffid > 0)
                    {
                        m.Liability.FirstOrDefault().staff = db.Staffs.Where(x => x.id == staffid).Select(x => x.name).FirstOrDefault().ToString();
                    }
              
                }
                else
                {
                    m = new CusSup_m_Citibella();
                    m.Cust_email = "";
                    m.Liability = db.CitiLiability.Where(x => x.id == LiabilityID);
                    m.Cust_code = m.Liability.FirstOrDefault().CustCode;
                    m.CustCode = db.CitiCustCode.Where(x => x.MainCustCode == m.Cust_code);
                    m.Cust_name = m.Liability.FirstOrDefault().CustName;
                    m.Cust_No = m.Liability.FirstOrDefault().CustContact;
                    m.Cust_JoinDate = m.Liability.FirstOrDefault().transactiondate.ToString();
                    int staffid = Convert.ToInt32(m.Liability.FirstOrDefault().staff);
                    if (staffid > 0)
                    {
                        m.Liability.FirstOrDefault().staff = db.Staffs.Where(x => x.id == staffid).Select(x => x.name).FirstOrDefault().ToString();
                    }

                }


                var sproductcode = m.Liability.FirstOrDefault().Code;
               
                int srchPID = 0;
                Product_m_ProductAltCode altp = db.productALTCode.Where(x => x.altcode == sproductcode).FirstOrDefault();
                //Changed by ZawZO on 20 Aug 2016
                if (altp == null){
                    srchPID = createSequoiaProduct(m.Liability.FirstOrDefault().Description, sproductcode);
                }
                else {
                    srchPID = altp.productid;
                }
                Product_m_Product p = db.products.Where(x => x.id == srchPID).FirstOrDefault();
                ViewBag.ProductCode = p.productcode;
                ViewBag.ProductDesc = p.desc;
                
                return View(m);
            //}
            //catch (Exception e)
            //{
            //    return View();
            //}
        }

        public JsonResult getCitiRate(int id =0)
        {
            //string returnvalue = "FAIL";
            try
            {
                //int cno = Convert.Toint(c);
                CusSup_z_CitibellaConversionRate r = db.CitiRate.Find(id);
                if (r!= null)
                    return Json(r, JsonRequestBehavior.AllowGet);
                else
                    return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult geCollectionCardList(jQueryDataTableParamModel param)
        {
            try
            {
                var cList = db.MemberCard.Include("Member").Where(x => (x.collectionhandlebyid == 0 || x.collectiondate == null || x.collectionhandlebyid == null) && (x.printeddate!=null && x.printedbyid !=null && x.printedbyid > 0)).OrderBy(x => new { x.Member.cussupname, x.cardno }).ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                //var filterCardList = cList.Where(x => x.Member.cussupname.ToUpper().Contains(searchValue.ToUpper()) || x.cardno.ToString().Contains(searchValue.ToUpper()) || x.cardserialno.ToString().Contains(searchValue.ToUpper()) || x.cardtype.ToUpper().Contains(searchValue.ToUpper()) || x.expirydate.ToString().Contains(searchValue.ToUpper()) || x.Status.Contains(searchValue.ToUpper())).ToList();
                var filterCardList = cList.Where(x => x.Member.inhousecode.ToUpper().Contains(searchValue.ToUpper()) || x.Member.cussupname.ToUpper().Contains(searchValue.ToUpper()) || x.cardno.ToString().Contains(searchValue.ToUpper()) || x.cardtype.ToUpper().Contains(searchValue.ToUpper()) || x.expirydate.ToString().Contains(searchValue.ToUpper()) || x.status.Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc

                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterCardList = filterCardList.OrderBy(x => x.Member.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterCardList = filterCardList.OrderBy(x => x.Member.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterCardList = filterCardList.OrderBy(x => x.cardtype).ToList();
                    else if (sortColumnIndex == 3)
                        filterCardList = filterCardList.OrderBy(x => x.cardno).ToList();
                    else if (sortColumnIndex == 4)
                        filterCardList = filterCardList.OrderBy(x => x.cardserialno).ToList();
                    else if (sortColumnIndex == 5)
                        filterCardList = filterCardList.OrderBy(x => x.expirydate).ToList();
                    else if (sortColumnIndex == 6)
                        filterCardList = filterCardList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterCardList = filterCardList.OrderByDescending(x => x.Member.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterCardList = filterCardList.OrderByDescending(x => x.Member.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterCardList = filterCardList.OrderByDescending(x => x.cardtype).ToList();
                    else if (sortColumnIndex == 3)
                        filterCardList = filterCardList.OrderByDescending(x => x.cardno).ToList();
                    else if (sortColumnIndex == 4)
                        filterCardList = filterCardList.OrderByDescending(x => x.cardserialno).ToList();
                    else if (sortColumnIndex == 5)
                        filterCardList = filterCardList.OrderByDescending(x => x.expirydate).ToList();
                    else if (sortColumnIndex == 6)
                        filterCardList = filterCardList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedQCListtmp = filterCardList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount });
                var paginatedQCList = paginatedQCListtmp.Join(db.CusSup, c => c.cussupid, cust => cust.id, (c, cust) => new { paginatedQCList = c, CusSup = cust }).Select(x => new { x.paginatedQCList.id, x.CusSup.inhousecode, x.CusSup.cussupname, x.paginatedQCList.cardtype, x.paginatedQCList.cardno, x.paginatedQCList.cardserialno, x.paginatedQCList.expirydate, x.paginatedQCList.status }).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = cList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterCardList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQCList
                },
                JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }
        //Added by ZawZO on 12 Jul 2016
        public ActionResult getMemberLiabilityListWithPaging(jQueryDataTableParamModel param,string status="",string branchcode="")
        {
            try
            {
            
                ICollection<CitibellaMemberLiability> mList = getMemberLiability();
                if (status != "All")
                {
                    mList = mList.Where(x => x.TransferStatus == status).ToList();
                }

                if (branchcode != "All")
                {
                    mList = mList.Where(x => x.BranchCode == branchcode).ToList();
                }

                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterMList = mList.Where(x => x.CustID.ToUpper().Contains(searchValue.ToUpper()) || x.CustName.ToUpper().Contains(searchValue.ToUpper()) || x.ProductCode.ToUpper().Contains(searchValue.ToUpper()) || x.ProductDescription.ToUpper().Contains(searchValue.ToUpper()) || x.ProductDescription2.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderBy(x => x.LiabilityID).ToList();
                    if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderBy(x => x.BranchCode).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderBy(x => x.ProductCode).ToList();
                    else if (sortColumnIndex == 3)
                        filterMList = filterMList.OrderBy(x => x.ProductDescription).ToList();
                    else if (sortColumnIndex == 4)
                        filterMList = filterMList.OrderBy(x => x.ProductDescription2).ToList();
                    else if (sortColumnIndex == 5)
                        filterMList = filterMList.OrderBy(x => x.ReceiptNo).ToList();
                    else if (sortColumnIndex == 6)
                        filterMList = filterMList.OrderBy(x => x.CustID).ToList();
                    else if (sortColumnIndex == 7)
                        filterMList = filterMList.OrderBy(x => x.CustCode).ToList();
                    else if (sortColumnIndex == 8)
                        filterMList = filterMList.OrderBy(x => x.CustName).ToList();
                    else if (sortColumnIndex == 9)
                        filterMList = filterMList.OrderBy(x => x.Total).ToList();
                    else if (sortColumnIndex == 10)
                        filterMList = filterMList.OrderBy(x => x.Done).ToList();
                    else if (sortColumnIndex == 11)
                        filterMList = filterMList.OrderBy(x => x.OpenBal).ToList();
                    else if (sortColumnIndex == 12)
                        filterMList = filterMList.OrderBy(x => x.UnitPrice).ToList();
                    else if (sortColumnIndex == 13)
                        filterMList = filterMList.OrderBy(x => x.TotalAmt).ToList();
                    else if (sortColumnIndex == 14)
                        filterMList = filterMList.OrderBy(x => x.OutstandingAmt).ToList();
                    else if (sortColumnIndex == 15)
                        filterMList = filterMList.OrderBy(x => x.Liability).ToList();
                    else if (sortColumnIndex == 16)
                        filterMList = filterMList.OrderBy(x => x.TransferStatus).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterMList = filterMList.OrderByDescending(x => x.LiabilityID).ToList();
                    if (sortColumnIndex == 1)
                        filterMList = filterMList.OrderByDescending(x => x.BranchCode).ToList();
                    else if (sortColumnIndex == 2)
                        filterMList = filterMList.OrderByDescending(x => x.ProductCode).ToList();
                    else if (sortColumnIndex == 3)
                        filterMList = filterMList.OrderByDescending(x => x.ProductDescription).ToList();
                    else if (sortColumnIndex == 4)
                        filterMList = filterMList.OrderByDescending(x => x.ProductDescription2).ToList();
                    else if (sortColumnIndex == 5)
                        filterMList = filterMList.OrderByDescending(x => x.ReceiptNo).ToList();
                    else if (sortColumnIndex == 6)
                        filterMList = filterMList.OrderByDescending(x => x.CustID).ToList();
                    else if (sortColumnIndex == 7)
                        filterMList = filterMList.OrderByDescending(x => x.CustCode).ToList();
                    else if (sortColumnIndex == 8)
                        filterMList = filterMList.OrderByDescending(x => x.CustName).ToList();
                    else if (sortColumnIndex == 9)
                        filterMList = filterMList.OrderByDescending(x => x.Total).ToList();
                    else if (sortColumnIndex == 10)
                        filterMList = filterMList.OrderByDescending(x => x.Done).ToList();
                    else if (sortColumnIndex == 11)
                        filterMList = filterMList.OrderByDescending(x => x.OpenBal).ToList();
                    else if (sortColumnIndex == 12)
                        filterMList = filterMList.OrderByDescending(x => x.UnitPrice).ToList();
                    else if (sortColumnIndex == 13)
                        filterMList = filterMList.OrderByDescending(x => x.TotalAmt).ToList();
                    else if (sortColumnIndex == 14)
                        filterMList = filterMList.OrderByDescending(x => x.OutstandingAmt).ToList();
                    else if (sortColumnIndex == 15)
                        filterMList = filterMList.OrderByDescending(x => x.Liability).ToList();
                    else if (sortColumnIndex == 16)
                        filterMList = filterMList.OrderByDescending(x => x.TransferStatus).ToList();   
                }

                var paginatedQMList = filterMList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = mList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterMList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQMList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json(e.Message.ToString(), JsonRequestBehavior.AllowGet); }
        }

        //Added by ZawZO on 12 Jul 2016
        public static ICollection<CitibellaMemberLiability> getMemberLiability()
        {
            using (var context = new BigMacEntities())
            {
                var mList = context.Database.SqlQuery<CitibellaMemberLiability>("call getCitibellaMemberLiability()").ToList();
                return mList;
            }
        }

        public ActionResult CollectionCardList(string rcode = "MEMBERCARDCOLLECTION")
        {
            try
            {
                //return View();
                ViewBag.Rcode = rcode;
                if (Session["staffname"] != null)
                    ViewBag.StaffName = Session["staffname"].ToString();
                else
                    ViewBag.StaffName = "";
            }
            catch (Exception e)
            {
                //return View();
            }
            return View();
        }

        [HttpPost]
        public JsonResult UpdateCollectionCard(int id, DateTime cdate, string rcode = "")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                string from = ""; string to = "";
                CusSup_m_Cards cobj = db.MemberCard.Find(id);

                if (cobj != null)
                {
                    from = "collected by - " + cobj.collectionhandlebyid.ToString() + ", collection date -" + cobj.collectiondate.ToString();
                    to = cdate.ToString() + "," + Session["staffid"].ToString();
                    cobj.collectiondate = cdate;                    
                    cobj.collectionhandlebyid = Convert.ToInt32(Session["staffid"]);
                    cobj.status = "Collected";
                    db.SaveChanges();
                    saveToLog(rid, id, "UPDATE", "Update Print Date", from, to);
                }
                returnStr = "SUCCESS";

            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult gePrintCardList(jQueryDataTableParamModel param)
        {
            try
            {
                var cList = db.MemberCard.Include("Member").Where(x => x.printedbyid == 0 || x.printeddate == null || x.printedbyid == null).OrderBy(x => new { x.Member.cussupname, x.cardno }).ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                //var filterCardList = cList.Where(x => x.Member.cussupname.ToUpper().Contains(searchValue.ToUpper()) || x.cardno.ToString().Contains(searchValue.ToUpper()) || x.cardserialno.ToString().Contains(searchValue.ToUpper()) || x.cardtype.ToUpper().Contains(searchValue.ToUpper()) || x.expirydate.ToString().Contains(searchValue.ToUpper()) || x.Status.Contains(searchValue.ToUpper())).ToList();
                var filterCardList = cList.Where(x => x.Member.inhousecode.ToUpper().Contains(searchValue.ToUpper()) || x.Member.cussupname.ToUpper().Contains(searchValue.ToUpper()) || x.cardno.ToString().Contains(searchValue.ToUpper()) || x.cardtype.ToUpper().Contains(searchValue.ToUpper()) || x.expirydate.ToString().Contains(searchValue.ToUpper()) || x.status.Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterCardList = filterCardList.OrderBy(x => x.Member.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterCardList = filterCardList.OrderBy(x => x.Member.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterCardList = filterCardList.OrderBy(x => x.cardtype).ToList();
                    else if (sortColumnIndex == 3)
                        filterCardList = filterCardList.OrderBy(x => x.cardno).ToList();
                    //else if (sortColumnIndex == 4)
                    //    filterCardList = filterCardList.OrderBy(x => x.cardserialno).ToList();
                    else if (sortColumnIndex == 4)
                        filterCardList = filterCardList.OrderBy(x => x.expirydate).ToList();
                    else if (sortColumnIndex == 5)
                        filterCardList = filterCardList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterCardList = filterCardList.OrderByDescending(x => x.Member.inhousecode).ToList();
                    else if (sortColumnIndex == 1)
                        filterCardList = filterCardList.OrderByDescending(x => x.Member.cussupname).ToList();
                    else if (sortColumnIndex == 2)
                        filterCardList = filterCardList.OrderByDescending(x => x.cardtype).ToList();
                    else if (sortColumnIndex == 3)
                        filterCardList = filterCardList.OrderByDescending(x => x.cardno).ToList();
                    //else if (sortColumnIndex == 4)
                    //    filterCardList = filterCardList.OrderByDescending(x => x.cardserialno).ToList();
                    else if (sortColumnIndex == 4)
                        filterCardList = filterCardList.OrderByDescending(x => x.expirydate).ToList();
                    else if (sortColumnIndex == 5)
                        filterCardList = filterCardList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedQCListtmp = filterCardList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount });
                var paginatedQCList = paginatedQCListtmp.Join(db.CusSup, c => c.cussupid, cust => cust.id, (c, cust) => new { paginatedQCList = c, CusSup = cust }).Select(x => new { x.paginatedQCList.id ,x.CusSup.inhousecode, x.CusSup.cussupname, x.paginatedQCList.cardtype, x.paginatedQCList.cardno,  x.paginatedQCList.expirydate, x.paginatedQCList.status }).ToList();
                
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = cList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterCardList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQCList
                },
                JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public ActionResult PrintCardList(string rcode = "MEMBERCARDPRINT")
        {
            try
            { 
                //return View();
                ViewBag.Rcode = rcode;
                if (Session["staffname"] != null)
                    ViewBag.StaffName = Session["staffname"].ToString();
                else
                    ViewBag.StaffName = "";
            }
            catch (Exception e)
            {
                //return View();
            }
            return View();
        }

        [HttpPost]
        public JsonResult UpdatePrintCard(int id, DateTime pdate,string csrno ="",string rcode = "")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                string from = ""; string to = "";
                CusSup_m_Cards cobj = db.MemberCard.Find(id);

                if (cobj != null)
                {
                    from = "print by - " + cobj.printedbyid.ToString() + ", print date -" + cobj.printeddate.ToString(); 
                    to = pdate.ToString() + "," + Session["staffid"].ToString();
                    cobj.printeddate= pdate;
                    cobj.cardserialno = csrno;
                    cobj.printedbyid = Convert.ToInt32(Session["staffid"]);
                    cobj.status = "Print";                    
                    db.SaveChanges();
                    saveToLog(rid, id, "UPDATE", "Update Print Date", from, to);
                }
                returnStr = "SUCCESS";
                
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MembershipEnquiry(int id =0,string mcode ="",string rcode="MembershipEnquiry")
        {
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                ViewBag.DepartmentOptions = db.Department.AsEnumerable();
                ViewBag.NationalityOptions = db.Nationality.AsEnumerable();
                ViewBag.RaceOptions = db.Race.AsEnumerable();
                ViewBag.NRICTypeOptions = db.NRICType.AsEnumerable();
                ViewBag.StatusOptions = db.CusSupStatus.AsEnumerable();
                ViewBag.CardTypeOptions = db.CardType.AsEnumerable();
                ViewBag.Type = "Customer";
                ViewBag.RCode = rcode;
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                var y = db.ConfigDefault.Where(x => x.key == "CEXPYEAR").FirstOrDefault().value;
                var yy = 0;
                if (y != "")
                    yy = Convert.ToInt32(y);
                ViewBag.ExpYear = DateTime.Today.AddYears(yy).Date;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;

                var userid = 0;
                var adjFlag = "0";
                var statusFlag = "0";
                var NewFlag = "0";
                var EditFlag = "0";
                var rvflag = "0";
                var tvflag = "0";
                var pvflag = "0";

                 //if (Session["userid"] != null)
                 //   userid = Convert.ToInt32(Session["userid"]);
                if (Session["staffuserid"] != null)
                {
                    if (Session["staffuserid"].ToString() != "" && Session["staffuserid"].ToString() != "0")
                    { userid = Convert.ToInt32(Session["staffuserid"]); }
                    else if (Session["userid"] != null)
                    { userid = Convert.ToInt32(Session["userid"]); }
                }
                else if (Session["userid"] != null)
                { userid = Convert.ToInt32(Session["userid"]); }

                if (db.Users.Where(x => x.id == userid).FirstOrDefault().roldid == 1)
                {
                    adjFlag = "1";
                    statusFlag = "1";
                    NewFlag = "1";
                    EditFlag = "1";
                    rvflag = "1";
                    tvflag = "1";
                    pvflag = "1";
                }
                else 
                {
                    
                    //if (Session["userid"] != null)
                    //userid = Convert.ToInt32(Session["userid"]); 
                
                    int roleid = db.Users.Where(x => x.id == userid).FirstOrDefault().roldid;
                    var aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.Resources.resource == "MemberAdjustment").Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberAdjustment").Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    if (aList != null)
                    { 
                        if (aList.Count > 0)
                            adjFlag = "1"; 
                    }
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.Resources.resource == "MemberStatus").Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberStatus").Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberStatus").ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            statusFlag = "1";
                    }
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && (x.Resources.resource == "CreateMember" || (x.Resources.resource == "Member" && x.create == true))).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && (x.Resources.resource == "CreateMember" || (x.Resources.resource == "Member" && x.create == true))).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "Member" && x.create == true).ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            NewFlag = "1";
                    }
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "CreateMember" && x.create == true).ToList();
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.Resources.resource == "Member" && x.update == true).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "Member" && x.update == true).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            EditFlag = "1";
                    }
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.voidField == true && (x.Resources.resource == "Redeem" || x.Resources.resource == "REDEEM")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.voidField == true && (x.Resources.resource == "Redeem" || x.Resources.resource == "REDEEM")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberStatus").ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            rvflag = "1";
                    }
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.voidField == true && (x.Resources.resource == "TopUp" || x.Resources.resource == "TOPUP")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.voidField == true && (x.Resources.resource == "TopUp" || x.Resources.resource == "TOPUP")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberStatus").ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            tvflag = "1";
                    }
                    aList = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == roleid && x.voidField == true && (x.Resources.resource == "Package" || x.Resources.resource == "PACKAGE")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.voidField == true && (x.Resources.resource == "TopUp" || x.Resources.resource == "TOPUP")).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct()).ToList();
                    //aList = db.AccessRights.Include("Resources").Where(x => x.userid == userid && x.Resources.resource == "MemberStatus").ToList();
                    if (aList != null)
                    {
                        if (aList.Count > 0)
                            pvflag = "1";
                    }
                }

                ViewBag.AdjustmentFlag = adjFlag;
                ViewBag.StatusFlag = statusFlag;
                ViewBag.NewFlag = NewFlag;
                ViewBag.EditFlag = EditFlag;
                ViewBag.rvFlag = rvflag;
                ViewBag.tvFlag = tvflag;
                ViewBag.pvFlag = pvflag;
                ICollection<CusSup_m_CusSupdtl> cusdtl= getMemberInfo("", "", "", id, mcode, 0, "");
                //CusSup_m_CusSup m = db.CusSup.Find(id);
                CusSup_m_CusSup m = new CusSup_m_CusSup();
                
                if (cusdtl.Count > 0)
                {
                    m.id = cusdtl.ElementAt(0).id;
                    m.cussupname = cusdtl.ElementAt(0).cussupname;
                    m.dateofbirth = cusdtl.ElementAt(0).dateofbirth;
                    m.inhousecode= cusdtl.ElementAt(0).inhousecode;
                    m.createdate = cusdtl.ElementAt(0).createdate; ;           
                    m.address = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).address);
                    m.postalcode = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).postalcode);
                    m.nric = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).nric);
                    m.Tel = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).Tel);
                    m.mobile = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).mobile);
                    m.email = cAESEncryption.getDecryptedString(cusdtl.ElementAt(0).email);
                    m.CBalance = Math.Round(cusdtl.ElementAt(0).CBalance,2);
                    m.BBalance = Math.Round(cusdtl.ElementAt(0).BBalance, 2);
                    m.CReserved = Math.Round(cusdtl.ElementAt(0).CReserved, 2);
                    m.BReserved = Math.Round(cusdtl.ElementAt(0).BReserved, 2);
                    m.CAvailable = Math.Round(cusdtl.ElementAt(0).CAvailable, 2);
                    m.BAvailable = Math.Round(cusdtl.ElementAt(0).BAvailable, 2);
                    m.nrictype = cusdtl.ElementAt(0).nrictype;
                    m.nationality = cusdtl.ElementAt(0).nationality;
                    m.departmentid = cusdtl.ElementAt(0).departmentid;
                    m.departmentname = cusdtl.ElementAt(0).departmentname;
                    m.race = cusdtl.ElementAt(0).race;
                    m.gender = cusdtl.ElementAt(0).gender;
                    m.canemail = cusdtl.ElementAt(0).canemail;
                    m.cansms = cusdtl.ElementAt(0).cansms;
                    m.cancall = cusdtl.ElementAt(0).cancall;
                    m.status = cusdtl.ElementAt(0).status;
                    m.photo = cusdtl.ElementAt(0).photo;
                    m.occupation = cusdtl.ElementAt(0).occupation;
                    m.branchcode = cusdtl.ElementAt(0).branchcode;
                }
                //Added by ZawZO on 11 Nov 2015
                string bcode = Session["branchcode"].ToString();
                string CoCode = Session["cocode"].ToString();
                Configuration_m_Company co = db.Companies.Where(x => x.cocode == CoCode).FirstOrDefault();
                ViewBag.CoRegNo = cAESEncryption.getDecryptedString(co.coregno);
                ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(co.gstregno);
                Configuration_m_Branches br = db.Branches.Where(x => x.branchcode == bcode).FirstOrDefault();
                ViewBag.CoName = cAESEncryption.getDecryptedString(br.branchname);
                if (br.address != null)
                {
                    if (br.address == "")
                    {
                        ViewBag.CoAddress = "-";
                    }
                    else
                    {
                        ViewBag.CoAddress = cAESEncryption.getDecryptedString(br.address);
                    }
                }
                else
                {
                    ViewBag.CoAddress = "-";
                }
                if (br.tel != null)
                {
                    if (br.tel == "")
                    {
                        ViewBag.Tel = "-";
                    }
                    else
                    {
                        ViewBag.Tel = cAESEncryption.getDecryptedString(br.tel);
                    }
                }
                else
                {
                    ViewBag.Tel = "-";
                }
                if (br.fax != null)
                {
                    if (br.fax == "")
                    {
                        ViewBag.Fax = "-";
                    }
                    else
                    {
                        ViewBag.Fax = cAESEncryption.getDecryptedString(br.fax);
                    }
                }
                else
                {
                    ViewBag.Fax = "-";
                }
                if (br.email != null)
                {
                    if (br.email == "")
                    {
                        ViewBag.Email = "-";
                    }
                    else
                    {
                        ViewBag.Email = cAESEncryption.getDecryptedString(br.email);
                    }
                }
                else
                {
                    ViewBag.Email = "-";
                }
                int intGSTReg = br.gstreg;
                if (intGSTReg == 1)
                {
                    ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(br.gstregno);
                }
                else
                {
                    ViewBag.GSTRegNo = "-";
                }

                return View(m);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult CreateMember(string type = "Customer",string rcode="CreateMember")
        {
            try
            {
                ViewBag.DepartmentOptions = db.Department.AsEnumerable();
                ViewBag.NationalityOptions = db.Nationality.AsEnumerable();
                ViewBag.RaceOptions = db.Race.AsEnumerable();
                ViewBag.NRICTypeOptions = db.NRICType.AsEnumerable();
                ViewBag.StatusOptions = db.CusSupStatus.AsEnumerable();
                ViewBag.CardTypeOptions = db.CardType.AsEnumerable();  
                ViewBag.Type = "Customer";
                ViewBag.RCode = rcode;
                ViewBag.AdjustmentFlag = "0";
                ViewBag.StatusFlag = "0";
                ViewBag.NewFlag = "1";
                ViewBag.EditFlag = "1";
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                var y = db.ConfigDefault.Where(x => x.key == "CEXPYEAR").FirstOrDefault().value;
                var yy = 0;
                if (y != "")
                    yy = Convert.ToInt32(y);
                ViewBag.ExpYear = DateTime.Today.AddYears(yy).Date;

                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                
                CusSup_m_CusSup cussup = new CusSup_m_CusSup();
                cussup.cussuptype = type;
                return View(cussup);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        //[HttpPost]
        //public ActionResult CreateMember(CusSup_m_CusSup cs)
        //{
        //    try
        //    {
        //        ViewBag.StatusOptions = db.CusSupStatus.AsEnumerable();
        //        ViewBag.Type = "Customer";


        //    }
        //    catch (Exception e)
        //    {
        //        return View(cs);
        //    }        
        //    return View(cs);
        //}

        [HttpPost]
        public JsonResult UploadImage()
        {
            var returnStr = "FAIL";
            string fileName="";
            try{
                //var fileSavePath = "";
                //var fileName = "";
                //var uploadedFile = Request.Files[0];
                //fileName = uploadedFile.FileName;
                //fileSavePath = Server.MapPath("~/assets/img/Services/" + fileName);
                //uploadedFile.SaveAs(fileSavePath);
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    //Use the following properties to get file's name, size and MIMEType
                    int fileSize = file.ContentLength;
                    fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    //To save file, use SaveAs method

                    var root = Server.MapPath("~/");
                    string parent = Path.GetDirectoryName(root);
                    string grandParent = Path.GetDirectoryName(parent);

                    AppSettings settings = new AppSettings();

                    var membersPath = grandParent + "\\" + settings.membersSite + "\\assets\\img\\Members\\" + fileName;
                    var backofficePath = grandParent + "\\" + settings.backofficeSite + "\\assets\\img\\Members\\" + fileName;

                    file.SaveAs(Server.MapPath("~/assets/img/Members/") + fileName);
                    file.SaveAs(membersPath);
                    file.SaveAs(backofficePath); 

               }
                returnStr = fileName;
                //return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch(Exception e)
            {}
            finally
                {
                    
                }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult isDuplicateNRIC(int mid = 0,string nric="")
        {
            var returnStr = "0"; 
            try
            {
                nric = cAESEncryption.getEncryptedString(nric);
                CusSup_m_CusSup ctmp = db.CusSup.Where(x => x.nric == nric && x.id != mid).FirstOrDefault();  //db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                if (ctmp != null)
                {
                    returnStr = "1";
                }
            }
            catch (Exception e)
            {
                returnStr = "0"; // e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MemberStatusSave(int mid=0,string uname="",string pwd="",string status="",string rcode="")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x=>x.resource == rcode).FirstOrDefault().id;
                int rid =0;
                if (ridtmp != null)  rid = Convert.ToInt32(ridtmp);
                string from = ""; string to = "";
                if (isValidUser(uname.ToUpper(), pwd))
                {
                    CusSup_m_CusSup cobj = db.CusSup.Find(mid);

                    if (cobj != null)
                    {
                        from = cobj.status;
                        to = status;
                        cobj.status = status;
                        db.SaveChanges();
                        saveToLog(rid, mid, "UPDATE", "Update Status", "Old Status - " + from, "New Status - " + to);
                    }
                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public Boolean isValidUser(string uname, string pwd)
        {
            Boolean flag = false;
            //string role =
            try
            {

                var euname = "";
                var epwd = "";
                var EncryptHex = "";

                cAESEncryption.AESEncryption(uname, out EncryptHex, out euname);
                cAESEncryption.AESEncryption(pwd, out EncryptHex, out epwd);

                using (var context = new BigMacEntities())
                {
                    ICollection<Access_m_Users> tmpuser = context.Database.SqlQuery<Access_m_Users>("call chkValidUser ('" + euname + "','" + epwd + "')").ToList();

                    if (tmpuser.Count() > 0)
                    {
//                        uid = tmpuser.ElementAt(0).id;
                        var  robj = db.Roles.Find(tmpuser.ElementAt(0).roldid);
                        if (robj != null)
                        {
                            if (robj.Role.ToUpper() == "MANAGER" || robj.Role.ToUpper() == "SYSTEM ADMINISTRATOR") flag = true;
                        }

                    }

                }
                //return flag;
            }
            catch (Exception e)
            {
                
            }

            return flag;

        }

        [HttpPost]
        public JsonResult CitibellaLiabilitySave(CusSup_m_CitibellaLiability c, string rcode = "CUSTOMERTRANSFER")
        {
            var returnStr = "FAIL";
            try
            {
           
                CusSup_m_CitibellaLiability cc = db.CitiLiability.Find(c.id);

                if (cc != null)
                {
                    cc.Code = c.Code;
                    cc.Description = c.Description;
                    cc.Description2 = c.Description2;
                    cc.ReceiptNo = c.ReceiptNo;
                    cc.Total = c.Total;
                    cc.Done = c.Done;
                    cc.Open = c.Open;
                    cc.UnitPrice = c.UnitPrice;
                    cc.TotalAmt = c.TotalAmt;
                    cc.OutstandingAmt = c.OutstandingAmt;
                    cc.Liability = c.Liability;

                    string dt = string.Format("{0:dd/MM/yyyy}", c.transactiondate);
                    cc.transactiondate = DateTime.ParseExact(dt, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None); 
                    
                    db.SaveChanges();

                    returnStr = "SUCCESS";
                }
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            
            var rmid = 0;
            if (returnStr == "SUCCESS") rmid = c.id;

            return Json(rmid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferCustomerByBranch(string rcode = "CustomerTransfer", string branchcode = "")
        {
            var returnStr = "FAIL";
            var rmid = 0;
            try
            {
     
                using (var context = new BigMacEntities())
                {
                    var members = context.Database.SqlQuery<CusSup_m_Citibella>("call getCitibellaLiabilityMemberList('" + branchcode + "')").ToList();
                    foreach (CusSup_m_Citibella citi in members)
                    {

                        var citidata = db.CusSupCiti.FirstOrDefault(x => x.Cust_code == citi.Cust_code);
                        if (citidata != null)
                        {
                            citi.Cust_DOB = (citidata.Cust_DOB != null) ? citidata.Cust_DOB : "01/01/00 12:00 AM";
                            citi.Cust_nric = (citidata.Cust_nric != null) ? citidata.Cust_nric : "";
                            citi.Cust_address = (citidata.Cust_address != null) ? citidata.Cust_address : "";
                        }
                        else
                        {
                            citi.Cust_DOB = "01/01/00 12:00 AM";
                            citi.Cust_nric = "";
                            citi.Cust_address = "";
                        }

                        citi.Liability = db.CitiLiability.Where(x => x.id == citi.id).ToList();
                        citi.Liability.ElementAt(0).FromAmount = citi.Liability.ElementAt(0).Open;
                        citi.Liability.ElementAt(0).ToCitiDollar = 0;
                        citi.Liability.ElementAt(0).ToRewardDollar = 0;

                        if (citi.Liability.ElementAt(0).Status == "Active" && citi.Liability.ElementAt(0).Open > 0)
                        {
                            TransferCustomer(citi, rcode, branchcode);
                        }

                    }

                    returnStr = "Transferred";
                }

            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 20 Aug 2016

        [HttpPost]
        public JsonResult TransferCustomer(CusSup_m_Citibella c, string rcode = "TransferCustomer",string branchcode = "")
        {
            var returnStr = "FAIL";
            var rmid = 0;
            try
            {
               
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                CusSup_m_Citibella cb = db.CusSupCiti.Where(x => x.Cust_code == c.Cust_code).FirstOrDefault();
                Boolean blnexist = false;
                if (cb != null){
                    if (cb.memberid.ToString() != "0"){
                        blnexist = true;
                    }
                    else {
                        blnexist = false;
                    }
                }
                CusSup_m_CusSup m = new CusSup_m_CusSup();
                
                CusSup_m_Citibella ctmp = new CusSup_m_Citibella();
                if (blnexist == false){
                    m.cocode = Session["cocode"].ToString();

                    Configuration_m_Branches bcode = db.Branches.Where(x => x.branchcode == branchcode).FirstOrDefault();
                    m.branchcode = (bcode == null) ? WebConfigurationManager.AppSettings["defaultbranchcode"] : branchcode;
                    m.createdate = DateTime.Now;
                    m.cussuptype = "Customer";
                    m.address = cAESEncryption.getEncryptedString(c.Cust_address);
                    m.postalcode = cAESEncryption.getEncryptedString(c.Cust_PostCode);
                    m.nric = cAESEncryption.getEncryptedString(c.Cust_nric);
                    m.Tel = cAESEncryption.getEncryptedString(c.Cust_phone1);
                    m.mobile = cAESEncryption.getEncryptedString(c.Cust_phone2);
                    m.email = c.Cust_email;
                    m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                    m.cussupname = c.Cust_name;
                    //Changed by ZawZO on 6 Jun 2016
                    //m.dateofbirth = DateTime.ParseExact(c.Cust_DOB, "MM/dd/yy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);  
                    m.dateofbirth = Convert.ToDateTime(c.Cust_DOB);
                    m.status = "Active";
                    m.gender = c.Gender;
                    m.race = c.race;
                    m.nationality = c.nationality;
                    m.nrictype = c.NRICType;
                    int bvalue = 0;
                    if (c.Email.ToUpper() == "Y" || c.Email == "1") bvalue = 1;
                    m.canemail = Convert.ToInt16(bvalue);
                    if (c.SMS.ToUpper() == "Y" || c.SMS == "1")
                        bvalue = 1;
                    else
                        bvalue = 0;
                    m.cansms = Convert.ToInt16(bvalue);
                    if (c.Email.ToUpper() == "Y" || c.Email == "1")
                        bvalue = 1;
                    else
                        bvalue = 0;
                    m.canemail = Convert.ToInt16(bvalue);
                    db.CusSup.Add(m);
                    db.SaveChanges();
                    ctmp = db.CusSupCiti.Where(x => x.Cust_code== c.Cust_code).FirstOrDefault();

                    if (ctmp != null)
                    {
                        ctmp.memberid = m.id;
                        db.SaveChanges();
                    }
                    else
                    {
                        CusSup_m_Citibella newmem = new CusSup_m_Citibella();
                        newmem.Cust_name = c.Cust_name;
                        newmem.Cust_phone1 = c.Cust_phone1;
                        newmem.Cust_phone2 = c.Cust_phone2;
                        newmem.NRICType = c.NRICType;
                        newmem.Status = c.Status;
                        newmem.memberid = m.id;
                        newmem.Saluation = c.Saluation;
                        newmem.Cust_code = c.Cust_code;
                        newmem.Cust_JoinDate = c.Cust_JoinDate;
                        newmem.Cust_email = c.Cust_email;
                        db.CusSupCiti.Add(newmem);
                        db.SaveChanges();


                    }

                    saveToLog(rid, c.id, "CREATE", "Transfer Citi Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode + ", Name" + c.Cust_name + ", Cust Code" + c.Cust_code);
                }
                else {
                    ctmp = db.CusSupCiti.Where(x => x.Cust_code == c.Cust_code).FirstOrDefault();
                    if (ctmp != null) {
                        m = db.CusSup.Where(x => x.id == ctmp.memberid).FirstOrDefault();
                    }

                    if (m == null)
                    {
                        m = new CusSup_m_CusSup();
                        m.cocode = Session["cocode"].ToString();
                        Configuration_m_Branches bcode = db.Branches.Where(x => x.branchcode == branchcode).FirstOrDefault();
                        m.branchcode = (bcode == null) ? WebConfigurationManager.AppSettings["defaultbranchcode"] : branchcode;
                        m.createdate = DateTime.Now;
                        m.cussuptype = "Customer";
                        m.address = cAESEncryption.getEncryptedString(c.Cust_address);
                        m.postalcode = cAESEncryption.getEncryptedString(c.Cust_PostCode);
                        m.nric = cAESEncryption.getEncryptedString(c.Cust_nric);
                        m.Tel = cAESEncryption.getEncryptedString(c.Cust_phone1);
                        m.mobile = cAESEncryption.getEncryptedString(c.Cust_phone2);
                        m.email = c.Cust_email;
                        m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                        m.cussupname = c.Cust_name;
                        m.dateofbirth = Convert.ToDateTime(c.Cust_DOB);
                        m.status = "Active";
                        m.gender = c.Gender;
                        m.race = c.race;
                        m.nationality = c.nationality;
                        m.nrictype = c.NRICType;
                        int bvalue = 0;
                        if (c.Email.ToUpper() == "Y" || c.Email == "1") bvalue = 1;
                        m.canemail = Convert.ToInt16(bvalue);
                        if (c.SMS.ToUpper() == "Y" || c.SMS == "1")
                            bvalue = 1;
                        else
                            bvalue = 0;
                        m.cansms = Convert.ToInt16(bvalue);
                        if (c.Email.ToUpper() == "Y" || c.Email == "1")
                            bvalue = 1;
                        else
                            bvalue = 0;
                        m.canemail = Convert.ToInt16(bvalue);
                        db.CusSup.Add(m);
                        db.SaveChanges();
                        ctmp = db.CusSupCiti.Where(x => x.Cust_code == c.Cust_code).FirstOrDefault();
                        //ctmp.Status = "Transfer";
                        ctmp.memberid = m.id;
                        db.SaveChanges();
                        saveToLog(rid, c.id, "CREATE", "Transfer Citi Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode + ", Name" + c.Cust_name + ", Cust Code" + c.Cust_code);

                    }
                }
                var lid=c.Liability.ElementAt(0).id;
                CusSup_m_CitibellaLiability ltmp = db.CitiLiability.Where(x => x.id==lid).FirstOrDefault();
                if (ltmp != null)
                {
                    //Changed by ZawZO on 13 Jul 2016
                    ltmp.FromAmount = c.Liability.ElementAt(0).FromAmount;
                    ltmp.ToCitiDollar = c.Liability.ElementAt(0).ToCitiDollar;
                    ltmp.ToRewardDollar = c.Liability.ElementAt(0).ToRewardDollar;
                    ltmp.rateid = c.Liability.ElementAt(0).rateid;
                    ltmp.memberid = m.id;
                    ltmp.transfernumber = GeneralController.getGeneratedNewID("cussup_m_citibellaliability", "transfernumber", "TPREFIX", "MEMNO");
                    ltmp.staff = Session["staffid"].ToString();
                    ltmp.transferdate = DateTime.Now;
                    db.SaveChanges();
                    using (var context = new BigMacEntities())
                    {
                        var liablility = context.Database.ExecuteSqlCommand("Update cussup_m_citibellaliability set status='Transfer' where id=" + lid.ToString());
                    }
                    
                    
                    saveToLog(rid, c.id, "UPDTE", "Update Citi Member liablility-" + ltmp.id.ToString() );
                    //Changed by ZawZO on 14 Jul 2016
                    //if (ltmp.Liability > 0)
                    //{
                    //    CusSup_m_CusRedemption rc = new CusSup_m_CusRedemption();
                    //    //Changed by ZawZO on 13 Jul 2016
                    //    rc.credit = ltmp.Liability * ltmp.ToCitiDollar;
                    //    rc.debit = 0;
                    //    rc.redemptiontype = "C$";
                    //    rc.cussupid = m.id;
                    //    rc.productdesc = "Opening Balance from Transfer Customer";
                    //    rc.resource = rcode;
                    //    SaveToRedemptionTable(rc, rid, m.id, m.inhousecode);
                    //    CusSup_m_CusRedemption rb = new CusSup_m_CusRedemption();
                    //    rb.productdesc = "Opening Balance from Transfer Customer";
                    //    //Changed by ZawZO on 13 Jul 2016
                    //    rb.credit = ltmp.Liability * ltmp.ToRewardDollar;
                    //    rb.debit = 0;
                    //    rb.redemptiontype = "B$";
                    //    rb.cussupid = m.id;
                    //    rb.resource = rcode;
                    //    SaveToRedemptionTable(rb, rid, m.id, m.inhousecode);
                    //}
                    //else
                    //{
                    //    CusSup_m_CusRedemption rc = new CusSup_m_CusRedemption();
                    //    rc.productdesc = "Opening Balance from Transfer Customer";
                    //    //Changed by ZawZO on 13 Jul 2016
                    //    rc.debit = ltmp.Liability * ltmp.ToCitiDollar;
                    //    rc.credit = 0;
                    //    rc.redemptiontype = "C$";
                    //    rc.cussupid = m.id;
                    //    rc.resource = rcode;
                    //    SaveToRedemptionTable(rc, rid, m.id, m.inhousecode);
                    //    CusSup_m_CusRedemption rb = new CusSup_m_CusRedemption();
                    //    rb.productdesc = "Opening Balance from Transfer Customer";
                    //    //Changed by ZawZO on 13 Jul 2016
                    //    rb.debit = ltmp.Liability * ltmp.ToRewardDollar;
                    //    rb.credit = 0;
                    //    rb.redemptiontype = "B$";
                    //    rb.cussupid = m.id;
                    //    rb.resource = rcode;
                    //    SaveToRedemptionTable(rb, rid, m.id, m.inhousecode);
                    //}
                    if (ltmp.ToCitiDollar > 0) {
                        CusSup_m_CusRedemption rc = new CusSup_m_CusRedemption();
                        rc.credit =  ltmp.ToCitiDollar;
                        rc.debit = 0;
                        rc.redemptiontype = "C$";
                        rc.cussupid = m.id;
                        rc.remark = ltmp.ReceiptNo;
                        rc.lastmodifieddate = DateTime.Now;
                        int ptid = 0;
                        Product_m_Product pt = db.products.Where(x => x.desc == "Opening Balance from Customer Transfer").FirstOrDefault();
                        if (pt != null) {
                            ptid = pt.id;
                        }
                        rc.productid = ptid;
                        rc.productdesc = "Opening Balance from Customer Transfer";
                        rc.resource = rcode;
                        rc.invoiceitemid = ltmp.id;
                        SaveToRedemptionTable(rc, rid, m.id, m.inhousecode);
                        if (ltmp.ToRewardDollar > 0) {
                            CusSup_m_CusRedemption rb = new CusSup_m_CusRedemption();
                            rb.productid = ptid;
                            rb.productdesc = "Opening Balance from Customer Transfer";
                            rb.credit =  ltmp.ToRewardDollar;
                            rb.debit = 0;
                            rb.redemptiontype = "B$";
                            rb.cussupid = m.id;
                            rb.remark = ltmp.ReceiptNo;
                            rb.resource = rcode;
                            rb.invoiceitemid = ltmp.id;
                            rb.lastmodifieddate = DateTime.Now;
                            SaveToRedemptionTable(rb, rid, m.id, m.inhousecode);
                        }
                    }
                    else if (ltmp.FromAmount>0) {
                        CusSup_m_CusRedemption rb = new CusSup_m_CusRedemption();
                        using (var context = new BigMacEntities())
                        {
                            var prodlist = context.Database.SqlQuery<Product_m_Product>("call getProductListWithAltCode('" + ltmp.Code + "')").ToList();

                            if (prodlist.Count == 0)
                            {    
                                createSequoiaProduct(ltmp.Description, ltmp.Code);
                                prodlist = context.Database.SqlQuery<Product_m_Product>("call getProductListWithAltCode('" + ltmp.Code + "')").ToList();
                            }


                            if (prodlist != null) {
                                if (prodlist.Count > 0) {
                                    rb.productid = prodlist.ElementAt(0).id;
                                    rb.productdesc = prodlist.ElementAt(0).desc.Trim();
                                    rb.uom = prodlist.ElementAt(0).uom;

                                    Product_m_Product prodPackage = db.products.Where(x => x.desc == rb.productdesc.Trim() && x.category == "Package").FirstOrDefault();
                                    int packid  = 0;
                                    if (prodPackage == null)
                                    {
                                       packid = createSequoiaPackage(rb.productdesc.Trim(), "");
                                       addSequoiaPackageBundle(packid, rb.productid);
                                    }
                                    else
                                    {
                                        Product_m_ProductBundles pkgExist = db.productBundle.Where(x => x.itemid == rb.productid && x.productid == prodPackage.id).FirstOrDefault();
                                        if (pkgExist == null)
                                        {
                                            addSequoiaPackageBundle(prodPackage.id, rb.productid);
                                        }
                                    }
                                }
                            }
                        }
                        Product_m_ProductBundles pkg = db.productBundle.Where(x => x.itemid == rb.productid).FirstOrDefault();
                        int pkgid = 0;
                        if (pkg != null) {
                            pkgid = pkg.productid;
                            Product_m_Product ppkg = db.products.Where(x => x.id== pkgid).FirstOrDefault();
                            if (ppkg != null){
                                rb.packagecode = ppkg.productcode.Trim();
                                rb.packagedesc = ppkg.desc.Trim();
                            }
                        }
                        rb.credit = ltmp.FromAmount;
                        rb.debit = 0;
                        rb.redemptiontype = "PQ";
                        rb.cussupid = m.id;
                        rb.resource = rcode;
                        rb.invoiceitemid = ltmp.id;
                        rb.remark = ltmp.ReceiptNo;
                        rb.lastmodifieddate = DateTime.ParseExact(c.Cust_JoinDate, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None); 
                     
                        rb.RefNo = GeneralController.getGeneratedNewID("CusSup_m_CusRedemption", "RefNo", "TRNPREFIX", "TRNNO");
                        SaveToRedemptionTable(rb, rid, m.id, m.inhousecode);
                    }
                }

                rmid = m.id;
                using (var context = new BigMacEntities())
                {
                    var blogNames = context.Database.ExecuteSqlCommand("Update CusSup_m_Citibella set memberid =" + m.id.ToString() + " where Cust_code='" +c.Cust_code +"'");
                    //Added by ZawZO on 15 Jul 2016, to update CusSup_m_Citibella status
                    CusSup_m_CitibellaLiability cl = db.CitiLiability.Where(x => x.CustCode == c.Cust_code && x.Status == "Active").FirstOrDefault();
                    if (cl == null)
                    {
                        var tmp = context.Database.ExecuteSqlCommand("Update CusSup_m_Citibella set Status='Transfer' where Cust_Code='" + c.Cust_code +"'");
                    }
                } 
                
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Json(rmid, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 20 Aug 2016

        [HttpPost]
        public  int createSequoiaProduct(string productdesc = "", string altCode = "")
        {
            string curr = "";
            ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

            if (config.Count > 0)
                curr = config.ElementAt(0).value;
            else
                curr = "";

            Product_m_Product ptmp = new Product_m_Product();
            string catdesc = ""; string subcatdesc = ""; string uomdesc = "";
            ptmp.createdate = DateTime.Now;
            ptmp.lastmodifieddate = DateTime.Now;
            ptmp.cocode = Convert.ToString(Session["cocode"]);
            ptmp.desc = productdesc;
            Product_z_Category pcat = db.productCategory.Where(x => x.Category == "Sequoia Product").FirstOrDefault();
            if (pcat != null)
            {
                catdesc = pcat.Category;
            }
            Product_z_CategorySub psubcat = db.productSubCategory.Where(x => x.Category == "Sequoia Product").FirstOrDefault();
            if (psubcat != null)
            {
                subcatdesc = psubcat.Category;
            }
            Common_z_UnitofMeasurment puom = db.UOM.Where(x => x.UOM == "Each").FirstOrDefault();
            if (puom != null)
            {
                uomdesc = puom.UOM;
            }
            ptmp.category = catdesc;
            ptmp.categorysub = subcatdesc;
            ptmp.brand = "-";
            ptmp.RangesNSeries = "-";
            ptmp.uom = uomdesc;
            ptmp.uom2 = uomdesc;
            ptmp.uom3 = uomdesc;
            ptmp.uomfactor2 = 0;
            ptmp.uomfactor3 = 0;
            ptmp.stock = 0;
            ptmp.status = "Active";
            ptmp.productcode = getGeneratedNewProductID(ptmp.category, ptmp.categorysub, ptmp.brand, ptmp.RangesNSeries);
            db.products.Add(ptmp);
            db.SaveChanges();
            using (var context = new BigMacEntities())
            {
                var blogNames = context.Database.ExecuteSqlCommand("Update Product_z_CategorySub set lastnumber = IFNULL(lastnumber,0) + 1 where Category ='" + ptmp.categorysub + "'");
            }   
            Product_m_ProductPrice pr = new Product_m_ProductPrice();
            pr.productid = ptmp.id;
            pr.currency = curr;
            pr.exchangerate = 1;
            pr.createdate = DateTime.Now;
            pr.lastmodifieddate = DateTime.Now;
            pr.productid = ptmp.id;
            pr.pricetype = "Selling Price";
            pr.stockreceivedref = 0;
            pr.uom = uomdesc;
            pr.sellprice = 0;
            pr.redeemdollars = 0;
            pr.redeembonus = 0;
            pr.awarddollars = 0;
            pr.awardbonus = 0;
            pr.servicecommission = 0;
            db.productprices.Add(pr);
            db.SaveChanges();
            Product_m_ProductAltCode palt = new Product_m_ProductAltCode();
            palt.productid = ptmp.id;
            palt.createdate = DateTime.Now;
            palt.lastmodifieddate = DateTime.Now;
            palt.altcode = altCode;
            db.productALTCode.Add(palt);
            db.SaveChanges();
            return ptmp.id;
        }
        
        [HttpPost]
        public void addSequoiaPackageBundle(int productid = 0,int itemid = 0)
        {
            Product_m_ProductBundles bundle = new Product_m_ProductBundles();
            bundle.productid = productid;
            bundle.itemid = itemid;
            bundle.qty = 1;
            bundle.createdate = DateTime.Now;
            bundle.lastmodifieddate = DateTime.Now;
            db.productBundle.Add(bundle);
            db.SaveChanges();
        }
         


        [HttpPost]
        public int createSequoiaPackage(string productdesc = "", string altCode = "")
        {
            string curr = "";
            ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

            if (config.Count > 0)
                curr = config.ElementAt(0).value;
            else
                curr = "";

            Product_m_Product ptmp = new Product_m_Product();
            string catdesc = ""; string subcatdesc = ""; string uomdesc = "";
            ptmp.createdate = DateTime.Now;
            ptmp.lastmodifieddate = DateTime.Now;
            ptmp.cocode = Convert.ToString(Session["cocode"]);
            ptmp.desc = productdesc;
            Product_z_Category pcat = db.productCategory.Where(x => x.Category == "Package").FirstOrDefault();
            if (pcat != null)
            {
                catdesc = pcat.Category;
            }
            Product_z_CategorySub psubcat = db.productSubCategory.Where(x => x.Category == "Sequoia Product").FirstOrDefault();
            if (psubcat != null)
            {
                subcatdesc = psubcat.Category;
            }
            Common_z_UnitofMeasurment puom = db.UOM.Where(x => x.UOM == "Session").FirstOrDefault();
            if (puom != null)
            {
                uomdesc = puom.UOM;
            }
            ptmp.category = catdesc;
            ptmp.categorysub = subcatdesc;
            ptmp.brand = "-";
            ptmp.RangesNSeries = "-";
            ptmp.uom = uomdesc;
            ptmp.uom2 = uomdesc;
            ptmp.uom3 = uomdesc;
            ptmp.uomfactor2 = 0;
            ptmp.uomfactor3 = 0;
            ptmp.stock = 0;
            ptmp.status = "Active";
            ptmp.productcode = getGeneratedNewProductID(ptmp.category, ptmp.categorysub, ptmp.brand, ptmp.RangesNSeries);
            db.products.Add(ptmp);
            db.SaveChanges();
            using (var context = new BigMacEntities())
            {
                var blogNames = context.Database.ExecuteSqlCommand("Update Product_z_CategorySub set lastnumber = IFNULL(lastnumber,0) + 1 where Category ='" + ptmp.categorysub + "'");
            }
            Product_m_ProductPrice pr = new Product_m_ProductPrice();
            pr.productid = ptmp.id;
            pr.currency = curr;
            pr.exchangerate = 1;
            pr.createdate = DateTime.Now;
            pr.lastmodifieddate = DateTime.Now;
            pr.productid = ptmp.id;
            pr.pricetype = "Selling Price";
            pr.stockreceivedref = 0;
            pr.uom = uomdesc;
            pr.sellprice = 0;
            pr.redeemdollars = 0;
            pr.redeembonus = 0;
            pr.awarddollars = 0;
            pr.awardbonus = 0;
            pr.servicecommission = 0;
            db.productprices.Add(pr);
            db.SaveChanges();
            Product_m_ProductAltCode palt = new Product_m_ProductAltCode();
            palt.productid = ptmp.id;
            palt.createdate = DateTime.Now;
            palt.lastmodifieddate = DateTime.Now;
            palt.altcode = altCode;
            db.productALTCode.Add(palt);
            db.SaveChanges();

           
            return ptmp.id;
        }
        //Added by ZawZO on 20 Aug 2016
        public static string getGeneratedNewProductID(string cat = "Product", string subcat = "SPA", string brand = "InHouse", string rnc = "")
        {
            string returnvalue = "";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    MySqlCommand cmd = new MySqlCommand("getGenerateProductID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("cat", cat));
                    cmd.Parameters.Add(new MySqlParameter("subcat", subcat));
                    cmd.Parameters.Add(new MySqlParameter("brand", brand));
                    cmd.Parameters.Add(new MySqlParameter("rnc", rnc));
                    var Result = new MySqlParameter();
                    Result.ParameterName = "pcode";
                    Result.MySqlDbType = MySqlDbType.VarChar;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Result);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    returnvalue = Result.Value.ToString();
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
        [HttpPost]
        public JsonResult getMemberCode(int id=0)
        {
            var membercode = "";
            CusSup_m_CusSup m = db.CusSup.Where(x => x.id == id).FirstOrDefault();
            if (m != null){
                membercode = m.inhousecode;
            }
            return Json(membercode, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveToRedemptionTable(CusSup_m_CusRedemption r, int rid =0 , int mid=0, string mcode="")
        {
            var returnStr = "FAIL";
            try
            {
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == r.cussupid && x.redemptiontype == r.redemptiontype).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                r.cocode = Session["cocode"].ToString();
                r.branchcode = Session["branchcode"].ToString();
                r.createdate = DateTime.Now;
                r.createid = Convert.ToInt32(Session["userid"].ToString());
                r.RefNo = GeneralController.getGeneratedNewID("CusSup_m_CusRedemption", "RefNo", "TRNPREFIX", "TRNNO");
                r.balance = (r.credit - r.debit) + prebalance;
                db.CusSupRedemption.Add(r);
                db.SaveChanges();
                saveToLog(rid, r.id, "CREATE", "Add Opening Balance for Customer Transfer, CustID - " + mid + ", CustCode - " + mcode + ",Ref No-" + r.RefNo + ", Type -" + r.redemptiontype + ", Redemption ID - " + r.id.ToString() + ",Debit -" + r.debit.ToString() + ", Credit -" + r.credit.ToString());
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult POSMembershipSave(CusSup_m_CusSup m, string rcode = "POS")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                if (m.id == 0)
                {
                    m.cocode = Session["cocode"].ToString();
                    m.branchcode = Session["branchcode"].ToString();
                    m.createdate = DateTime.Now;
                    m.cussuptype = "Customer";
                    m.nric = cAESEncryption.getEncryptedString(m.nric);
                    m.mobile = cAESEncryption.getEncryptedString(m.mobile);
                    m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                    db.CusSup.Add(m);
                    db.SaveChanges();
                    saveToLog(rid, m.id, "CREATE", "Add New Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode);
                    for (int i = 0; i < (m.cards.Count()); i++)
                    {
                        m.cards.ElementAt(i).cussupid = m.id;
                        m.cards.ElementAt(i).createdate = DateTime.Now;
                        m.cards.ElementAt(i).lastmodifieddate = DateTime.Now;

                        if (m.cards.ElementAt(i).cardtype.ToUpper() != "TEMPORARY")
                        {
                            //    int cno = m.cards.ElementAt(i).cardno;
                            //    ICollection<CusSup_m_Cards> tmpcard= db.MemberCard.Where(x => x.cardno == cno).ToList();
                            //    if (tmpcard != null)
                            //    {
                            //        if (tmpcard.Count > 0)
                            //            m.cards.ElementAt(i).cardno = generateMemberCardNo();
                            //    }
                            m.cards.ElementAt(i).cardno = generateMemberCardNo();
                            updateMemberNewCardNo(m.cards.ElementAt(i).cardno.ToString(), "POS");
                        }
                        db.MemberCard.Add(m.cards.ElementAt(i));
                    }
                    //db.SaveChanges();
                    //saveToLog(rid,m.inhousecode,"")                    
                }

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            //dynamic obj = new ExpandoObject();
            //obj.status = returnStr;
            //obj.mid = m.id;
            var rmid = "";
            if (returnStr == "SUCCESS") rmid = "SUCCESS" + ","+ m.id +","+  m.inhousecode + "," + m.cussupname;

            return Json(rmid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TopUpMembershipSave(CusSup_m_CusSup m, string rcode = "TOPUP")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                if (m.id == 0)
                {
                    m.cocode = Session["cocode"].ToString();
                    m.branchcode = Session["branchcode"].ToString();
                    m.createdate = DateTime.Now;
                    m.cussuptype = "Customer";
                    m.nric = cAESEncryption.getEncryptedString(m.nric);
                    m.mobile = cAESEncryption.getEncryptedString(m.mobile);
                    m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                    db.CusSup.Add(m);
                    db.SaveChanges();
                    saveToLog(rid, m.id, "CREATE", "Add New Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode);
                    for (int i = 0; i < (m.cards.Count()); i++)
                    {
                        m.cards.ElementAt(i).cussupid = m.id;
                        m.cards.ElementAt(i).createdate = DateTime.Now;
                        m.cards.ElementAt(i).lastmodifieddate = DateTime.Now;
                        
                        if (m.cards.ElementAt(i).cardtype.ToUpper() != "TEMPORARY")
                        {
                        //    int cno = m.cards.ElementAt(i).cardno;
                        //    ICollection<CusSup_m_Cards> tmpcard= db.MemberCard.Where(x => x.cardno == cno).ToList();
                        //    if (tmpcard != null)
                        //    {
                        //        if (tmpcard.Count > 0)
                        //            m.cards.ElementAt(i).cardno = generateMemberCardNo();
                        //    }
                         m.cards.ElementAt(i).cardno = generateMemberCardNo();
                         updateMemberNewCardNo(m.cards.ElementAt(i).cardno.ToString(), "TopUP");
                        }                        
                        db.MemberCard.Add(m.cards.ElementAt(i));
                    }
                    db.SaveChanges();
                    //saveToLog(rid,m.inhousecode,"")                    
                }

                returnStr =  "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }

            //dynamic obj = new ExpandoObject();
            //obj.status = returnStr;
            //obj.mid = m.id;
            var rmid = 0;
            if (returnStr == "SUCCESS") rmid = m.cards.ElementAt(0).cardno;

            return Json(rmid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MembershipSave(CusSup_m_CusSup m,string rcode="MEMBERSHIPENQUIRY")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                if (m.id == 0)
                {
                    m.cocode = Session["cocode"].ToString();
                    m.branchcode = Session["branchcode"].ToString();
                    m.departmentid = m.departmentid;
                    m.departmentname = m.departmentname;
                    m.createdate = DateTime.Now;
                    m.cussuptype = "Customer";
                    m.address = cAESEncryption.getEncryptedString(m.address);
                    m.postalcode = cAESEncryption.getEncryptedString(m.postalcode);
                    m.nric = cAESEncryption.getEncryptedString(m.nric);
                    m.Tel = cAESEncryption.getEncryptedString(m.Tel);
                    m.mobile = cAESEncryption.getEncryptedString(m.mobile);
                    m.email = cAESEncryption.getEncryptedString(m.email);
                    m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                    //m.photo = "";
                    //cs.
                    db.CusSup.Add(m);
                    db.SaveChanges();
                    saveToLog(rid, m.id, "CREATE", "Add New Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode);
                    if (m.cards != null)
                    {
                        for (int i = 0; i < (m.cards.Count()); i++)
                        {
                            m.cards.ElementAt(i).cussupid = m.id;
                            m.cards.ElementAt(i).createdate = DateTime.Now;
                            m.cards.ElementAt(i).lastmodifieddate = DateTime.Now;
                            if (m.cards.ElementAt(i).cardtype.ToUpper() != "TEMPORARY")
                            {
                                //int cno = m.cards.ElementAt(i).cardno;
                                //ICollection<CusSup_m_Cards> tmpcard = db.MemberCard.Where(x => x.cardno == cno).ToList();
                                //if (tmpcard != null)
                                //{
                                //    if (tmpcard.Count > 0)
                                //        m.cards.ElementAt(i).cardno = generateMemberCardNo();
                                //}
                                m.cards.ElementAt(i).cardno = generateMemberCardNo();
                                updateMemberNewCardNo(m.cards.ElementAt(i).cardno.ToString(), "TopUP");
                            }
                            
                            db.MemberCard.Add(m.cards.ElementAt(i));
                        }
                        db.SaveChanges();
                        //saveToLog(rid,m.inhousecode,"")
                    }
                }
                else
                {
                    CusSup_m_CusSup mtmp = db.CusSup.Find(m.id);
                    string from = ""; string to = "";
                    if (mtmp != null)
                    {
                        //mtmp.cocode = Session["cocode"].ToString();
                        //mtmp.branchcode = Session["branchcode"].ToString();
                        //mtmp.createdate = DateTime.Today;
                        from = "Name-" + mtmp.cussupname + " , type - " + mtmp.cussuptype + ", DOB =" + string.Format("{0:dd/MM/yyyy}", mtmp.dateofbirth) + ", Status - " + mtmp.status + ", Address -" + cAESEncryption.getDecryptedString(mtmp.address) + cAESEncryption.getDecryptedString(mtmp.postalcode) + ", nric -" + cAESEncryption.getDecryptedString(mtmp.nric) + " , tel -" + cAESEncryption.getDecryptedString(mtmp.Tel) + " , mobile -" + cAESEncryption.getDecryptedString(mtmp.mobile) + ", email -" + cAESEncryption.getDecryptedString(mtmp.email) + ", photo - " + mtmp.photo + ", nrictype -" + mtmp.nrictype + ", Natinality -" + mtmp.nationality + ", race -" + mtmp.race + ", gender -" + mtmp.gender + ", cancall-" + mtmp.cancall + ", cansms -" + mtmp.cansms + ", caneamil -" + mtmp.canemail;
                        to = "Name-" + m.cussupname + " , type - " + m.cussuptype + ", DOB =" + string.Format("{0:dd/MM/yyyy}", m.dateofbirth) + ", Status - " + m.status + ", Address -" + m.address + m.postalcode + ", nric -" + m.nric + " , tel -" + m.Tel + " , mobile -" + m.mobile + ", email -" + m.email + ", photo - " + m.photo + ", nrictype -" + m.nrictype + ", Natinality -" + m.nationality + ", race -" + m.race + ", gender -" + m.gender + ", cancall-" + m.cancall + ", cansms -" + m.cansms + ", caneamil -" + m.canemail;
                        mtmp.cussuptype = "Customer";
                        mtmp.cussupname = m.cussupname;
                        mtmp.occupation = m.occupation;
                        mtmp.dateofbirth = m.dateofbirth;
                        //mtmp.departmentid = m.departmentid;
                        mtmp.departmentname = m.departmentname;
                        //mtmp.status = m.status;
                        mtmp.address = cAESEncryption.getEncryptedString(m.address);
                        mtmp.postalcode = cAESEncryption.getEncryptedString(m.postalcode);
                        mtmp.nric = cAESEncryption.getEncryptedString(m.nric);
                        mtmp.Tel = cAESEncryption.getEncryptedString(m.Tel);
                        mtmp.mobile = cAESEncryption.getEncryptedString(m.mobile);
                        mtmp.email = cAESEncryption.getEncryptedString(m.email);
                        mtmp.photo = m.photo;
                        mtmp.nrictype = m.nrictype;
                        mtmp.nationality = m.nationality;
                        mtmp.race = m.race;
                        mtmp.gender = m.gender;
                        mtmp.cancall = m.cancall;
                        mtmp.canemail = m.canemail;
                        mtmp.cansms = m.cansms;
                        db.SaveChanges();
                        saveToLog(rid, m.id, "UPDATE", "Update Member MID -" + mtmp.id.ToString() + ", Member Code" + m.inhousecode,from,to);
                        if (m.cards != null)
                        {
                            for (int i = 0; i < m.cards.Count(); i++)
                            {
                                if (m.cards.ElementAt(i).id == 0)
                                {
                                    m.cards.ElementAt(i).cussupid = m.id;
                                    m.cards.ElementAt(i).createdate = DateTime.Now;
                                    m.cards.ElementAt(i).lastmodifieddate = DateTime.Now;
                                    if (m.cards.ElementAt(i).cardtype.ToUpper() != "TEMPORARY")
                                    {
                                        //ICollection<CusSup_m_Cards> tmpcard = db.MemberCard.Where(x => x.cardno == m.cards.ElementAt(i).cardno).ToList();
                                        //if (tmpcard != null)
                                        //{
                                        //    if (tmpcard.Count > 0)
                                        //        m.cards.ElementAt(i).cardno = generateMemberCardNo();
                                        //}
                                        m.cards.ElementAt(i).cardno = generateMemberCardNo();
                                        updateMemberNewCardNo(m.cards.ElementAt(i).cardno.ToString(), "TopUP");
                                    }
                                    db.MemberCard.Add(m.cards.ElementAt(i));
                                    db.SaveChanges();
                                    saveToLog(rid, m.cards.ElementAt(i).id, "CREATE", "Add new card, Member Code -" + mtmp.inhousecode + ",Member MID -" + mtmp.id.ToString() + " Card ID-" + m.cards.ElementAt(i).id,"Card No -" +  m.cards.ElementAt(i).cardno + ", Ctype - " + m.cards.ElementAt(i).cardtype + ", ExpDate -" + string.Format("{0:dd/MM/yyyy}",m.cards.ElementAt(i).expirydate));
                                }
                                else
                                {
                                    CusSup_m_Cards mcardtmp = db.MemberCard.Find(m.cards.ElementAt(i).id);
                                    from = "Card No -" + mcardtmp.cardno + ", Ctype - " + mcardtmp.cardtype + ", ExpDate -" + string.Format("{0:dd/MM/yyyy}", mcardtmp.expirydate);
                                    to = "Card No -" + m.cards.ElementAt(i).cardno + ", Ctype - " + m.cards.ElementAt(i).cardtype + ", ExpDate -" + string.Format("{0:dd/MM/yyyy}", m.cards.ElementAt(i).expirydate);
                                    mcardtmp.cussupid = m.id;
                                    mcardtmp.cardtype = m.cards.ElementAt(i).cardtype;
                                    mcardtmp.cardno = m.cards.ElementAt(i).cardno;
                                    mcardtmp.expirydate = m.cards.ElementAt(i).expirydate;
                                    //bdltmp.lastmodifieddate
                                    db.SaveChanges();
                                    saveToLog(rid, m.cards.ElementAt(i).id, "UPDATE", "Update Card , Member Code -" + mtmp.inhousecode + ",Member MID -" + mtmp.id.ToString() + " Card ID-" + mcardtmp.id,from,to);
                                }
                            }
                        }
                    }
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { 
                returnStr = e.Message.ToString(); 
            }

            //dynamic obj = new ExpandoObject();
            //obj.status = returnStr;
            //obj.mid = m.id;
            var rmid=0;
            if (returnStr == "SUCCESS") rmid = m.id;

            return Json(rmid, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 13 Jan 2016
        [HttpPost]
        public JsonResult CustomerSave(CusSup_m_CusSup m, string rcode = "MEMBERSHIPENQUIRY")
        {
            var returnStr = "FAIL";
            var strmobileno = "";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                if (m.id == 0)
                {
                    m.cocode = Session["cocode"].ToString();
                    m.branchcode = Session["branchcode"].ToString();
                    m.createdate = DateTime.Now;
                    m.cussuptype = "Walk in Customer";
                    m.address = cAESEncryption.getEncryptedString(m.address);
                    m.postalcode = cAESEncryption.getEncryptedString(m.postalcode);
                    m.nric = cAESEncryption.getEncryptedString(m.nric);
                    m.Tel = cAESEncryption.getEncryptedString(m.Tel);
                    strmobileno = m.mobile;
                    m.mobile = cAESEncryption.getEncryptedString(m.mobile);
                    m.email = m.email;
            
                    m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "MEMPREFIX", "MEMNO");
                    //m.inhousecode = GeneralController.getGeneratedNewID("CusSup_m_CusSup", "inhousecode", "CUSTPREFIX", "CUSTNO");
                    db.CusSup.Add(m);
                    db.SaveChanges();
                    saveToLog(rid, m.id, "CREATE", "Add New Member MID -" + m.id.ToString() + ", Member Code -" + m.inhousecode);
                }
                else
                {
                    CusSup_m_CusSup mtmp = db.CusSup.Find(m.id);
                    string from = ""; string to = "";
                    if (mtmp != null)
                    {
                        from = "Name-" + mtmp.cussupname + " , type - " + mtmp.cussuptype + ", DOB =" + string.Format("{0:dd/MM/yyyy}", mtmp.dateofbirth) + ", Status - " + mtmp.status + ", Address -" + cAESEncryption.getDecryptedString(mtmp.address) + cAESEncryption.getDecryptedString(mtmp.postalcode) + ", nric -" + cAESEncryption.getDecryptedString(mtmp.nric) + " , tel -" + cAESEncryption.getDecryptedString(mtmp.Tel) + " , mobile -" + cAESEncryption.getDecryptedString(mtmp.mobile) + ", email -" + cAESEncryption.getDecryptedString(mtmp.email) + ", photo - " + mtmp.photo + ", nrictype -" + mtmp.nrictype + ", Natinality -" + mtmp.nationality + ", race -" + mtmp.race + ", gender -" + mtmp.gender + ", cancall-" + mtmp.cancall + ", cansms -" + mtmp.cansms + ", caneamil -" + mtmp.canemail;
                        to = "Name-" + m.cussupname + " , type - " + m.cussuptype + ", DOB =" + string.Format("{0:dd/MM/yyyy}", m.dateofbirth) + ", Status - " + m.status + ", Address -" + m.address + m.postalcode + ", nric -" + m.nric + " , tel -" + m.Tel + " , mobile -" + m.mobile + ", email -" + m.email + ", photo - " + m.photo + ", nrictype -" + m.nrictype + ", Natinality -" + m.nationality + ", race -" + m.race + ", gender -" + m.gender + ", cancall-" + m.cancall + ", cansms -" + m.cansms + ", caneamil -" + m.canemail;
                        mtmp.cussuptype = "Customer";
                        mtmp.cussupname = m.cussupname;
                        mtmp.occupation = m.occupation;
                        mtmp.dateofbirth = m.dateofbirth;
                        mtmp.address = cAESEncryption.getEncryptedString(m.address);
                        mtmp.postalcode = cAESEncryption.getEncryptedString(m.postalcode);
                        mtmp.nric = cAESEncryption.getEncryptedString(m.nric);
                        mtmp.Tel = cAESEncryption.getEncryptedString(m.Tel);
                        strmobileno = m.mobile;
                        mtmp.mobile = cAESEncryption.getEncryptedString(m.mobile);
                        mtmp.email = m.email;
                        mtmp.photo = m.photo;
                        mtmp.nrictype = m.nrictype;
                        mtmp.nationality = m.nationality;
                        mtmp.race = m.race;
                        mtmp.gender = m.gender;
                        mtmp.cancall = m.cancall;
                        mtmp.canemail = m.canemail;
                        mtmp.cansms = m.cansms;
                        db.SaveChanges();
                        saveToLog(rid, m.id, "UPDATE", "Update Member MID -" + mtmp.id.ToString() + ", Member Code" + m.inhousecode, from, to);
                    }
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            var rmid = "";
            if (returnStr == "SUCCESS") rmid = "SUCCESS" + "," + m.id + "," + m.inhousecode + "," + m.cussupname + "," + strmobileno;
            return Json(rmid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getMemberCardList(int memberid = 0)
        {
            try
            {
                ICollection<CusSup_m_Cards> cards = db.MemberCard.Where(x => x.cussupid == memberid).ToList();
                return Json(cards , JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult Member(int id = 0, string type = "Customer")
        {
            try
            {
                CusSup_m_CusSup cussup = new CusSup_m_CusSup();
                

                return View(cussup);
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public ICollection<CusSup_m_CusRedemptionDtl> getMemberTransList(int mid=0, string resource ="Redeem")
        {
            try
            {
                //IQueryable<Invoice_m_Invoice> cusinv = db.sales.Where(x => x.cussupid == cussupid);
                //var pid = db.ConfigDefault.Where(x => x.key == "TUPID").FirstOrDefault().value;
                //int p = 0;
                //if (pid.Length > 0)
                //    p = Convert.Toint(pid);MembershipSave
                using (var context = new BigMacEntities())
                {
                    var transList = context.Database.SqlQuery<CusSup_m_CusRedemptionDtl>("call getMemberTransactionList(" + mid + ",'" + resource + "')").ToList();
                   
                    return transList;
                }
            }
            catch (Exception e)
            { return null; }
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


        //public JsonResult getMemberRedeemList(int mid=0,int pageno =1)
        //{
        //    try
        //    {
        //        int pagesize = Convert.Toint(WebConfigurationManager.AppSettings["pagesize"]);
        //        ICollection<CusSup_m_CusRedemptionDtl> transList = getMemberTransList(mid, "Redeem");
        //        var paginatedQTransList = new PaginatedList<CusSup_m_CusRedemptionDtl>(transList.AsQueryable(), pageno, pagesize);
        //        var paginatedTransList = paginatedQTransList.ToList();

        //        if (paginatedTransList != null)
        //        {

        //            paginatedTransList.ElementAt(0).TotalPages = paginatedQTransList.TotalPages;
        //        }
                
        //        return Json(paginatedTransList, JsonRequestBehavior.AllowGet);
                
        //    }
        //    catch (Exception e)
        //    { return Json("Failed", JsonRequestBehavior.AllowGet); }

        //}

        //public JsonResult getMemberTopUpList(int mid, int pageno = 1)
        //{
        //    try
        //    {                
        //        int pagesize = Convert.Toint(WebConfigurationManager.AppSettings["pagesize"]);
        //        ICollection<CusSup_m_CusRedemptionDtl> transList = getMemberTransList(mid, "TopUp");
        //        var paginatedQTransList = new PaginatedList<CusSup_m_CusRedemptionDtl>(transList.AsQueryable(), pageno, pagesize);
        //        var paginatedTransList = paginatedQTransList.ToList();

        //        if (paginatedTransList != null)
        //        {
        //            if (paginatedTransList.Count > 0)
        //                paginatedTransList.ElementAt(0).TotalPages = paginatedQTransList.TotalPages;
        //        }

        //        return Json(paginatedTransList, JsonRequestBehavior.AllowGet);                
        //    }
        //    catch (Exception e)
        //    { return Json("Failed", JsonRequestBehavior.AllowGet); }

        //}

        public ActionResult getMemberCitiListPaging(int mid, jQueryDataTableParamModel param,string rtype="C$")
        {

            try
            {
                ICollection<CusSup_m_CusRedemption> RList = db.CusSupRedemption.Include("Create").Where(x => x.cussupid == mid && x.redemptiontype == rtype).OrderByDescending(x => x.id).ToList();
               
                for (int i = 0; i < RList.Count; i++)
                {
                    //RList.ElementAt(i).cname = cAESEncryption.getDecryptedString(RList.ElementAt(i).Create.name);
                    RList.ElementAt(i).cname = cAESEncryption.getDecryptedString(RList.ElementAt(i).Create.name);
                }
                //var RList = (from r in RList1 join sale in db.sales on r.RefNo equals sale.resourcecode where r.cussupid == sale.cussupid select new { id = r.id, createdate = r.createdate, branchcode = r.branchcode, RefNo = r.RefNo, productdesc = r.productdesc, remark = sale.remark, debit = r.debit, credit = r.credit, balance = r.balance, cname = r.cname }).ToList();
               
                var searchValue = "";
                                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterRList = RList.Where(x => x.productdesc.ToUpper().Contains(searchValue.ToUpper()) || x.remark.ToUpper().Contains(searchValue.ToUpper()) || x.RefNo.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterRList = filterRList.OrderBy(x => x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderBy(x => x.RefNo).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderBy(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 4)
                        filterRList = filterRList.OrderBy(x => x.debit).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderBy(x => x.credit).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderBy(x => x.balance).ToList();
                    else if (sortColumnIndex == 7)
                        filterRList = filterRList.OrderBy(x => x.cname).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterRList = filterRList.OrderByDescending(x => x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderByDescending(x => x.RefNo).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderByDescending(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 4)
                        filterRList = filterRList.OrderByDescending(x => x.debit).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderByDescending(x => x.credit).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderByDescending(x => x.balance).ToList();
                    else if (sortColumnIndex == 7)
                        filterRList = filterRList.OrderByDescending(x => x.cname).ToList();

                }

                var paginatedQPList = filterRList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = RList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterRList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult getMemberRedeemListPaging(int mid,jQueryDataTableParamModel param)
        {

            try
            {
                ICollection<CusSup_m_CusRedemptionDtl> transList = getMemberTransList(mid, "Redeem");
                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterTList = transList.Where(x => x.productdesc.ToUpper().Contains(searchValue.ToUpper()) || x.remark.ToUpper().Contains(searchValue.ToUpper()) || x.resourcecode.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderBy(x => x.productdesc).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderByDescending(x => x.productdesc).ToList();
                }

                var paginatedQPList = filterTList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = transList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterTList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult getMemberTopUpListPaging(int mid,jQueryDataTableParamModel param)
        {
            
            try
            {
                ICollection<CusSup_m_CusRedemptionDtl> transList = getMemberTransList(mid, "TopUp");
                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterTList = transList.Where(x => x.productdesc.ToUpper().Contains(searchValue.ToUpper()) || x.remark.ToUpper().Contains(searchValue.ToUpper()) || x.resourcecode.ToUpper().Contains(searchValue.ToUpper())).OrderByDescending(x => x.createdate).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderBy(x => x.productdesc).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderByDescending(x => x.productdesc).ToList();
                }

                var paginatedQPList = filterTList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = transList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterTList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult getMemberRedeemPackageListPaging(int mid, jQueryDataTableParamModel param, string rtype = "PACKAGE")
        {

            try
            {
                  var context = new BigMacEntities();
                  ICollection<Redempt_Package> RList = context.Database.SqlQuery<Redempt_Package>("call getCussupPackageItemList(" + mid + ")").ToList();
                  ICollection<Package_Summary> packages = new List<Package_Summary>();

             
                  foreach (Redempt_Package sp in RList)
                  {

                      CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == mid && x.packagecode == sp.packagecode && x.productid == sp.productid && x.uom == sp.uom).OrderBy(x=>x.createdate).FirstOrDefault();
                      Package_Summary summary = new Package_Summary();
                        summary.date = redempt.lastmodifieddate;
                        summary.receipt = redempt.remark ?? redempt.RefNo;
                        summary.packagecode = sp.packagecode;
                        summary.packagedesc = sp.packagedesc;
                        summary.productid  = sp.productid;
                        summary.productdesc  = sp.productdesc;
                        summary.uom  = sp.uom;
                        summary.credit  = sp.credit;
                        summary.debit  = sp.debit;
                        summary.balance = sp.balance;

                        if (redempt.remark != null)
                        {
                            var list = db.CitiLiability.Where(x => x.ReceiptNo == redempt.remark).ToList();
                            if (list.Count > 0)
                            {
                                if (list[0].transfernumber != "")
                                {
                                    summary.remarks = list[0].transfernumber ?? "";
                                }
                                //string remarks = db.CitiLiability.Where(x => x.ReceiptNo == redempt.remark).Select(x => x.transfernumber).FirstOrDefault().ToString();
                             
                            }
                        }

                      packages.Add(summary);
                          
                  }

                var searchValue = "";


                

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterRList = packages.Where(x => x.packagecode.ToUpper().Contains(searchValue.ToUpper()) || x.packagedesc.ToUpper().Contains(searchValue.ToUpper()) || x.productdesc.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderBy(x => x.date).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderBy(x => x.receipt).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderBy(x => x.packagedesc).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderBy(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderBy(x => x.uom).ToList();
                }
                else
                {
                    if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderByDescending(x => x.date).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderByDescending(x => x.receipt).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderByDescending(x => x.packagedesc).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderByDescending(x => x.productdesc).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderByDescending(x => x.uom).ToList();
                }

                var paginatedQPList = filterRList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var finalPaginatedQPList = paginatedQPList; 
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = RList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterRList.Count, //paginatedQPList.TotalCount,
                    aaData = finalPaginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        public JsonResult getMemberRedeemPackageHistoryListPaging(int mid, int pid, string rtype = "PACKAGE")
        {
            //var returnRList;
            try
            {
                ICollection<CusSup_m_CusRedemption> RList = db.CusSupRedemption.Include("Create").Where(x => x.cussupid == mid && x.redemptiontype == rtype && x.productid == pid).OrderByDescending(x => x.id).ToList();
                var returnRList = RList.Join(db.saleItems, p => p.invoiceitemid, itm => itm.id, (p, itm) => new { paginatedQPList = p, saleItems = itm }).Select(x => new { x.paginatedQPList.createdate, x.paginatedQPList.branchcode, x.paginatedQPList.resource, x.paginatedQPList.RefNo, x.paginatedQPList.credit, x.paginatedQPList.debit, x.paginatedQPList.balance, x.paginatedQPList.cname, x.saleItems.invoiceid }).ToList();
                return Json(returnRList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); 
            }            
        }

        public ActionResult getMemberRedeemPackageHistoryListPaging_Old(int mid, int pid, jQueryDataTableParamModel param, string rtype = "PACKAGE")
        {

            try
            {
                ICollection<CusSup_m_CusRedemption> RList = db.CusSupRedemption.Include("Create").Where(x => x.cussupid == mid && x.redemptiontype == rtype && x.productid == pid).OrderByDescending(x => x.id).ToList();
                var context = new BigMacEntities();
                //for (int i = 0; i < RList.Count; i++)
                //{
                //    RList.ElementAt(i).cname = cAESEncryption.getDecryptedString(RList.ElementAt(i).Create.name);
                //}

                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterRList = RList.Where(x => x.productdesc.ToUpper().Contains(searchValue.ToUpper()) || x.uom.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterRList = filterRList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderBy(x => x.RefNo).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderBy(x => x.debit).ToList();
                    else if (sortColumnIndex == 4)
                        filterRList = filterRList.OrderBy(x => x.credit).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderBy(x => x.balance).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderBy(x => x.cname).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterRList = filterRList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterRList = filterRList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterRList = filterRList.OrderByDescending(x => x.RefNo).ToList();
                    else if (sortColumnIndex == 3)
                        filterRList = filterRList.OrderByDescending(x => x.debit).ToList();
                    else if (sortColumnIndex == 4)
                        filterRList = filterRList.OrderByDescending(x => x.credit).ToList();
                    else if (sortColumnIndex == 5)
                        filterRList = filterRList.OrderByDescending(x => x.balance).ToList();
                    else if (sortColumnIndex == 6)
                        filterRList = filterRList.OrderByDescending(x => x.cname).ToList();
                }

                //var paginatedQPList = filterRList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var paginatedQPList = filterRList;
                var finalPaginatedQPList = paginatedQPList.Join(db.saleItems, p => p.invoiceitemid, itm => itm.id, (p, itm) => new { paginatedQPList = p, saleItems = itm }).Select(x => new { x.paginatedQPList.createdate, x.paginatedQPList.branchcode, x.paginatedQPList.resource, x.paginatedQPList.RefNo, x.paginatedQPList.credit, x.paginatedQPList.debit, x.paginatedQPList.balance, x.paginatedQPList.cname, x.saleItems.invoiceid }).ToList(); 
                //var finalPaginatedQPList= paginatedQPList.Join(db.saleItems, p => p.invoiceitemid, itm => itm.id,(p,itm) =>  new { paginatedQPList = p, saleItems = itm }).Select(x=>new { x.paginatedQPList.createdate , x.paginatedQPList.branchcode, x.paginatedQPList.resource , x.paginatedQPList.RefNo ,x.paginatedQPList.productdesc, x.paginatedQPList.credit, x.paginatedQPList.debit, x.paginatedQPList.balance,x.paginatedQPList.cname, x.saleItems.invoiceid }).ToList(); 
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = RList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterRList.Count, //paginatedQPList.TotalCount,
                    aaData = finalPaginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult CusSupSave(int id = 0, string type = "")
        {
            try
            {
                int i = 0;
                ViewBag.TypeOptions = db.CusSupType.AsEnumerable();
                ViewBag.StatusOptions = db.CusSupStatus.AsEnumerable();
                ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
                for (i = 0; i < branches.Count; i++)
                {
                    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                }
                ViewBag.BranchOptions = branches;
                ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                for (i = 0; i < companies.Count; i++)
                {
                    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = companies;

                if (id == 0)
                {
                    CusSup_m_CusSup cussup = new CusSup_m_CusSup();
                    cussup.cussuptype = type;
                    return View(cussup);
                }
                else
                {
                    CusSup_m_CusSup cussup =  db.CusSup.Find(id);
                    cussup.address = cAESEncryption.getDecryptedString(cussup.address);
                    cussup.postalcode = cAESEncryption.getDecryptedString(cussup.postalcode);
                    cussup.nric = cAESEncryption.getDecryptedString(cussup.nric);
                    cussup.Tel = cAESEncryption.getDecryptedString(cussup.Tel);
                    cussup.mobile = cAESEncryption.getDecryptedString(cussup.mobile);
                    cussup.email = cAESEncryption.getDecryptedString(cussup.email);
                    cussup.userid = cAESEncryption.getDecryptedString(cussup.userid);
                    return View(cussup);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult CusSupSave(CusSup_m_CusSup cs)
        {
            return View(cs);
        }

        public ActionResult FeedBackIndex()
        {
            return View(db.CusSupFeedback.ToList());
        }

        public ActionResult FeedbackSave(int id = 0)
        {
            try
            {
                int i = 0;

                ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                for (i = 0; i < companies.Count; i++)
                {
                    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = companies;

                if (id == 0)
                {
                    CusSup_m_CusFeedback c = new CusSup_m_CusFeedback();
                    return View(c);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult RedemptionIndex()
        {
            return View(db.CusSupRedemption.ToList());
        }

        public ActionResult RedemptionSave(int id = 0)
        {
            try
            {
                int i = 0;

                ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
                for (i = 0; i < branches.Count; i++)
                {
                    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                }
                ViewBag.BranchOptions = branches;

                ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                for (i = 0; i < companies.Count; i++)
                {
                    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = companies;

                // ICollection<Product_m_Product> products = db.products.ToList();
                // for (i = 0; i < products.Count; i++)
                //  {
                //     products.ElementAt(i).desc = cAESEncryption.getDecryptedString(products.ElementAt(i).desc);
                //   }
                //  ViewBag.ProductOptions = products;

                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();

                if (id == 0)
                {
                    CusSup_m_CusRedemption c = new CusSup_m_CusRedemption();
                    return View(c);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        //public JsonResult getCustInvoices(int cussupid)
        //{            
        //    try
        //    {                

        //        //IQueryable<Invoice_m_Invoice> cusinv = db.sales.Where(x => x.cussupid == cussupid);
        //        var pid = db.ConfigDefault.Where(x => x.key == "TUPID").FirstOrDefault().value;
        //        int p = 0;
        //        if (pid.Length > 0)
        //            p = Convert.Toint(pid);
        //        var cusinv = db.sales.Join(db.saleItems, s => s.id, itm => itm.invoiceid, (s, itm) => new { sales = s, salesItems = itm }).Where(x => x.sales.cussupid == cussupid && x.salesItems.productid != p).Select(x => new { x.sales.id, x.sales.resourcecode, x.sales.total_total, x.sales.createdate,x.sales.currency}).Distinct();
        //        return Json(cusinv.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    { return Json("Failed", JsonRequestBehavior.AllowGet); }            

        //}



        public JsonResult getCustitems(int cussupid)
        {
            try
            {
                //IQueryable<Invoice_m_Invoice_Items>  
                var cusinv = db.saleItems.Join(db.sales, itm => itm.invoiceid, s => s.id, (itm, s) => new { salesItems = itm, sales = s }).Where(x => x.sales.cussupid == cussupid).Select(x => new { x.salesItems.id, x.salesItems.resourcecode, x.salesItems.productdesc, x.salesItems.qty, x.salesItems.uom, x.salesItems.unitprice, x.salesItems.discountamount});
                return Json(cusinv.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public JsonResult getCustApp(int cussupid)
        {
            try
            {

                IQueryable<Appointment_m_Appointment> cusapp = db.Appointments.Where(x => x.cussupid == cussupid).OrderByDescending(x=>x.appointmentdate) ;
                return Json(cusapp.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public JsonResult getCustTopUp(int cussupid)
        {
            try
            {
                var pid = db.ConfigDefault.Where(x => x.key == "TUPID").FirstOrDefault().value;
                int p=0;
                if (pid.Length > 0)
                    p = Convert.ToInt32(pid);
                var custopup = db.sales.Join(db.saleItems, s => s.id, itm => itm.invoiceid, (s, itm) => new { sales = s, salesItems = itm }).Where(x => x.sales.cussupid == cussupid && x.salesItems.productid == p).Select(x => new { x.sales.id,x.sales.resourcecode,x.sales.total_total,x.sales.createdate });
                return Json(custopup.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
    }
}
