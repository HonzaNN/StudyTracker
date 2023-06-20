using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Repositories;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;

namespace StudyTracker.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _context;
    private readonly IEntityMapper<TEntity> _entityMapper;

    public Repository(DbContext context, IEntityMapper<TEntity> entityMapper)
    {
        _context = context.Set<TEntity>();
        _entityMapper = entityMapper;
    }

    public IQueryable<TEntity> Get() => _context;

    public async ValueTask<bool> ExistsAsync(TEntity entity)
        => entity.Id != Guid.Empty && await _context.AnyAsync(e => e.Id == entity.Id);

    public async Task<TEntity> AddAsync(TEntity entity)
        => (await _context.AddAsync(entity)).Entity;

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity existingEntity = await _context.SingleAsync(e => e.Id == entity.Id);
        _entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public void Delete(Guid entityID)
        => _context.Remove(_context.Single(i => i.Id == entityID));
}