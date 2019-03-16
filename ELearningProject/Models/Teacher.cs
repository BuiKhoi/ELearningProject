using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningProject.Models
{
    public class Teacher
    {
        public int id { get; set; }
        public Web_user User { get; set; }
        public float MeanRating { get; set; }
    }
}