using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatRoom.Repositories.Interfaces
{
    public interface IRepository<TEntity, TID> 
        where TEntity : class
        where TID : struct
    {
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Returns Entity and includes Navigation properties
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Complete Entity with included navigation properties</returns>
        Task<TEntity> GetCompleteAsync(TID id);
        Task<IEnumerable<TEntity>> GetAllCompleteAsync();

        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        Task<TEntity> DeleteAsync(TID id);

        /// <summary>
        /// In some cases service examins an entity which should be deleted.
        /// No neccesity Select it one more time for deletion
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <returns>Deleted entity</returns>
        TEntity Remove(TEntity entity);
    }
}
