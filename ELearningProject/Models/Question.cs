using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningProject.Models
{
    public class Question
    {
        public int id { get; set; }
        public string Name { get; set; }
        public float Level { get; set; }

        public QType Type { get; set; }
        public QContent Content { get; set; }
        public Answer Answer { get; set; }
        public Teacher Creator { get; set; }

        public ICollection<TestQuestionDeploy> TestQuestionDeploys { get; set; }
    }

    public class PuzzelQuestionViewModel
    {
        public int id { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }

        public PuzzelQuestionViewModel()
        {}

        public PuzzelQuestionViewModel(Question question)
        {
            this.id = question.id;
            this.Content = question.Content.Content;
            this.Answer = question.Answer.Content;
        }
    }

    public class MultipieChoiceViewModel
    {
        public int id { get; set; }
        public string Content { get; set; }
        public QuizMultichoice Quiz { get; set; }
        public string fuckingdata { get; set; }

        public MultipieChoiceViewModel() { }
    }
    public class QuizMultichoice
    {
        public string Quiz1 { get; set; }
        public string Quiz2 { get; set; }
        public string Quiz3 { get; set; }
        public string Quiz4 { get; set; }
        public int Answer { get; set; }
        public QuizMultichoice()
        {

        }
    }

    public class TranslateQuestViewModel
    {
        public int id { get; set; }
        public string Content { get; set; }
        public List<string> Answer { get; set; }

        public TranslateQuestViewModel() { }
    }
}