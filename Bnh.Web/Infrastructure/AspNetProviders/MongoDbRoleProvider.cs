using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using Bnh.Core;
using Cms.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB.Web.Providers
{
    public class MongoDbRoleProvider : RoleProvider
    {
        private MongoCollection rolesMongoCollection;
        private MongoCollection usersInRolesMongoCollection;

        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            // Make sure each role exists
            foreach (var roleName in roleNames)
            {
                if (!this.RoleExists(roleName))
                {
                    throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
                }
            }
            
            foreach (var username in usernames)
            {
                var membershipUser = Membership.GetUser(username);

                if (membershipUser == null)
                {
                    throw new ProviderException(String.Format("The user '{0}' was not found.", username));
                }
                
                foreach (var roleName in roleNames)
                {
                    if (this.IsUserInRole(username, roleName))
                    {
                        throw new ProviderException(String.Format("The user '{0}' is already in role '{1}'.", username, roleName));
                    }

                    var bsonDocument = new BsonDocument
                    {
                        { "ApplicationName", this.ApplicationName },
                        { "Role", roleName },
                        { "Username", username }
                    };

                    this.usersInRolesMongoCollection.Insert(bsonDocument);
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            
            if (this.rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
            {
                throw new ProviderException(String.Format("The role '{0}' already exists.", roleName));
            }

            var bsonDocument = new BsonDocument
            {
                { "ApplicationName", this.ApplicationName },
                { "Role", roleName }
            };

            this.rolesMongoCollection.Insert(bsonDocument);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!RoleExists(roleName))
            {
                throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
            }
            
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));

            if (throwOnPopulatedRole && this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
            {
                throw new ProviderException("This role cannot be deleted because there are users present in it.");
            }

            this.usersInRolesMongoCollection.Remove(query);
            this.rolesMongoCollection.Remove(query);

            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (!RoleExists(roleName))
            {
                throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
            }

            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
        }

        public override string[] GetAllRoles()
        {
            var query = Query.EQ("ApplicationName", this.ApplicationName);
            return this.rolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", username));
            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
        }

        public override void Initialize(string name, NameValueCollection settings)
        {
            var config = DependencyResolver.Current.GetService<IConfig>();

            this.ApplicationName = settings["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;

            var mongoDatabase = ConnectionUtils.GetDatabase(settings, config);
            this.rolesMongoCollection = mongoDatabase.GetCollection(settings["collection"] ?? "Roles");
            this.usersInRolesMongoCollection = mongoDatabase.GetCollection("UsersInRoles");

            this.rolesMongoCollection.EnsureIndex("ApplicationName");
            this.rolesMongoCollection.EnsureIndex("ApplicationName", "Role");
            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role");
            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Username");
            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role", "Username");

            base.Initialize(name, settings);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
                    this.usersInRolesMongoCollection.Remove(query);
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this.rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
        }
    }
}
