using System.Collections;
using System.Reflection;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.Repositories;
using StudyTracker.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;


namespace StudyTracker.BL.Facade;

public abstract class
    FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper> : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

    protected FacadeBase(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        ModelMapper = modelMapper;
    }

    protected virtual string IncludesNavigationPathDetail => string.Empty;
    protected virtual string IncludesNavigationPathDetail2 => string.Empty;
    protected virtual string IncludesNavigationPathDetail3 => string.Empty;
    protected virtual string IncludesNavigationPathDetail4 => string.Empty;

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) &&
            !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TEntity> entities = await uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;

        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = model.Id;
            TEntity insertedEntity = await repository.AddAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
            
        }

        await uow.CommitAsync();

        return result;
    }

    public static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }

    public virtual async Task<TDetailModel> UpdateAsync(TDetailModel model)
    {
        GuardCollectionsAreNotSet(model);
        TEntity entity = ModelMapper.MapToEntity(model);
        IUnitOfWork uow = UnitOfWorkFactory.Create();
        TEntity updatedEntity = await uow.GetRepository<TEntity, TEntityMapper>().UpdateAsync(entity);
        await uow.CommitAsync();
        return ModelMapper.MapToDetailModel(updatedEntity);
    }

    public virtual async Task<TDetailModel?> GetAsyncMtM(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail3) is false)
        {
            query = query.Include(IncludesNavigationPathDetail);
        }

        

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

}

public abstract class
    FacadeBaseManyToMany<TEntity, TDetailModel, TEntityMapper> : IFacadeManyToMany<TEntity, TDetailModel>
    where TEntity : class, IEntity
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IModelMapperManyToMany<TEntity, TDetailModel> ModelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

    protected FacadeBaseManyToMany(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapperManyToMany<TEntity, TDetailModel> modelMapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        ModelMapper = modelMapper;
    }

    protected virtual string IncludesNavigationPathDetail => string.Empty;
    protected virtual string IncludesNavigationPathDetail2 => string.Empty;

    protected virtual string IncludesNavigationPathDetail3 => string.Empty;

    protected virtual string IncludesNavigationPathDetail4 => string.Empty;




    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) is false)
        {
            query = query.Include(IncludesNavigationPathDetail);
        }

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2) is false)
        {
            query = query.Include(IncludesNavigationPathDetail);
        }

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    


    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model, Guid id)
    {
        TDetailModel result;

        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model, id);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.AddAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
            
        }

        await uow.CommitAsync();

        return result;
    }

   
    public static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}