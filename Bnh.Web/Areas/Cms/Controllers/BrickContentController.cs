using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Cms.Models;
using Bnh.Cms.Repositories;

namespace Bnh.Cms.Controllers
{
    public class BrickContentController : Controller
    {
        private Config config;
        private CmsRepos db;

        public BrickContentController(Config config, CmsRepos db)
        {
            this.config = config;
            this.db = db;
        }


        //
        // GET: /BrickContent/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var content = db.BrickContents.First(b => b.BrickContentId == id);
            var linkableContent = content as LinkableContent;
            if (linkableContent != null)
            {
                this.ViewBag.BricksToLink = this.db.Scenes
                    .First(s => s.SceneId == Constants.LinkableBricksSceneId)
                    .Walls.SelectMany(w => w.Bricks)
                    .Select(b => new SelectListItem { Value = b.BrickContentId, Text = b.Title });
            }
            return View(ContentUrl.Views.BrickContent.Edit, content);
        }

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Edit(BrickContent content)
        {
            var htmlContent = content as HtmlContent;
            if (htmlContent != null)
            {
                htmlContent.Html = HttpUtility.HtmlDecode(htmlContent.Html);
            }
            db.BrickContents.Save(content);

            // redirect to previous url
            return Redirect(this.Request.RequestContext.GetBackUrl());
        }

        [HttpGet]
        public new ActionResult View(string id)
        {
            var content = db.BrickContents.First(b => b.BrickContentId == id);
            return View(ContentUrl.Views.BrickContent.View, content);
        }
    }
}