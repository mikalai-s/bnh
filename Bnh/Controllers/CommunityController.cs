using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Data.Objects.DataClasses;

namespace Bnh.Controllers
{ 
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

        //
        // GET: /Community/Details/5

        public ViewResult Details(Guid id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            return View(community);
        }

        //
        // GET: /Community/Create

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
        public ActionResult Create(Community community)
        {
            if (ModelState.IsValid)
            {
                community.Id = Guid.NewGuid();
                db.Communities.AddObject(community);
                db.SaveChanges();

                using(var cm = new CmEntities())
                {
                    var sceneTemplateId = Guid.Parse(this.Request.Form["SceneTemplateId"]);
                    var sceneTemplate = cm.SceneTemplates.FirstOrDefault(t => t.Id == sceneTemplateId);

                    foreach (var wall in sceneTemplate.Walls)
                    {
                        var newWall = wall.Clone();
                        newWall.Bricks = null;                        

                        foreach (var brick in wall.Bricks)
                        {
                            var newBrick = brick.Clone();
                            newBrick.Wall = newWall;                            
                        }

                        cm.Walls.AddObject(newWall);
                    }
                }
                return RedirectToAction("Index");  
            }

            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                db.Communities.Attach(community);
                db.ObjectStateManager.ChangeObjectState(community, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZoneId = new SelectList(db.Zones, "Id", "Name", community.ZoneId);
            return View(community);
        }

        //
        // GET: /Community/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Community community = db.Communities.Single(c => c.Id == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
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