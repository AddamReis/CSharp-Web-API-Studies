using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorPreco(); //além dos métodos da IGenérica, adicionado este método
    }
}
