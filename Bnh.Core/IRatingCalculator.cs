using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core
{
    public interface IRatingCalculator
    {
        /// <summary>
        /// Returns rating of entity with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int? GetTargetRating(string id);
    }
}