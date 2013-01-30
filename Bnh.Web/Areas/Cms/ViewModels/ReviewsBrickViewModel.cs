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

        public ReviewsBrickViewModel(ViewModelContext context, string title, float width, string brickContentId, ReviewsContent content, IRepositories repos)
            : base(context, title, width, brickContentId, content, repos)
        {
            var reviewable = context.ViewBag.GlobalModel as IReviewable;
            if (reviewable == null)
            {
                this.Reviews = Enumerable.Empty<ReviewViewModel>();
            }
            else
            {
                this.Reviews = repos.Reviews
                    .Where(r => r.TargetId == reviewable.ReviewableTargetId)
                    .Select(r => new ReviewViewModel
                    {
                        ReviewId = r.ReviewId,
                        UserName = "test",
                        UserAvatarSrc = context.HtmlHelper.Avatar("msilivonik@gmail.com", 64).ToString(),
                        Created = r.Created.ToLocalTime().ToUserFriendlyString(),
                        Message = r.Message,
                        Comments = (r.Comments ?? Enumerable.Empty<Comment>())
                            .Select(c => new CommentViewModel(c)),
                        PostCommentActionUrl = context.UrlHelper.Action("PostReviewComment")
                    });
            }
        }
    }
}