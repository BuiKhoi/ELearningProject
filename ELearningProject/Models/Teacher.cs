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

    public class CreateTestViewModel
    {
        public int TeacherId { get; set; }
        public List<PuzzelQuestionViewModel> Questions { get; set; }
        public List<int> ChosenQuestions { get; set; }

        public CreateTestViewModel()
        {
            ChosenQuestions = new List<int>();
        }

        public CreateTestViewModel(List<PuzzelQuestionViewModel> puzzelQuestions, int TeacherId)
        {
            this.Questions = puzzelQuestions;
            ChosenQuestions = new List<int>();
            this.TeacherId = TeacherId;
        }
    }

    public class SingleQuestionAddViewModel
    {
        public int TeacherId { get; set; }
        public int QuestionId { get; set; }
        public bool Submit { get; set; }
        public string TestName { get; set; }
    }
}