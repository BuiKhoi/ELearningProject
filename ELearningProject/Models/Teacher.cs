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
}