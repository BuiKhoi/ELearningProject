using ELearningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ELearningProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetStvm());
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
        private StudentTestViewModel GetStvm()
        {
            //Get a list of tests and add it to viewbag since we use many object here
            List<TestTypeView> ttests = new List<TestTypeView>();
            using (var db = new ApplicationDbContext())
            {
                ttests = (from t in db.Tests
                          join tt in db.TTypes on t.Type.id equals tt.id
                          select new TestTypeView() { test = t, type = tt }).ToList<TestTypeView>();
            }

            //Create new list with our tests
            List<Test> tests = new List<Test>();
            foreach (var ttest in ttests)
            {
                var test = new Test()
                {
                    id = ttest.test.id,
                    Desc = ttest.test.Desc,
                    Image = ttest.test.Image,
                    Rating = ttest.test.Rating,
                    Tags = ttest.test.Tags,
                    Type = ttest.type
                };
                tests.Add(test);
            }

            //Shift through the list and cast them to TestViewModels, then add to the array divided by type
            var stvm = new StudentTestViewModel();
            foreach (var test in tests)
            {
                //Create a list of tags for each test
                List<string> tags = JsonConvert.DeserializeObject<List<string>>(test.Tags);
                var t = new TestViewModel()
                {
                    id = test.id,
                    Desc = test.Desc,
                    Image = test.Image,
                    Rating = test.Rating,
                    Tags = tags
                };

                //Check if this type is in the list or not, and add it to
                if (stvm.TestTypes.IndexOf(test.Type.name) == -1)
                {
                    stvm.TestTypes.Add(test.Type.name);
                    stvm.Tests.Add(new List<TestViewModel>());
                }

                stvm.Tests[stvm.TestTypes.IndexOf(test.Type.name)].Add(t);
            }
            return stvm;
        }
    }
}