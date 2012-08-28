using System;
using System.Linq;
using System.Collections.Generic;

using Bnh.Core;
using Bnh.Core.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Bnh.Infrastructure.Repositories
{
    public partial class EntityRepositories : IEntityRepositories, IDisposable
    {
        public IRepository<Community> Communities { get; private set; }

        public IRepository<City> Cities { get; private set; }

        public IReviewRepository Reviews { get; private set; }

        public EntityRepositories(Config config)
        {
            this.Communities = new MongoRepository<Community>(config.ConnectionStrings["mongo"]);
            this.Cities = new MongoRepository<City>(config.ConnectionStrings["mongo"]);
            this.Reviews = new ReviewRepository(config.ConnectionStrings["mongo"]);

            EnsureData();
        }

        /// <summary>
        /// Registers class map
        /// </summary>
        static EntityRepositories()
        {
            BsonClassMap.RegisterClassMap<City>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(city => city.CityId).SetRepresentation(BsonType.ObjectId);
            });
            BsonClassMap.RegisterClassMap<Community>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(community => community.CommunityId).SetRepresentation(BsonType.ObjectId);
            });
            BsonClassMap.RegisterClassMap<Review>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(review => review.ReviewId).SetRepresentation(BsonType.ObjectId);
                cm.GetMemberMap(review => review.TargetId).SetRepresentation(BsonType.ObjectId);
            });
            BsonClassMap.RegisterClassMap<Comment>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(comment => comment.CommentId).SetRepresentation(BsonType.ObjectId);
            });
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Returns true if given string is valid ID representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsValidId(string id)
        {
            ObjectId oId;
            return ObjectId.TryParse(id, out oId);
        }

        /// <summary>
        /// Ensures initial data exists in repository
        /// </summary>
        private void EnsureData()
        {
            // just a simple check whether there is need to initilize data
            if (this.Cities.Where(s => s.Name == "Calgary").Any()) { return; }

            // insert calgary
            var calgary = new City
            {
                Name = "Calgary",
                UrlId = "Calgary",
                Zones = new[] { "NW", "NE", "SE", "SW" }
            };
            this.Cities.Insert(calgary);

            // insert communitities
            this.Communities.Insert(new Community { Name = "Saddlebrook", Zone = "NE", UrlId = "Saddlebrook", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Auburn Bay", Zone = "SE", UrlId = "AuburnBay", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Rock Lake Estates", Zone = "NW", UrlId = "RockLakeEstates", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Beacon Heights", Zone = "NW", UrlId = "BeaconHeights", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Cougar Ridge", Zone = "SW", UrlId = "CougarRidge", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Valley Pointe Estates", Zone = "NW", UrlId = "ValleyPointeEstates", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Valley View ", Zone = "SE", UrlId = "ValleyView", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Spring Valley Estates", Zone = "SW", UrlId = "SpringValleyEstates", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Bearspaw Willow Creek", Zone = "NW", UrlId = "BearspawWillowCreek", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Garrison Green", Zone = "SW", UrlId = "GarrisonGreen", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Walden", Zone = "SE", UrlId = "Walden", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Silverado", Zone = "SW", UrlId = "Silverado", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Evansview", Zone = "NW", UrlId = "Evansview", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Springbank Hill", Zone = "SW", UrlId = "SpringbankHill", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Cranston", Zone = "SE", UrlId = "Cranston", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Currie Barracks", Zone = "SW", UrlId = "CurrieBarracks", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Saddlestone", Zone = "NE", UrlId = "Saddlestone", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Garricon Woods", Zone = "SW", UrlId = "GarriconWoods", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Castle Keep", Zone = "SW", UrlId = "CastleKeep", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Sage Meadows", Zone = "NW", UrlId = "SageMeadows", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "New Discovery", Zone = "SW", UrlId = "NewDiscovery", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Royal Oak", Zone = "NW", UrlId = "RoyalOak", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Crestmont", Zone = "SW", UrlId = "Crestmont", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Mahogany", Zone = "SE", UrlId = "Mahogany", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Evanston", Zone = "NW", UrlId = "Evanston", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Skyview Ranch", Zone = "NE", UrlId = "SkyviewRanch", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Citylife", Zone = "NW", UrlId = "Citylife", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Coulee Way Estates", Zone = "SW", UrlId = "CouleeWayEstates", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Elmont Bay", Zone = "SW", UrlId = "ElmontBay", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "McKenzie Towne", Zone = "SE", UrlId = "McKenzieTowne", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Kincora", Zone = "NW", UrlId = "Kincora", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Evergreen", Zone = "SW", UrlId = "Evergreen", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Evansridge", Zone = "NW", UrlId = "Evansridge", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Westpark", Zone = "SW", UrlId = "Westpark", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Watermark at Bearspaw", Zone = "NW", UrlId = "WatermarkatBearspaw", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Sage Hill", Zone = "NW", UrlId = "SageHill", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "New Brighton", Zone = "SE", UrlId = "NewBrighton", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Chaparral Valley", Zone = "SE", UrlId = "ChaparralValley", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Bridlewood", Zone = "SW", UrlId = "Bridlewood", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Sherwood", Zone = "NW", UrlId = "Sherwood", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Tuscany", Zone = "NW", UrlId = "Tuscany", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Copperfield", Zone = "SE", UrlId = "Copperfield", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Quarry Park", Zone = "SE", UrlId = "QuarryPark", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Aspen Hills", Zone = "SW", UrlId = "AspenHills", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Martindale", Zone = "NE", UrlId = "Martindale", CityId = calgary.CityId });
            this.Communities.Insert(new Community { Name = "Panorama Hills", Zone = "NW", UrlId = "PanoramaHills", CityId = calgary.CityId });

            //-- Add 172 rows to [bl].[Builder]
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('3d61cf05-1638-4f90-b0e3-001e5e2682f0', N'Millenium Plus Homes', N'MilleniumPlusHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e3c18654-1d16-419b-b106-01f3473f4848', N'Crossley Custom Homes Ltd', N'CrossleyCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('809a8746-7a61-41d1-9cfd-039dc0b397c4', N'Niklas Group Inc.', N'NiklasGroupInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a41734e6-04c8-4ce2-af25-03bc72bd3df4', N'Harvest Builders', N'HarvestBuilders', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('38b774de-f56d-4867-a508-0469606aa5cf', N'Classic Craft Homes Inc', N'ClassicCraftHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4dcc5e2a-efa0-4eff-8b8d-052021aab66b', N'Canterra Custom Homes Ltd', N'CanterraCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2fc18589-9f2e-4128-aff9-096d5a77d4ca', N'DaVInci Group', N'DaVInciGroup', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('c060bd01-8d44-4aab-91b7-0a4bf4c7ea75', N'Maillot Homes', N'MaillotHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('5cf8ae95-c74b-4b3f-a177-0b0340d42442', N'Cardel Homes', N'CardelHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('1778ba9d-5691-4c50-8d30-0c835b64423c', N'Innovations by Jayman', N'InnovationsbyJayman', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('55f6090a-ffc7-40f4-a61f-0d7c05acd58d', N'Today’s Community Limited Partnership', N'Today’sCommunityLimitedPartnership', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2a23b6dd-2057-467d-b0de-0ee69c9d5f6f', N'Cozy Shacks', N'CozyShacks', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('921bcd8d-1e1c-4a42-8d57-12bcf3d3dc17', N'Tudor Homes Inc.', N'TudorHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('517a31eb-2b0d-479f-ad1a-130a2e21594f', N'Augusta Fine Homes', N'AugustaFineHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a074f1f9-bac3-4bb1-9c4a-13a343b81eb8', N'Stanford Homes', N'StanfordHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2d287a1a-2860-41e9-8eba-154bb7b5479b', N'Oxford Homes', N'OxfordHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b2b129a7-afe1-4216-b952-15574ae04c2e', N'Laratta Constuction Ltd', N'LarattaConstuctionLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('75e4543d-71bb-455a-af60-15fcc4dfa811', N'Mattamy Homes Calgary', N'MattamyHomesCalgary', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d0872d56-bad6-470f-9f71-163a6a100f20', N'Astoria Homes Ltd', N'AstoriaHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4336f6f4-bece-42e0-a4be-17b6ab0fa611', N'Birchwood Properties Corp', N'BirchwoodPropertiesCorp', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e789c616-7f91-4d95-85ec-1824d5bd517e', N'Upland Developments Inc', N'UplandDevelopmentsInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a60bd2ab-a5c5-4c34-a6f3-188e0ced8950', N'Vintage Fine Homes, Inc.', N'VintageFineHomes,Inc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a59e4f08-7902-4337-b234-196a5894d978', N'Mountain View Custom Homes Ltd.', N'MountainViewCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('978c181f-d5ac-4cf1-999f-1b2e06c8773f', N'Keystone Homes', N'KeystoneHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e9b1de02-e40f-4386-b445-1c868272f3c7', N'Woodmaster Homes Ltd.', N'WoodmasterHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('82b4be31-beb2-41be-8174-1d152c0e4678', N'Unity Builders Group', N'UnityBuildersGroup', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('bdd8d57f-a158-4e55-89aa-21365ec6fe9b', N'Klair Custom Homes', N'KlairCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('133b0fa3-a5b4-498b-a9d0-238074b8d8d4', N'Rawlyk Developments Inc.', N'RawlykDevelopmentsInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2841bc11-792e-446d-af5a-25d605e798d8', N'Vesta Properties (Alberta)', N'VestaProperties(Alberta)', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ac4663ab-0d3b-481c-a5f0-25fe3dc5beb9', N'Reliant Homes', N'ReliantHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('baf1b0be-4cd3-437c-b4ac-28f2b0b8abf4', N'Stonebridge Crafted Homes', N'StonebridgeCraftedHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d76e1c67-6b52-4879-bdf0-2a2f47529887', N'Karoleena Custom Homes', N'KaroleenaCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('126f9329-f35f-4a2f-9fb5-2aa9983d8682', N'Terra Banah Builders Ltd', N'TerraBanahBuildersLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e5e05005-51a5-4b7a-ae08-2c97fabebe12', N'Urban Image Fine Homes Ltd', N'UrbanImageFineHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('159e9d90-fcc8-46d8-ade9-2de46be9958d', N'Shane Homes Ltd.', N'ShaneHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('15e477b5-fabd-47f3-95ee-2f85ad95a3b0', N'Kimcorp Developments', N'KimcorpDevelopments', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('1b463de5-41b9-4ca6-9c80-317f1f03ad3c', N'Artistique Homes', N'ArtistiqueHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('da3110a6-9d91-42e6-863a-330c5a6c47e4', N'Pacesetter Homes', N'PacesetterHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('20c72d17-45f9-4fa2-8ded-33381e5bfb15', N'Harmony Park Developments', N'HarmonyParkDevelopments', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b8ca35f0-55e1-4773-9b45-37ef0e2b6ae3', N'Newcastle Homes', N'NewcastleHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('fb01f8c2-6cac-40ae-b98b-38b26eeea4f3', N'Knightsbridge Homes Ltd', N'KnightsbridgeHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('41057432-098c-4ed5-aea4-38ea4e7e085f', N'Calbridge Homes', N'CalbridgeHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d8ae58ee-e883-4d48-a5e6-39a122051ee1', N'Stepper Custom Homes Inc.', N'StepperCustomHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('f2a78e7a-69bb-4e13-a761-39bb5de00c70', N'Carma Developers', N'CarmaDevelopers', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('372e33ee-a434-44e2-91b9-3a12bd942978', N'Dundee Developments', N'DundeeDevelopments', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e2701f1d-be1d-434e-955d-3ac18e741b0e', N'Beattie Homes Ltd', N'BeattieHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('885f3e6a-fc41-4326-90aa-3adb4be0f729', N'Landmark Homes', N'LandmarkHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('102b2784-6221-4601-9fa7-3b8f8d13be60', N'Kurmak Builders Inc.', N'KurmakBuildersInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8fe3a8e1-7a3a-4ea2-9820-3ba324759a22', N'Enviro Custom Homes', N'EnviroCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('766dc8e4-6ff4-4783-8794-3be513eef000', N'Grandscape Homes', N'GrandscapeHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('bdf4e5d5-f7ad-456e-9feb-3c3f3ca1433b', N'Lionsworthe Homes Corporation', N'LionswortheHomesCorporation', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2767a5eb-4b02-47d8-a70c-3d60ceb3b870', N'Talisman Homes', N'TalismanHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('50c0ee34-36ba-4127-917c-401c7f25e6ef', N'Genstar Development Company', N'GenstarDevelopmentCompany', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('270d6e4e-1bc4-4c67-8ca6-4092c0af2090', N'Jaden Homes', N'JadenHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2eee852b-e03f-4a66-b2b4-443dfbbef3bd', N'Cornerstone Homes', N'CornerstoneHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('042390f7-b746-4389-a42f-46788e89c46a', N'Sarina Developments Ltd.', N'SarinaDevelopmentsLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6c38080d-fd0b-4d15-8567-469bc9410525', N'DreamWest Homes', N'DreamWestHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('75d8e0dc-6485-4d49-bb83-473bf37307ea', N'Sundance Custom Homes', N'SundanceCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ac4f9915-7b0e-4253-82aa-4873517981f5', N'Dreamwood Homes Ltd', N'DreamwoodHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('db280d30-f6c8-4316-af99-48d7a6fbd15a', N'Winwood Homes Ltd.', N'WinwoodHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ee1708fa-a86d-4e13-9060-4bb309d7a380', N'Greenboro Estate Homes Limited Parntership', N'GreenboroEstateHomesLimitedParntership', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('abb8d544-7c06-4bff-982d-4bdfe21c0e1f', N'Timberock Home Developments Ltd', N'TimberockHomeDevelopmentsLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4ed60e3c-0871-436d-a185-505419d5aea4', N'Prominent Homes', N'ProminentHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d533566e-77e6-415f-8c6c-512af0c118a1', N'Gold Seal Homes', N'GoldSealHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('92057350-64ae-467a-8dd3-53051c7d9c5e', N'Nelson Homes', N'NelsonHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('7b8e3f38-30d2-4200-956f-53da35bfcdb0', N'McDonald Luxurious Custom Homes Ltd.', N'McDonaldLuxuriousCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d1738319-00b3-4fef-851c-552c9ff54241', N'The Kunz Group Inc', N'TheKunzGroupInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('88e6bc67-b08d-4351-9d34-57df57ef919c', N'Thompson Luxury Living', N'ThompsonLuxuryLiving', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('015fe379-8a04-484f-953a-5887d2fdc40a', N'Carolina Homes Inc', N'CarolinaHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6a7b99bb-507b-4fd9-9733-58f65b5a35f8', N'Canada Lands Company CLC Limited', N'CanadaLandsCompanyCLCLimited', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d0e396c6-6fa2-49f7-a522-599e7c91a9d2', N'Distinctive Living', N'DistinctiveLiving', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('3c787c5a-8fb4-4dde-beb1-5a2ddc264be5', N'United Communities', N'UnitedCommunities', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('c1a4ae5e-c698-476b-a708-5b4d1e2b30e3', N'Jesse Homes', N'JesseHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('dd8a003c-fa94-4364-a880-5bab88d6ccbe', N'Homes by Us Ltd', N'HomesbyUsLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('32f7e380-2c2d-47de-a6ef-5e074928e943', N'Walton Development and Management', N'WaltonDevelopmentandManagement', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('16a86539-8634-4061-a17b-5e33c4e71310', N'Eiffel Homes Ltd', N'EiffelHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('7aa2b073-f45b-416c-8da6-5f3b545afca9', N'Aquilla Homes Ltd', N'AquillaHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('7fd4438d-095b-4c1d-9d63-60db6133e38a', N'WestCreek Developments', N'WestCreekDevelopments', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('9fa8a172-759c-4ea0-871f-60e102e69c68', N'Lifestyle Homes Inc', N'LifestyleHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2dd1de79-41ee-4146-9967-60eafa3ebaa2', N'Schickedanz Bros. Ltd.', N'SchickedanzBrosLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b03dedc6-d2d9-4fee-a422-62218f6e882a', N'Jujhar Custom Homes', N'JujharCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e12d310f-d2f0-4843-84b3-6266bcf13441', N'Rykell Homes Ltd.', N'RykellHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ec75a484-85cb-4f02-b017-62bd4cbf130d', N'Design Factor Homes', N'DesignFactorHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('24e75e0f-44ad-420a-9dc1-63a1dbbd38c1', N'Calista Homes Ltd.', N'CalistaHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d3fe4b4b-5d5e-49c2-9d64-6423d68d9138', N'Arctek Homes Ltd', N'ArctekHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('356c5c01-5c78-4f8b-813b-66188fc2b989', N'Jayman MasterBUILT, Inc.', N'JaymanMasterBUILT,Inc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a409b0dc-d940-4173-a5cd-673127947d94', N'Brad-Mar Homes', N'Brad-MarHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('87ff5a55-b1fa-4dbf-9fd5-69a62d6a173d', N'Trico Homes', N'TricoHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4571fc79-f5e3-46ba-bcf0-6ab1c0296691', N'Custom Touch Homes Ltd', N'CustomTouchHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ca0b3180-4ee8-432d-988b-6b1bff98d05e', N'Broadview Homes', N'BroadviewHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6aabfe3e-b3b2-475f-9856-6b55a271649a', N'Lask Homes Ltd', N'LaskHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8fbf0e1d-8519-4ac2-a9bd-6b60f361efd2', N'Bellagio Homes Inc', N'BellagioHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4d20aa2b-c30d-455a-af65-70041e5eaa8f', N'Winterfell Development Corp', N'WinterfellDevelopmentCorp', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('dabe47d8-180e-4e79-8af7-718ac73b9a68', N'Reid Built Homes Calgary', N'ReidBuiltHomesCalgary', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('4905c4a8-d13d-4be1-8413-74232b18949e', N'Heritage Pointe', N'HeritagePointe', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('0e9f667d-6d93-42e1-9da9-7561742a3655', N'Willowbrook Homes Inc', N'WillowbrookHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ca4b4257-3252-4301-b4c0-7584214cefa7', N'Jigsaw Homes', N'JigsawHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('087e593a-4c89-4261-8a8d-7634ad1e6a4d', N'McKinley Masters', N'McKinleyMasters', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('81bc4049-6490-4e13-868c-768f1ce8af41', N'Jager Homes Ltd', N'JagerHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6797456b-d375-4822-a6f2-779565363b26', N'House Brand Construction Ltd', N'HouseBrandConstructionLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a20dcd62-6655-4979-9aa0-799633d8d98b', N'Hillson Homes', N'HillsonHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('3cb5bed3-7cc1-4c90-8b30-7ae25ef23e01', N'Matisse Homes Inc.', N'MatisseHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('72b3424e-fe63-4c12-9fbd-7b2dc6cce5a3', N'Christina Homes', N'ChristinaHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('557a6af8-7f62-498e-8c8f-7c1a4d71f5f8', N'Qualico Communities (Calgary)', N'QualicoCommunities', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6b4ff8ed-584e-4f49-bafe-7d933b6aee2d', N'Truman Homes', N'TrumanHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('67541ad5-ef0a-4b63-a4bd-7e40b14c0a39', N'Burke Builders Inc', N'BurkeBuildersInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('da4e922d-2cd0-4035-84d7-7f29151d9be8', N'Wolf Custom Homes', N'WolfCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('43a90a57-a376-4c2d-b640-858bd03c6ac2', N'O2 Developments', N'O2Developments', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e0120703-745f-4b5a-8d33-8700b3e20cc6', N'Homes by Bellia Inc', N'HomesbyBelliaInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d63fe7f2-f865-4464-ab3a-8a3cfedcbceb', N'Easy Homes', N'EasyHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('bc108d81-2cc0-45c7-9788-8abd2789f26c', N'Canyon Custom Homes', N'CanyonCustomHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('14947df0-9da8-4496-9e8a-8ba423933701', N'Wolverine Custom Homes Ltd.', N'WolverineCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b5ab4879-f9f9-42c4-8c6d-8d5760b050bb', N'Strathmore Homes Ltd.', N'StrathmoreHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6e7ba5c2-f0aa-4a63-b21b-8dd2cc3e7c68', N'Cambridge Homes', N'CambridgeHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('31fe4806-5802-49bf-b64f-92aa8dd06da2', N'Sabal Homes (Calgary)', N'SabalHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2101c362-9f75-4373-9ed4-95b95ccbf93a', N'Arriva Homes', N'ArrivaHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ebbf5271-2da2-4e45-b001-972a4ec5c193', N'Van Manna Homes Inc.', N'VanMannaHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('324fd34c-3191-4523-b09f-985c1672eadb', N'Arcuri Homes Inc', N'ArcuriHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('df0647c4-7917-4cf5-be05-997ef24fe3dd', N'Camelot Custom Homes Inc', N'CamelotCustomHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e553ac0b-9b84-4aaa-b600-9acf7dc26aee', N'Medallion Development Corp.', N'MedallionDevelopmentCorp', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('617d51b1-f0ee-4a26-8eff-9dccd6e31546', N'Charik Custom Homes Inc', N'CharikCustomHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ae672e24-19f4-4397-8f24-9eeeb528a7fd', N'Artisan Homes', N'ArtisanHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('0143a21b-8477-4408-91fe-a5501d19c472', N'Alloy Homes Incorporated', N'AlloyHomesIncorporated', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8492c2d3-b485-4a51-80d5-a5acf9f3bafb', N'Loreck Homes Ltd', N'LoreckHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('416527d2-3869-46be-876c-a5d6b2d7f81a', N'Janssen Homes Ltd', N'JanssenHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('22c07e5f-fb7a-4fd2-aac7-a82c35549261', N'Rethink Homes', N'RethinkHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('03269c1a-1e3e-4f4b-a5a3-a830bc671a63', N'Summerbrae Homes Ltd.', N'SummerbraeHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('99927bf8-249e-45c3-a365-a9569f6977ba', N'Burntwood Holdings Limited', N'BurntwoodHoldingsLimited', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('3c8401fa-f6bf-478d-bcb7-aa2e1f93a7ae', N'Ravenview Homes Inc.', N'RavenviewHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('cbe07f5c-078d-4fef-8771-aaf6fc9427a2', N'Passion Homes', N'PassionHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('866b5850-a12b-421b-94e3-ac2180468873', N'Lupi Construction Ltd', N'LupiConstructionLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('c6735521-b85d-4e8d-bcfa-aef3cb73d425', N'Greystone Custom Homes Ltd', N'GreystoneCustomHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8ffa60c1-11cd-46c4-9b91-afa0265c3bbb', N'Jameswood Homes', N'JameswoodHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('096c2960-3415-462b-b0ac-b09593a3aca5', N'Coco Homes Ltd', N'CocoHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ec35fed5-d323-4494-a48a-b494f890906e', N'Apex Land', N'ApexLand', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b9fde2ed-604c-4e9d-8e8d-b8930d306eb3', N'Cidex Signature Homes', N'CidexSignatureHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2664c7e2-b502-431e-8e2f-b940d7c4fab0', N'Sterling Homes', N'SterlingHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('aa8ca18e-0bf1-47a7-a5ed-ba98d366111c', N'SF Homes Ltd.', N'SFHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('49255a79-22c2-42d5-b271-bbbb0dce6c27', N'Winter Green Homes Ltd.', N'WinterGreenHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('fe86aafd-3430-4352-9447-bc686cab8e95', N'Baywest Homes Ltd', N'BaywestHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e57fb2e3-70e2-4f34-bcc3-bcb60127d395', N'McKee Homes Ltd.', N'McKeeHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('91e28166-dd7e-4c68-90e8-bea6c0bdd20c', N'New Casa Company Ltd.', N'NewCasaCompanyLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8b3e406a-d6de-4631-9a37-c370dcf13672', N'Mapeland Homes Ltd.', N'MapelandHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8babd560-5871-456c-a617-c5d254678381', N'Douglas Homes Ltd', N'DouglasHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('b5fe443a-722b-4045-8c3c-c774c3c91af5', N'Elm Homes Ltd', N'ElmHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('e0585211-ea98-4664-ad73-c7fc97309cc7', N'Farrelly Homes Ltd', N'FarrellyHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('69bce151-3f34-4ef8-b72c-c8a4feeaae24', N'Vanity Homes Ltd.', N'VanityHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('784fe40c-5352-47b5-9927-c8e855c3778d', N'Genesis Land Development Corp.', N'GenesisLandDevelopmentCorp', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a37b8d59-c149-4c37-b8f9-c946dabe99d6', N'Crystal Creek Homes Inc', N'CrystalCreekHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('6b723142-81cf-414a-90fe-ca0e3eb66339', N'Goldmark Homes Ltd', N'GoldmarkHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('2d40f9bc-ad4e-40c8-87e0-ca39682d6c14', N'Morrison Homes', N'MorrisonHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('08ce9891-e5cf-4aec-b004-cb482c3e51ba', N'NuVista Homes Ltd.', N'NuVistaHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('faaf4b3d-fa37-4b45-a5eb-d1b76d59b227', N'Hopewell Residential Communities', N'HopewellResidentialCommunities', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('ed95e9a0-eb8c-446e-bf8e-d3fc5114ef50', N'Edenridge Homes Ltd', N'EdenridgeHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('11e6a85b-2ac2-4188-bf23-dafec389495d', N'Hermosa Homes Inc', N'HermosaHomesInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('939dd8ea-f5f7-401c-bd42-db37cc31ab9f', N'Avalon Master Builder', N'AvalonMasterBuilder', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('f0cfe7a4-5fe0-4227-a81c-dfefb0305c1e', N'Ashton Design Inc', N'AshtonDesignInc', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('aecf6a37-7715-4dff-81c5-e2293176a35c', N'Albi Homes Ltd', N'AlbiHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('c0b538e3-bea0-46ae-b096-e9dfd7b6584d', N'Rock Creek Builders', N'RockCreekBuilders', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('5987ea21-c2bf-499c-8e5a-ea430bcd8bdb', N'Today’s Homes Limited Partnership', N'Today’sHomesLimitedPartnership', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('1891769c-3e0b-4829-a065-ec580852208f', N'WestView Builders', N'WestViewBuilders', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('12e6be0f-d78c-4fca-a990-ece018dccca1', N'Veranda Estate Homes', N'VerandaEstateHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('cf6e64bf-a306-4411-958f-f08639464df5', N'Cedarglen Homes', N'CedarglenHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('f57b6c6a-9b8e-43ed-bf30-f1a42b2b02df', N'Heartland Homes (Calgary)', N'HeartlandHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('68f48a63-40ab-450a-b2eb-f22014d33842', N'Homes by Avi (Calgary)', N'HomesbyAvi', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('a4b5dd48-0b59-473d-84f3-f275c038d4dd', N'Rembrandt Master Home Builder', N'RembrandtMasterHomeBuilder', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('8344d04e-bc61-4085-9a65-f286f553c18d', N'Excel Homes', N'ExcelHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('146f1354-b2e1-4c42-939c-f4abb24bed42', N'Country Rose Homes', N'CountryRoseHomes', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('08511f35-78af-42bf-8563-f4c7beb87702', N'Shergill Homes Ltd.', N'ShergillHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('0eb9c1e7-4f41-49a1-96b7-f552d5e7b99f', N'Elite Homes Limited Partnership', N'EliteHomesLimitedPartnership', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('0d260e2b-a2e3-4fe2-829d-f9723ed889ef', N'Oak Manor Homes Ltd.', N'OakManorHomesLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
            //INSERT INTO [bl].[Builder] ([Id], [Name], [UrlId], [CityId]) VALUES ('d1eba47e-115c-499c-a953-ffd8f6ecb2ad', N'The Genesis Marketing Group Ltd', N'TheGenesisMarketingGroupLtd', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
        }
    }
}
