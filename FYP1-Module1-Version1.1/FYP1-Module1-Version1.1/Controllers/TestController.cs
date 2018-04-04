using NUnit.Framework;
using FYP1_Module1_Version1._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYP1_Module1_Version1._1.Controllers
{
    [TestFixture]
    public class TestController : Controller
    {
        Test1Controller t1 = new Test1Controller();

        // GET: Test
        [TestCase]
        public void RegisterStudentTest()
        {
            student s1 = new student();
            s1.Name = "Talha123";
            s1.Password = "password123";
            s1.RollNumber = "i1302645";

            Assert.AreEqual(true, t1.RegisterStudent(s1));
        }
        [TestCase]
        public void LoginStudentTest()
        {
            int i1 = 0;
            student s2 = new student();

            while (i1 < 2)
            {
                //group exist
                //registered student
                if (i1 == 0)
                {
                    s2.Name = "Muhammad Faizan";
                    s2.Password = "password";
                    s2.RollNumber = "i130205";
                    Assert.AreEqual(1, t1.LoginStudent(s2));
                    
                }
                else if (i1 == 1)
                {
                    s2.Name = "Qazi1";
                    s2.Password = "password";
                    s2.RollNumber = "i130010";
                    Assert.AreEqual(0, t1.LoginStudent(s2));

                }
                i1++;
            }

        }
        [TestCase]
        public void addideatest()
        {
            string session1 = "1";
            idea i11 = new idea();
            int i1 = 0;
            while (i1 < 2)
            {
                if (i1 == 0)
                {
                    i11.Title = "Penman";
                    i11.Description = "Some Description";
                    i11.Category = "developement";
                    Assert.AreEqual(true, t1.addIdeasTest(i11,session1));

                }
                if (i1 == 1)
                {
                    i11.Title = "Penman123";
                    i11.Description = "Some Description123";
                    
                    session1 = "0"; // student
                    Assert.AreEqual(false, t1.addIdeasTest(i11, session1));

                }
                i1++;
            }
        }
        [TestCase]
        public void milestonetest()
        {
            milestone m1 = new milestone();;
            int i1 = 0;
            while (i1 < 3)
            {
                if (i1 == 0)
                {

                    m1.name = "do testing";
                    m1.complexity = 4;
                    m1.description = "We have to do statement coverage for assignment 1";
                    Assert.AreEqual(1, t1.milestonesTest(m1));
                    m1.name = null;
                    m1.description = null;
                    
                }
                else if (i1 == 1)
                {
                    m1.complexity = 4;
                    m1.description = "We have to do statement coverage for assignment 1";


                    Assert.AreNotEqual(1, t1.milestonesTest(m1));
                    m1.name = null;
                    m1.description = null;

                }
                else if (i1 == 2)
                {
                    m1.name = "do testing";
                    m1.complexity = 4;
                    

                    Assert.AreNotEqual(1, t1.milestonesTest(m1));
                    m1.name = null;
                    m1.description = null;

                }
                i1++;
            }

            
        }
        [TestCase]
        public void listmilestonetest()
        {
            milestone m2 = new milestone();
            List<milestone> m1 = new List<milestone>();
            bool check=true;
            int i1 = 0;
            while (i1 < 2)
            {
                if (i1 == 0)
                {
                    int i2 = 0;
                    while (i2 < 2)
                    {
                        if (i2 == 0)
                        {
                            m2.name = "add idea";
                            m2.complexity = 3;
                            m2.description = "some description";
                            if (System.Text.RegularExpressions.Regex.IsMatch(m2.name, "^[a-zA-Z0-9\x20]+$"))
                            {
                                m1.Add(m2);
                                check = true;
                            }
                            else
                               check = false;
                              
                            Assert.AreEqual(check, t1.listmilestonestest(m1));

                        }
                        else if (i2 == 1)
                        {
                            m2.name = "add idea_";
                            m2.complexity = 3;
                            m2.description = "some description";
                            if (System.Text.RegularExpressions.Regex.IsMatch(m2.name, "^[a-zA-Z0-9\x20]+$"))
                            {
                                m1.Add(m2);
                                check = true;
                            }
                            else
                              check = false;
                            
                            Assert.AreNotEqual(check, t1.listmilestonestest(m1));

                        }
                        i2++;
                    }


                    m1 = null;       
                }
                else if (i1 == 1)
                {
                    Assert.AreNotEqual(true, t1.listmilestonestest(m1));

                }
               
                i1++;
            }
            
        }
        [TestCase]
        public void LoginSupervisorTest()
        {
            int i1 = 0;
            teacher s2 = new teacher();

            while (i1 < 2)
            {
                //group exist
                //registered student
                if (i1 == 0)
                {
                    s2.Email = "Shahzada.Zeeshan@nu.edu.pk";
                    s2.password = "password";
                    Assert.AreEqual(true, t1.supervisorlogintest (s2));

                }
                else if (i1 == 1)
                {
                    s2.Email = "Shahzada111.Zeeshan@nu.edu.pk";
                    s2.password = "password";
                    Assert.AreNotEqual(true, t1.supervisorlogintest(s2));

                }
                i1++;
            }
        }
        [TestCase]
        public void RegisterSupervisorTest()
        {
            teacher s1 = new teacher();
            s1.Name = "Ahmed";
            s1.password = "password123";
            s1.Email = "Ahmed@nu.edu.pk";

            Assert.AreEqual(true, t1.RegisterSupervisor(s1));
        }
        [TestCase]
        public void makegroupTest()
        {
       
            int i1 = 0;
            while (i1 < 2)
            {
                if (i1 == 0)
                {
                    fyp_group f1 = new fyp_group();
                    f1.group_member1 = 111;
                    f1.group_member2 = 22;
                    f1.group_member3 = 33;
                    Assert.AreEqual(true, t1.makegrouptest(f1));

                }
                else if (i1 == 1)
                {
                    fyp_group f1 = new fyp_group();
                    f1.group_member1 = 1;
                    f1.group_member2 = 2;
                    f1.group_member3 = 3;
                    Assert.AreNotEqual(false, t1.makegrouptest(f1));

                }
              
                i1++;
            }

        }
        [TestCase]
        public void registrationTest1() {

            registration r1 = new registration();

            int y1 = 0;
            while (y1 < 2)
            {
                if (y1 == 0)
                {
                    idea i1 = new idea();
                    i1.Id = 1;
                    i1.Title = "Hotel Management System";
                    i1.Description = "Some Description";
                    i1.Category = "developement";

                    fyp_group f1 = new fyp_group();
                    f1.group_id = 1;
                    f1.group_member1 = 1;
                    f1.group_member1 = 2;
                    f1.group_member1 = 3;

                    teacher t2 = new teacher();
                    t2.Id = 1;
                    t2.Email = "Ahmed@nu.edu.pk";
                    t2.Name = "Ahmed";

                   r1.idRegistration = 1;
                    r1.co_supervisorName = "Hamza";
                    r1.co_supervisorEmail = "Hamza@nu.edu.pk";


                    r1.idea = i1.Id;
                    r1.fypgroup = f1.group_id;
                    r1.supervisor = t2.Id;
                    r1.approve = 0;
                    Assert.AreEqual(true, t1.registrationTest(r1));
                    
                  
                }
                else if (y1 == 1)
                {
                    Assert.AreNotEqual(false, t1.registrationTest(r1));

                }
                y1++;
            }
        }
    }
}