using Gltf_file_sharing.Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Services
{
    public interface IModelsService
    {
        Task CreateModelAsync(ModelDto modelDto, Guid userId);

        Task DeleteModelAsync(string id, Guid userId);

        Task<ICollection<ModelDto>> GetUserModelsAsync(Guid userId);
    }
}
