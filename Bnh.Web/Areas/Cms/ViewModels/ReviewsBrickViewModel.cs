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
    public class ReviewsBrickViewModel : BrickViewModel<ReviewsContent>
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public ReviewsBrickViewModel(ViewModelContext context, string title, float width, string brickContentId, ReviewsContent content)
            : base(context, title, width, brickContentId, content)
        {
            var reviewable = context.ViewBag.GlobalModel as IReviewable;
            if (reviewable == null)
            {
                this.Reviews = Enumerable.Empty<ReviewViewModel>();
            }
            else
            {
                var userProfiles = context.Repos.Profiles.ToDictionary(p => p.UserName, p => p);

                this.Reviews = context.Repos.Reviews
                    .Where(r => r.TargetId == reviewable.ReviewableTargetId)
                    .Select(r => new ReviewViewModel
                    {
                        ReviewId = r.ReviewId,
                        UserName = userProfiles[r.UserName].DisplayName,
                        UserAvatarSrc = context.HtmlHelper.Avatar(userProfiles[r.UserName].GravatarEmail, 64).ToString(),
                        Created = r.Created.ToLocalTime().ToUserFriendlyString(),
                        Message = r.Message,
                        Comments = (r.Comments ?? Enumerable.Empty<Comment>())
                            .Select(c => new CommentViewModel(c, userProfiles[c.UserName])),
                        Ratings = context.Config.Review.Questions
                            .Where(q => r.Ratings[q.Key].HasValue)
                            .Select(q => new RatingQuestionViewModel
                            {
                                Question = q.Value + ":",
                                AnswerHtml = context.HtmlHelper.RatingStars(r.Ratings[q.Key]).ToString()
                            }),
                        PostCommentActionUrl = context.UrlHelper.Action("PostReviewComment")
                    });
            }
        }
    }
}