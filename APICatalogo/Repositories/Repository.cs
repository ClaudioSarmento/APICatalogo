using System.Linq.Expressions;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var dados = await _context.Set<T>().AsNoTracking().ToListAsync();
            return dados;
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var dados = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            return dados;
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

    }
}
