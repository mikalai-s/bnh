using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;

namespace Bnh.Web.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class AdminController : Controller
    {
        IEntityRepositories Entities { get; set; }

        public AdminController(IEntityRepositories entities)
        {
            this.Entities = entities;
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeedbackReport()
        {
            return View(this.Entities.Feedback.OrderBy(c => c.Created));
        }
    }
}
