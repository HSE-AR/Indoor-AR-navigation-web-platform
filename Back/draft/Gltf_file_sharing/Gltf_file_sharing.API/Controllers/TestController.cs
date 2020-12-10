using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {

        [HttpGet]
        public async Task<string> GetTest()
        {
            await Task.Delay(5000);
            return "заебись, ответ пришел";
        }
    }
}
