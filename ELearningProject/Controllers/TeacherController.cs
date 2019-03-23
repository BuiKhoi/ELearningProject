using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningProject.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CreateTest()
        {
            int TeacherId = int.Parse(HttpContext.Response.Cookies["UserID"].Value);
            List<PuzzelQuestionViewModel> questions = new List<PuzzelQuestionViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext)
            {
                questions = (from q in db.Questions
                             where q.)
            }
            return View();
        }
    }
}