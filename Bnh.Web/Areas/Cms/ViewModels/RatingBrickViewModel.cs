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
    public class RatingBrickViewModel : BrickViewModel<RatingBrick>
    {
        public MvcHtmlString BigStars { get; private set; }

        public IEnumerable<RatingQuestionViewModel> Ratings { get; private set; }

        public bool Enabled { get; private set; }

        public RatingBrickViewModel(SceneViewModelContext context, RatingBrick brick)
            : base(context, brick)
        {
            this.Enabled = context.Config.Review.RatingEnabled;
            if (!this.Enabled) { return; }

            var reviewable = GetReviewable(context);
            if (reviewable == null)
            {
                this.BigStars = new MvcHtmlString("Not rated yet");
                this.Ratings = Enumerable.Empty<RatingQuestionViewModel>();
                return;
            }

            var reviewableRatings = reviewable.Ratings ?? new Dictionary<string, double?>();

            this.BigStars = new MvcHtmlString(context.HtmlHelper.RatingStars(reviewableRatings.Values.Average()).ToString());

            this.Ratings = from question in context.Config.Review.Questions
                           where reviewableRatings.ContainsKey(question.Key)
                           let answer = reviewableRatings[question.Key]
                           select new RatingQuestionViewModel
                               {
                                   Question = question.Value,
                                   AnswerHtml = context.HtmlHelper.RatingStars(answer).ToString()
                               };
        }

        private IReviewable GetReviewable(SceneViewModelContext context)
        {
            return (context == null)
                ? null
                : context.SceneHolder as IReviewable;
        }
    }
}