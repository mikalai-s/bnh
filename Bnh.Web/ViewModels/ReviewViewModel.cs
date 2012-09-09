using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.Web.ViewModels
{
    public class ReviewViewModel
    {
        public string ReviewId { get; set; }

        public string UserName { get; set; }

        public string UserAvatarSrc { get; set; }

        public IEnumerable<RatingQuestionViewModel> Ratings { get; set; }

        public string Message { get; set; }

        public string Created { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public string PostCommentActionUrl { get; set; }
    }
}