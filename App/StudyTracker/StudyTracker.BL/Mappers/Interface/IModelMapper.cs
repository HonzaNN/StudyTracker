namespace StudyTracker.BL.Mappers.Interface;

public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);

    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);

    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
}

public interface IModelMapperManyToMany<TEntity, TDetailModel>
{
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model, Guid Id);
}