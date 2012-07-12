using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.Models
{
    /// <summary>
    /// Temporary class. Should be removed after migration to MongoDb completed
    /// </summary>
    public class CommunityDto
    {
        public Guid Id
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String UrlId
        {
            get;
            set;
        }

        public bool HasLake
        {
            get;
            set;
        }

        public bool HasWaterFeature
        {
            get;
            set;
        }

        public bool HasClubOfFacility
        {
            get;
            set;
        }

        public bool HasMountainView
        {
            get;
            set;
        }

        public bool HasParksAndPathways
        {
            get;
            set;
        }

        public bool HasShoppingPlaza
        {
            get;
            set;
        }

        public int DistanceToCenter
        {
            get;
            set;
        }

        public String InfoPopup
        {
            get;
            set;
        }

        public String GpsLocation
        {
            get;
            set;
        }

        public String GpsBounds
        {
            get;
            set;
        }

        public String DeleteUrl
        {
            get;
            set;
        }

        public String DetailsUrl
        {
            get;
            set;
        }
    }
}