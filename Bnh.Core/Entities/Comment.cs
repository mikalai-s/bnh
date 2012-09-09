using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Core.Entities
{
    public class Comment
    {
        public string CommentId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }
    }
}
