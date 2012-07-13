using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Linq.Expressions;
using System.Collections;

namespace Ms.Cms.Models
{
    public class MongoRepository<TId, T> : IRepository<TId, T>, IQueryable<T> where T : class
    {
        private readonly string _connectionString;

        public MongoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private MongoCollection<T> _collection;

        public MongoCollection<T> Collection
        {
            get { return _collection ?? (_collection = MongoDatabase.Create(_connectionString).GetCollection<T>(typeof(T).Name)); }
        }

        public virtual void Insert(T item)
        {
            try
            {
                Collection.Insert(item);
            }
            catch (MongoSafeModeException ex)
            {
                if (!ex.Message.Contains("E11000"))
                    throw;
            }
        }

        public virtual void InsertBatch(IEnumerable<T> items)
        {
            Collection.InsertBatch(items);
        }

        public virtual void Save(T item)
        {
            Collection.Save(item);
        }

        public virtual long Count()
        {
            var count = Collection.Count();
            return count;
        }

        protected virtual BsonValue CastId(TId id)
        {
            if (id is string)
            {
                return BsonValue.Create(ObjectId.Parse(id as string));
            }
            return BsonValue.Create(id);
        }

        public virtual T Find(TId id)
        {
            T result = Collection.FindOneById(CastId(id));
            return result;
        }

        public virtual IEnumerable<T> List()
        {
            return Collection.FindAll();
        }

        public virtual void Delete(TId id)
        {
            Collection.Remove(Query.EQ("_id", CastId(id)));
        }

        public virtual void DeleteAll()
        {
            Collection.RemoveAll();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Collection.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Collection.AsQueryable().GetEnumerator();
        }

        public Type ElementType
        {
            get { return this.Collection.AsQueryable().ElementType; }
        }

        public Expression Expression
        {
            get { return this.Collection.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return this.Collection.AsQueryable().Provider; }
        }
    }
}