using System.Linq.Expressions;

namespace MvcDatabaseApp.Repositories.Contracts
{
    public interface IRepository<T> where T : class
    {
        //Get all entities
        Task<IEnumerable<T>> GetAllAsync();
        //Get Entity by Id
        Task<T> GetByIdAsync(int id);
        // Get entities with conditions
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Get single entity with conditions
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Get first entity with conditions
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Add entity
        Task AddAsync(T entity);

        // Add multiple entities
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update entity
        void Update(T entity);

        // Remove entity
        void Remove(T entity);

        // Remove multiple entities
        void RemoveRange(IEnumerable<T> entities);

        // Check if any entity exists with condition
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        // Count entities
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        // Save changes
        Task<int> SaveChangesAsync();
    }
}
