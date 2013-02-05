using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Bnh;
using Cms.Models;
using Bnh.Core.Entities;
using Bnh.Models;
using Microsoft.Web.Helpers;
using Cms.Helpers;

namespace Cms.ViewModels
{
    public class ReviewsViewModel
    {
        public string TargetUrlId { get; set; }
        public string TargetName { get; set; }
        public IEnumerable<LinkViewModel> PagerLinks { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public LinkViewModel AddReviewLink { get; set; }
        public bool Admin { get; set; }
        public string DeleteReviewUrl { get; set; }
        public string DeleteCommentUrl { get; set; }

        public ReviewsViewModel(ViewModelContext context, double? rating, string targetUrlId, string targetName, IDictionary<string, string> ratingQuestions, Pager<Review> pager, List<Profile> profiles)
        {
            var userProfiles = profiles.ToDictionary(p => p.UserName, p => p);

            //this.Description = context.HtmlHelper.RatingStars(rating).ToString();
            this.TargetUrlId = targetUrlId;
            this.TargetName = targetName;
            //this.Title = targetName + " Reviews";
            this.AddReviewLink = new LinkViewModel
            {
                Text = "Add review",
                Href = context.UrlHelper.Action("AddReview", new { id = targetUrlId })
            };

            if (pager.NumberOfPages > 1)
            {
                Func<Pager<Review>.Link, string> classResolver = (link) =>
                    (link.Active && link.Numeric) ? "active page" :
                    link.Numeric ? "page" : 
                    link.Disabled ? "disabled" : "";

                this.PagerLinks = pager.Links
                    .Select(link => new LinkViewModel
                    {
                        Text = link.Text,
                        Css = classResolver(link),
                        Href = (link.Disabled || link.Active) 
                            ? string.Empty
                            : context.UrlHelper.Action(link.Action, new { id = targetUrlId, size = pager.PageSize, page = link.PageIndex + 1 })
                    });
            }
            this.Reviews = pager
                .PageItems
                .Select(r => new ReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    UserName = userProfiles[r.UserName].DisplayName,
                    UserAvatarSrc = Gravatar.GetUrl(userProfiles[r.UserName].GravatarEmail, 64) + "&d=identicon",
                    Created = r.Created.ToLocalTime().ToUserFriendlyString(),
                    Message = r.Message,
                    Comments = (r.Comments ?? Enumerable.Empty<Comment>())
                        .Select(c => new CommentViewModel(c, userProfiles[c.UserName])),
                    Ratings = ratingQuestions
                        .Where(q => r.Ratings[q.Key].HasValue)
                        .Select(q => new RatingQuestionViewModel
                        {
                            Question = q.Value + ":",
                            AnswerHtml = context.HtmlHelper.RatingStars(r.Ratings[q.Key]).ToString()
                        }),
                    PostCommentActionUrl = context.UrlHelper.Action("PostReviewComment", "Reviews")
                });
            this.Admin = context.IsUserInRole("content_manager");
            this.DeleteReviewUrl = this.Admin ? context.UrlHelper.Action("DeleteReview") : null;
            this.DeleteCommentUrl = this.Admin ? context.UrlHelper.Action("DeleteReviewComment") : null;
        }
    }
}