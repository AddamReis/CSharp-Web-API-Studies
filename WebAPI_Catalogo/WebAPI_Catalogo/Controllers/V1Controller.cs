using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Catalogo.Controllers
{
    [ApiVersion("1")]
    [Route("api/version1")] //v1/version
    [ApiController]
    public class V1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>Teste Controller V1 </h2></body></html>", "text/html");
        }
    }
}
