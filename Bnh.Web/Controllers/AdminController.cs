using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Cms.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bnh.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class AdminController : Controller
    {
        readonly IRatingCalculator rating;
        readonly IBnhRepositories repos;
        readonly IConfig config;

        public AdminController(IBnhRepositories repos, IRatingCalculator rating, IConfig config)
        {
            this.repos = repos;
            this.rating = rating;
            this.config = config;
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeedbackReport()
        {
            return View(this.repos.Feedback.OrderBy(c => c.Created));
        }

        public ActionResult RefreshRatings()
        {
            // get all reviews
            var reviews = this.repos.Reviews.ToList();
            var reviewIds = reviews.Select(r => r.ReviewId).ToList();
            var targetIdsList = reviews.Select(r => r.TargetId).Distinct().ToList();

            // q: { id: a }
            var allRatings = this.config.Review.Questions
                    .ToDictionary(
                        q => q.Key,
                        q => this.rating.GetTargetRatingMap(targetIdsList, q.Key));

            // id: { q: a }
            var reorganizedRatings = new Dictionary<string, Dictionary<string, double?>>();
            foreach(var question in allRatings)
            {
                foreach(var targetWithAnswer in question.Value)
                {
                    var targetQuestion = reorganizedRatings.ContainsKey(targetWithAnswer.Key)
                        ? reorganizedRatings[targetWithAnswer.Key]
                        : (reorganizedRatings[targetWithAnswer.Key] = new Dictionary<string,double?>());

                    targetQuestion[question.Key] = targetWithAnswer.Value;
                }
            }

            foreach(var targetEntry in reorganizedRatings)
            {
                // set fresh rating
                this.repos.Communities.Collection.Update(
                    Query.EQ("_id", BsonValue.Create(ObjectId.Parse(targetEntry.Key))),
                    Update.Set("Ratings", BsonDocumentWrapper.Create(targetEntry.Value)));
            }

            foreach (var noRatingTarget in this.repos.Communities.Select(c => c.CommunityId).ToList().Except(reorganizedRatings.Keys))
            {
                // set fresh rating
                this.repos.Communities.Collection.Update(
                    Query.EQ("_id", BsonValue.Create(ObjectId.Parse(noRatingTarget))),
                    Update.Unset("Ratings"));
            }

            return View("Message", model: "Community ratings has been updated");
        }
    }
}
