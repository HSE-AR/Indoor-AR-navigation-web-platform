using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Mvc;
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
            public async Task<ActionResult<ICollection<Model>>> Get() =>
                 new JsonResult(await _modelsRepository.Get());
 

            [HttpGet("{id:length(24)}", Name = "GetModel")]
            public async Task<ActionResult<Model>> Get(string id)
            {
                var model = await _modelsRepository.Get(id);

                if (model == null)
                {
                    return NotFound();
                }

                return model;
            }

            [HttpPost]
            public async Task<ActionResult<Model>> Create([FromBody]Model model)
            {
                await _modelsRepository.Create(model);

                return CreatedAtRoute("GetModel", new { id = model.Id.ToString() }, model);
            }

            [HttpPut("{id:length(24)}")]
            public async Task<IActionResult> Update(string id, Model modelIn)
            {
                var model = _modelsRepository.Get(id);

                if (model == null)
                {
                    return NotFound();
                }

                await _modelsRepository.Update(id, modelIn);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            public async Task<IActionResult> Delete(string id)
            {
                var model =  await _modelsRepository.Get(id);

                if (model == null)
                {
                    return NotFound();
                }

                _modelsRepository.Remove(model.Id);

                return NoContent();
            }
        }
    }

