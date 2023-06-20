using System.Collections.ObjectModel;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers;

public class ActivityToUserModelMapper :
    ModelMapperBaseManytoMany<ActivityToUserEntity, ActivityToUserDetailModel>, IActivityToUserModelMapper
{
    public ActivityToUserModelMapper()
    {
    }


    public List<ActivityListModel> MapActivityToListModels(ICollection<ActivityToUserEntity> entities)
    {
        List<ActivityListModel> list = new();
        ActivityModelMapper activityModelMapper = new();
        foreach (var entity in entities)
        {
            if (entity.Activity != null) list.Add(activityModelMapper.MapToListModel(entity.Activity));
        }

        return list;
    }

    public List<UserListModel> MapUserToListModels(ICollection<ActivityToUserEntity> entities)
    {
        List<UserListModel> list = new();
        UserModelMapper userModelMapper = new();
        foreach (var entity in entities)
        {
            if (entity.User != null) list.Add(userModelMapper.MapToListModel(entity.User));
        }

        return list;
    }

    public override ActivityToUserDetailModel MapToDetailModel(ActivityToUserEntity? entity)
    {
        return (entity is null)
            ? ActivityToUserDetailModel.Empty
            : new ActivityToUserDetailModel
            {
                Id = entity.Id,
                UserId = entity.UserId ?? Guid.Empty,
                ActivityId = entity.ActivityId ?? Guid.Empty
            };
    }


    public override ActivityToUserEntity MapToEntity(ActivityToUserDetailModel detailModel, Guid UserId)
        => new()
        {
            Id = detailModel.Id,
            ActivityId = detailModel.ActivityId,
            UserId = UserId
        };
}