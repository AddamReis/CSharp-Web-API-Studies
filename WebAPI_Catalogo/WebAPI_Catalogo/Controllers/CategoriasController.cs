using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Services;

namespace WebAPI_Catalogo.Controllers
{
    //Utilizar restrições de rotas somente para destinguir entre duas rotas parecidas
    //não utilizar este recurso para validar entrada do usuário na URL
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext contexto, IConfiguration config)
        {
            _context = contexto;
            _configuration = config;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["Autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];
            return $"Autor : {autor} e Conexão : {conexao}";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico, string nome) //Injeção foi aplicada diretamente ao Método Action
        {
            return meuservico.Saudacao(nome);
        }

        [HttpGet("produtos")] //adiciona ao endpoint para não dar conflito pois o controller já possui um get sem parametros
        //[HttpGet("/produtos")] --host://produtos  //É possível definir mais de 1 endpoint para o mesmo método

        //[HttpGet("{valor:alpha:length(5)}")] //incluindo restrição para api aceitar somente alpha numéricos com tamanho de 5

        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(x => x.Produtos).ToList(); //além de retornar as categorias, agora retorna os respectivos produtos associados
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")] //incluído restrição para que a api só aceite valores inteiros e meior que zero
        public async Task<ActionResult<Categoria>> GetAsync(/*[FromQuery]*/ int id/*, [BindRequired] string nome*/)
        {
            //Para o caso do FromQuery, os valores devem ser passados na url = /1?id=5 
            //var nomeProd = nome; //BindRequired faz com que seja obrigatório passar o valor na url = /1?nome=jonas
            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(p => p.CategoriaId == id);
            if (categoria == null)
                return NotFound();

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }
    }
}
