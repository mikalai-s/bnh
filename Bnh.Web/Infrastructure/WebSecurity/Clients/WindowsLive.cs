using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bnh.Infrastructure.WebSecurity.Clients
{
    public class WindowsLive : MicrosoftClient
    {
        private readonly string clientId;
        // Methods
        public WindowsLive(string clientId, string clientSecret)
            : base(clientId, clientSecret)
        {
            this.clientId = clientId;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            UriBuilder builder = new UriBuilder("https://oauth.live.com/authorize");
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("client_id", this.clientId);
            args.Add("scope", "wl.emails");
            args.Add("response_type", "code");
#if DEBUG
            args.Add("redirect_uri", (new Uri(returnUrl.AbsoluteUri.Replace("localhost", "mstestdomain.com"))).AbsoluteUri);
#else
            args.Add("redirect_uri", returnUrl.AbsoluteUri);
#endif
            builder.AppendQueryArgs(args);
            return builder.Uri;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            using (WebResponse response = WebRequest.Create("https://apis.live.net/v5.0/me?access_token=" + Ext.EscapeUriDataStringRfc3986(accessToken)).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    var s = new StreamReader(stream).ReadToEnd();
                    var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(s);

                    return new Dictionary<string, string>
                    {
                        { "id", (string)data["id"] },
                        { "name", (string)data["name"] },
                        { "email", GetEmail(data["emails"] as IDictionary<string, JToken>) }
                    };
                }
            }
        }

        private string GetEmail(IDictionary<string, JToken> emails)
        {
            var email = (string)emails["preferred"];
            if(string.IsNullOrEmpty(email))
            {
                return (string)emails["account"];
            }
            return email;
        }
    }
}