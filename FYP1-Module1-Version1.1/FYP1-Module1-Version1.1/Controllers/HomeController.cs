
using FYP1_Module1_Version1._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FYP1_Module1_Version1._1.Controllers
{
    public class HomeController : Controller
    {


        private fypEntities db = new fypEntities();
        public ActionResult Index()
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                int groupId = (int)Session["GroupID"];
                var query = (from c in db.registrations
                             where c.fypgroup == groupId && c.approve == 1
                             select new
                             {
                                 c.idRegistration,

                             }).ToList();
                if (query.FirstOrDefault() != null)
                {
                    Session["CHECKREG"] = 1;
                    int regId = query.FirstOrDefault().idRegistration;
                    List<milestone> milestones;
                    var query2 = from c in db.milestones
                                 where c.idregistration == regId
                                 select c;
                    milestones = query2.ToList();
                    ViewData["milestones"] = milestones;     
                }
                else
                {
                    Session["CHECKREG"] = 0;
                }
                ViewData["ideas"] = db.ideas.ToList();
                List<student> fypgroup = new List<student>(3);
                fypgroup.Add(db.students.Find((int)Session["Member1"]));
                fypgroup.Add(db.students.Find((int)Session["Member2"]));
                fypgroup.Add(db.students.Find((int)Session["Member3"]));
                ViewData["fypgroup"] = fypgroup;

                return View(notificationList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStudent(student newStudent)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(newStudent);
                db.SaveChanges();
                Success(string.Format("<b>{0}</b> Successfully Registered!!", newStudent.Name), true);
                Information("You can Login NOW!", true);

                return Redirect("MainView");
            }
            Warning("Registeration Unsuccessful! Please Try Again!!", true);
            return View();
        }

        public ActionResult LoginStudent()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginStudent(student currentStudent)
        {

            var details = (from student in db.students
                           where student.RollNumber == currentStudent.RollNumber && student.Password == currentStudent.Password
                           select new
                           {
                               student.Id,
                               student.Name
                           }).ToList();
            if (details.FirstOrDefault() != null)
            {
                Success(string.Format("<b>{0}</b> Successfully Logined!! Welcome.", details.FirstOrDefault().Name), true);
                Session["Id"] = details.FirstOrDefault().Id;
                Session["Name"] = details.FirstOrDefault().Name;
                Session["Check"] = 0;
                Session["Display"] = "none";
                int id = (int)Session["Id"];
                var detail = (from fyp_group in db.fyp_group
                              where fyp_group.group_member1 == id || fyp_group.group_member2 == id || fyp_group.group_member3 == id
                              select new
                              {
                                  fyp_group.group_id,
                                  fyp_group.group_member1,
                                  fyp_group.group_member2,
                                  fyp_group.group_member3
                              }).ToList();

                if (detail.FirstOrDefault() != null)
                {
                    Session["GroupID"] = detail.FirstOrDefault().group_id;
                    Session["Member1"] = detail.FirstOrDefault().group_member1;
                    Session["Member2"] = detail.FirstOrDefault().group_member2;
                    Session["Member3"] = detail.FirstOrDefault().group_member3;
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return RedirectToAction("MakeGroup", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
                Warning("Invalid Credentials", true);
            }


            return View(currentStudent);
        }

        public ActionResult RegisterSupervisor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterSupervisor(teacher newTeacher)
        {
            if (ModelState.IsValid)
            {

                db.teachers.Add(newTeacher);
                db.SaveChanges();
                Success(string.Format("<b>{0}</b> Successfully Registered!!", newTeacher.Name), true);
                Information("You can Login NOW!", true);
                return Redirect("MainView");
            }
            Warning("Registeration Unsuccessful! Please Try Again!!", true);
            return View();
        }
        public ActionResult LoginSupervisor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginSupervisor(teacher currentTeacher)
        {

            var details = (from teacher in db.teachers
                           where teacher.Email == currentTeacher.Email && teacher.password == currentTeacher.password
                           select new
                           {
                               teacher.Id,
                               teacher.Name
                           }).ToList();
            if (details.FirstOrDefault() != null)
            {
                Session["Id"] = details.FirstOrDefault().Id;
                Session["Name"] = details.FirstOrDefault().Name;
                Session["Check"] = 1;
                Session["Display"] = "block";
                Success(string.Format("<b>{0}</b> Successfully Logined!! Welcome.", details.FirstOrDefault().Name), true);
                return RedirectToAction("SupervisorMainView", "Home");
            }

            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
                Warning("Invalid Credentials", true);
            }

            return View(currentTeacher);
        }

        public ActionResult newRegisterIdea()
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newRegisterIdea(idea newidea)
        {
            if (ModelState.IsValid)
            {

                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                newidea.checker = Convert.ToInt32(Session["Check"]);
                if (newidea.checker == 0)
                    newidea.PostedByStudent = Convert.ToInt32(Session["Id"]);
                else if (newidea.checker == 1)
                    newidea.PostedByTeacher = Convert.ToInt32(Session["Id"]);
                newidea.approve = 0;
                db.ideas.Add(newidea);
                db.SaveChanges();
                Success("Idea Successfuly Added!!", true);
                return RedirectToAction("GetIdea");
            }
            Warning("Failed to Add your Idea.Please Try Again!!", true);
            return View();
        }

        public ActionResult GetIdea()
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                var query1 = from c in db.teachers
                             select c;
                ViewBag.list = query1.ToList();
                var query = from c in db.ideas
                            select c;
                ViewModel mymodel = new ViewModel();
                mymodel.Teachers = query1.ToList();
                mymodel.Ideas = query.ToList();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        public ActionResult GetIdea(ViewModel mymodel)
        {

            var query1 = from c in db.teachers
                         select c;
            mymodel.Teachers = query1.ToList();
            if (mymodel.SelectedTeacherId == 0)
            {
                var query3 = from c in db.ideas
                             select c;
                mymodel.Ideas = query3.ToList();
            }
            else
            {
                var query2 = from c in db.ideas
                             where c.PostedByTeacher == mymodel.SelectedTeacherId
                             select c;
                mymodel.Ideas = query2.ToList();
            }
            return View(mymodel);
        }

        public ActionResult MakeGroup()
        {
            var query1 = from student in db.students
                         select student;
            fyp_group fypgroup = new fyp_group();
            fypgroup.students = query1.ToList();

            return View(fypgroup);
        }
        [HttpPost]
        public ActionResult MakeGroup(fyp_group fypgroup)
        {
            if (ModelState.IsValid)
            {
                fypgroup.group_member3 = Convert.ToInt32(Session["Id"]);
                db.fyp_group.Add(fypgroup);
                db.SaveChanges();
                //   Success(string.Format("<b>{0}{1}{2}</b> Group has been created Successfully.", fypgroup.student.Name, fypgroup.student1.Name, fypgroup.student2.Name), true);

                return RedirectToAction("Index");
            }
            Warning("Group Could not be Created!! Try Again", true);
            return View();
        }
        public ActionResult MyIdeas()
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;

                List<idea> IDEAS = null;
                int myid = Convert.ToInt32(Session["Id"]);
                if (Convert.ToInt32(Session["Check"]) == 1)
                {
                    var query1 = from c in db.ideas
                                 where c.PostedByTeacher == myid
                                 select c;
                    IDEAS = query1.ToList();
                }
                else if (Convert.ToInt32(Session["Check"]) == 0)
                {
                    var query2 = from c in db.ideas
                                 where c.PostedByStudent == myid
                                 select c;
                    IDEAS = query2.ToList();
                }

                return View(IDEAS);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Details(int? id)
        {

            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                idea idea = db.ideas.Find(id);
                if (idea == null)
                {
                    return HttpNotFound();
                }
                ViewData["Id"] = idea.Id;
                return View(idea);


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public ActionResult makegrop()
        {
            var query1 = from student in db.students
                         select student;
            fyp_group fypgroup = new fyp_group();
            //fypgroup.students = query1.ToList();

            return View(fypgroup);
        }
        [HttpPost]
        public ActionResult makegrop(fyp_group fypgroup)
        {
            if (ModelState.IsValid)
            {
                fypgroup.group_member3 = Convert.ToInt32(Session["Id"]);
                db.fyp_group.Add(fypgroup);
                db.SaveChanges();
                Success(" Group has been created Successfully.", true);

                return RedirectToAction("MyIdeas");
            }
            Warning("Group Could not be Created!! Try Again", true);
            return View();
        }
        public ActionResult EditIdea(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                idea idea = db.ideas.Find(id);
                return View(idea);
            }
        }
        [HttpPost]
        public ActionResult EditIdea(idea newIdea)
        {
            idea idea = db.ideas.Find(newIdea.Id);
            if (idea.checker == 1)
            {
                newIdea.PostedByTeacher = idea.PostedByTeacher;
            }
            else if (idea.checker == 0)
            {
                newIdea.PostedByStudent = idea.PostedByStudent;
            }
            newIdea.checker = idea.checker;
            DeleteConfirmed(idea.Id);
            db.ideas.Add(newIdea);
            db.SaveChanges();
            return RedirectToAction("MyIdeas");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            idea idea = db.ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }
        public ActionResult RegisterationForm(int? id)
        {


            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                ViewData["fypgroup"] = db.fyp_group.Find((int)Session["GroupID"]);
                ViewData["idea"] = db.ideas.Find(id);
                Session["idea"] = id;
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterationForm(registration reg)
        {

            reg.approve = 0;
            reg.fypgroup = (int)Session["GroupId"];
            reg.idea = (int)Session["idea"];
            reg.supervisor = (int)db.ideas.Find(reg.idea).PostedByTeacher;
            db.registrations.Add(reg);
            db.SaveChanges();
            notification notification = new notification();
            notification.group = (int)Session["GroupID"];
            notification.notifier = Session["Name"].ToString();
            notification.type = " Wants to register idea ";
            notification.seen = 0;
            notification.registration = db.registrations.ToList().Last().idRegistration;
            notification.time = DateTime.Now;
            notification.idea = (int)Session["currentIdea"];
            db.notifications.Add(notification);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }
        public ActionResult Communication(int? id)
        {

            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;

                ChatModel mymodel = new ChatModel();
                if ((int)Session["check"] == 0)
                {
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
                    /* notification notification = new notification();
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
                    Session["IdNotification"] = id;
                    notification currentNotification = db.notifications.Find(id);
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
                return View(mymodel);


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Communication(ChatModel model)
        {

            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                if (ModelState.IsValid)
                {
                    conversation con = new conversation();
                    con.message = model.Text.message;
                    con.name = Session["Name"].ToString();
                    con.time = DateTime.Now;
                    con.idea = (int)Session["currentIdea"];
                    con.group = (int)Session["GroupID"];
                    db.conversations.Add(con);
                    db.SaveChanges();
                    notification notification = new notification();
                    notification.group = (int)Session["GroupID"];
                    notification.notifier = Session["Name"].ToString();
                    notification.type = "Messaged in group chat";
                    notification.seen = 0;
                    notification.time = DateTime.Now;
                    notification.idea = (int)Session["currentIdea"];
                    db.notifications.Add(notification);
                    db.SaveChanges();

                }
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
                return View(mymodel);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


        public ActionResult Chat(string Text)
        {

            return RedirectToAction("Communication");
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            idea idea = db.ideas.Find(id);
            db.ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("MyIdeas");
        }
        public ActionResult Approve(int id)
        {
            notification currentNotification = db.notifications.Find(id);
            int reg = (int)currentNotification.registration;

            registration newreg = db.registrations.Find(reg);
            newreg.approve = 1;
            idea currentIdea = newreg.idea1;
            db.ideas.Attach(currentIdea);
            var entry = db.Entry(currentIdea);
            entry.Property(e => e.approve).IsModified = true;
            db.registrations.Attach(newreg);
            var entry1 = db.Entry(newreg);
            entry1.Property(e => e.approve).IsModified = true;
            db.SaveChanges();

            db.notifications.Attach(currentNotification);
            db.notifications.Remove(currentNotification);
            db.SaveChanges();

            notification notification = new notification();
            notification.group = newreg.fypgroup;
            notification.notifier = Session["Name"].ToString();
            notification.type = " Approved your registration of ";
            notification.seen = 0;
            notification.time = DateTime.Now;
            notification.idea = newreg.idea;
            db.notifications.Add(notification);
            db.SaveChanges();
            return RedirectToAction("SupervisorMainView", "Home");

        }
        public ActionResult Decline(int id)
        {

            notification currentNotification = db.notifications.Find(id);
            int reg = (int)currentNotification.registration;

            db.notifications.Attach(currentNotification);
            db.notifications.Remove(currentNotification);
            db.SaveChanges();

            registration newreg = db.registrations.Find(reg);
            notification notification = new notification();
            notification.group = newreg.fypgroup;
            notification.notifier = Session["Name"].ToString();
            notification.type = " declined your registration of ";
            notification.seen = 0;
            notification.time = DateTime.Now;
            notification.idea = newreg.idea;
            db.notifications.Add(notification);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        void DeleteRegistration(int id)
        {
            registration idea = db.registrations.Find(id);
            db.registrations.Remove(idea);
            db.SaveChanges();
        }
        public ActionResult MainView()
        {
            Session["Name"] = null;
            return View();
        }

        public ActionResult SetMilestones(int id)
        {
            if (Session["Name"] != null)
            {
                Session["idReg"] = id;
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                int currentid = (int)Session["idReg"];
                List<milestone> milestones;
                var query2 = from c in db.milestones
                             where c.idregistration == currentid
                             select c;
                milestones = query2.ToList();
                ViewData["milestones"] = milestones;
                ViewData["idReg"] = currentid;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetMilestones(milestone newMilestone)
        {
            if (Session["Name"] != null)
            {
               
                newMilestone.idregistration = (int)Session["idReg"];
                db.milestones.Add(newMilestone);
                db.SaveChanges();
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                int currentid = (int)Session["idReg"];
                List<milestone> milestones;
                var query2 = from c in db.milestones
                             where c.idregistration == currentid
                             select c;
                milestones = query2.ToList();
                ViewData["milestones"] = milestones;
                ViewData["idReg"] = currentid;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AprrovedIdeas()
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;

                List<registration> RegisteredIdeas = null;
                int myid = Convert.ToInt32(Session["Id"]);
                if (Convert.ToInt32(Session["Check"]) == 1)
                {
                    var query1 = from c in db.registrations
                                 where c.supervisor == myid && c.approve == 1
                                 select c;
                    RegisteredIdeas = query1.ToList();
                }


                return View(RegisteredIdeas);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult SupervisorMainView(int? id)
        {
            if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }

                List<registration> RegisteredIdeas = null;
                int myid = Convert.ToInt32(Session["Id"]);
                if (Convert.ToInt32(Session["Check"]) == 1)
                {
                    var query1 = from c in db.registrations
                                 where c.supervisor == myid && c.approve == 1
                                 select c;
                    RegisteredIdeas = query1.ToList();
                }
                ViewData["RegisteredIdeas"] = RegisteredIdeas;
                if (id != null)
                {
                    List<milestone> milestones;
                var query2 = from c in db.milestones
                             where c.idregistration == id
                             select c;
                milestones = query2.ToList();
                ViewData["milestones"] = milestones;
            
            registration reg = db.registrations.Find(id);
            fyp_group group = reg.fyp_group;
                
                    List<student> fypgroup = new List<student>(3);
                    fypgroup.Add(group.student1);
                    fypgroup.Add(group.student2);
                    fypgroup.Add(group.student);
                    ViewData["fypgroup"] = fypgroup;
                }
                
                Session["CHECKREG"] = "1";
            return View(notificationList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

       
        public ActionResult MileStones()
        {

            
                List<notification> notificationList = new List<notification>();
                List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
                    }
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
            ViewData["Notifications"] = notificationList;
            int groupId = (int)Session["GroupID"];
            var query = (from c in db.registrations
                         where c.fypgroup == groupId && c.approve == 1
                         select new
                         {
                             c.idRegistration,

                         }).ToList();
            if (query.FirstOrDefault() != null)
            {
                int regId = query.FirstOrDefault().idRegistration;
                List<milestone> milestones;
                var query2 = from c in db.milestones
                             where c.idregistration == regId
                             select c;
                milestones = query2.ToList();
                ViewData["milestones"] = milestones;
            }

            return View();
        }
    public ActionResult MileStoneDetail(int? id)
        { 
        if (Session["Name"] != null)
            {
                List<notification> notificationList = new List<notification>();
        List<notification> allNotifications = db.notifications.ToList();
                if ((int)Session["check"] == 0)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (((noti.seen == 0) && (noti.fyp_group.group_id == (int)Session["GroupID"]) && !(name.Equals(noti.notifier))))
                        {
                            notificationList.Add(noti);
                        }
}
                }
                else if ((int)Session["check"] == 1)
                {
                    string name = Session["Name"].ToString();
                    foreach (notification noti in allNotifications)
                    {
                        if (noti.idea1.PostedByTeacher == (int)Session["Id"] && !(name.Equals(noti.notifier)))
                        {

                            notificationList.Add(noti);
                        }

                    }
                }
                ViewData["Notifications"] = notificationList;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                milestone milestone = db.milestones.Find(id);
                if (milestone == null)
                {
                    return HttpNotFound();
                }
                ViewData["Id"] = milestone.idmileStones;
                return View(milestone);


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
    
}
