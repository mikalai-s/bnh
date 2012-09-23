using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;
using MongoDB.Bson;
using MongoDB.Web.Providers;

namespace Bnh.Infrastructure.WebSecurity
{
    public static class WebSecurity
    {
        private static MongoDbMembershipProvider Provider
        {
            get
            {
                var provider = Membership.Provider as MongoDbMembershipProvider;
                if (provider == null)
                {
                    throw new InvalidOperationException("WebDataResources.Security_NoExtendedMembershipProvider");
                }
                return provider;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Login is used more consistently in ASP.Net")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This is a helper class, and we are not removing optional parameters from methods in helper classes")]
        public static bool Login(string userName, string password, bool persistCookie = false)
        {
            bool success = Membership.ValidateUser(userName, password);
            if (success)
            {
                FormsAuthentication.SetAuthCookie(userName, persistCookie);
            }
            return success;
        }


        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "Login is used more consistently in ASP.Net")]
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }


        public static void CreateUser(string userName, string password)
        {
            var status = MembershipCreateStatus.ProviderError;
            Provider.CreateUser(userName, password, userName, null, null, true, null, out status);
        }


        public static ObjectId GetUserId(string userName)
        {
            var user = Membership.GetUser(userName);
            if (user == null)
            {
                return ObjectId.Empty;
            }
            return (ObjectId)user.ProviderUserKey;
        }


        public static bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            bool success = false;
            try
            {
                var currentUser = Membership.GetUser(userName, true /* userIsOnline */);
                success = currentUser.ChangePassword(currentPassword, newPassword);
            }
            catch (ArgumentException)
            {
                // An argument exception is thrown if the new password does not meet the provider's requirements
            }
            return success;
        }


        /*

        public static bool ConfirmAccount(string accountConfirmationToken)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this
            return provider.ConfirmAccount(accountConfirmationToken);
        }

        public static bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this
            return provider.ConfirmAccount(userName, accountConfirmationToken);
        }

        

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This is a helper class, and we are not removing optional parameters from methods in helper classes")]
        public static string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            var provider = VerifyProvider();
            var status = MembershipCreateStatus.ProviderError;
            var result = provider.CreateUser(userName, password, userName, null, null, true, null, out status);
            if (requireConfirmationToken)
            {
                provider.SetRequireConfirmationToken(userName);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This is a helper class, and we are not removing optional parameters from methods in helper classes")]
        public static string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow);
        }

        public static bool UserExists(string userName)
        {
            VerifyProvider();
            return Membership.GetUser(userName) != null;
        }

        

        public static ObjectId GetUserIdFromPasswordResetToken(string token)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GetUserIdFromPasswordResetToken(token);
        }

        public static bool IsCurrentUser(string userName)
        {
            VerifyProvider();
            return String.Equals(CurrentUserName, userName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsConfirmed(string userName)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.IsConfirmed(userName);
        }

        // Make sure the logged on user is same as the one specified by the id
        private static bool IsUserLoggedOn(ObjectId userId)
        {
            VerifyProvider();
            return CurrentUserId == userId;
        }

        // Make sure the user was authenticated
        public static void RequireAuthenticatedUser()
        {
            VerifyProvider();
            var user = Context.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        // Make sure the user was authenticated
        public static void RequireUser(ObjectId userId)
        {
            VerifyProvider();
            if (!IsUserLoggedOn(userId))
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        public static void RequireUser(string userName)
        {
            VerifyProvider();
            if (!String.Equals(CurrentUserName, userName, StringComparison.OrdinalIgnoreCase))
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        public static void RequireRoles(params string[] roles)
        {
            VerifyProvider();
            foreach (string role in roles)
            {
                if (!Roles.IsUserInRole(CurrentUserName, role))
                {
                    Response.SetStatus(HttpStatusCode.Unauthorized);
                    return;
                }
            }
        }

        public static bool ResetPassword(string passwordResetToken, string newPassword)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this
            return provider.ResetPasswordWithToken(passwordResetToken, newPassword);
        }

        public static bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds)
        {
            VerifyProvider();
            return IsAccountLockedOut(userName, allowedPasswordAttempts, TimeSpan.FromSeconds(intervalInSeconds));
        }

        public static bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return IsAccountLockedOutInternal(provider, userName, allowedPasswordAttempts, interval);
        }

        internal static bool IsAccountLockedOutInternal(MongoDbMembershipProvider provider, string userName, int allowedPasswordAttempts, TimeSpan interval)
        {
            return (provider.GetUser(userName, false) != null &&
                    provider.GetPasswordFailuresSinceLastSuccess(userName) > allowedPasswordAttempts &&
                    provider.GetLastPasswordFailureDate(userName).Add(interval) > DateTime.UtcNow);
        }

        public static int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GetPasswordFailuresSinceLastSuccess(userName);
        }

        public static DateTime GetCreateDate(string userName)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GetCreateDate(userName);
        }

        public static DateTime GetPasswordChangedDate(string userName)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GetPasswordChangedDate(userName);
        }

        public static DateTime GetLastPasswordFailureDate(string userName)
        {
            var provider = VerifyProvider();
            Debug.Assert(provider != null); // VerifyProvider checks this

            return provider.GetLastPasswordFailureDate(userName);
        }
         */
    }
}
