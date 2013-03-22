using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.Core.Entities;
using Bnh.Models;
using Bnh.ViewModels;
using Cms.Controllers;
using Cms.Models;
using System.Web.Mvc.Html;
using Bnh.Helpers;
using System.Collections.Generic;
using Cms.ViewModels;
using Cms.Core;
using Cms.Infrastructure;
using Cms.Utils;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace Bnh.Controllers
{
    public class CommunityController : SceneController
    {
        private IBnhConfig config = null;
        private IBnhRepositories repos = null;
        private IRatingCalculator rating = null;

        public CommunityController(IBnhConfig config, IBnhRepositories repos, IRatingCalculator rating)
            : base(config, repos, rating)
        {
            this.config = config;
            this.repos = repos;
            this.rating = rating;
        }



        protected override ISceneHolder GetSceneHolder(string communityId)
        {
            return GetCommunity(communityId, true);
        }

        protected override void SaveScene(string communityId, Scene scene)
        {

            this.repos.Communities.Collection.Update(
                GetCommunityQuery(communityId),
                Update.Set("Scene", scene.ToBsonDocument()));
        }



        //
        // GET: /Community/
        public ViewResult Index()
        {
            var city = this.repos.Cities.First(c => c.Name == config.City);
            var communities = this.repos.Communities.Where(c => c.CityId == city.CityId);
            var model = new CommunityIndexViewModel(GetViewModelContext(), city.Zones, communities);
            return View(model);
        }

        private ViewModelContext GetViewModelContext()
        {
            return new ViewModelContext(this, this.config, this.repos, this.rating);
        }


        public ActionResult Details(string id)
        {
            var community = GetCommunity(id, true);
            if (this.repos.IsValidId(id))
            {
                return RedirectToAction("Details", new { id = community.UrlId });
            }

            return View(community);
        }

        //
        // GET: /Community/Create
        [DesignerAuthorize]
        public ActionResult Create()
        {
            throw new NullReferenceException();
            //ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones);
            //var sceneTemplates = from s in this.repos.Scenes
            //                        where s.IsTemplate
            //                        select new { id = s.SceneId, title = s.Title };
            //ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

            //var city = this.repos.Cities.First(c => c.Name == config.City);
            //ViewBag.CityZones = new SelectList(city.Zones);
            //ViewBag.CityId = city.CityId;
            //return View();
        } 

        //
        // POST: /Community/Create

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Create(Community community)
        {
            if (ModelState.IsValid)
            {
                this.repos.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    SceneUtils.ApplyTemplate(this.repos, templateSceneId, community.CommunityId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [DesignerAuthorize]
        public ActionResult Edit(string id)
        {
            var community = GetCommunity(id);
            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        [DesignerAuthorize]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                community.Ratings = CalculateCommunityRating(community.CommunityId);
                community.Scene = GetSceneHolder(community.CommunityId).Scene;
                this.repos.Communities.Save(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.CityZones = new SelectList(this.repos.Cities.First(c => c.Name == config.City).Zones, community.Zone);
            return View(community);
        }

        [HttpGet]
        [DesignerAuthorize]
        public ActionResult EditScene(string id)
        {
            var community = GetCommunity(id, true);
            return View(community);
        }



        //
        // GET: /Community/Delete/5
        [DesignerAuthorize]
        public ActionResult Delete(string id)
        {
            return View(GetCommunity(id));
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [DesignerAuthorize]
        public ActionResult DeleteConfirmed(string id)
        {
            var communityId = GetCommunity(id).CommunityId;
            this.repos.Communities.Delete(communityId);
            return RedirectToAction("Index");
        }

        private IMongoQuery GetCommunityQuery(string id)
        {
            return this.repos.IsValidId(id)
                ? Query.EQ("_id", new BsonObjectId(id))
                : Query.Matches("UrlId", BsonRegularExpression.Create(new Regex(id, RegexOptions.IgnoreCase)));
        }

        private Community GetCommunity(string id, bool includeScene = false)
        {
            return this.repos.Communities.Collection
                .Find(GetCommunityQuery(id))
                .SetFields(includeScene ? Fields.Null : Fields.Exclude("Scene"))
                .Single();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddReview(string id)
        {
            var community = GetCommunity(id);

            if (this.repos.Reviews.Any(r => (r.TargetId == community.CommunityId) && (r.UserName == User.Identity.Name)))
            {
                // current user has already left a review, show error message
                return this.ErrorMessage("Unable to leave a review", "You have already left a review for given community. If you changed your opinion, please, leave a comment to existing one.");
            }

            ViewBag.CommunityUrlId = id;
            ViewBag.CommunityName = community.Name;
            ViewBag.Questions = this.config.Review.Questions;

            return View(new ReviewViewModel(GetViewModelContext(), community.CommunityId));
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddReview(Review review)
        {
            review.UserName = User.Identity.Name;
            review.Message = review.Message.IsEmpty() ? string.Empty : Encoding.FromBase64(review.Message);
            review.Created = DateTime.Now.ToUniversalTime();
            this.repos.Reviews.Insert(review);

            // update community rating now
            var comm = this.repos.Communities.Single(c => c.CommunityId == review.TargetId);
            comm.Ratings = CalculateCommunityRating(review.TargetId);
            this.repos.Communities.Save(comm);

            return Redirect(Url.Action("Details", new { id = this.RouteData.Values["id"] })/* + "#" + review.ReviewId*/);
        }

        private IDictionary<string, double?> CalculateCommunityRating(string communityId)
        {
            return this.config.Review.Questions
                .ToDictionary(
                    q => q.Key,
                    q => 
                        {
                            var map = this.rating.GetTargetRatingMap(new[] { communityId }, q.Key);
                            return map.ContainsKey(communityId)
                                ? map[communityId]
                                : null;
                        });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }





       
    }
}