using ELearningProject.Models;
using Newtonsoft.Json;
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
                //List<Question> quests = new List<Question>();
                //foreach (var quest in AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)])
                //{
                //    using (var db = new ApplicationDbContext())
                //    {
                //        var temp = (from q in db.Questions
                //                    join qc in db.QContents on q.Content.id equals qc.id
                //                    join a in db.Answers on q.Answer.id equals a.id
                //                    select q).ToList<Question>();
                //        quests.AddRange(temp);
                //    }
                //}

                using (var db = new ApplicationDbContext())
                {
                    //First we have to create a new test
                    Test t = new Test()
                    {
                        Rating = 0.0F,
                        Desc = model.TestName,
                        //Creator = (from tea in db.Teachers where tea.id == model.TeacherId select tea).First()
                        Image = model.Image,
                        Type = (from tt in db.TTypes
                                join qt in db.QTypes on tt.name.ToLower() equals qt.Name.ToLower()
                                where qt.id == model.TestType
                                select tt).FirstOrDefault<TType>(),
                        Tags = model.Tags,
                    };
                    db.Tests.Add(t);

                    //Shift throug all questions to be added and add them to TestQuestionDeploy
                    int count = 0;
                    foreach (var quest in AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)])
                    {
                        Question question = (from q in db.Questions
                                             where q.id == quest
                                             select q).FirstOrDefault();
                        TestQuestionDeploy tqd = new TestQuestionDeploy()
                        {
                            Question = question,
                            Test = t,
                            Order = count++,
                        };
                        db.TestQuestionDeploys.Add(tqd);
                    }

                    //And save the changes
                    db.SaveChanges();

                    //And remove informations of this text
                    AddingQuestions.Remove(AddingQuestions[AddingTeachers.IndexOf(model.TeacherId)]);
                    AddingTeachers.Remove(model.TeacherId);
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
                        }
                        catch (ArgumentOutOfRangeException)
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
            List<QType> Types = new List<QType>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                questions = (from q in db.Questions
                             join qc in db.QContents on q.Content.id equals qc.id
                             join a in db.Answers on q.Answer.id equals a.id
                             select new PuzzelQuestionViewModel() { id = q.id, Content = qc.Content, Answer = a.Content })
                             .ToList<PuzzelQuestionViewModel>();

                Types = (from tt in db.QTypes
                         select tt).ToList<QType>();
            }
            return View(new CreateTestViewModel(questions, Types, 1));
        }
        
        public JsonResult FilterQuestions(int? TestType)
        {
            List<QuestionViewModel> questions = new List<QuestionViewModel>();
            if (TestType != null)
            {
                questions = GetQuestionsByType(TestType);
            }
            return Json(questions);
        }

        public List<QuestionViewModel> GetQuestionsByType(int? TypeId)
        {
            List<QuestionViewModel> questions = new List<QuestionViewModel>();
            using (var db = new ApplicationDbContext())
            {
                switch (TypeId)
                {
                    case 1:
                        {
                            questions.AddRange((from q in db.Questions
                                                join qc in db.QContents on q.Content.id equals qc.id
                                                join a in db.Answers on q.Answer.id equals a.id
                                                join qt in db.QTypes on q.Type.id equals qt.id
                                                where qt.id == TypeId
                                                select new PuzzelQuestionViewModel()
                                                {
                                                    id = q.id,
                                                    Content = qc.Content,
                                                    Answer = a.Content
                                                })
                            .ToList<PuzzelQuestionViewModel>());
                            break;
                        }
                    case 2:
                        {
                            questions.AddRange((from q in db.Questions
                                                join qc in db.QContents on q.Content.id equals qc.id
                                                join a in db.Answers on q.Answer.id equals a.id
                                                join qt in db.QTypes on q.Type.id equals qt.id
                                                where qt.id == TypeId
                                                select new MultipieChoiceViewModel()
                                                {
                                                    id = q.id,
                                                    Content = qc.Content,
                                                    fuckingdata = a.Content
                                                })
                             .ToList<MultipieChoiceViewModel>());
                            break;
                        }
                    case 3:
                        {
                            questions.AddRange((from q in db.Questions
                                                join qc in db.QContents on q.Content.id equals qc.id
                                                join a in db.Answers on q.Answer.id equals a.id
                                                join qt in db.QTypes on q.Type.id equals qt.id
                                                where qt.id == TypeId
                                                select new TranslateQuestViewModel()
                                                {
                                                    id = q.id,
                                                    Content = qc.Content,
                                                    Answer = JsonConvert.DeserializeObject<List<string>>(a.Content)
                                                })
                             .ToList<TranslateQuestViewModel>());
                            break;
                        }
                }
            }
            return questions;
        }
    }
}