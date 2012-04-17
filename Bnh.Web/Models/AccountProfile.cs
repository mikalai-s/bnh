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
        public static AccountProfile Current
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                {
                    var profile = GetProfile(HttpContext.Current.User.Identity.Name);
                    profile.FirstName = HttpContext.Current.User.Identity.Name;
                    profile.Save();
                    return profile;
                }

                return GetProfile(user.UserName);
            }
        }
       
        public string FirstName
        {
            get { return ((string)(base["FirstName"])); }
            set { base["FirstName"] = value; }
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

        public virtual string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public static AccountProfile GetProfile(string username)
        {
            return Create(username) as AccountProfile;
        }


        public enum GenderEnum
        {
            Male = 0,
            Female = 1
        }
    }

    public class EmptyAccountProfile : AccountProfile
    {
        public override string FullName
        {
            get { return "[No Profile!]"; }
        }
    }
}