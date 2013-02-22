using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using Bnh.ViewModels;
using Cms.Core;
using Cms.Infrastructure;
using Cms.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bnh.Controllers
{
    [DesignerAuthorizeAttribute]
    public class AdminController : Controller
    {
        readonly IRatingCalculator rating;
        readonly IBnhRepositories repos;
        readonly IBnhConfig config;
        readonly IPathMapper pathMapper;

        public AdminController(IBnhRepositories repos, IRatingCalculator rating, IBnhConfig config, IPathMapper pathMapper)
        {
            this.repos = repos;
            this.rating = rating;
            this.config = config;
            this.pathMapper = pathMapper;
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

        public ActionResult Info()
        {
            return View(new Dictionary<string, object>
            {
                { "Version", BnhConfig.Version },
                { "Host", this.HttpContext.Request.ServerVariables["HTTP_HOST"] },
                { "Activator", BnhConfig.Activator },
                { "Is Valid Host?", this.config.IsValidHost(this.HttpContext) }
            });
        }

        [HttpGet]
        public ActionResult Files(string path)
        {
            path = path ?? string.Empty;

            var uploadsFolderPath = this.pathMapper.Map(config.UploadsFolder);
            var fullPath = Path.Combine(uploadsFolderPath, path);

            // Check whether uploads folder exist
            var uploadsFolder = this.pathMapper.Map(config.UploadsFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            return View(new FilesViewModel(path, fullPath, config.UploadsFolder));
        }

        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files, string path)
        {
            var uploadsFolder = this.pathMapper.Map(config.UploadsFolder);
            foreach (var file in files.Where(f => f != null))
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fullPath = Path.Combine(uploadsFolder, path ?? string.Empty, fileName);
                    file.SaveAs(fullPath);
                }
            }
            return RedirectToAction("Files", new { path });
        }

        [HttpPost]
        public ActionResult CreateFolder(string folderName, string path)
        {
            var uploadsFolder = this.pathMapper.Map(config.UploadsFolder);
            var folderPath = Path.Combine(uploadsFolder, path ?? string.Empty, folderName);
            Directory.CreateDirectory(folderPath);

            return RedirectToAction("Files", new { path });
        }

        [HttpPost]
        public ActionResult DeleteFile(string path, string fileName)
        {
            if (!fileName.IsEmpty())
            {
                var uploadsFolder = this.pathMapper.Map(config.UploadsFolder);
                var filePath = Path.Combine(uploadsFolder, path, fileName);
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                else if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Files", new { path} );
        }
    }
}
