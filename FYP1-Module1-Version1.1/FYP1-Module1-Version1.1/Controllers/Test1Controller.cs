using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Mvc;
using FYP1_Module1_Version1._1.Models;

namespace FYP1_Module1_Version1._1.Controllers
{
    public class Test1Controller : Controller
    {
        private fypEntities db = new fypEntities();
        // GET: Test1
      
        public Boolean RegisterStudent(student s1)
        {
            if (s1.RollNumber != null)
            {
                Console.WriteLine("Successfully added");
                return true;
            }
            return false;

        }
        public int LoginStudent(student s2)
        {

            var details = (from student in db.students
                           where student.RollNumber == s2.RollNumber && student.Password == s2.Password
                           select new
                           {
                               student.Id,
                               student.Name
                           }).ToList();
            if (details.FirstOrDefault() != null)
            {

                return 1;
            }



            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
                //   Warning("Invalid Credentials", true);

                return 0;

            }
        }
        public bool addIdeasTest(idea i1, string session1)
        {
            if (session1 == "1")
            {
                if (i1.Title != null)
                {
                    db.ideas.Add(i1);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;


        }
   
        public int milestonesTest(milestone m1)
        {
            if (m1.name != null && m1.description != null)
            {
                m1.idregistration = 3;
                db.milestones.Add(m1);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        public bool listmilestonestest(List<milestone> m3)
        {

            
           
            if (m3 != null )
            {
                return true;
                
            }
            
    return false;
        }
        public bool supervisorlogintest(teacher s2)
        {
            var details = (from teacher in db.teachers
                           where teacher.Email == s2.Email && teacher.password == s2.password
                           select new
                           {
                               teacher.Id,
                               teacher.Name
                           }).ToList();
            if (details.FirstOrDefault() != null)
            {

                return true;
            }



            else
            {
              
                return false;

            }

        }
        public bool RegisterSupervisor(teacher s1)
        {
            if (s1.Email != null)
            {
                
                Console.WriteLine("Successfully added");
                return true;
            }
            return false;

        }
        public bool makegrouptest(fyp_group fypgroup)
        {
            var query = (from fypgrp in db.fyp_group
                         where fypgrp.group_member1 != fypgroup.group_member1 && fypgrp.group_member2 != fypgroup.group_member2 && fypgrp.group_member2 != fypgroup.group_member2
                         select new
                         {
                             fypgrp.group_id,
                             fypgrp.group_member1,
                             fypgrp.group_member2,
                             fypgrp.group_member3
                         }).ToList();

            var q1 = query;
            if (fypgroup.group_member1 != null && fypgroup.group_member2 != null && fypgroup.group_member3 != null && query.FirstOrDefault() != null)
            {
                
                return true;
            }

            return false;
        }
        public bool registrationTest(registration r1)
        {
            if (r1.idRegistration != null && r1.co_supervisorEmail != null && r1.co_supervisorName != null && r1.fypgroup != null && r1.idea != null && r1.supervisor != null)
            {
                notification notification = new notification();
                notification.group = 1;
                notification.notifier = "Hamza";
                notification.type = " Wants to register idea ";
                notification.seen = 0;
                notification.registration = r1.idRegistration;
                notification.time = DateTime.Now;
                notification.idea = r1.idea;
                notification.idnotification = 1;
                return true;
            }
            return false;
        }
    
        public bool chatTest()
        {

            Session["Name1"] = "Muhammad Faizan";
            if (Session["Name1"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name1"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == 1)))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name1"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == 1)
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                    conversation con = new conversation();
                    con.message = "Hellow there";
                    con.name = Session["Name1"].ToString();
                    con.time = DateTime.Now;
                    con.idea = 1;
                    con.group = 1;
                    db.conversations.Add(con);
                    db.SaveChanges();
                    notification notification = new notification();
                    notification.group = 1;
                    notification.notifier = Session["Name1"].ToString();
                    notification.type = "Messaged in group chat";
                    notification.seen = 0;
                    notification.time = DateTime.Now;
                    notification.idea = 1;
                    db.notifications.Add(notification);
                    db.SaveChanges();

                
                ChatModel mymodel = new ChatModel();
                if ((int)Session["check"] == 0)
                {
                    int id = (int)Session["currentIdea"];
                    teacher t = db.teachers.Find(db.ideas.Find(id).PostedByTeacher);
                    ViewData["Teacher"] = t;
                    Session["currentIdea"] = id;
                    List<student> fypgroup = new List<student>();
                    student s = db.students.Find((int)Session["Member1"]);
                    fypgroup.Add(s);
                    s = db.students.Find((int)Session["Member2"]);
                    fypgroup.Add(s);
                    s = db.students.Find((int)Session["Member3"]);
                    fypgroup.Add(s);
                    int groupid = (int)Session["GroupID"];
                    /*notification notification = new notification();
                    notification.group = groupid;
                    notification.notifier = Session["Name"].ToString();
                    notification.type = "Interesed in your idea";
                    notification.seen = 0;
                    notification.time = DateTime.Now;
                    notification.idea = (int)id;
                    db.notifications.Add(notification);
                    db.SaveChanges();*/
                    ViewData["fypgroup"] = fypgroup;
                    ViewData["idea"] = db.ideas.Find(id);
                    var query1 = from c in db.conversations
                                 where c.idea == id && c.@group == groupid
                                 select c;

                    mymodel.chat = query1.ToList();

                }
                else if ((int)Session["check"] == 1)
                {
                    notification currentNotification = db.notifications.Find((int)Session["IdNotification"]);
                    idea currentIdea = currentNotification.idea1;
                    teacher currentTeacher = currentIdea.teacher;
                    ViewData["Teacher"] = currentTeacher;
                    Session["currentIdea"] = currentIdea.Id;


                    List<student> fypgroup = new List<student>();
                    fyp_group currentFypGroup = currentNotification.fyp_group;
                    Session["GroupID"] = currentFypGroup.group_id;

                    fypgroup.Add(currentFypGroup.student);
                    fypgroup.Add(currentFypGroup.student1);
                    fypgroup.Add(currentFypGroup.student2);
                    ViewData["fypgroup"] = fypgroup;
                    ViewData["idea"] = currentIdea;
                    List<conversation> currentConversation = new List<conversation>();
                    foreach (conversation con in db.conversations.ToList())
                    {
                        if (con.idea == currentIdea.Id && con.group == currentFypGroup.group_id)
                        {
                            currentConversation.Add(con);
                        }
                    }
                    mymodel.chat = currentConversation;
                }
                return true;

            }
            else
            {
            }

            return true;
        }

    }
}