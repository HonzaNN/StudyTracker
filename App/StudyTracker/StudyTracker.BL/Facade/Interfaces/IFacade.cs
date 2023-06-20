using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Facade.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();

    Task<TDetailModel?> GetAsyncMtM(Guid id);
    Task<TDetailModel> SaveAsync(TDetailModel model);

    Task<TDetailModel> UpdateAsync(TDetailModel model);
}

public interface IFacadeManyToMany<TEntity, TDetailModel>
    where TEntity : class, IEntity
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);

    
    Task<TDetailModel> SaveAsync(TDetailModel model, Guid Id);
}