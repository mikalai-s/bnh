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
            var city = db.Cities.First(c => c.Name == "Calgary");
            city.Communities = db.Communities.Where(c => c.CityId ==city.Id).ToList();
            return View(city);
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
                                                                            communityDto.HasMountainView = community.HasMountainView;
                                                                            communityDto.HasClubOfFacility = community.HasClubOrFacility;
                                                                            communityDto.HasParksAndPathways = community.HasParksAndPathways;
                                                                            communityDto.HasShoppingPlaza = community.HasParksAndPathways;
                                                                            communityDto.HasWaterFeature = community.HasWaterFeature;
                                                                            communityDto.GpsBounds = community.GpsBounds;
                                                                            communityDto.GpsLocation = community.GpsLocation;
                                                                            communityDto.DeleteUrl = urlHelper.Action("Delete", "Community", new { id = community.Id });
                                                                            communityDto.DetailsUrl = urlHelper.Action("Details", "Community", new { id = community.UrlId });
                                                                            communityDto.InfoPopup = String.Format("<a href='{0}'>{1}</a>",
                                                                                urlHelper.Action("Details", "Community", new { id = community.UrlId }), community.Name);
                                                                            return
                                                                                communityDto;
                                                                        });
                            return
                                zone;
                        }), JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(string id)
        {
            ObjectId oid;
            if (ObjectId.TryParse(id, out oid))
            {
                return View(db.Communities.Single(c => c.Id == id));
            }
            else
            {
                return View(db.Communities.Single(c => c.UrlId == id));
            }           
        }

        //
        // GET: /Community/Create
        [Authorize(Roles = "content_manager")]
        public ActionResult Create()
        {
            // TODO: fix that
            //ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name");
            using(var cm = new CmsEntities())
            {/*
                var sceneTemplates = from s in cm.Scenes
                                     from t in cm.SceneTemplates
                                     where s.OwnerGuidId == t.Id
                                     select new { id = s.Id, title = t.Title };
                ViewBag.Templates = new SelectList(sceneTemplates.ToList(), "id", "title");*/
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

                var templateSceneIdString = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneIdString))
                {/*
                    // NOTE: make sure community ID is already created when move to "code first"
                    using (var cms = new CmsEntities())
                    {
                        var scene = cms.Scenes.Attach(community.GetScene());
                        var templateSceneId = long.Parse(templateSceneIdString);
                        var templateScene = cms.Scenes.First(s => s.Id == templateSceneId);
                        scene.ApplyTemplate(cms, templateScene);
                        cms.SaveChanges();
                    }*/
                }
                return RedirectToAction("Index");  
            }

            // TODO: Fix that
            //ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(string id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            // TODO: fix that
            //ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
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
                db.Communities.Insert(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            //TODO: fix that:
            //ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }

        //
        // GET: /Community/Delete/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Delete(string id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
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