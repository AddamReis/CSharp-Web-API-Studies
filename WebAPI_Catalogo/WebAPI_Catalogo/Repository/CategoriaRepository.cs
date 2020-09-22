using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Pagination;

namespace WebAPI_Catalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            //return Get()
            //    .OrderBy(on => on.CategoriaId)
            //    .Skip((categoriasParameters.PageNumber - 1) * categoriasParameters.PageSize)
            //    .Take(categoriasParameters.PageSize)
            //    .ToList();

            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId),
                categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }
        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
