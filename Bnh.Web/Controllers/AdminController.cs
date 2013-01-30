using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Cms.Repositories;
using Bnh.Core;

namespace Bnh.Web.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class AdminController : Controller
    {
        CmsRepos cms { get; set; }

        public AdminController(CmsRepos cms)
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
