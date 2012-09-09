using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core.Entities;
using Microsoft.Web.Helpers;

namespace Bnh.Web.ViewModels
{
    public class CommentViewModel
    {
        public string CommentId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public string UserAvatarSrc { get; set; }

        public string Created { get; set; }

        public CommentViewModel(Comment comment)
        {
            this.CommentId = comment.CommentId;
            this.UserName = comment.UserName;
            this.UserAvatarSrc = Gravatar.GetUrl(comment.UserName, 32) + "&d=identicon";
            this.Created = comment.Created.ToLocalTime().ToUserFriendlyString();
            this.Message = comment.Message;
        }
    }
}