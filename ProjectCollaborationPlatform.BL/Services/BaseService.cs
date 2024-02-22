using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Services
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        private readonly ProjectPlatformContext _context;

        protected BaseService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return await SaveAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<TEntity>().Remove(entity);

            return await SaveAsync();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Set<TEntity>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await SaveAsync();
        }
    }
}
