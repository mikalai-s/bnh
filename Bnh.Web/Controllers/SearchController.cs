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
        ISearchProvider Searcher { get; set; }

        public SearchController(ISearchProvider searcher)
        {
            this.Searcher = searcher;
        }

        //
        // GET: /Search/

        public ActionResult Index(SearchViewModel criteria)
        {
            var searchViewModel = new SearchViewModel()
            {
                Query = criteria.Query
            };

            if (!criteria.Query.IsEmpty())
            {
                searchViewModel.Result = this.Searcher.Search(criteria.Query)
                    .Cast<SearchResult>()
                    .Select(r => new SearchResultEntryViewModel
                    {
                        Text = r.Content
                    })
                    .ToList();
            }

            return View(searchViewModel);
        }

        public ActionResult RebuildIndex()
        {
            this.Searcher.RebuildIndex();

            return this.JavaScript("alert('done')");
        }
    }
}
