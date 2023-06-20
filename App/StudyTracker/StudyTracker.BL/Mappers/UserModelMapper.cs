using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>, IUserModelMapper
{
    private readonly ISubjectToUserModelMapper _subjectToUserModelMapper;
    private readonly IActivityToUserModelMapper _activityToUserModelMapper;

    public UserModelMapper(ISubjectToUserModelMapper subjectToUserModelMapper,
        IActivityToUserModelMapper activityToUserModelMapper)
    {
        _subjectToUserModelMapper = subjectToUserModelMapper;
        _activityToUserModelMapper = activityToUserModelMapper;
    }

    public UserModelMapper()
    {
        _subjectToUserModelMapper = new SubjectToUserModelMapper();
        _activityToUserModelMapper = new ActivityToUserModelMapper();
    }

    public override UserListModel MapToListModel(UserEntity? entity)
    {
        return (entity is null)
            ? UserListModel.Empty
            : new UserListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                ImageUri = entity.ImageUri
            };
    }

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
    {
        return (entity is null)
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                ImageUri = entity.ImageUri,
                Subjects = _subjectToUserModelMapper.MapSubjectToListModels(entity.Subjects).ToObservableCollection(),
                Activities = _activityToUserModelMapper.MapActivityToListModels(entity.Activities)
                    .ToObservableCollection()
            };
    }

    public override UserEntity MapToEntity(UserDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            Name = detailModel.Name,
            Surname = detailModel.Surname,
            ImageUri = detailModel.ImageUri
        };
}