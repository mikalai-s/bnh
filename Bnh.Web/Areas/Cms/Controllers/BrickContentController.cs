using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Cms.Core;
using Cms.Helpers;
using Cms.Infrastructure;
using Cms.Models;

namespace Cms.Controllers
{
    public class BrickContentController : Controller
    {
        private IConfig config;
        private IRepositories repos;

        public BrickContentController(IConfig config, IRepositories repos)
        {
            this.config = config;
            this.repos = repos;
        }

        //
        // GET: /BrickContent/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var content = repos.BrickContents.First(b => b.BrickContentId == id);
            var linkableContent = content as LinkableContent;
            if (linkableContent != null)
            {
                this.ViewBag.BricksToLink = this.repos.Scenes
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
            repos.BrickContents.Save(content);

            // redirect to previous url
            return Redirect(this.Request.RequestContext.GetBackUrl());
        }

        [HttpGet]
        public new ActionResult View(string id)
        {
            var content = repos.BrickContents.First(b => b.BrickContentId == id);
            return View(ContentUrl.Views.BrickContent.View, content);
        }
    }
}