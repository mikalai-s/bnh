using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace Ms.Cms.Models
{
    public interface IRepository<in TId, T> where T : class
    {
        MongoCollection<T> Collection { get; }

        void Insert(T item);
        void InsertBatch(IEnumerable<T> items);
        void Save(T item);

        long Count();

        T Find(TId id);

        IEnumerable<T> List();

        void Delete(TId id);
    }
}