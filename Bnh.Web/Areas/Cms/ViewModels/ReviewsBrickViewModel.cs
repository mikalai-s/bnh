using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.Models;
using Microsoft.Web.Helpers;
using Cms.Helpers;

namespace Cms.ViewModels
{
    public class ReviewsBrickViewModel : BrickViewModel<ReviewsBrick>
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public LinkViewModel AddReviewLink { get; set; }

        public bool Admin { get; set; }

        public string DeleteReviewUrl { get; set; }

        public string DeleteCommentUrl { get; set; }

        public bool RatingEnabled { get; private set; }

        public ReviewsBrickViewModel(SceneViewModelContext context, ReviewsBrick content)
            : base(context, content)
        {
            this.RatingEnabled = context.Config.Review.RatingEnabled;

            var reviewable = GetReviewable(context);
            if (reviewable == null)
            {
                this.Reviews = Enumerable.Empty<ReviewViewModel>();
            }
            else
            {
                var userProfiles = context.Repos.Profiles.ToDictionary(p => p.UserName, p => p);

                this.Reviews = context.Repos.Reviews
                    .Where(r => r.TargetId == reviewable.ReviewableTargetId)
                    .OrderByDescending(r => r.Created)
                    .Select(r => new ReviewViewModel
                    {
                        ReviewId = r.ReviewId,
                        UserName = userProfiles[r.UserName].DisplayName,
                        UserAvatarSrc = context.HtmlHelper.Avatar(userProfiles[r.UserName].GravatarEmail, 64).ToString(),
                        Created = r.Created.ToLocalTime().ToUserFriendlyString(),
                        Message = r.Message,
                        Comments = (r.Comments ?? Enumerable.Empty<Comment>())
                            .OrderBy(c => c.Created)
                            .Select(c => new CommentViewModel(c, userProfiles[c.UserName])),
                        Ratings = context.Config.Review.RatingEnabled
                            ? context.Config.Review.Questions
                                .Where(q => r.Ratings[q.Key].HasValue)
                                .Select(q => new RatingQuestionViewModel
                                {
                                    Question = q.Value + ":",
                                    AnswerHtml = context.HtmlHelper.RatingStars(r.Ratings[q.Key]).ToString()
                                })
                            : Enumerable.Empty<RatingQuestionViewModel>(),
                        PostCommentActionUrl = context.UrlHelper.Action("PostReviewComment", "Reviews")
                    });

                this.AddReviewLink = new LinkViewModel
                {
                    Text = "Write a review",
                    Href = context.UrlHelper.Action("AddReview", new { id = reviewable.ReviewableTargetId })
                };

                this.Admin = context.IsUserInRole(context.Config.Roles["ContentManager"]);
                this.DeleteReviewUrl = this.Admin ? context.UrlHelper.Action("DeleteReview", "Reviews") : null;
                this.DeleteCommentUrl = this.Admin ? context.UrlHelper.Action("DeleteReviewComment", "Reviews") : null;
            }
        }

        private IReviewable GetReviewable(SceneViewModelContext context)
        {
            return (context == null)
                ? null
                : context.SceneHolder as IReviewable;
        }
    }
}