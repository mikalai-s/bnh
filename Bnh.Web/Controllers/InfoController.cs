using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.Web.Controllers
{
    public class InfoController : Controller
    {
        //
        // GET: /Info/

        public ActionResult Index()
        {
            return Content(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

    }
}
