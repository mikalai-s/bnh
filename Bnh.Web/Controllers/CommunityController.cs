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

namespace Bnh.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private BlEntities db = new BlEntities();

        //
        // GET: /Community/
        public ViewResult Index()
        {
            //db.Zones.Include("Communities");
            var communities = db.Communities.Include("Zone");
            return View(communities.ToList());
        }

        [HttpGet]
        public JsonResult Zones()
        {
            UrlHelper urlHelper = new UrlHelper(this.HttpContext.Request.RequestContext);
            return Json(db.Zones.OrderBy(m => m.Order).Include("Communities").ToArray().Select((item) =>
                                                                             {
                                                                                 ZoneDto zone = new ZoneDto();
                                                                                 zone.Name = item.Name;
                                                                                 zone.Communities = item.Communities.Select((community) =>
                                                                                                                                {
                                                                                                                                    CommunityDto communityDto = new CommunityDto();
                                                                                                                                    communityDto.DistanceToCenter = community.Remoteness;
                                                                                                                                    communityDto.Id = community.Id;
                                                                                                                                    communityDto.Name = community.Name;
                                                                                                                                    communityDto.UrlId = community.UrlId;
                                                                                                                                    communityDto.HasLake = community.HasLake;
                                                                                                                                    communityDto.GpsBounds = community.GpsBounds;
                                                                                                                                    communityDto.GpsLocation = community.GpsLocation;

                                                                                                                                    communityDto.InfoPopup = String.Format("<a href='{0}'>{1}</a>",
                                                                                                                                        urlHelper.Action("Details", "Community", new { id = community.UrlId}), community.Name); 
                                                                                                                                    return
                                                                                                                                        communityDto;
                                                                                                                                });
                                                                                 return
                                                                                     zone;
                                                                             }), JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(string id)
        {
            Guid guid;
            var isGuid = Guid.TryParse(id, out guid);
            Community community = db.Communities.Single(c => (isGuid && c.Id == guid) || (c.UrlId == id));
            return View(community);
        }

        //
        // GET: /Community/Create
        [Authorize(Roles = "content_manager")]
        public ActionResult Create()
        {
            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name");
            using(var cm = new CmsEntities())
            {
                var sceneTemplates = from s in cm.Scenes
                                     from t in cm.SceneTemplates
                                     where s.OwnerGuidId == t.Id
                                     select new { id = s.Id, title = t.Title };
                ViewBag.Templates = new SelectList(sceneTemplates.ToList(), "id", "title");
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
                community.Id = Guid.NewGuid();
                db.Communities.AddObject(community);
                db.SaveChanges();

                var templateSceneIdString = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneIdString))
                {
                    // NOTE: make sure community ID is already created when move to "code first"
                    using (var cms = new CmsEntities())
                    {
                        var scene = cms.Scenes.Attach(community.GetScene());
                        var templateSceneId = long.Parse(templateSceneIdString);
                        var templateScene = cms.Scenes.First(s => s.Id == templateSceneId);
                        scene.ApplyTemplate(cms, templateScene);
                        cms.SaveChanges();
                    }
                }
                return RedirectToAction("Index");  
            }

            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(Guid id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
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
                db.Communities.Attach(community);
                db.ObjectStateManager.ChangeObjectState(community, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }

        //
        // GET: /Community/Delete/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Delete(Guid id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "content_manager")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Community community = db.Communities.Single(c => c.Id == id);
            db.Communities.DeleteObject(community);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}