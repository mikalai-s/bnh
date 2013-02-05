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
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;

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
            if (!(content is TabsContent))
            {
                var sceneBricks = this.repos.Scenes.Collection
                    .Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(content.BrickContentId)))
                    .SelectMany(s => s.Walls.SelectMany(w => w.Bricks.Select(b => b.BrickContentId)))
                    .ToList();
                var tabBricks = this.repos.BrickContents
                    .OfType<TabsContent>()
                    .Where(b => sceneBricks.Contains(b.BrickContentId))
                    .ToList();
                this.ViewBag.TabsAvailable = from tabBrick in tabBricks
                                             let tabs = tabBrick.Tabs.ToList()
                                             from tab in tabs
                                             let tabId = tabBrick.BrickContentId
                                             let index = tabs.IndexOf(tab)
                                             select new SelectListItem
                                             {
                                                 Selected = (content.TabId == tabId) && (content.TabIndex == index),
                                                 Text = tabBrick.ContentTitle + ": " + tab,
                                                 Value = JsonConvert.SerializeObject(new { id = tabId, index })
                                             };
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