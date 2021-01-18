using Avatar.App.Api.Controllers;
using Gltf_file_sharing.Core.Services;
using Gltf_file_sharing.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{

    [Route("api/[controller]")]
    public class ModificationController : BaseAuthorizeController
    {
        private readonly IModificationService _modificationService;

        public ModificationController(IModificationService modificationService)
        {
            _modificationService = modificationService;
        }

        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public async Task<ActionResult<IEnumerable<ModificationDto>>> GetAsync()
        {
            try
            {
                return new JsonResult(await _modificationService.GetAsync());
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<bool>> SetModification([FromBody] ModificationDto modificationDto)
        {
            try
            {
                var userId = GetUserId();
                return await _modificationService.ModifyModel(modificationDto, userId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("list")]
        [Authorize]
        public async Task<ActionResult<bool>> SetModifications([FromBody] IEnumerable<ModificationDto> modificationDtos)
        {
            try
            {
                var userId = GetUserId();
                return await _modificationService.ModifyModels(modificationDtos, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
