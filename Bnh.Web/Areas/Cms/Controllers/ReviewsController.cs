using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Helpers;
using Cms.Core;
using Cms.Infrastructure;
using Cms.Models;
using Cms.ViewModels;

namespace Bnh.Web.Areas.Cms.Controllers
{
    public class ReviewsController : Controller
    {
        private IRepositories repos;

        public ReviewsController(IRepositories repos)
        {
            this.repos = repos;
        }

        /*
        [SinglePage(Module="views/review-index")]
        public ActionResult Reviews(string id, int page = 1, int size = int.MaxValue)
        {
            if (page < 1)
                return HttpNotFound();

            var community = GetCommunity(id);

            var total = this.repositories.Reviews.Where(r => r.TargetId == community.ReviewableTargetId).Count();
            var pager = new Pager<Review>(page - 1, size, total, this.repositories.Reviews.Where(r => r.TargetId == community.ReviewableTargetId).OrderBy(r => r.Created));

            if (page > pager.NumberOfPages)
                return HttpNotFound();

            // prepare information about all participants
            var participants = pager.PageItems.SelectMany(r => r.GetParticipants()).Distinct().ToList();
            var profiles = this.repositories.Profiles.Where(p => participants.Contains(p.UserName)).ToList();

            return View(new ReviewsViewModel(
                GetViewModelContext(),
                this.rating.GetTargetRating(community.CommunityId), 
                id,
                community.Name,
                this.config.Review.Questions,
                pager,
                profiles));
        }
        */
        
        [HttpDelete]
        [DesignerAuthorize]
        public ActionResult DeleteReview(string reviewId)
        {
            this.repos.Reviews.Delete(reviewId);
            return Json(true);
        }

        [HttpDelete]
        [DesignerAuthorize]
        public ActionResult DeleteReviewComment(string reviewId, string commentId)
        {
            this.repos.Reviews.DeleteReviewComment(reviewId, commentId);
            return Json(true);
        }
        

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult PostReviewComment(string reviewId, string message)
        {
            var comment = new Comment
            {
                Created = DateTime.UtcNow,
                UserName = this.User.Identity.Name,
                Message = message
            };
            this.repos.Reviews.AddReviewComment(reviewId, comment);
            return Json(new CommentViewModel(comment, this.repos.Profiles.Single(p => p.UserName == comment.UserName)));
        }
    }
}