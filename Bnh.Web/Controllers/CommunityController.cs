using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Core.Entities;
using Bnh.Web.Models;

using Ms.Cms.Controllers;
using Ms.Cms.Models;

namespace Bnh.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private Config config = null;
        private IEntityRepositories repositories = null;
        private IRatingCalculator rating = null;

        public CommunityController(Config config, IEntityRepositories repositories, IRatingCalculator rating)
        {
            this.config = config;
            this.repositories = repositories;
            this.rating = rating;
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
                            infoPopup = "<a href='{0}'>{1}</a>".FormatWith(urlHelper.Action("Details", "Community", new { id = c.UrlId }), c.Name)
                        }
                    })
                .OrderBy(g => zones.IndexOf(g.Key))
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(c => c.community.Name));

            return Json(communities, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(string id)
        {
            return View(GetCommunity(id));
        }

        //
        // GET: /Community/Create
        [Authorize(Roles = "content_manager")]
        public ActionResult Create()
        {
            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones);
            using(var cm = new CmsEntities())
            {
                var sceneTemplates = from s in cm.Scenes
                                     where s.IsTemplate
                                     select new { id = s.SceneId, title = s.Title };
                ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

                var city = this.repositories.Cities.First(c => c.Name == config.City);
                ViewBag.CityZones = new SelectList(city.Zones);
                ViewBag.CityId = city.CityId;
            }
            return View();
        } 

        //
        // POST: /Community/Create

        [HttpPost]
        [Authorize(Roles = "content_manager")]
        public ActionResult Create(Community community)
        {
            if (ModelState.IsValid)
            {
                this.repositories.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    var sceneController = new SceneController();
                    sceneController.ApplyTemplate(community.CommunityId, templateSceneId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(string id)
        {
            var community = GetCommunity(id);
            ViewBag.CityZones = new SelectList(this.repositories.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        [Authorize(Roles = "content_manager")]
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
            return View(GetCommunity(id));
        }



        //
        // GET: /Community/Delete/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Delete(string id)
        {
            var community = this.repositories.Communities.Single(c => c.CommunityId == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "content_manager")]
        public ActionResult DeleteConfirmed(string id)
        {            
            this.repositories.Communities.Delete(id);
            return RedirectToAction("Index");
        }


        public ActionResult Reviews(string id, int page = 1, int size = int.MaxValue)
        {
            if (page < 1)
                return HttpNotFound();

            var community = GetCommunity(id);

            // save current id so we can reuse it in review
            ViewBag.Rating = this.rating.GetTargetRating(community.CommunityId);
            ViewBag.CommunityUrlId = id;
            ViewBag.CommunityName = community.Name;
            ViewBag.Questions = this.config.Review.Questions;

            var total = this.repositories.Reviews.Where(r => r.TargetId == community.CommunityId).Count();
            var pager = new Pager<Review>(page - 1, size, total, this.repositories.Reviews.Where(r => r.TargetId == community.CommunityId));
            
            if (page > pager.NumberOfPages)
                return HttpNotFound();

            return View(pager);
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
            review.Message = HttpUtility.HtmlDecode(review.Message);
            review.Created = DateTime.Now.ToUniversalTime();
            this.repositories.Reviews.Insert(review);
            return Redirect(Url.Action("Reviews", new { id = this.RouteData.Values["id"] }) + "#" + review.ReviewId);
        }

        [HttpDelete]
        [Authorize(Roles = "content_manager")]
        public ActionResult DeleteReview(string reviewId)
        {
            this.repositories.Reviews.Delete(reviewId);
            return Json(null);
        }

        [HttpPost]
        public ActionResult PostReviewComment(string reviewId, Comment comment)
        {
            comment.Created = DateTime.UtcNow;
            comment.UserName = this.User.Identity.Name;
            this.repositories.Reviews.AddReviewComment(reviewId, comment);
            return Json(null);
        }


        private Community GetCommunity(string id)
        {
            if(this.repositories.IsValidId(id))
                return this.repositories.Communities.Single(c => c.CommunityId == id);
            else
                return this.repositories.Communities.Single(c => c.UrlId == id);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}