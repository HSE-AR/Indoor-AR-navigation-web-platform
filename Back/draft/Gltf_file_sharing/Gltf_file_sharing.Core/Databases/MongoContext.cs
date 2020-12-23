using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Settings;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO;


namespace Gltf_file_sharing.Core.Databases
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly IModelsDatabaseSettings _settings;

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

        public MongoContext(IModelsDatabaseSettings settings)
        {
            _settings = settings;

            var client = new MongoClient(_settings.ConnectionString);

            _database = client.GetDatabase(_settings.DatabaseName);

            if (!IsDatabaseInitialised())  InitialiseDatabase();

        }


        #region PrivateMethods

        private bool IsDatabaseInitialised()
        {
            return _database.GetCollection<Model>(_settings.ModelsCollectionName).CountDocuments(x => true) != 0;
        }

        private void InitialiseDatabase()
        {
            using (var streamReader = new StreamReader("../data/json/initial_scene.json"))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    using (var jsonReader = new JsonReader(line))
                    {
                        var context = BsonDeserializationContext.CreateRoot(jsonReader);
                        var document = Models.DocumentSerializer.Deserialize(context);
                        Models.InsertOne(document);
                    }
                }
            }
        }
        #endregion
    }
}
