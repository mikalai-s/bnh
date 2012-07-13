using System;
using System.Collections.Generic;

namespace Bnh.Web.Models
{
    /// <summary>
    /// Temporary class. Should be removed after migration to MongoDb completed
    /// </summary>
    public class ZoneDto
    {
        public String Name
        {
            get;
            set;
        }

        public IEnumerable<CommunityDto> Communities
        {
            get;
            set;
        }
    }
}