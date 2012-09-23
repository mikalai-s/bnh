using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace Bnh.Infrastructure.WebSecurity.Clients
{
    public sealed class Yahoo : OpenIdClient
    {
        // Methods
        public Yahoo()
            : base("yahoo", WellKnownProviders.Yahoo)
        {
        }

        protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response)
        {
            FetchResponse extension = response.GetExtension<FetchResponse>();
            if (extension != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("email", extension.GetAttributeValue("http://axschema.org/contact/email"));
                dictionary.Add("name", extension.GetAttributeValue("http://axschema.org/namePerson"));
                return dictionary;
            }
            return null;
        }

        protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request)
        {
            FetchRequest extension = new FetchRequest();
            extension.Attributes.AddRequired("http://axschema.org/contact/email");
            extension.Attributes.AddRequired("http://axschema.org/namePerson");
            request.AddExtension(extension);
        }
    }
}