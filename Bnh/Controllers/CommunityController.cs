using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Models;

namespace Bnh.Controllers
{ 
    public class CommunityController : Controller
    {
        private BlEntities db = new BlEntities();

        //
        // GET: /Community/

        public ViewResult Index()
        {
            var communityentities = db.CommunityEntities.Include("City");
            return View(communityentities.ToList());
        }

        //
        // GET: /Community/Details/5

        public ViewResult Details(Guid id)
        {
            Community community = db.CommunityEntities.Single(c => c.Id == id);
            return View(community);
        }

        //
        // GET: /Community/Create

        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.CitiyEntities, "Id", "Name");
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
                db.CommunityEntities.AddObject(community);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CityId = new SelectList(db.CitiyEntities, "Id", "Name", community.City.Id);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Community community = db.CommunityEntities.Single(c => c.Id == id);
            ViewBag.CityId = new SelectList(db.CitiyEntities, "Id", "Name", community.City.Id);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                db.CommunityEntities.Attach(community);
                db.ObjectStateManager.ChangeObjectState(community, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.CitiyEntities, "Id", "Name", community.City.Id);
            return View(community);
        }

        //
        // GET: /Community/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Community community = db.CommunityEntities.Single(c => c.Id == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Community community = db.CommunityEntities.Single(c => c.Id == id);
            db.CommunityEntities.DeleteObject(community);
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