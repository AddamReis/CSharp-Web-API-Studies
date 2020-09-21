using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI_Catalogo.Repository
{
    public interface IRepository<T> //genérica
    {
        IQueryable<T> Get(); //retorna a lista de um tipo
        Task <T> GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
