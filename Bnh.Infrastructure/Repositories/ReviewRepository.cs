using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core;
using Bnh.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bnh.Infrastructure.Repositories
{
    public class ReviewRepository : MongoRepository<Review>, IReviewRepository
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
            throw new NotImplementedException();
        }

        public void UpdateComment(string reviewId, Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
