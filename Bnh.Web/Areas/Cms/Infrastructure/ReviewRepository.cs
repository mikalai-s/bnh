using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cms.Models;
using Bnh.Core;
using Bnh.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Cms.Infrastructure
{
    public class ReviewRepository : MongoRepository<Review>
    {
        public ReviewRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddReviewComment(string reviewId, Comment comment)
        {
            comment.CommentId = ObjectId.GenerateNewId().ToString();

            var idQuery = Query.EQ("_id", CastId(reviewId));
            var count = this.Collection.Count(Query.And(idQuery, Query.EQ("Comments", BsonNull.Value)));
            if (count == 1)
            {
                // comments array is null or doesn't exist - create one
                this.Collection.Update(
                    Query.EQ("_id", CastId(reviewId)),
                    Update.SetWrapped("Comments", new[] { comment }));
            }
            else
            {
                // push new comment
                this.Collection.Update(
                    Query.EQ("_id", CastId(reviewId)),
                    Update.Push("Comments", BsonDocumentWrapper.Create(comment)));
            }
        }

        public void DeleteReviewComment(string reviewId, string commentId)
        {
            var reviewIdQuery = Query.EQ("_id", CastId(reviewId));
            var commentIdQuery = Query.EQ("_id", CastId(commentId));

            var r = this.Collection.Update(reviewIdQuery, Update.Pull("Comments", commentIdQuery));
        }

        public void UpdateComment(string reviewId, Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
