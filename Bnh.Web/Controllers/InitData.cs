using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Bnh.Web.Models;
using MongoDB.Driver;
using System.Configuration;

namespace Bnh.Web.Controllers
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
            userProfile.Gender = AccountProfile.GenderEnum.Male;
            userProfile.FirstName = "Bob";
            userProfile.LastName = "Man";
            userProfile.Birthday = new DateTime(1969, 1, 6);
            userProfile.Save();


            var admin = Membership.CreateUser("admin@bnh.com", "1", "admin@bnh.com");
            var adminProfile = AccountProfile.GetProfile(admin.UserName);
            adminProfile.Gender = AccountProfile.GenderEnum.Female;
            adminProfile.FirstName = "Alice";
            adminProfile.LastName = "Baker";
            adminProfile.Birthday = new DateTime(1979, 5, 16);
            adminProfile.Save();


            Roles.CreateRole("content_manager");
            Roles.AddUserToRole(admin.UserName, "content_manager");


        }
    }
}