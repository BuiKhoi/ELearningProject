using ELearningProject.Models;
using Newtonsoft.Json;
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
            //Get a list of tests and add it to viewbag since we use many object here
            List<TestTypeView> ttests = new List<TestTypeView>();
            using (var db = new ApplicationDbContext())
            {
                ttests = (from t in db.Tests
                         join tt in db.TTypes on t.Type.id equals tt.id
                         select new TestTypeView() { test = t, type = tt}).ToList<TestTypeView>();
            }

            //Create new list with our tests
            List<Test> tests = new List<Test>();
            foreach(var ttest in ttests)
            {
                var test = new Test()
                {
                    id = ttest.test.id,
                    Desc= ttest.test.Desc,
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
                List<string> tags = JsonConvert.DeserializeObject<List<string>>("[\"Children\",\"Cartoon\"]");
                //List<string> tags = new List<string>();
                //tags.Add("Children");
                //tags.Add("Cartoon");
                //string json = JsonConvert.SerializeObject(tags);
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
            return View(stvm);
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