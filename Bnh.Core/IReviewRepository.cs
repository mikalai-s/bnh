using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core.Entities;

namespace Bnh.Core
{
    public interface IReviewRepository : IRepository<Review>
    {
        void AddReviewComment(string reviewId, Comment comment);
        void DeleteReviewComment(string reviewId, string commentId);
        void UpdateComment(string reviewId, Comment comment);
    }
}
