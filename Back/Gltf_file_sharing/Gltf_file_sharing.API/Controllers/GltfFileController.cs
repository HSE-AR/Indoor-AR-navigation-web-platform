using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{
    [Route("api/[controller]")]
    public class GltfFileController : Controller
    {
        private readonly IGltfFileRepository _gltfFileRepository;

        public GltfFileController(IGltfFileRepository gltfFileRepository)
        {
            _gltfFileRepository = gltfFileRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<GltfFile>>> Get()
        {
            try
            {
                return new JsonResult(await _gltfFileRepository.GetAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<GltfFile>> Get(Guid id)
        {
            try
            {
                return new JsonResult(await _gltfFileRepository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<GltfFile>> Post(IFormFile file)
        {
            try
            {
                return new JsonResult(await _gltfFileRepository.CreateAsync(file));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                return new JsonResult(await _gltfFileRepository.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
