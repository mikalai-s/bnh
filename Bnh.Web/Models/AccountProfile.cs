using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace Bnh.Web.Models
{
    public class AccountProfile : ProfileBase
    {       
        public string FirstName
        {
            get { return (string)base.GetPropertyValue("FirstName"); }
            set { base.SetPropertyValue("FirstName", value); }
        }

        public string LastName
        {
            get { return ((string)(base["LastName"])); }
            set { base["LastName"] = value; }
        }

        public DateTime Birthday
        {
            get { return ((DateTime)(base["Birthday"])); }
            set { base["Birthday"] = value; }
        }
       
        public GenderEnum Gender
        {
            get { return ((GenderEnum)(base["Gender"])); }
            set { base["Gender"] = value; }
        }

        public string GetFullName()
        {
            var fullName = (this.FirstName + " " + this.LastName);
            return string.IsNullOrWhiteSpace(fullName) ? this.UserName : fullName;
        }

        /// <summary>
        /// Get the profile of the currently logged-on user.
        /// </summary>     
        public static AccountProfile GetProfile()
        {
            return (AccountProfile)HttpContext.Current.Profile;
        }
 
        /// <summary>
        /// Gets the profile of a specific user.
        /// </summary>
        /// <param name="userName">The user name of the user whose profile you want to retrieve.</param>
        public static AccountProfile GetProfile(string userName)
        {
            return (AccountProfile) Create(userName);
        }

        public enum GenderEnum
        {
            Male = 0,
            Female = 1
        }
    }
}