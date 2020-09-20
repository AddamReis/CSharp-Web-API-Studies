using System.Collections.Generic;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco(); //além dos métodos da IGenérica, adicionado este método
    }
}
