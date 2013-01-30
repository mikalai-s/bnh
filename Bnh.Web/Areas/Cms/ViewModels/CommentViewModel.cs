using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;
using Bnh.Core;
using Bnh.Core.Entities;
using Cms.Helpers;
using Microsoft.Web.Helpers;

namespace Cms.ViewModels
{
    public class CommentViewModel
    {
        public string CommentId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public string UserAvatarSrc { get; set; }

        public string Created { get; set; }

        //public CommentViewModel(Comment comment, IEntityRepositories repos) 
        //    : this(comment, repos.Profiles.First(p => p.UserName == comment.UserName))
        //{
        //}

        public CommentViewModel(Comment comment, Profile user) 
            : this(comment)
        {
            this.UserName = user.DisplayName;
            this.UserAvatarSrc = Gravatar.GetUrl(user.GravatarEmail, 32) + "&d=identicon";
        }

        public CommentViewModel(Comment comment)
        {
            this.CommentId = comment.CommentId;
            this.UserName = comment.UserName;
            this.Created = comment.Created.ToLocalTime().ToUserFriendlyString();
            this.Message = comment.Message;
        }
    }
}