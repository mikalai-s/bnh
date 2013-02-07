﻿using System;
using System.Linq;

using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Bnh.Core;

using Cms.Models;
using Cms.Core;

namespace Cms.Infrastructure
{
    public class Repositories : IRepositories
    {
        public MongoRepository<Scene> Scenes { get; private set; }
        public MongoRepository<BrickContent> BrickContents { get; private set; }
        public ReviewRepository Reviews { get; private set; }
        public MongoRepository<Profile> Profiles { get; private set; }
        public MongoRepository<Comment> Feedback { get; private set; }

        public Repositories(IConfig config) 
        {
            var connectionString = config.ConnectionStrings["cms"];
            this.Scenes = new MongoRepository<Scene>(connectionString);
            this.BrickContents = new MongoRepository<BrickContent>(connectionString);
            this.Reviews = new ReviewRepository(connectionString);
            this.Profiles = new MongoRepository<Profile>(connectionString, "Profiles");
            this.Feedback = new MongoRepository<Comment>(connectionString, "Feedback");

            InitData.Init(this);
        }
    }
}