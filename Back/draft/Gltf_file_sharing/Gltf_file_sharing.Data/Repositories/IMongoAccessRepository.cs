using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Data.Repositories
{
    public interface IMongoAccessRepository<T>
    {
        Task<ICollection<T>> GetAsync();

        Task<T> GetAsync(string id);

        Task<T> CreateAsync(T entity);

        Task<ReplaceOneResult> UpdateAsync(T entityIn);

        Task<DeleteResult> RemoveAsync(T entityIn);

        Task<DeleteResult> RemoveAsync(string id);

    }
}
