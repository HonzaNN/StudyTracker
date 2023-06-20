using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        //gets null if entity doesnt exist
        IQueryable<TEntity> Get();

        //check if entity exists 
        ValueTask<bool> ExistsAsync(TEntity entity);

        //async crud operations
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        void Delete(Guid entityIdGuid);
    }
}