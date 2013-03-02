using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Cms.Core;
using Cms.Helpers;
using Cms.Infrastructure;
using Cms.Models;
using Cms.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cms.Controllers
{
    public abstract class SceneController : Controller
    {
        private IConfig config = null;
        private IRepositories repos = null;
        private IRatingCalculator rating = null;

        public SceneController(IConfig config, IRepositories repos, IRatingCalculator rating)
        {
            this.config = config;
            this.repos = repos;
            this.rating = rating;
        }

        public virtual SceneViewModel GetSceneViewModel(ISceneHolder sceneHolder)
        {
            var scene = sceneHolder.Scene ?? new Scene();

            // var templates = Enumerable.Empty<Scene>();
         //   this.ViewBag.Templates = new SelectList(templates, "id", "title");
           // this.ViewBag.LinkableBricksSceneId = Constants.LinkableBricksSceneId;

            return scene.ToViewModel(GetViewModelContext(sceneHolder));
        }

        protected abstract ISceneHolder GetSceneHolder(string entityId);
        protected abstract void SaveScene(string entityId, Scene scene);
        //protected abstract ActionResult RedirectToDesignScene();


        private class SceneViewModelResolver : JsonConverter
        {
            SceneViewModelContext sceneViewModelContext;

            public SceneViewModelResolver(SceneViewModelContext sceneViewModelContext)
            {
                this.sceneViewModelContext = sceneViewModelContext;
            }

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(IBrickViewModel<Brick>));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // Load JObject from stream
                var jObject = JObject.Load(reader);

                var brickType = Assembly.GetExecutingAssembly().GetType(jObject["brickType"].Value<string>());
                var brick = (Brick)Activator.CreateInstance(brickType);
                // Create target object based on JObject

                var brickViewModel = BrickViewModel<Brick>.Create(this.sceneViewModelContext, brick);

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), brickViewModel);

                return brickViewModel;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }


        [DesignerAuthorize]
        public virtual ActionResult SaveScene(string id, string sceneJson)
        {
            var sceneHolder = GetSceneHolder(id);
            var converter = new SceneViewModelResolver(GetViewModelContext(sceneHolder));
            var scene = JsonConvert.DeserializeObject<SceneViewModel>(sceneJson, converter);
            if (ModelState.IsValid)
            {
                var originalSceneBricks = id.IsEmpty()
                    ? new Dictionary<string, Brick>()
                    : (this.GetSceneHolder(id).Scene ?? new Scene()).Walls
                        .SelectMany(w => w.Bricks)
                        .ToDictionary(b => b.BrickId, b => b);

                var sceneEntity = new Scene();
                //sceneEntity.SceneId = scene.SceneId;
                sceneEntity.Title = scene.Title;
                sceneEntity.IsTemplate = scene.IsTemplate;

                var wallList = new List<Wall>();

                foreach (var wall in scene.Walls)
                {
                    var wallEntity = new Wall();
                    wallEntity.Title = wall.Title;
                    wallEntity.Width = wall.Width;

                    wallList.Add(wallEntity);

                    var brickList = new List<Brick>();

                    foreach (var brick in wall.Bricks)
                    {
                        Brick brickEntity = null;
                        if (!brick.BrickId.IsEmpty() && originalSceneBricks.ContainsKey(brick.BrickId))
                        {
                            brickEntity = originalSceneBricks[brick.BrickId];
                        }

                        if (brickEntity == null)
                        {
                            brickEntity = brick.Content;
                            brickEntity.BrickId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                        }

                        brickEntity.Title = brick.Title;
                        brickEntity.Width = brick.Width;

                        brickList.Add(brickEntity);
                    }

                    wallEntity.Bricks = brickList;
                }

                sceneEntity.Walls = wallList;

                this.SaveScene(id, sceneEntity);
            }

            return RedirectToAction("EditScene", null, new { id });
        }

        private SceneViewModelContext GetViewModelContext(ISceneHolder sceneHolder)
        {
            return new SceneViewModelContext(this, this.config, this.repos, this.rating, sceneHolder);
        }


        //
        // GET: /Brick/Edit/5
        [DesignerAuthorize]
        public ActionResult EditBrick(string id, string brickId)
        {
            var sceneHolder = this.GetSceneHolder(id);
            var brick = sceneHolder.Scene.Walls.SelectMany(w => w.Bricks).Single(b => b.BrickId == brickId);

            return View(ContentUrl.Views.Brick.Edit, BrickViewModel<Brick>.Create(GetViewModelContext(sceneHolder), brick));
        }


        [HttpPost]
        [DesignerAuthorize]
        public ActionResult EditBrick(string id, IBrickViewModel<Brick> brickViewModel)
        {
            var htmlBrick = brickViewModel.Content as HtmlBrick;
            if (htmlBrick != null)
            {
                htmlBrick.Html = HttpUtility.HtmlDecode(htmlBrick.Html);
            }


            var scene = this.GetSceneHolder(id).Scene;

            foreach(var wall in scene.Walls)
            {
                var brickList = wall.Bricks.ToList();

                for(int i = 0; i < brickList.Count; i ++)
                {
                    var brick = brickList[i];
                    if(brick.BrickId == brickViewModel.BrickId)
                    {
                        var newBrick = brickViewModel.Content;
                        newBrick.Title = brick.Title;
                        newBrick.Width = brick.Width;

                        brickList.RemoveAt(i);
                        brickList.Insert(i, newBrick);

                        wall.Bricks = brickList;

                        break;
                    }
                }
            }

            this.SaveScene(id, scene);

            return RedirectToAction("EditScene", null, new { id });
        }
    }
}