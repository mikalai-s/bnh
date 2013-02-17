using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;
using Bnh.Core;
using Bnh.Core.Entities;

namespace Bnh.Controllers
{
    public class HomeController : Controller
    {
        IBnhRepositories repos { get; set; }

        public HomeController(IBnhRepositories repos)
        {
            this.repos = repos;
        }

        public ActionResult Index()
        {
            if (BnhConfig.IsProduction)
            {
                return View("HomeLimited");
            }

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
                this.repos.Feedback.Insert(new Comment
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
