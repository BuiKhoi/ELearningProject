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
        private static List<List<int>> AddingQuestions = new List<List<int>>();
        private static List<int> AddingTeachers = new List<int>();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SubmitTest(SingleQuestionAddViewModel model)
        {
            if (model.Submit)
            {
                //Submit the test to the database

                //Add the chosen questions to the quests list
                List<Question> quests = new List<Question>();
                foreach (var quest in AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)])
                {
                    using (var db = new ApplicationDbContext())
                    {
                        quests.Add((from q in db.Questions
                                    where q.id == quest
                                    select q).First());
                    }
                }

                using (var db = new ApplicationDbContext())
                {
                    //First we have to create a new test
                    Test t = new Test()
                    {
                        Rating = 0.0F,
                        Desc = model.TestName,
                        //Creator = (from tea in db.Teachers where tea.id == model.TeacherId select tea).First()
                    };
                    db.Tests.Add(t);

                    //Shift throug all questions to be added and add them to TestQuestionDeploy
                    foreach (var q in quests)
                    {
                        TestQuestionDeploy tqd = new TestQuestionDeploy()
                        {
                            Question = q,
                            Test = t,
                            Order = quests.IndexOf(q),
                        };
                        db.TestQuestionDeploys.Add(tqd);
                    }

                    //And save the changes
                    db.SaveChanges();
                }

                return Json("SucSub");
            }
            else
            {
                //Add the selected question to the "AddingQuestion" list and devided by teacher id, since this is a server side
                using (var db = new ApplicationDbContext())
                {
                    if (AddingTeachers.IndexOf(model.TeacherId) == -1)
                    {
                        AddingTeachers.Add(model.TeacherId);
                        int idx = AddingTeachers.IndexOf(model.TeacherId);
                        try
                        {
                            AddingQuestions[idx] = new List<int>();
                        } catch (ArgumentOutOfRangeException)
                        {
                            AddingQuestions.Add(new List<int>());
                        }
                    }

                    List<int> qts = AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)];
                    qts.Add(model.QuestionId);
                    AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)] = qts;
                }
                return Json("SucAdd");
            }
        }

        public ActionResult CreateTest()
        {
            //int TeacherId = int.Parse(HttpContext.Response.Cookies["UserID"].Value);
            List<PuzzelQuestionViewModel> questions = new List<PuzzelQuestionViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                questions = (from q in db.Questions
                             join qc in db.QContents on q.Content.id equals qc.id
                             join a in db.Answers on q.Answer.id equals a.id
                             select new PuzzelQuestionViewModel() {id = q.id,  Content = qc.Content, Answer = a.Content})
                             .ToList<PuzzelQuestionViewModel>();
            }
            return View(new CreateTestViewModel(questions, 1));
        }
    }
}