using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Cms.Core;

namespace Bnh.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class AdminController : Controller
    {
        IRepositories cms { get; set; }

        public AdminController(IRepositories cms)
        {
            this.cms = cms;
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeedbackReport()
        {
            return View(this.cms.Feedback.OrderBy(c => c.Created));
        }
    }
}
