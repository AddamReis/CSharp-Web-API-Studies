using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI_Catalogo.Models;
using WebAPI_Catalogo.Pagination;

namespace WebAPI_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorPreco(); //além dos métodos da IGenérica, adicionado este método
    }
}
