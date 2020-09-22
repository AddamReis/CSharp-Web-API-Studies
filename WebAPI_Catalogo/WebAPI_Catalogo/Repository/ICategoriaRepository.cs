using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Pagination;

namespace WebAPI_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
