using Gltf_file_sharing.Core.Databases;
using Gltf_file_sharing.Core.EF;
using Gltf_file_sharing.Data.Converters;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Repositories
{
    public class ModelsRepository
    {
        private readonly IMongoCollection<Model> _models;

        public ModelsRepository(MongoContext mongoContext)
        {
            _models = mongoContext.Models;
        }

        public async Task<ICollection<ModelDto>> GetAsync() =>
             ModelConverter.Convert(await _models.Find(m => true).ToListAsync());

        public async Task<ModelDto> GetAsync(string id) =>
            ModelConverter.Convert(await _models.Find(m => m.Id == id).FirstOrDefaultAsync());

        public async Task<ICollection<ModelDto>> GetAsync(ICollection<string> ids) =>
            ModelConverter.Convert(await _models.Find(m => ids.Contains(m.Id)).ToListAsync());

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
