using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Controllers
{
    //ActionResult retorna os status possíveis as requisições (no caso de API)
    //IEnumerable<> expõe um enumerador que suporta uma interação entre uma lista de objetos (retorno de lista)
    //AsnoTraking só pode ser utilizado em consultas, o mesmo aumenta o desempenho da operação.
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
           return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState); //A partir da versão do asp net core 2.1, essa validação é feita automática, desde que o controller esteja definido como [ApiController] 

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", //informo o nome da rota no qual o produto estará disponível (definido no GET Id)
                new { id = produto.ProdutoId }, produto);
        }
    }
}
