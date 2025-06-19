using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;
using System.Data.Objects;

namespace BigMac.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult Index()
        {
            return View(db.MessageChatGroups.Include("CreateBy").ToList());
        }

        public ActionResult DetailIndex()
        {
            return View(db.MessageChatGroups.Include("CreateBy").Where(x => x.discontinued == 0).ToList());
        }

        [HttpPost]
        public ActionResult changeDFlag(int id=0)
        {
            string returnStr = "";
            try
            {
                Message_m_ChatsGroups cg = db.MessageChatGroups.Find(id);
                if (cg.discontinued == 0)
                    cg.discontinued = 1;
                else
                    cg.discontinued = 0;

                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Content(returnStr);
        }

        public ActionResult ChatGroup(int id = 0)
        {
            if (id == 0)
            {
                Message_m_ChatsGroups chatGroup = new Message_m_ChatsGroups();
                return View(chatGroup);
            }
            else
            {
                Message_m_ChatsGroups chatGroup = db.MessageChatGroups.Find(id);
                return View(chatGroup);
            }
        }

        [HttpPost]
        public ActionResult ChatGroup(Message_m_ChatsGroups chatGroup)
        {

            //dbtmp.ope
            //var userid = db.Users.Where(x =>x.username == User.Identity.Name).AsEnumerable().First().id;
            try
            {
                //if (campaign.id)
                //{
                if (chatGroup.id > 0)
                {
                    Message_m_ChatsGroups ch = db.MessageChatGroups.Find(chatGroup.id);
                    ch.groupname = chatGroup.groupname;
                    db.SaveChanges();
                }
                else
                    SaveNewChatGroup(chatGroup);

            }
            catch (Exception e)
            {
                            }
            return RedirectToAction("Index");
        }

        public void SaveNewChatGroup(Message_m_ChatsGroups chatGroup)
        {
            if (ModelState.IsValid)
            {
                chatGroup.createdbyid = Convert.ToInt32(Session["userid"].ToString());   //userid;
                chatGroup.datecreated = DateTime.Now;
                db.MessageChatGroups.Add(chatGroup);
                db.SaveChanges();
                //return RedirectToAction("CreateStep2", new { id = campaign.id });
            }
        }

        public ActionResult ChatDetail(int gid, string gname)
        {
            ViewBag.gName = gname;
            ViewBag.gID = gid; 
            //return View(db.MessageChats.Where(x=>x.groupid == gid).OrderByDescending(x=>x.datecreated).ToList());    
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(int groupid, int senderid, int receiverid, string message)
        {
            string returnStr = "";
            try
            {
                Message_m_Chats m = new Message_m_Chats();
                m.groupid = groupid;
                m.senderid = senderid;
                m.receiverid = receiverid;
                m.message = message;
                m.datecreated = DateTime.Now;
                m.lastmodifieddate =  DateTime.Now;   
                //staff.status = 
                db.MessageChats.Add(m);
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
        public JsonResult MessageList(int groupid, int mid = 0)
        {
            //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).Include("Group").Include("Branch").Include("Staff").OrderBy(x => new { x.Group.group,x.branchcode,x.Staff.name}).ToList();
            ICollection<Message_m_Chats> msg = db.MessageChats.Include("Sender").Where(x => x.groupid == groupid && x.id > mid).OrderBy(x => x.datecreated).ToList();
            int i = 0;

            for (i = 0; i < msg.Count; i++)
            {
                msg.ElementAt(i).groupName = cAESEncryption.getDecryptedString(msg.ElementAt(i).Sender.username);
                msg.ElementAt(i).ctime = string.Format("{0:HH:mm}", msg.ElementAt(i).datecreated);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
