using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Services.Impl
{
    public class ModelsService : IModelsService
    {
        private readonly ModelsRepository _modelsRepository;
        private readonly IUserModelIdRepository _userModelIdRepository;
        private readonly UserManager<User> _userManager;

        public ModelsService(ModelsRepository modelsRepository, UserManager<User> userManager, IUserModelIdRepository userModelIdRepository)
        {
            _modelsRepository = modelsRepository;
            _userModelIdRepository = userModelIdRepository;
            _userManager = userManager;
        }

        public async Task CreateModelAsync(ModelDto modelDto, Guid userId)
        {
            var user = await GetUserAsync(userId);
            var modelId = (await _modelsRepository.CreateAsync(modelDto)).Id;
            var userModelId = new UserModelId
            {
                ModelId = modelId,
                UserId = user.Id
            };
            await _userModelIdRepository.AddAsync(userModelId);
            user.ModelsId.Add(userModelId);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteModelAsync(string id, Guid userId)
        {
            var user = await GetUserAsync(userId);
            var userModelId = await _userModelIdRepository.GetAsync(id, userId);
            await _userModelIdRepository.DeleteAsync(userModelId);
            user.ModelsId.Remove(userModelId);
            await _userManager.UpdateAsync(user);
            await _modelsRepository.RemoveAsync(id);
        }

        public async Task<ICollection<ModelDto>> GetUserModelsAsync(Guid userId)
        {
            var user = await GetUserAsync(userId);
            return await _modelsRepository.GetAsync(user.ModelsId.Select(x => x.ModelId).ToList());
        }

        private async Task<User> GetUserAsync(Guid id)
        {
            var user = await _userManager.Users.Include(x => x.ModelsId).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new Exception("User not found");
            return user;
        }
    }
}
