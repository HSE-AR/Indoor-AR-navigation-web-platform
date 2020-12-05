using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpGLTF.Geometry;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPosition;

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

        [HttpGet("{id}")]
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
                if (CheckExtension(file.FileName))
                    return new JsonResult(await _gltfFileRepository.CreateAsync(file));

                return StatusCode(500, new Exception("Invalid file type"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
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

        [HttpPost("create3d")]
        public async Task<ActionResult<bool>> Create3d(int x, int y, int z, int h, int w, int l)
        {
            try
            {
                CreateCube(x, y, z, h, w, l);
                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #region PrivateMethods
        private static bool CheckExtension(string filename)
        {
            if (filename == null || Path.GetExtension(filename) != ".glb")
                return false;

            return true;
        }

        

        private void CreateCube(int x, int y, int z, int h, int w, int l)
        {
            var material1 = new MaterialBuilder()
               .WithDoubleSide(true)
               .WithMetallicRoughnessShader()
               .WithChannelParam("BaseColor", new Vector4(1, 1, 0, 1));


            // create a mesh with two primitives, one for each material

            var mesh = new MeshBuilder<VERTEX>("mesh");

            var prim = mesh.UsePrimitive(material1);

            prim.AddQuadrangle(new VERTEX(x, y, z), new VERTEX(x+w, y, z), new VERTEX(x+w, y+l, z), new VERTEX(x, y+l, z));
            prim.AddQuadrangle(new VERTEX(x, y, z), new VERTEX(x + w, y, z), new VERTEX(x + w, y, z+h), new VERTEX(x, y, z+h));
            prim.AddQuadrangle(new VERTEX(x, y, z+h), new VERTEX(x + w, y, z+h), new VERTEX(x + w, y + l, z+h), new VERTEX(x, y + l, z+h));
            prim.AddQuadrangle(new VERTEX(x, y+l, z), new VERTEX(x + w, y+l, z), new VERTEX(x + w, y+l, z + h), new VERTEX(x, y+l, z + h));
            prim.AddQuadrangle(new VERTEX(x+w, y+l, z), new VERTEX(x + w, y, z), new VERTEX(x + w, y,z+h), new VERTEX(x + w, y + l, z+h));
            prim.AddQuadrangle(new VERTEX(x, y + l, z), new VERTEX(x, y, z), new VERTEX(x, y, z + h), new VERTEX(x, y + l, z + h));

            // create a scene

            var scene = new SharpGLTF.Scenes.SceneBuilder();

            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveAsWavefront("mesh.obj");
            model.SaveGLB("mesh.glb");
            model.SaveGLTF("mesh.gltf");
        }
        #endregion

    }
}
