using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Bnh.Models;
using System.Configuration;

namespace Bnh.Controllers
{
    public static class InitData
    {
        public static void Init()
        {
            // simple check whether there is need to initialize database data
            if (Membership.GetUserNameByEmail("admin@bnh.com") == "admin@bnh.com")
                return;

            var user = Membership.CreateUser("user@bnh.com", "1", "user@bnh.com");
            var userProfile = AccountProfile.GetProfile(user.UserName);
            userProfile.DisplayName = "Bob Man";
            userProfile.RealName = "Bob Man";
            userProfile.GravatarEmail = "user@bnh.com";
            userProfile.Save();

            var admin = Membership.CreateUser("admin@bnh.com", "1", "admin@bnh.com");
            var adminProfile = AccountProfile.GetProfile(admin.UserName);
            adminProfile.DisplayName = "Alice Baker";
            adminProfile.RealName = "Alice Baker";
            adminProfile.GravatarEmail = "admin@bnh.com";
            adminProfile.Save();

            Roles.CreateRole("content_manager");
            Roles.AddUserToRole(admin.UserName, "content_manager");
        }
    }
}