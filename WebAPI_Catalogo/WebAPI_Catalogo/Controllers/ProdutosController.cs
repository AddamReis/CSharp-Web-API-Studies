﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.DTOs;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Pagination;
using WebAPI_Catalogo.Repository;

namespace WebAPI_Catalogo.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))] //incluído convenção a nível controlador (forma mais prática)
    [Produces("application/json")] //define um único tipo de requisição no swagger para o GET
    [Route("api/[Controller]")]
    [ApiController]
    [EnableCors("PermitirApiRequest")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        /// <summary>
        /// Exibe uma relalação de produtos
        /// </summary>
        /// <returns>Retorna uma lista de Objetos Produtos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        //public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters) //?pageNumber=Value&pageSize=Value
        {
            var produtos = await _uof.ProdutoRepository.Get().ToListAsync();
            //var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            //var metadata = new
            //{
            //    produtos.TotalCount,
            //    produtos.PageSize,
            //    produtos.CurrentPage,
            //    produtos.TotalPages,
            //    produtos.HasNext,
            //    produtos.HasPrevius
            //};

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        /// <summary>
        /// Obtém um produto pelo seu identificador produtoId
        /// </summary>
        /// <param name="id">Código do Produto</param>
        /// <returns>Um objeto Produto</returns>
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            
            if (produto == null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
        }
    }
}
