using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Core.Entities
{
    public class Review
    {
        public string ReviewId { get; set; }

        public string TargetId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public IDictionary<string, int?> Ratings { get; set; }

        public DateTime Created { get; set; }
    }
}