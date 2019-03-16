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

        public ICollection<TestQuestionDeploy> TestQuestionDeploys { get; set; }
    }
}