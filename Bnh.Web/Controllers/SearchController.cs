using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Web.ViewModels;

namespace Bnh.Web.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index(SearchViewModel criteria)
        {
            return View(new SearchViewModel()
            {
                Query = criteria.Query,
                Result = new [] 
                {
                    new SearchResultEntryViewModel()
                    {
                        Text = "Result 1"
                    },
                    new SearchResultEntryViewModel()
                    {
                        Text = "Result 2"
                    },
                }
            });
        }
    }
}
