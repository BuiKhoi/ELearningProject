using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            /*
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var qcons = db.QContents.ToList<QContent>();
                var ans = db.Answers.ToList<Answer>();
                var type = db.QTypes.ToList<QType>()[0];
                foreach (QContent content in qcons)
                {
                    var idx = db.QContents.Local.IndexOf(content);
                    var quest = new Question()
                    {
                        Name = "Arnold falling in the elevator No." + idx.ToString(),
                        Content = content,
                        Answer = ans[idx],
                        Type = type,
                        Level = 2
                    };
                    db.Questions.Add(quest);
                }
                db.SaveChanges();
            }*/

            /*using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var test = db.Tests.ToList<Test>()[0];
                var quests = db.Questions.ToList<Question>();
                var count = 1;
                foreach (Question q in quests)
                {
                    var dpl = new TestQuestionDeploy()
                    {
                        Question = q,
                        Test = test,
                        Order = count++
                    };
                    db.TestQuestionDeploys.Add(dpl);
                }
                db.SaveChanges();
            }*/

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}