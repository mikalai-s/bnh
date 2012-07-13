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
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id) :
                (Func<Community, bool>)(c => c.UrlId == id);
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
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id):
                (Func<Community, bool>)(c => c.UrlId == id);
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
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id) :
                (Func<Community, bool>)(c => c.UrlId == id);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}