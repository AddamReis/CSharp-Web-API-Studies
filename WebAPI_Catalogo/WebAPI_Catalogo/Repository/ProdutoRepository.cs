using System.Collections.Generic;
using System.Linq;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository //Herdando ds duas interfaces, tanto da genérica quanto da específica
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
