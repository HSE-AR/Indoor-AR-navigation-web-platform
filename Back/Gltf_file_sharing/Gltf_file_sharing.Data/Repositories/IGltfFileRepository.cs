using Gltf_file_sharing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Data.Repositories
{
    public interface IGltfFileRepository
    {
        Task<GltfFile> GetById(Guid id);

        Task<ICollection<GltfFile>> Get();

        Task<GltfFile> Create(GltfFile gltfFile);

        Task<bool> Delete(Guid id);

        Task<GltfFile> Update(GltfFile gltfFile);

    }
}
