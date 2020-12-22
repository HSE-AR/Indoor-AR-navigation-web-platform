using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Core.Databases
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly IModelsDatabaseSettings _settings;

        public MongoContext(IModelsDatabaseSettings settings)
        {
            _settings = settings;

            var client = new MongoClient(_settings.ConnectionString);

            _database = client.GetDatabase(_settings.DatabaseName);

            if (!IsDatabaseInitialised())
            {
                InitialiseDatabase();
            }
        }

        public IMongoCollection<Model> Models
        {
            get { return _database.GetCollection<Model>(_settings.ModelsCollectionName); }
        }

        public IMongoCollection<BsonDocument> ModelsAsBsonDocument
        {
            get { return _database.GetCollection<BsonDocument>(_settings.ModelsCollectionName); }
        }

        public IMongoCollection<Modification> Modifications
        {
            get { return _database.GetCollection<Modification>(_settings.ModificationsCollectionName); }
        }

        public IMongoCollection<BsonDocument> ModificationsAsBsonDocument
        {
            get { return _database.GetCollection<BsonDocument>(_settings.ModificationsCollectionName); }
        }


        private bool IsDatabaseInitialised()
        {
            return _database.GetCollection<Model>(_settings.ModelsCollectionName).CountDocuments(x => true) != 0;
        }

        private void InitialiseDatabase()
        {

        }
    }
}
