using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Core
{
    public interface IRatingCalculator
    {
        /// <summary>
        /// Returns rating of entity with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        double? GetTargetRating(string id);

        /// <summary>
        /// Returns map of reviewable entity's ID and it's overage rating on a particular question.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="questionKey"></param>
        /// <returns></returns>
        IDictionary<string, double?> GetTargetRatingMap(IEnumerable<string> ids, string questionKey);
    }
}