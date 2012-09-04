using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Bnh.Core.Entities;
using Bnh.Web.Models;

using Microsoft.Web.Helpers;

namespace Bnh.Web.ViewModels
{
    public class ReviewsViewModel
    {
        public string Title { get; set; }
        public string Rating { get; set; }
        public string TargetUrlId { get; set; }
        public string TargetName { get; set; }
        public IEnumerable<LinkViewModel> PagerLinks { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public LinkViewModel AddReviewLink { get; set; }
        public bool Admin { get; set; }
        public string DeleteReviewUrl { get; set; }
        public string DeleteCommentUrl { get; set; }

        public ReviewsViewModel(Controller controller, int? rating, string targetUrlId, string targetName, IDictionary<string, string> ratingQuestions, Pager<Review> pager)
        {
            var urlHelper = new UrlHelper(controller.HttpContext.Request.RequestContext);
            var htmlHelper = new HtmlHelper(new ViewContext(controller.ControllerContext, new WebFormView(controller.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());

            this.Rating = "Rating: " + (rating.HasValue ? rating.ToString() : "Not rated");
            this.TargetUrlId = targetUrlId;
            this.TargetName = targetName;
            this.Title = targetName + " Reviews";
            this.AddReviewLink = new LinkViewModel
            {
                Text = "Add review",
                Href = urlHelper.Action("AddReview", new { id = targetUrlId })
            };

            if (pager.NumberOfPages > 1)
            {
                Func<Pager<Review>.Link, string> classResolver = (link) =>
                    (link.Active && link.Numeric) ? "active page" :
                    link.Numeric ? "page" : "";

                this.PagerLinks = pager.Links
                    .Select(link => new LinkViewModel
                    {
                        Text = link.Text,
                        Class = classResolver(link),
                        Href = (link.Disabled || link.Active) ? string.Empty : urlHelper.Action(link.Action, new { id = targetUrlId, page = link.PageIndex + 1 })
                    });
            }
            this.Reviews = pager
                .PageItems
                .Select(r => new ReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    UserName = r.UserName,
                    UserAvatarSrc = Gravatar.GetUrl(r.UserName, 64) + "&d=identicon",
                    Created = r.Created.ToLocalTime().ToUserFriendlyString(),
                    Message = r.Message,
                    Comments = (r.Comments ?? Enumerable.Empty<Comment>())
                        .Select(c => new CommentViewModel(c)),
                    Ratings = ratingQuestions
                        .Where(q => r.Ratings[q.Key].HasValue)
                        .Select(q => new RatingQuestionViewModel
                        {
                            Question = q.Value + ":",
                            AnswerHtml = RatingAnswerHtml(r.Ratings[q.Key].Value)
                        }),
                    PostCommentActionUrl = urlHelper.Action("PostReviewComment")
                });
            this.Admin = controller.HttpContext.User.IsInRole("content_manager");
            this.DeleteReviewUrl = this.Admin ? urlHelper.Action("DeleteReview") : null;
            this.DeleteCommentUrl = this.Admin ? urlHelper.Action("DeleteReviewComment") : null;
        }

        private static string RatingAnswerHtml(int rating)
        {
            return "<div class='scale l' style='width:{0}px'></div><div class='scale r' style='width:{1}px'></div>"
                .FormatWith(rating * 10, 100 - rating * 10);
        }
    }
}