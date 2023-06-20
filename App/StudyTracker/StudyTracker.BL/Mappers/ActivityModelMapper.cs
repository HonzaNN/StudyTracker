using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    public readonly ISubjectToUserModelMapper _subjectToUserModelMapper;
    public readonly IActivityToUserModelMapper _activityToUserModelMapper;

    public ActivityModelMapper(ISubjectToUserModelMapper subjectToUserModelMapper,
        IActivityToUserModelMapper activityToUserModelMapper)
    {
        _subjectToUserModelMapper = subjectToUserModelMapper;
        _activityToUserModelMapper = activityToUserModelMapper;
    }

    public ActivityModelMapper()
    {
        _activityToUserModelMapper = new ActivityToUserModelMapper();
        _subjectToUserModelMapper = new SubjectToUserModelMapper();
    }

    public override ActivityListModel MapToListModel(ActivityEntity? entity)
    {
        return (entity is null)
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                State = entity.State,
                Type = entity.Type,
                ActivityCreatorId = entity.ActivityCreatorId,
                SubjectId = entity.SubjectId,
            };
    }

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
    {
        UserModelMapper userModelMapper = new();
        return (entity is null)
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                State = entity.State,
                Type = entity.Type,
                Users = _activityToUserModelMapper.MapUserToListModels(entity.Users).ToObservableCollection(),
                ActivityCreatorId = entity.ActivityCreatorId,
                SubjectId = entity.SubjectId
            };
    }

    public override ActivityEntity MapToEntity(ActivityDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            Name = detailModel.Name,
            StartDate = detailModel.StartDate,
            EndDate = detailModel.EndDate,
            State = detailModel.State,
            Type = detailModel.Type,
            ActivityCreatorId = detailModel.ActivityCreatorId,
            SubjectId = detailModel.SubjectId,

        };
}