using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;
using Cms.ViewModels;
using Cms.Helpers;

namespace Cms.ViewModels
{
    public class RatingViewModel : BrickViewModel<RatingContent>
    {
        public MvcHtmlString BigStars { get; private set; }

        public IEnumerable<RatingQuestionViewModel> Ratings { get; private set; }

        public RatingViewModel(ViewModelContext context, string title, float width, string brickContentId, RatingContent content)
            : base(context, title, width, brickContentId, content)
        {
            var reviewable = context.ViewBag.GlobalModel as IReviewable;
            if (reviewable == null)
            {
                this.BigStars = new MvcHtmlString("Not rated yet");
                this.Ratings = Enumerable.Empty<RatingQuestionViewModel>();
                return;
            }


            this.BigStars = new MvcHtmlString(context.HtmlHelper.RatingStars(context.RatingCalculator.GetTargetRating(reviewable.ReviewableTargetId)).ToString());

            var answers = context.Repos.Reviews.Where(r => r.TargetId == reviewable.ReviewableTargetId).Select(r => r.Ratings).ToList();
            this.Ratings = from question in  context.Config.Review.Questions
                           let answer = answers.Average(a => a[question.Key])
                           select new RatingQuestionViewModel
                               {
                                   Question = question.Value,
                                   AnswerHtml = context.HtmlHelper.RatingStars(answer).ToString()
                               };
        }
    }
}