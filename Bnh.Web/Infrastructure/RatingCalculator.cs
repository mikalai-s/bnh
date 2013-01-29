using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core;
using Bnh.Core.Entities;
using Bnh.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Bnh.Infrastructure
{
    public class RatingCalculator : IRatingCalculator
    {
        Config config = null;
        IEntityRepositories repos = null;

        public RatingCalculator(Config config, IEntityRepositories repos)
        {
            this.config = config;
            this.repos = repos;
        }

        public double? GetTargetRating(string id)
        {
//            var reviews = this.repos.Reviews as MongoRepository<Review>;
//            if (!reviews.Database.CollectionExists(reviews.CollectionName)) { return null;  }

//            var map =
//@"function Map() {
//    emit(
//        this.TargetId,
//        {count: 0, ratings: this.Ratings}
//    );
//}";

//            var reduce =
//@"function Reduce(key, values) {
//    var reduced = {count: 0, ratings: {}}, r;
//    values.forEach(function (val) {
//        for (r in val.ratings) {
//            if (val.ratings.hasOwnProperty(r) && (val.ratings[r] === 0 || val.ratings[r] > 0)) {
//                if(!reduced.ratings[r]) {
//                    reduced.ratings[r] = 0;
//                }
//                reduced.ratings[r] += val.ratings[r];
//                reduced.count += 1;
//            }
//        }
//    });
//    return reduced;
//}";
            
//            var results = reviews.Collection.MapReduce(Query.EQ("TargetId", BsonValue.Create(ObjectId.Parse(id as string))), map, reduce);
//            var result = results.GetResultsAs<Result>().FirstOrDefault();
//            if (result == null) return null; // no rating yet

//            // NOTE: Reduce function is not getting executed when there is only one element in map
//            // that's why we have additional counter to handle such situation
//            var rating = 0.0;
//            var count = 0;
//            foreach (var rate in result.value.ratings.Values.Where(v => v.HasValue).Select(v => v.Value))
//            {
//                rating += rate;
//                count++;
//            }
//            if (result.value.count == 0 && count == 0) return null;

//            return rating / ((result.value.count > 0) ? (double)result.value.count : (double)count);
            return null;
        }

        /// <summary>
        ///  
        /// 
        /// </summary>
        internal class Result
        {
            public object _id { get; set; }
            public ResultValue value { get; set; }

            public class ResultValue
            {
                public int count { get; set; }
                public IDictionary<string, double?> ratings { get; set; }
            }
        }
    }
}
