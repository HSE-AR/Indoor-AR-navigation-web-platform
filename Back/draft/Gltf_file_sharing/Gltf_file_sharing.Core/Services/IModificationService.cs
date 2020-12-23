﻿using Gltf_file_sharing.Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Services
{
    public interface IModificationService
    {
        Task<bool> ModifyModel(ModificationDto modificationDto);

        Task<bool> ModifyModels(IEnumerable<ModificationDto> modificationDtos);

        Task<IEnumerable<ModificationDto>> GetAsync();
    }

}
