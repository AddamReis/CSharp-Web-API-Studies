using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
