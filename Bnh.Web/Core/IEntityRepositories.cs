using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Models;
using Bnh.Core.Entities;

namespace Bnh.Core
{
    public interface IEntityRepositories
    {
        IRepository<Community> Communities { get; }

        IRepository<City> Cities { get; }

        /// <summary>
        /// Returns true if given string is valid ID representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsValidId(string id);
    }
}