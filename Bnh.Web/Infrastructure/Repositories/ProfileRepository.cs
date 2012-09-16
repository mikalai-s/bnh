using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Core.Entities;

namespace Bnh.Infrastructure.Repositories
{
    public class ProfileRepository : MongoRepository<Profile>
    {
        public ProfileRepository(string connectionString)
            : base(connectionString)
        {
        }

        public override string CollectionName
        {
            get { return "Profiles"; }
        }
    }
}