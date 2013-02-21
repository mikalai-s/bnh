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
using Cms.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;

namespace Cms.Controllers
{
    public class BrickController : Controller
    {
        private IConfig config;
        private IRepositories repos;

        public BrickController(IConfig config, IRepositories repos)
        {
            this.config = config;
            this.repos = repos;
        }

        //
        // GET: /Brick/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var brick = repos.Scenes.FindBrick(id);
            //var linkableContent = brick as LinkableBrick;
            //if (linkableContent != null)
            //{
            //    this.ViewBag.BricksToLink = this.repos.Scenes
            //        .First(s => s.SceneId == Constants.LinkableBricksSceneId)
            //        .Walls.SelectMany(w => w.Bricks)
            //        .Select(b => new SelectListItem { Value = b.BrickContentId, Text = b.Title });
            //}
            //if (!(brick is TabsBrick))
            //{
            //    var sceneBricks = this.repos.Scenes.Collection
            //        .Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(brick.BrickContentId)))
            //        .SelectMany(s => s.Walls.SelectMany(w => w.Bricks.Select(b => b.BrickContentId)))
            //        .ToList();
            //    var tabBricks = this.repos.BrickContents
            //        .OfType<TabsBrick>()
            //        .Where(b => sceneBricks.Contains(b.BrickContentId))
            //        .ToList();
            //    this.ViewBag.TabsAvailable = from tabBrick in tabBricks
            //                                 let tabs = tabBrick.Tabs.ToList()
            //                                 from tab in tabs
            //                                 let tabName = tabBrick.TabName
            //                                 let index = tabs.IndexOf(tab)
            //                                 select new SelectListItem
            //                                 {
            //                                     Selected = (brick.OwnerTabName == tabName) && (brick.OwnerTabIndex == index),
            //                                     Text = tabName + ": " + tab,
            //                                     Value = JsonConvert.SerializeObject(new { id = tabName, index })
            //                                 };
            //}
            return View(ContentUrl.Views.Brick.Edit, BrickViewModel<Brick>.Create(null, brick));
        }

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Edit(IBrickViewModel<Brick> brick)
        {
            var htmlBrick = brick.Content as HtmlBrick;
            if (htmlBrick != null)
            {
                htmlBrick.Html = HttpUtility.HtmlDecode(htmlBrick.Html);
            }
            repos.Scenes.UpdateBrick(brick.Content);

            // redirect to previous url
            return Redirect(this.Request.RequestContext.GetBackUrl());
        }

        [HttpGet]
        public new ActionResult View(string id)
        {
            throw new NotImplementedException();
            //var content = repos.BrickContents.First(b => b.BrickContentId == id);
            //return View(ContentUrl.Views.Brick.View, content);
        }
    }
}