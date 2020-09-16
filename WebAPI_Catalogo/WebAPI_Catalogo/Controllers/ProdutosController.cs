﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {
            //throw new Exception("Exception forçada"); 

            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
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

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified; //altera estado da entidade para modified e persiste as informações no banco
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id); //firstOrDefault sempre vai no banco de dados
            //var produto = _context.Produtos.Find(id); //Find primeiro procura o objeto na memória, se não encontrar então vai no banco, mas o find só pode ser utilizado se o parametro passardo for o ID (PK)

            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }
    }
}
