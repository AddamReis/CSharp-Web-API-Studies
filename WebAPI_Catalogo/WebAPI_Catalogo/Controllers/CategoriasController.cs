using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Repository;

namespace WebAPI_Catalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public CategoriasController(IUnitOfWork contexto)
        {
            _uof = contexto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _uof.CategoriaRepository.Get().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
            if (categoria == null)
                return NotFound();

            return categoria;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria == null)
                return NotFound();

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return categoria;
        }
    }
}
