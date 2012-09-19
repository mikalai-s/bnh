using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using Newtonsoft.Json;

namespace Bnh.Infrastructure.WebSecurity.Clients
{
    public class Facebook : OAuth2Client
    {
        // Fields
        private readonly string appId;
        private readonly string appSecret;
        private const string AuthorizationEndpoint = "https://www.facebook.com/dialog/oauth";
        private const string TokenEndpoint = "https://graph.facebook.com/oauth/access_token";

        // Methods
        public Facebook(string appId, string appSecret)
            : base("facebook")
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        public override void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            base.RequestAuthentication(context, returnUrl);
        }

        public override DotNetOpenAuth.AspNet.AuthenticationResult VerifyAuthentication(HttpContextBase context, Uri returnPageUrl)
        {
            return base.VerifyAuthentication(context, returnPageUrl);
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            UriBuilder builder = new UriBuilder("https://www.facebook.com/dialog/oauth");
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("client_id", this.appId);
            args.Add("redirect_uri", returnUrl.AbsoluteUri);
            args.Add("scope", "email");
            builder.AppendQueryArgs(args);
            return builder.Uri;
        }


        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            using (WebResponse response = WebRequest.Create("https://graph.facebook.com/me?access_token=" + Ext.EscapeUriDataStringRfc3986(accessToken)).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    var s = new StreamReader(stream).ReadToEnd();
                    var data = JsonConvert.DeserializeObject<FacebookGraphData>(s);
                    return new Dictionary<string, string>
                    {
                        { "id", data.Id },
                        { "name", data.Name },
                        { "email", data.Email }
                    };
                }
            }
        }

        private static string NormalizeHexEncoding(string url)
        {
            char[] chArray = url.ToCharArray();
            for (int i = 0; i < (chArray.Length - 2); i++)
            {
                if (chArray[i] == '%')
                {
                    chArray[i + 1] = char.ToUpperInvariant(chArray[i + 1]);
                    chArray[i + 2] = char.ToUpperInvariant(chArray[i + 2]);
                    i += 2;
                }
            }
            return new string(chArray);
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            UriBuilder builder = new UriBuilder("https://graph.facebook.com/oauth/access_token");
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("client_id", this.appId);
            args.Add("redirect_uri", NormalizeHexEncoding(returnUrl.AbsoluteUri));
            args.Add("client_secret", this.appSecret);
            args.Add("code", authorizationCode);
            builder.AppendQueryArgs(args);
            using (WebClient client = new WebClient())
            {
                string str = client.DownloadString(builder.Uri);
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                return HttpUtility.ParseQueryString(str)["access_token"];
            }
        }

    }

    public static class Ext
    {
        internal static void AppendQueryArgs(this UriBuilder builder, IEnumerable<KeyValuePair<string, string>> args)
        {
            if ((args != null) && (args.Count<KeyValuePair<string, string>>() > 0))
            {
                StringBuilder builder2 = new StringBuilder(50 + (args.Count<KeyValuePair<string, string>>() * 10));
                if (!string.IsNullOrEmpty(builder.Query))
                {
                    builder2.Append(builder.Query.Substring(1));
                    builder2.Append('&');
                }
                builder2.Append(CreateQueryString(args));
                builder.Query = builder2.ToString();
            }
        }

        internal static string CreateQueryString(IEnumerable<KeyValuePair<string, string>> args)
        {
            if (!args.Any<KeyValuePair<string, string>>())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(args.Count<KeyValuePair<string, string>>() * 10);
            foreach (KeyValuePair<string, string> pair in args)
            {
                builder.Append(EscapeUriDataStringRfc3986(pair.Key));
                builder.Append('=');
                builder.Append(EscapeUriDataStringRfc3986(pair.Value));
                builder.Append('&');
            }
            builder.Length--;
            return builder.ToString();
        }



        static string[] UriRfc3986CharsToEscape = new string[] { "!", "*", "'", "(", ")" };



        internal static string EscapeUriDataStringRfc3986(string value)
        {
            StringBuilder builder = new StringBuilder(Uri.EscapeDataString(value));
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                builder.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }
            return builder.ToString();
        }




    }
}