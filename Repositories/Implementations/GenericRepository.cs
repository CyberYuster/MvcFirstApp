using Microsoft.EntityFrameworkCore;
using MvcDatabaseApp.Data;
using MvcDatabaseApp.Models;
using MvcDatabaseApp.Repositories.Contracts;
using System.Linq.Expressions;

namespace MvcDatabaseApp.Repositories.Implementations
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ??throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
        await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if(predicate == null) 
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Remove(T entity)
        {
          _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
        _dbSet.RemoveRange(entities);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
        return await _context.SaveChangesAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public virtual void Update(T entity)
        {
        _dbSet.Update(entity);
        }

        // Dispose pattern
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
