using ChatRoom.Data;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatRoom.Repositories.Implementation
{
    public class Repository<TEntity, TID> : IRepository<TEntity, TID>
        where TEntity : Entity<TID>
        where TID : struct
    {
        protected IQueryable<TEntity> _completeQuery = null;
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync<TEntity>();
        }

        public virtual async Task<TEntity> GetCompleteAsync(TID id)
        {
            if (_completeQuery != null)
            {
                return await _completeQuery.SingleOrDefaultAsync(e => e.ID.Equals(id));
            }
            else
            {
                return await SingleOrDefaultAsync(e => e.ID.Equals(id));
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllCompleteAsync()
        {
            if (_completeQuery != null)
            {
                return await _completeQuery.ToListAsync();
            }
            else
            {
                return await GetAllAsync();
            }
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return entity;
        }

        public virtual async Task<TEntity> DeleteAsync(TID id)
        {
            TEntity entity = await _context.Set<TEntity>().SingleOrDefaultAsync(b => b.ID.Equals(id));
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return entity;
        }
    }
}
