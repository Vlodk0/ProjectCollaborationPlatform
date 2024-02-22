

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IBaseService<TEntity>
    {
        Task<TEntity> GetById(Guid id);
        Task<bool> Add(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(int id);
        Task<bool> SaveAsync();
    }
}
