using Gltf_file_sharing.Core.EF;
using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Repositories
{
    public class UserModelIdRepository : IUserModelIdRepository
    {
        private readonly GltfContext _context;

        public UserModelIdRepository(GltfContext context)
        {
            _context = context;
        }

        public async Task<ICollection<UserModelId>> GetAsync() =>
            await _context.UserModelIds.AsNoTracking().ToListAsync();

        public async Task<UserModelId> GetAsync(string modelId, Guid userId) =>
            await _context.UserModelIds.FirstOrDefaultAsync(x => x.UserId == userId && x.ModelId == modelId);

        public async Task AddAsync(UserModelId item)
        {
            await _context.UserModelIds.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserModelId item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.UserModelIds.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
