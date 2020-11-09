using Gltf_file_sharing.Core.EF;
using Gltf_file_sharing.Core.Services;
using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Repositories
{
    public class GltfFileRepository : IGltfFileRepository
    {
        private readonly GltfContext _gltfContext;
        private readonly IStorageService _storageService;

        public GltfFileRepository(GltfContext gltfContext, IStorageService storageService)
        {
            _gltfContext = gltfContext;
            _storageService = storageService;
        }

        public async Task<GltfFile> CreateAsync(IFormFile file)
        {
            var gltfEntity = new GltfFile(file.FileName, DateTime.Now);
            gltfEntity.Path = await _storageService.UploadAsync(file, Guid.NewGuid() + file.FileName, "gltfs/");
            var result = await _gltfContext.GltfFiles.AddAsync(gltfEntity);
            _gltfContext.SaveChanges();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var gltfEntity = _gltfContext.GltfFiles.FirstOrDefault(g => g.Id == id);
            if (gltfEntity != null)
            {
                _storageService.RemoveFileByFullPath(gltfEntity.Path);
                _gltfContext.GltfFiles.Remove(gltfEntity);
                await _gltfContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<GltfFile>> GetAsync()
        {
            return await _gltfContext.GltfFiles.ToListAsync();
        }

        public async Task<GltfFile> GetByIdAsync(Guid id)
        {
            return await _gltfContext.GltfFiles.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<GltfFile> UpdateAsync(GltfFile gltfFile)
        {
            var result = _gltfContext.GltfFiles.Update(gltfFile);
            await _gltfContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
