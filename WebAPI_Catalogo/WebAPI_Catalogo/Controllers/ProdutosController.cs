using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Repository;

namespace WebAPI_Catalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly UnitOfWork _uof;
        public ProdutosController(UnitOfWork contexto)
        {
            _uof = contexto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null)
                return NotFound();
            return produto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (id != produto.ProdutoId)
                return BadRequest();

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return produto;
        }
    }
}
