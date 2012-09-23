using System;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using MongoDB.Web.Providers;

namespace Bnh.Infrastructure.WebSecurity
{
    internal class WebPagesOAuthDataProvider : IOpenAuthDataProvider
    {
        public string GetUserNameFromOpenAuth(string openAuthProvider, string openAuthId)
        {
            var provider = Membership.Provider as MongoDbMembershipProvider;

            var userId = provider.GetUserIdFromOAuth(openAuthProvider, openAuthId);
            return provider.GetUserNameFromId(userId);
        }
    }
}