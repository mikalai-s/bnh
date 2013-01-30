using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    public class Profile : IAccountProfile
    {
        public string ProfileId { get; set; }

        public string ApplicationName { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string RealName { get; set; }

        public string Location { get; set; }

        public string GravatarEmail { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public DateTime LastActivityDate { get; set; }
    }
}