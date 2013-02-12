using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Core.Entities;
using Bnh.Models;
using Bnh.ViewModels;
using Cms.Controllers;
using Cms.Models;
using System.Web.Mvc.Html;
using Bnh.Helpers;
using System.Collections.Generic;
using Cms.ViewModels;
using Cms.Core;
using Cms.Infrastructure;

namespace Bnh.Controllers
{
    public class CommunityController : Controller
    {
        private IBnhConfig config = null;
        private IBnhRepositories repos = null;
        private IRatingCalculator rating = null;
        private HtmlHelper htmlHelper = null;
        private SceneController sceneController = null;

        public CommunityController(IBnhConfig config, IBnhRepositories repos, IRatingCalculator rating, SceneController sceneController)
        {
            this.config = config;
            this.repos = repos;
            this.rating = rating;
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
            var city = this.repos.Cities.First(c => c.Name == config.City);
            var communities = this.repos.Communities.Where(c => c.CityId == city.CityId);
            var model = new CommunityIndexViewModel(GetViewModelContext(), city.Zones, communities);
            return View(model);
        }

        private ViewModelContext GetViewModelContext()
        {
            return new ViewModelContext(this, this.config, this.repos, this.rating);
        }


        public ActionResult Details(string id)
        {
            var community = GetCommunity(id);
            if (this.repos.IsValidId(id))
            {
                return RedirectToAction("Details", new { id = community.UrlId });
            }

            return View(community);
        }

        //
        // GET: /Community/Create
        [DesignerAuthorize]
        public ActionResult Create()
        {
            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones);
            var sceneTemplates = from s in this.repos.Scenes
                                    where s.IsTemplate
                                    select new { id = s.SceneId, title = s.Title };
            ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

            var city = this.repos.Cities.First(c => c.Name == config.City);
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
                this.repos.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    this.sceneController.ApplyTemplate(community.CommunityId, templateSceneId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var community = GetCommunity(id);
            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
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
                this.repos.Communities.Save(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
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
            var community = this.repos.Communities.Single(c => c.CommunityId == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [DesignerAuthorize]
        public ActionResult DeleteConfirmed(string id)
        {            
            this.repos.Communities.Delete(id);
            return RedirectToAction("Index");
        }

        private Community GetCommunity(string id)
        {
            if (this.repos.IsValidId(id))
            {
                return this.repos.Communities.Single(c => c.CommunityId == id);
            }
            else
            {
                id = id.ToLower();
                return this.repos.Communities.Single(c => c.UrlId.ToLower() == id);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddReview(string id)
        {
            var community = GetCommunity(id);

            ViewBag.CommunityUrlId = id;
            ViewBag.CommunityName = community.Name;
            ViewBag.Questions = this.config.Review.Questions;

            return View(new Review { TargetId = community.CommunityId });
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddReview(Review review)
        {
            review.UserName = User.Identity.Name;
            review.Message = review.Message.IsEmpty() ? string.Empty : Encoding.FromBase64(review.Message);
            review.Created = DateTime.Now.ToUniversalTime();
            this.repos.Reviews.Insert(review);
            return Redirect(Url.Action("Details", new { id = this.RouteData.Values["id"] })/* + "#" + review.ReviewId*/);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}