using Gltf_file_sharing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Data.Repositories
{
    public interface IUserModelIdRepository
    {
        Task AddAsync(UserModelId item);

        Task DeleteAsync(UserModelId item);

        Task<UserModelId> GetAsync(string modelId, Guid userId);

        Task<ICollection<UserModelId>> GetAsync();
    }
}
