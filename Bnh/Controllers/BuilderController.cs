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
    public class BuilderController : Controller
    {
        private BlEntities db = new BlEntities();

        //
        // GET: /Builder/

        public ViewResult Index()
        {
            return View(db.Builders.ToList());
        }

        //
        // GET: /Builder/Details/5

        public ViewResult Details(Guid id)
        {
            Builder builder = db.Builders.Single(b => b.Id == id);
            return View(builder);
        }

        //
        // GET: /Builder/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Builder/Create

        [HttpPost]
        public ActionResult Create(Builder builder)
        {
            if (ModelState.IsValid)
            {
                builder.Id = Guid.NewGuid();
                db.Builders.AddObject(builder);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(builder);
        }
        
        //
        // GET: /Builder/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Builder builder = db.Builders.Single(b => b.Id == id);
            return View(builder);
        }

        //
        // POST: /Builder/Edit/5

        [HttpPost]
        public ActionResult Edit(Builder builder)
        {
            if (ModelState.IsValid)
            {
                db.Builders.Attach(builder);
                db.ObjectStateManager.ChangeObjectState(builder, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(builder);
        }

        //
        // GET: /Builder/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Builder builder = db.Builders.Single(b => b.Id == id);
            return View(builder);
        }

        //
        // POST: /Builder/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Builder builder = db.Builders.Single(b => b.Id == id);
            db.Builders.DeleteObject(builder);
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