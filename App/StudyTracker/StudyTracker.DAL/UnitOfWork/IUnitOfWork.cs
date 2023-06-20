using StudyTracker.DAL.Repositories;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}