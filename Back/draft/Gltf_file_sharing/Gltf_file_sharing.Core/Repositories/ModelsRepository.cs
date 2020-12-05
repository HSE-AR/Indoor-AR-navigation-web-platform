using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Repositories
{
    public class ModelsRepository
    {
        private readonly IMongoCollection<Model> _models;

        public ModelsRepository(IModelsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _models= database.GetCollection<Model>(settings.ModelsCollectionName);
        }

        public async Task<ICollection<Model>> Get() =>
             await _models.Find(book => true).ToListAsync();

        public async Task<Model> Get(string id) =>
            await _models.Find<Model>(m => m.Id == id).FirstOrDefaultAsync();


        public async Task<Model> Create(Model model)
        {
            await _models.InsertOneAsync(model);
            return model;
        }

        public async Task Update(string id, Model modelIn) =>
          await  _models.ReplaceOneAsync(m => m.Id == id, modelIn);

        public void Remove(Model modelIn) =>
            _models.DeleteOne(model => model.Id == modelIn.Id);

        public void Remove(string id) =>
            _models.DeleteOne(model => model.Id == id);
    }
}
