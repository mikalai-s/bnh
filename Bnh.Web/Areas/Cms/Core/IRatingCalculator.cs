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
    }
}