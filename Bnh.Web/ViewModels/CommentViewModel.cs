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
        public string UserName { get; set; }

        public string Message { get; set; }

        public string UserAvatarSrc { get; set; }

        public string Created { get; set; }

        public CommentViewModel(Comment c)
        {
            this.UserName = c.UserName;
            this.UserAvatarSrc = Gravatar.GetUrl(c.UserName, 32) + "&d=identicon";
            this.Created = c.Created.ToLocalTime().ToUserFriendlyString();
            this.Message = c.Message;
        }
    }
}