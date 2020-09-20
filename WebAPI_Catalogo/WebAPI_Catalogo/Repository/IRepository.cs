using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebAPI_Catalogo.Repository
{
    public interface IRepository<T> //genérica
    {
        IQueryable<T> Get(); //retorna a lista de um tipo
        T GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
