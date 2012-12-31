using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Core.Entities;
using Bnh.Web.Models;
using Bnh.Web.ViewModels;
using Bnh.Cms.Controllers;
using Bnh.Cms.Models;
using System.Web.Mvc.Html;

namespace Bnh.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private Config config = null;
        private IEntityRepositories repositories = null;
        private IRatingCalculator rating = null;
        private CmsEntities cms = null;
        private HtmlHelper htmlHelper = null;
        private SceneController sceneController = null;

        public CommunityController(Config config, IEntityRepositories repositories, IRatingCalculator rating, CmsEntities cms, SceneController sceneController)
        {
            this.config = config;
            this.repositories = repositories;
            this.rating = rating;
            this.cms = cms;
            this.sceneController = sceneController;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            this.htmlHelper = new HtmlHelper(new ViewContext(this.ControllerContext, new WebFormView(this.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());
        }

        //
        // GET: /Community/
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Zones()
        {
            var urlHelper = new UrlHelper(this.HttpContext.Request.RequestContext);

            var city = this.repositories.Cities.First(c => c.Name == config.City);
            var zones = city.Zones.ToList();
            var communities = this.repositories
                .Communities
                .Where(c => c.CityId == city.CityId)
                .ToList()
                .GroupBy(
                    c => c.Zone,
                    c => new
                    {
                        community = c,
                        uiHelpers = new
                        {
                            deleteUrl = urlHelper.Action("Delete", "Community", new { id = c.UrlId }),
                            detailsUrl = urlHelper.Action("Details", "Community", new { id = c.UrlId }),
                            infoPopup = GetCommunityInfoPopupHtml(urlHelper, c)
                        }
                    })
                .OrderBy(g => zones.IndexOf(g.Key))
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(c => c.community.Name));

            return Json(communities, JsonRequestBehavior.AllowGet);
        }

        private string GetCommunityInfoPopupHtml(UrlHelper urlHelper, Community community)
        {
            return this.htmlHelper.ActionLink(community.Name, "Details", new { id = community.UrlId }).ToString()
                + "<br/>"
                + this.htmlHelper.ActionLink("Reviews", "Reviews", new { id = community.UrlId }).ToString();
        }

        public ViewResult Details(string id)
        {
            var community = GetCommunity(id);
            return View(community);
        }

        //
        // GET: /Community/Create
        [DesignerAuthorize]
        public ActionResult Create()
        {
            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones);
            var sceneTemplates = from s in cms.Scenes
                                    where s.IsTemplate
                                    select new { id = s.SceneId, title = s.Title };
            ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

            var city = this.repositories.Cities.First(c => c.Name == config.City);
            ViewBag.CityZones = new SelectList(city.Zones);
            ViewBag.CityId = city.CityId;
            return View();
        } 

        //
        // POST: /Community/Create

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Create(Community community)
        {
            if (ModelState.IsValid)
            {
                this.repositories.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    this.sceneController.ApplyTemplate(community.CommunityId, templateSceneId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var community = GetCommunity(id);
            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                this.repositories.Communities.Save(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }

        [HttpGet]
        public ActionResult EditScene(string id)
        {
            var community = GetCommunity(id);
            return View(community);
        }



        //
        // GET: /Community/Delete/5
        [DesignerAuthorize]
        public ActionResult Delete(string id)
        {
            var community = this.repositories.Communities.Single(c => c.CommunityId == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [DesignerAuthorize]
        public ActionResult DeleteConfirmed(string id)
        {            
            this.repositories.Communities.Delete(id);
            return RedirectToAction("Index");
        }

        [SinglePage(Module="views/review-index")]
        public ActionResult Reviews(string id, int page = 1, int size = int.MaxValue)
        {
            if (page < 1)
                return HttpNotFound();

            var community = GetCommunity(id);

            var total = this.repositories.Reviews.Where(r => r.TargetId == community.CommunityId).Count();
            var pager = new Pager<Review>(page - 1, size, total, this.repositories.Reviews.Where(r => r.TargetId == community.CommunityId).OrderBy(r => r.Created));

            if (page > pager.NumberOfPages)
                return HttpNotFound();

            // prepare information about all participants
            var participants = pager.PageItems.SelectMany(r => r.GetParticipants()).Distinct().ToList();
            var profiles = this.repositories.Profiles.Where(p => participants.Contains(p.UserName)).ToList();

            return View(new ReviewsViewModel(
                this,
                this.rating.GetTargetRating(community.CommunityId), 
                id,
                community.Name,
                this.config.Review.Questions,
                pager,
                profiles));
        }

        [HttpGet]
        public ActionResult AddReview(string id)
        {
            var community = GetCommunity(id);

            ViewBag.CommunityUrlId = id;
            ViewBag.CommunityName = community.Name;
            ViewBag.Questions = this.config.Review.Questions;

            return View(new Review { TargetId = community.CommunityId });
        }

        [HttpPost]
        public ActionResult AddReview(Review review)
        {
            review.UserName = User.Identity.Name;
            review.Message = review.Message.IsEmpty() ? string.Empty : Encoding.FromBase64(review.Message);
            review.Created = DateTime.Now.ToUniversalTime();
            this.repositories.Reviews.Insert(review);
            return Redirect(Url.Action("Reviews", new { id = this.RouteData.Values["id"] }) + "#" + review.ReviewId);
        }

        [HttpDelete]
        [DesignerAuthorize]
        public ActionResult DeleteReview(string reviewId)
        {
            this.repositories.Reviews.Delete(reviewId);
            return Json(null);
        }

        [HttpDelete]
        [DesignerAuthorize]
        public ActionResult DeleteReviewComment(string reviewId, string commentId)
        {
            this.repositories.Reviews.DeleteReviewComment(reviewId, commentId);
            return Json(null);
        }

        [HttpPost]
        public ActionResult PostReviewComment(string reviewId, string message)
        {
            var comment = new Comment
            {
                Created = DateTime.UtcNow,
                UserName = this.User.Identity.Name,
                Message = message
            };
            this.repositories.Reviews.AddReviewComment(reviewId, comment);
            return Json(new CommentViewModel(comment, this.repositories));
        }


        private Community GetCommunity(string id)
        {
            if (this.repositories.IsValidId(id))
            {
                return this.repositories.Communities.Single(c => c.CommunityId == id);
            }
            else
            {
                id = id.ToLower();
                return this.repositories.Communities.Single(c => c.UrlId.ToLower() == id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}