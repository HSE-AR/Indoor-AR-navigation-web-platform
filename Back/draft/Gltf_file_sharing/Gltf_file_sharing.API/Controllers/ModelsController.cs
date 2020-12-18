using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
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
        public class ModelsController : Controller
        {
            private readonly ModelsRepository _modelsRepository;

            public ModelsController(ModelsRepository modelsRepository)
            {
                _modelsRepository = modelsRepository;
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

            [HttpPost]
            public async Task<ActionResult<ModelDto>> Create([FromBody]ModelDto modelDto)
            {
               return await _modelsRepository.CreateAsync(modelDto);
            }

            [HttpPut("{id:length(24)}")]
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
            public async Task<IActionResult> Delete(string id)
            {
                var model =  await _modelsRepository.GetAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                await _modelsRepository.RemoveAsync(model.Id);

                return NoContent();
            }
        }
    }

