using System.Collections.ObjectModel;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers.Interface;

public interface IActivityToUserModelMapper : IModelMapperManyToMany<ActivityToUserEntity, ActivityToUserDetailModel>
{
    List<ActivityListModel> MapActivityToListModels(ICollection<ActivityToUserEntity> entities);
    List<UserListModel> MapUserToListModels(ICollection<ActivityToUserEntity> entities);
}