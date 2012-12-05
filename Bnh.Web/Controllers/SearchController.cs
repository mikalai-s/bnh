using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Web.Infrastructure.Search;
using Bnh.Web.ViewModels;

namespace Bnh.Web.Controllers
{
    public class SearchController : Controller
    {
        ISearchProvider Search { get; set; }

        public SearchController(ISearchProvider search)
        {
            this.Search = search;
        }

        //
        // GET: /Search/

        public ActionResult Index(SearchViewModel criteria)
        {
            return View(new SearchViewModel()
            {
                Query = criteria.Query,
                Result = this.Search.Search(criteria.Query)
                    .Cast<SearchResult>()
                    .Select(r => new SearchResultEntryViewModel
                    {
                        Text = r.Content
                    })
                    .ToList()
            });
        }

        public ActionResult RebuildIndex()
        {
            this.Search.RebuildIndex();

            return this.JavaScript("alert('done')");
        }
    }
}
