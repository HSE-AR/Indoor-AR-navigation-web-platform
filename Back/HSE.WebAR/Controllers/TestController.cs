using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSE.WebAR.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController :Controller
    {

        [HttpGet]
        public string GetStringStatus()
        {
                return "заебись, все работает"; 
        }
    }
}
