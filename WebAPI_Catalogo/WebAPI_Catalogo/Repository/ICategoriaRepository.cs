using System.Collections.Generic;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
