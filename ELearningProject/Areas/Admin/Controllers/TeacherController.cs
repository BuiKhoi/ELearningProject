using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningProject.Areas.Admin.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Admin/Teacher
 
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/User
        public ActionResult Index()
        {
            return View(GetTuvm());
        }

        private TeacherUserViewModel GetTuvm()
        {
            //Get a list of tests and add it to viewbag since we use many object here

            List<TeacherUserModel> teachers = new List<TeacherUserModel>();
            using (var db = new ApplicationDbContext())
            {
                teachers = (from st in db.Teachers
                            join u in db.Web_Users on st.User.id equals u.id
                            select new TeacherUserModel() { id = st.id, name = u.Name, birthday = u.Birthday, creditid = u.CreditId/*,status=u.status*/}).ToList<TeacherUserModel>();
            }
            var suvm = new TeacherUserViewModel();
            foreach (var t in teachers)
            {
                suvm.users.Add(t);
            }



            return suvm;
           
        }

    }
  

}
