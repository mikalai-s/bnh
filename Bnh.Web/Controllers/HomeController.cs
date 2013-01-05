using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Core.Entities;

namespace Bnh.Web.Controllers
{
    public class HomeController : Controller
    {
        IEntityRepositories Entities { get; set; }

        public HomeController(IEntityRepositories entities)
        {
            this.Entities = entities;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Feedback(string message)
        {
            if(!message.IsEmpty())
            {
                this.Entities.Feedback.Insert(new Comment
                {
                    Created = DateTime.UtcNow,
                    Message = message,
                    UserName = this.User.Identity.Name
                });
            }

            ViewBag.BackUrl = HttpContext.Request.UrlReferrer.AbsoluteUri;

            return View();
        }
    }
}
