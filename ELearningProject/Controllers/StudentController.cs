using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityAuthentication.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PuzzelEnglish(int? TestId)
        {
            if (TestId == null)
            {
                return View();
            }

            List<PuzzelQuestionViewModel> pquests = new List<PuzzelQuestionViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                pquests = (from q in db.Questions
                              join qt in db.TestQuestionDeploys on q.id equals qt.Question.id
                              join t in db.Tests on qt.Test.id equals t.id
                              join qc in db.QContents on q.Content.id equals qc.id
                              join a in db.Answers on q.Answer.id equals a.id
                              where t.id == TestId
                              select new PuzzelQuestionViewModel() {id = q.id, Content = qc.Content, Answer = a.Content})
                              .ToList<PuzzelQuestionViewModel>();
            }
            return View("PuzzelEnglishTest", pquests);
        }
    }
}