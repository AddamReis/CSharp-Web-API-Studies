﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Catalogo.Controllers
{
    [ApiVersion("2")]
    [Route("api/version2")] //v2/version
    [ApiController]
    public class V2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>Teste Controller V2 </h2></body></html>", "text/html");
        }
    }
}
