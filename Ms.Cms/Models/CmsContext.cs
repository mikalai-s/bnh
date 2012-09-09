using System;
using System.Linq;

using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Bnh.Core;
using Bnh.Infrastructure.Repositories;

namespace Ms.Cms.Models
{
    public partial class CmsEntities
    {
        public SceneRepository Scenes { get; private set; }
        public MongoRepository<BrickContent> BrickContents { get; private set; }

        public CmsEntities(Config config) 
        {
            this.Scenes = new SceneRepository(config.ConnectionStrings["cms"]);
            this.BrickContents = new MongoRepository<BrickContent>(config.ConnectionStrings["cms"]);

            InitData.Init(this);
        }
    }
}
