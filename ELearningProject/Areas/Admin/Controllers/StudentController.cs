using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningProject.Areas.Admin.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/User
        public ActionResult Index()
        {
            return View(GetSuvm());
        }

        private StudentUserViewModel GetSuvm()
            {
            //Get a list of tests and add it to viewbag since we use many object here

            List<StudentUserModel> students = new List<StudentUserModel>();
            using (var db = new ApplicationDbContext())
            {
                 students = (from st in db.Students
                            join u in db.Web_Users on st.web_User.id equals u.id
                            select new StudentUserModel() { id = st.id, name = u.Name,birthday=u.Birthday,creditid=u.CreditId/*,status=u.status*/}).ToList<StudentUserModel>();
            }
            var suvm = new StudentUserViewModel();
            foreach (var t in students)
            {
                suvm.users.Add(t);
            }
          
           

            return suvm ;
                //if (!string.IsNullOrEmpty(searchText))
                //{
                //    search = searchText;
                //    page = 1;
                //}
                //int pageNumber = page ?? 1;
                //var user_list = db.Web_Users.OrderBy(x => x.id);
                //if (!String.IsNullOrEmpty(search))
                //{
                //    user_list = user_list.Where(p =>p.Name.Contains(search) || p.Name.Contains(search)).OrderBy(x => x.id);
                //}
                //ViewBag.Search = search;
                //return View(user_list.ToList().ToPagedList(pageNumber,pageSize));
            }

        }
        //Get the Student Test View contains all the test with types
  
    
}