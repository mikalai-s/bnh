using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Cms.ViewModels
{
    public class ReviewViewModel
    {
        private readonly ViewModelContext context;

        public string ReviewId { get; set; }

        public string TargetId { get; set; }

        public string UserName { get; set; }

        public string UserAvatarSrc { get; set; }

        public IEnumerable<RatingQuestionViewModel> Ratings { get; set; }

        public string Message { get; set; }

        public string Created { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public string PostCommentActionUrl { get; set; }

        public bool RatingEnabled { get; private set; }

        public ReviewViewModel()
        {
        }

        public ReviewViewModel(ViewModelContext context, string targetId)
        {
            this.context = context;
            this.TargetId = targetId;
            this.RatingEnabled = context.Config.Review.RatingEnabled;
        }
    }
}