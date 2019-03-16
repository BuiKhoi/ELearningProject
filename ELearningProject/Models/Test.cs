using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningProject.Models
{
    public class Test
    {
        public int id { get; set; }
        public Teacher Creator { get; set; }
        public float Rating { get; set; }
        public string Desc { get; set; }
        public string Tags { get; set; }

        public ICollection<TestQuestionDeploy> TestQuestionDeploys { get; set; }
    }
}