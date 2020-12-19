using Gltf_file_sharing.Data.Converters;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Settings;
using MongoDB.Bson;
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
            //регистрацию бд вынести в отдельный класс MongoContext
            //пример: https://metanit.com/nosql/mongodb/4.12.php
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _models= database.GetCollection<Model>(settings.ModelsCollectionName);
        }

        public async Task<ICollection<ModelDto>> GetAsync() =>
             ModelConverter.Convert(await _models.Find(m => true).ToListAsync());

        public async Task<ModelDto> GetAsync(string id) =>
            ModelConverter.Convert(await _models.Find(m => m.Id == id).FirstOrDefaultAsync());


        public async Task<ModelDto> CreateAsync(ModelDto modelDto)
        {
            Model model = new Model
            {
                Name = modelDto.Name,
                Scene = BsonDocument.Parse(modelDto.Scene.ToString()),
                CreatedAtUtc = DateTime.Now
            };
            await _models.InsertOneAsync(model);
            return ModelConverter.Convert(model);
        }

        public async Task<ReplaceOneResult> UpdateAsync(string id, Model modelIn) =>
          await  _models.ReplaceOneAsync(m => m.Id == id, modelIn);

        public async Task<DeleteResult> RemoveAsync(ModelDto modelIn) =>
            await _models.DeleteOneAsync(model => model.Id == modelIn.Id);

        public async Task<DeleteResult> RemoveAsync(string id) =>
            await _models.DeleteOneAsync(model => model.Id == id);
    }
}
