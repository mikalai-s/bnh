using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

using Bnh.Core;

namespace Bnh.Cms.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly string connectionString;
        private readonly Lazy<MongoDatabase> database;
        private MongoCollection<T> collection;
        private string collectionName;

        public MongoRepository(string connectionString, string collectionName = null)
        {
            this.connectionString = connectionString;
            this.database = new Lazy<MongoDatabase>(() => MongoDatabase.Create(connectionString));
            this.collectionName = collectionName.IsEmpty() ? typeof(T).Name : collectionName;
        }

        public string CollectionName
        {
            get { return this.collectionName; }
        }

        public MongoDatabase Database
        {
            get { return this.database.Value; }
        }

        public MongoCollection<T> Collection
        {
            get { return this.collection ?? (this.collection = this.Database.GetCollection<T>(this.CollectionName)); }
        }

        public virtual void Insert(T item)
        {
            try
            {
                this.Collection.Insert(item);
            }
            catch (MongoSafeModeException ex)
            {
                if (!ex.Message.Contains("E11000"))
                    throw;
            }
        }

        public virtual void InsertBatch(IEnumerable<T> items)
        {
            this.Collection.InsertBatch(items);
        }

        public virtual void Save(T item)
        {
            this.Collection.Save(item);
        }

        public virtual long Count()
        {
            var count = this.Collection.Count();
            return count;
        }

        protected virtual BsonValue CastId(string id)
        {
            if (id is string)
            {
                return BsonValue.Create(ObjectId.Parse(id as string));
            }
            return BsonValue.Create(id);
        }

        public virtual T Find(string id)
        {
            T result = this.Collection.FindOneById(CastId(id));
            return result;
        }

        public virtual IEnumerable<T> List()
        {
            return this.Collection.FindAll();
        }

        public virtual void Delete(string id)
        {
            this.Collection.Remove(Query.EQ("_id", CastId(id)));
        }

        public virtual void DeleteAll()
        {
            this.Collection.RemoveAll();
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