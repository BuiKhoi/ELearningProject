using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningProject.Models
{
    public class StudentTestResult
    {
        public int id { get; set; }
        public Test Test { get; set; }
        public Student Student { get; set; }
        public float Score { get; set; }

        public StudentTestResult()
        {

        }

        public StudentTestResult(ScoreStudent score)
        {
            this.Test = score.Test;
            this.Student = score.Student;
            this.Score = score.Score;
        }
    }

    public class ScoreStudent
    {
        public Test Test { get; set; }
        public Student Student { get; set; }
        public float Score { get; set; }
    }
}