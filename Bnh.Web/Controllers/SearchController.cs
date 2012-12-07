using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Web.Infrastructure.Search;
using Bnh.Web.ViewModels;
using System.Web.Mvc.Html;
using System.IO;

namespace Bnh.Web.Controllers
{
    public class SearchController : Controller
    {
        IEntityRepositories Entities { get; set; }

        ISearchProvider Searcher { get; set; }

        public SearchController(IEntityRepositories entities, ISearchProvider searcher)
        {
            this.Entities = entities;
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
                var htmlHelper = new HtmlHelper(new ViewContext(this.ControllerContext, new WebFormView(this.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());

                var found = this.Searcher.Search(criteria.Query).ToList();

                var communitiesFound = found.OfType<CommunitySearchResult>().ToList();
                var communitiesIdsFound = communitiesFound.Select(r => r.CommunityId).ToList();
                var communities = this.Entities.Communities.Where(c => communitiesIdsFound.Contains(c.CommunityId)).ToList();

                searchViewModel.Result =
                    from r in communitiesFound
                    let community = communities.First(c => c.CommunityId == r.CommunityId)
                    select new SearchResultEntryViewModel
                    {
                        Category = "community",
                        Link = htmlHelper.ActionLink(community.Name, "Details", "Community", null, null, r.ContentId, new { id = community.UrlId }, null).ToString(),
                        Description = r.Content
                    };
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
