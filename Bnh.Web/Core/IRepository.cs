using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core
{
    public interface IRepository<T> : IQueryable<T> where T : class
    {
        void Insert(T item);
        void InsertBatch(IEnumerable<T> items);
        void Save(T item);

        long Count();

        T Find(string id);

        IEnumerable<T> List();

        void Delete(string id);
    }
}