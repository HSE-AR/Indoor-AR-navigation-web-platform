using Gltf_file_sharing.Core.Databases;
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
    public class ModificationRepository
    {
        private readonly IMongoCollection<Modification> _modifications;

        public ModificationRepository(MongoContext mongoContext)
        {
            _modifications = mongoContext.Modifications;
        }

        public async Task<ICollection<Modification>> GetAsync() =>
             await _modifications.Find(m => true).ToListAsync();

        public async Task<Modification> GetAsync(string id) =>
            await _modifications.Find(m => m.Id == id).FirstOrDefaultAsync();


        public async Task<Modification> CreateAsync(Modification modification)
        {
            modification.EditedAtUtc = DateTime.Now;
            await _modifications.InsertOneAsync(modification);
            return modification;
        }

        public async Task<ReplaceOneResult> UpdateAsync(string id, Modification modelIn) =>
          await _modifications.ReplaceOneAsync(m => m.Id == id, modelIn);

        public async Task<DeleteResult> RemoveAsync(ModelDto modelIn) =>
            await _modifications.DeleteOneAsync(model => model.Id == modelIn.Id);

        public async Task<DeleteResult> RemoveAsync(string id) =>
            await _modifications.DeleteOneAsync(model => model.Id == id);
    }
}
