using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Data.Objects.DataClasses;
using Bnh.Web.Models;
using Ms.Cms.Models;
using MongoDB.Bson;
using Ms.Cms.Controllers;

namespace Bnh.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private BleEntities db = new BleEntities();

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

            var city = db.Cities.First(c => c.Name == "Calgary");
                var zones = city.Zones.ToList();
                var communities = city
                    .GetCommunities(db)
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
            var checker = GetCommunityResolver(id);
            return View(db.Communities.Single(checker));
        }

        //
        // GET: /Community/Create
        [Authorize(Roles = "content_manager")]
        public ActionResult Create()
        {
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones);
            using(var cm = new CmsEntities())
            {
                var sceneTemplates = from s in cm.Scenes
                                     where s.IsTemplate
                                     select new { id = s.SceneId, title = s.Title };
                ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

                var city = db.Cities.First(c => c.Name == "Calgary");
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
                db.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    var sceneController = new SceneController();
                    sceneController.ApplyTemplate(community.CommunityId, templateSceneId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(string id)
        {
            var checker = GetCommunityResolver(id);
            Community community = db.Communities.Single(checker);
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
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
                db.Communities.Save(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
            return View(community);
        }

        [HttpGet]
        public ActionResult EditScene(string id)
        {
            var checker = GetCommunityResolver(id);
            Community community = db.Communities.Single(checker);
            return View(community);
        }



        //
        // GET: /Community/Delete/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Delete(string id)
        {
            Community community = db.Communities.Single(c => c.CommunityId == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "content_manager")]
        public ActionResult DeleteConfirmed(string id)
        {            
            db.Communities.Delete(id);
            return RedirectToAction("Index");
        }

        private Review[] _reviews = new Review[]
        {
            new Review { Message = "Review 1", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 2", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 3", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 4", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 5", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 6", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 7", Ratings = new [] { 1, 2, 3, 4, 5 } },
            new Review { Message = "Review 8", Ratings = new [] { 1, 2, 3, 4, 5 } }
        };


        public ActionResult Review(string id, int page = 0, int itemsPerPage = int.MaxValue)
        {
            // save curretn id so review view can use it
            ViewBag.CommunityUrlId = id;

            ObjectId oid;
            if (!ObjectId.TryParse(id, out oid))
                id = db.Communities.Where(c => c.UrlId == id).Select(c => c.CommunityId).Single();
            
            //var total = db.Reviews.Where(r => r.ReviewId == id).Count();
            //var pager = new Pager<Review>(page, itemsPerPage, total, db.Reviews.Where(r => r.ReviewId == id), new { CommunityUrlId = id });
            var total = this._reviews.Length;
            var pager = new Pager<Review>(page, 3, total, _reviews);
            if (page >= pager.NumberOfPages)
                return HttpNotFound("Page not found");
            return View("Review", pager);
        }


        private Func<Community, bool> GetCommunityResolver(string id)
        {
            ObjectId oid;
            if(ObjectId.TryParse(id, out oid))
                return c => (c.CommunityId == id);
            return c => (c.UrlId == id);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}