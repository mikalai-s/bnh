using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    /// <summary>
    /// Adds review brick support to an entity
    /// </summary>
    public interface IReviewable
    {
        /// <summary>
        /// Id of review target
        /// </summary>
        string ReviewableTargetId { get; }
    }
}