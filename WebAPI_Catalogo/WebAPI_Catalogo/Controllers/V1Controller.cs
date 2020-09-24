using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Catalogo.Controllers
{
    [ApiVersion("1"/*, Deprecated = true*/)] //versão defazada
    [ApiVersion("2")] //métodos dentro do mesmo controller para versões diferentes
    [Route("api/v{v:apiVersion}/version")] //v1/version
    [ApiController]
    public class V1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>Teste Controller V1 </h2></body></html>", "text/html");
        }

        [HttpGet, MapToApiVersion("2")]
        public IActionResult GetV2()
        {
            return Content("<html><body><h2>Teste Controller V2 New </h2></body></html>", "text/html");
        }
    }
}
