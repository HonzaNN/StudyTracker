using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers;

public class SubjectModelMapper : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>,
    ISubjectModelMapper
{
    private readonly ISubjectToUserModelMapper _subjectToUserModelMapper;
    private readonly IActivityModelMapper _activityModelMapper;

    public SubjectModelMapper(ISubjectToUserModelMapper subjectToUserModelMapper,
        IActivityModelMapper activityModelMapper)
    {
        _subjectToUserModelMapper = subjectToUserModelMapper;
        _activityModelMapper = activityModelMapper;
    }

    public SubjectModelMapper()
    {
        _subjectToUserModelMapper = new SubjectToUserModelMapper();
        _activityModelMapper = new ActivityModelMapper();
    }

    public override SubjectListModel MapToListModel(SubjectEntity? entity)
    {
        return (entity is null)
            ? SubjectListModel.Empty
            : new SubjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Shortcut = entity.Shortcut
            };
    }


    public override SubjectDetailModel MapToDetailModel(SubjectEntity? entity)
    {
        UserModelMapper userModelMapper = new();
        return (entity is null)
            ? SubjectDetailModel.Empty
            : new SubjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Shortcut = entity.Shortcut,
                Users = _subjectToUserModelMapper.MapUserToListModels(entity.Users).ToObservableCollection(),
                Activities = _activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection(),
                TeacherId = entity.TeacherId,
            };
    }

    public override SubjectEntity MapToEntity(SubjectDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            Name = detailModel.Name,
            Shortcut = detailModel.Shortcut,
            TeacherId = detailModel.TeacherId,
        };
}