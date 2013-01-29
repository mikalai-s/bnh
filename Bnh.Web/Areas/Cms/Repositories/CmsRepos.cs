using System;
using System.Linq;

using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Bnh.Core;
using Bnh.Infrastructure.Repositories;
using Bnh.Cms.Models;

namespace Bnh.Cms.Repositories
{
    public partial class CmsRepos
    {
        public MongoRepository<Scene> Scenes { get; private set; }
        public MongoRepository<BrickContent> BrickContents { get; private set; }
        public ReviewRepository Reviews { get; private set; }

        public CmsRepos(Config config) 
        {
            this.Scenes = new MongoRepository<Scene>(config.ConnectionStrings["cms"]);
            this.BrickContents = new MongoRepository<BrickContent>(config.ConnectionStrings["cms"]);
            this.Reviews = new ReviewRepository(config.ConnectionStrings["cms"]);

            InitData.Init(this);
        }
    }
}
