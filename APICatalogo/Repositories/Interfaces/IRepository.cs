using System.Linq.Expressions;

namespace APICatalago.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
