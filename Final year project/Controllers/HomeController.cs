using Final_year_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Final_year_project.Controllers
{
    public class HomeController : Controller
    {

        Quiz_ApplicationEntities2 db = new Quiz_ApplicationEntities2();

        [HttpGet]
        public ActionResult SRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SRegister(student svm, HttpPostedFileBase imgfile)
        {

            student s = new student();

            try 
            {
                s.std_name = svm.std_name;
                s.std_password = svm.std_password;
                s.std_image = uploadImage(imgfile);
                db.students.Add(s);
               db.SaveChanges();
                return RedirectToAction("slogin");
            }
            catch (Exception)     
            {
                ViewBag.msg = "Data could not be registerd";
           }

  
           return View();
        }

        public string uploadImage(HttpPostedFileBase imgfile)

        {

            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (imgfile != null && imgfile.ContentLength > 0)
            {
                string extension = Path.GetExtension(imgfile.FileName);
                if (extension.ToLower().Equals("jpg") || extension.ToLower().Equals("jpeg") || extension.ToLower().Equals("png"))

                {

                    try

                    {
                        path = Path.Combine(Server.MapPath("~/Content/img"), random + Path.GetFileName(imgfile.FileName));
                        imgfile.SaveAs(path);
                        path = "~/Content/img" + random + Path.GetFileName(imgfile.FileName);

                        //ViewBag.Message = "File Successfully Added";
                    }

                    catch (Exception ex)
                    {
                        path = "-1";

                    }

                }
                else
                {
                    Response.Write("<script>alert('only jpg,jpeg or png formates are acceptable'); </script>");

                    path = "-1";

                }
                
            }
            return path;

        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
        public ActionResult tlogin()
        {
            return View();   
           
        }

        [HttpPost]
        public ActionResult tlogin(tbl_admin ad) 
        {
            tbl_admin a = db.tbl_admin.Where(x => x.admin_name == ad.admin_name && x.admin_pass == ad.admin_pass).SingleOrDefault();
            if (a!=null) 
            {
                Session["ad_id"] = a.admin_id;
                return RedirectToAction("Dashboard");
            }
            else 
            {
                ViewBag.msg = "Invalid Your Name or Password";
            }
            return View();
        }
        public ActionResult slogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult slogin(student s)
        {
            student sv = db.students.Where(x=>x.std_name== s.std_name &&  x.std_password == s.std_password).SingleOrDefault();
            if (sv == null)
            {
                ViewBag.msg = "Invalid User Name or Password";

            }

            else 
            {
                Session["std_id"] = sv.std_id;
               return RedirectToAction("ExamDashboard");

            }


            return View();
        }

        public ActionResult ExamDashboard()
        {
            if (Session["std_id"] == null)
            {
                return RedirectToAction("slogin");
            }
            return View();
        }

        [HttpPost]    
        public ActionResult ExamDashboard(string room)
        {
            List<tbl_category> list = db.tbl_category.ToList();
            foreach(var item in list)
            {
                if (item.cat_encrytped__string == room)
                {
                    TempData["examid"] = item.cat_id;
                    TempData.Keep();
                    return RedirectToAction("StartQuiz");
                }
                else 
                {
                    ViewBag.error = "Not Room Found..";
                
                }

            }
            
            return View();
        }

        public ActionResult StartQuiz()
        {
            tbl_questions q=null;

            if (TempData["question_id"]==null) 
            {
                int examID = Convert.ToInt32(TempData["examid"]);
                 q = db.tbl_questions.First(x=>x.q_fk_cat_id==examID); 
                //tbl_questions q = db.tbl_questions.Where(x => x.q_fk_cat_id == examID && ).SingleOrDefault();
            }
           
            return View(q);
        }

        public ActionResult Dashboard()//THis is our main dash board of screen
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Dashboard");
            }


            return View();
        }


        [HttpGet]
        public ActionResult Add_Category()
        {

            //Session["ad_id"] = 2; // we will remove this

            //int ad_id = Convert.ToInt32(Session["ad_id"].ToString());
            int ad_id = Convert.ToInt32(Session["ad_id"]);
            List<tbl_category> catLi = db.tbl_category.Where(x => x.cat_fk_ad_id == ad_id).OrderByDescending(x => x.cat_id).ToList();
            ViewData["List"] = catLi;

            return View();
        }

        [HttpPost]
        public ActionResult Add_Category(tbl_category cat)
        {
           
            List<tbl_category> catLi = db.tbl_category.OrderByDescending(x => x.cat_id).ToList();
            ViewData["List"] = catLi;
            tbl_category c = new tbl_category();

            Random r = new Random();

            c.cat_name = cat.cat_name;
            c.cat_fk_ad_id = Convert.ToInt32(Session["ad_id"].ToString());
            c.cat_encrytped__string = cat.cat_name + r.Next().ToString();
            db.tbl_category.Add(c);
            db.SaveChanges();
            return RedirectToAction("Add_Category");
          
        }

        [HttpGet]
        public ActionResult AddQuestions()
        {
            int sid = Convert.ToInt32(Session["ad_id"]);
            List<tbl_category> li = db.tbl_category.Where(x => x.cat_fk_ad_id == sid).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult AddQuestions(tbl_questions q)
        {
            int sid = Convert.ToInt32(Session["ad_id"]);
            List<tbl_category> li = db.tbl_category.Where(x => x.cat_fk_ad_id == sid).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");
            tbl_questions qa = new tbl_questions();
            qa.question_text = q.question_text;
            qa.QA = q.QA;
            qa.QB = q.QB;
            qa.QC = q.QC;
            qa.QD = q.QD;
            qa.QCorrectAns = q.QCorrectAns;
            qa.q_fk_cat_id=q.q_fk_cat_id;
            db.tbl_questions.Add(qa);
            db.SaveChanges();
            TempData["ms"] = ("Question Successfully Added");
            TempData.Keep();

            return RedirectToAction("AddQuestions");   
           
        }
        public ActionResult Index()
        {
            if (Session["ad_id"]!=null) 
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}