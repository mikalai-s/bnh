using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Models;
using Bnh.Core.Entities;
using Cms.Core;
using Cms.Infrastructure;

namespace Bnh.Core
{
    public interface IBnhRepositories : IRepositories
    {
        MongoRepository<Community> Communities { get; }

        MongoRepository<City> Cities { get; }

        /// <summary>
        /// Returns true if given string is valid ID representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsValidId(string id);
    }
}