using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Data.Repositories
{
    public interface IGltfFileRepository
    {
        Task<GltfFile> GetByIdAsync(Guid id);

        Task<ICollection<GltfFile>> GetAsync();

        Task<GltfFile> CreateAsync(IFormFile file);

        Task<bool> DeleteAsync(Guid id);

        Task<GltfFile> UpdateAsync(GltfFile gltfFile);

    }
}
