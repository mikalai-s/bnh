using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Data.Objects.DataClasses;

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
            var communities = db.Communities.Include("Zone");
            return View(communities.ToList());
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
            using(var cm = new CmEntities())
            {
                ViewBag.Templates = new SelectList(cm.SceneTemplates.ToList(), "Id", "Title");
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

                var sceneTemplateIdString = this.Request.Form["SceneTemplateId"];
                if (!string.IsNullOrEmpty(sceneTemplateIdString))
                {
                    using (var cm = new CmEntities())
                    {
                        SceneTemplating.CloneScene(Guid.Parse(sceneTemplateIdString), community.Id, cm);

                        cm.SaveChanges();
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