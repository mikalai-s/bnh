using System.Collections.Specialized;
using System.Linq;
using System.Web.Management;
using System.Web.Mvc;
using Bnh.Core;
using Cms.Core;
using MongoDB.Driver;

namespace MongoDB.Web.Providers
{
    public class MongoDbWebEventProvider : BufferedWebEventProvider
    {
        private MongoCollection mongoCollection;

        public override void Initialize(string name, NameValueCollection settings)
        {
            var config = DependencyResolver.Current.GetService<IConfig>();

            this.mongoCollection = ConnectionUtils.GetCollection(settings, config, "WebEvents");

            settings.Remove("collection");
            settings.Remove("connectionString");
            settings.Remove("database");

            base.Initialize(name, settings);
        }

        public override void ProcessEventFlush(WebEventBufferFlushInfo flushInfo)
        {
            this.mongoCollection.InsertBatch<WebEvent>(flushInfo.Events.Cast<WebBaseEvent>().ToList().ConvertAll<WebEvent>(WebEvent.FromWebBaseEvent));
        }
    }
}