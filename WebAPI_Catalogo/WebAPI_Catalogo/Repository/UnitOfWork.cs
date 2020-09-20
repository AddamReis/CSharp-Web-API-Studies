using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Context;

namespace WebAPI_Catalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRep;
        private CategoriaRepository _categoriaRep;
        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRep = _produtoRep ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository {
        get
            {
                return _categoriaRep = _categoriaRep ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose(); //libera recursos utilizados na injeção do context
        }

    }
}
