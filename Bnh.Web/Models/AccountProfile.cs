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
        public string DisplayName
        {
            get { return (string)base.GetPropertyValue("DisplayName"); }
            set { base.SetPropertyValue("DisplayName", value); }
        }

        public string RealName
        {
            get { return (string)base["RealName"]; }
            set { base["RealName"] = value; }
        }

        public string Location
        {
            get { return (string)base["Location"]; }
            set { base["Location"] = value; }
        }

        public string GravatarEmail
        {
            get { return (string)base["GravatarEmail"]; }
            set { base["GravatarEmail"] = value; }
        }

        public string GetSafeName()
        {
            if (!string.IsNullOrEmpty(this.RealName)) return this.RealName;
            if (!string.IsNullOrEmpty(this.DisplayName)) return this.DisplayName;
            return this.UserName;
        }

        public override void Save()
        {
            if (this.GravatarEmail.IsEmpty())
            {
                this.GravatarEmail = this.UserName;
            }
            base.Save();
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
    }
}