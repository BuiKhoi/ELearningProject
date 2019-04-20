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

        private void SetTeacherCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserRole"];
            HttpCookie IdCookie = HttpContext.Current.Request.Cookies["UserID"];
            if (cookie == null)
            {
                cookie = new HttpCookie("UserRole", "Teacher");
                cookie.Expires = DateTime.Now.AddHours(1);
                HttpContext.Current.Request.Cookies.Add(cookie);
            } else
            {
                cookie.Value = "Teacher";
                cookie.Expires = DateTime.Now.AddHours(1);
            }

            if (IdCookie == null)
            {
                IdCookie = new HttpCookie("UserId", this.id.ToString());
                IdCookie.Expires = DateTime.Now.AddHours(1);
                HttpContext.Current.Request.Cookies.Add(IdCookie);
            } else
            {
                IdCookie.Value = this.id.ToString();
                IdCookie.Expires = DateTime.Now.AddHours(1);
            }
        }
    }

    public class TeacherIndexViewModel
    {
        public List<TType> types { get; set; }
    }

    public class CreateTestViewModel
    {
        public int TeacherId { get; set; }
        public List<QType> Types { get; set; }
        public List<PuzzelQuestionViewModel> PuzzQuestions { get; set; }
        public List<MultipieChoiceViewModel> MultQuestions { get; set; }

        public CreateTestViewModel()
        {
        }

        public CreateTestViewModel(List<PuzzelQuestionViewModel> puzzelQuestions, 
            List<MultipieChoiceViewModel> multipieChoices, List<QType> Types, int TeacherId)
        {
            this.PuzzQuestions = puzzelQuestions;
            this.MultQuestions = multipieChoices;
            this.Types = Types;
            this.TeacherId = TeacherId;
        }
    }

    public class SingleQuestionAddViewModel
    {
        public int TeacherId { get; set; }
        public int QuestionId { get; set; }
        public bool Submit { get; set; }
        public string TestName { get; set; }
        public int Order { get; set; }
        public int TestType { get; set; }
        public string Image { get; set; }
        public string Tags { get; set; }
    }

    public class TeacherUserModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime birthday { get; set; }
        public string creditid { get; set; }
        public bool status { get; set; }


    }

    public class TeacherUserViewModel
    {

        public List<TeacherUserModel> users { get; set; }

        public TeacherUserViewModel()
        {

            users = new List<TeacherUserModel>();
        }
    }
}