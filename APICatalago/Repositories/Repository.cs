using System.Linq.Expressions;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories.Interfaces;
using NuGet.Protocol.Core.Types;

namespace APICatalago.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            var dados = _context.Set<T>().ToList();
            return dados;
        }
        public T? Get(Expression<Func<T, bool>> predicate)
        {
            var dados = _context.Set<T>().FirstOrDefault(predicate);
            return dados;
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return entity;
        }

    }
}
