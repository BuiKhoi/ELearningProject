﻿using ELearningProject.Models;
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
            return View(GetStvm());
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
            List<PuzzelQuestionViewModel> PuzzQuestions = new List<PuzzelQuestionViewModel>();
            List<MultipieChoiceViewModel> MultQuestions = new List<MultipieChoiceViewModel>();
            List<QType> Types = new List<QType>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                PuzzQuestions = (from q in db.Questions
                                 join qc in db.QContents on q.Content.id equals qc.id
                                 join a in db.Answers on q.Answer.id equals a.id
                                 join qt in db.QTypes on q.Type.id equals qt.id
                                 where q.Type.Name == "Puzzel Game"
                                 select new PuzzelQuestionViewModel() { id = q.id, Content = qc.Content, Answer = a.Content })
                             .ToList<PuzzelQuestionViewModel>();

                MultQuestions = (from q in db.Questions
                                 join qc in db.QContents on q.Content.id equals qc.id
                                 join a in db.Answers on q.Answer.id equals a.id
                                 join qt in db.QTypes on q.Type.id equals qt.id
                                 where q.Type.Name == "Multipie Choice"
                                 select new MultipieChoiceViewModel()
                                 {
                                     id = q.id,
                                     Content = qc.Content,
                                     Quiz = new QuizMultichoice(),
                                     fuckingdata = a.Content,
                                     QuestionId = q.id
                                 }).ToList<MultipieChoiceViewModel>();
                foreach (var t in MultQuestions)
                {
                    t.Quiz = JsonConvert.DeserializeObject<QuizMultichoice>(t.fuckingdata);
                }

                Types = (from tt in db.QTypes
                         select tt).ToList<QType>();
            }
            return View(new CreateTestViewModel(PuzzQuestions, MultQuestions, Types, 1));
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