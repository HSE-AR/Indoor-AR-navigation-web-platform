using Gltf_file_sharing.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using StudentResumes.API.Models;
using StudentResumes.AUTH.Interfaces;
using System;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthentificationController : Controller
    {
        private readonly IAuthService _authService;

        public AuthentificationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> Login([FromBody] LoginViewModel form)
        {
            try
            {
                return new JsonResult(await _authService.Login(form.Email, form.Password));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> Register([FromBody]UserDto item)
        {
            try
            {
                return new JsonResult(await _authService.Register(item));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
    }
}
