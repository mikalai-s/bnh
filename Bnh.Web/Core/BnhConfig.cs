using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Cms.Core;

namespace Bnh.Core
{
    public class BnhConfig : IBnhConfig
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public string Host { get; set; }
        public IDictionary<string, IDictionary<string, string>> Authentification { get; set; }
        public string City { get; set; }
        public ReviewConfig Review { get; set; }
        public string SearchIndexFolder { get; set; }
        public IDictionary<string, string> Roles { get; set; }

        /// <summary>
        /// Checks whether given request is in expected host
        /// </summary>
        /// <param name="http"></param>
        /// <returns></returns>
        public bool IsValidHost(HttpContextBase http)
        {
            if (http == null)
                throw new ArgumentNullException("http");

            return this.Host.IsEmpty()
                ? true
                : this.Host == http.Request.ServerVariables["HTTP_HOST"];
        }

        public static bool IsStaging { get { return (Activator == ActivatorType.Staging); } }
        public static bool IsProduction { get { return (Activator == ActivatorType.Production); } }
        public static bool IsDebug { get { return (Activator == ActivatorType.Debug); } }

        public static ActivatorType Activator
        {
            get
            {
#if STAGING
                return ActivatorType.Staging;
#elif PRODUCTION
                return ActivatorType.Production;
#else
                return ActivatorType.Debug;
#endif
            }
        }
    }

    public enum ActivatorType
    {
        Debug,
        Production,
        Staging
    }
}