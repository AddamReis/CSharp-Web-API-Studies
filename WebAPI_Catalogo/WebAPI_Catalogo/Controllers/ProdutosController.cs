using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Controllers
{
    //ActionResult retorna os status possíveis as requisições (no caso de API)
    //IEnumerable<> expõe um enumerador que suporta uma interação entre uma lista de objetos
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context; //dependency injection
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
           return _context.Produtos.ToList();
        }
    }
}
