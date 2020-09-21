using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Catalogo.DTOs;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Repository;

namespace WebAPI_Catalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            
            if (produto == null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPrecos()
        {
            var produtos =_uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
        }
    }
}
