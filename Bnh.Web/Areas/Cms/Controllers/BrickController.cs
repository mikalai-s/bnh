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

        


        [HttpGet]
        public new ActionResult View(string id)
        {
            throw new NotImplementedException();
            //var content = repos.BrickContents.First(b => b.BrickContentId == id);
            //return View(ContentUrl.Views.Brick.View, content);
        }
    }
}