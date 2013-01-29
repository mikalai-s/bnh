using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Cms.Models
{
    public class Review
    {
        public string ReviewId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }

        public string TargetId { get; set; }

        public IDictionary<string, double?> Ratings { get; set; }

        public Comment[] Comments { get; set; }

        public int HelpfulCount { get; set; }


        /// <summary>
        /// Returns participants usernames
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetParticipants()
        {
            yield return this.UserName;

            if (this.Comments == null || this.Comments.Length == 0)
            {
                yield break;
            }

            foreach (var comment in this.Comments)
            {
                yield return comment.UserName;
            }
        }
    }
}