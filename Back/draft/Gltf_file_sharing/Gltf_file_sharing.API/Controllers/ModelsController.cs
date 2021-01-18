using Avatar.App.Api.Controllers;
using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Core.Services;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{

        [Route("api/[controller]")]
        public class ModelsController : BaseAuthorizeController
        {
            private readonly ModelsRepository _modelsRepository;
            private readonly IModelsService _modelsService;

            public ModelsController(ModelsRepository modelsRepository, IModelsService modelsService)
            {
                _modelsRepository = modelsRepository;
                _modelsService = modelsService;
            }

            [HttpGet]
            public async Task<ActionResult<ICollection<ModelDto>>> Get() =>
                 new JsonResult(await _modelsRepository.GetAsync());
 

            [HttpGet("{id:length(24)}", Name = "GetModel")]
            public async Task<ActionResult<ModelDto>> Get(string id)
            {
                var model = await _modelsRepository.GetAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                return model;
            }
            
            
            [HttpGet("user")]
            [Authorize]
            public async Task<ActionResult<ICollection<ModelDto>>> GetUserModels()
            {
                try
                {
                    var userId = GetUserId();
                    
                    return new JsonResult(await _modelsService.GetUserModelsAsync(userId));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }


            [HttpPost]
            [Authorize]
            public async Task<ActionResult<ModelDto>> Create([FromBody]ModelDto modelDto)
            {
                try
                {
                    var userId = GetUserId();
                    await _modelsService.CreateModelAsync(modelDto, userId);
                    return Ok();
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }
                
            }

            [HttpPut("{id:length(24)}")]
            [Authorize]
            public async Task<IActionResult> Update(string id, Model modelIn)
            {
                var model = _modelsRepository.GetAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                await _modelsRepository.UpdateAsync(id, modelIn);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            [Authorize]
            public async Task<IActionResult> Delete(string id)
            {
                try
                {
                    var userId = GetUserId();
                    await _modelsService.DeleteModelAsync(id, userId);
                    return Ok();
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }

            }
        }
    }

