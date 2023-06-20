using System.Collections.ObjectModel;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers;

public class SubjectToUserModelMapper : ModelMapperBaseManytoMany<SubjectToUserEntity, SubjectToUserDetailModel>,
    ISubjectToUserModelMapper
{
    public SubjectToUserModelMapper()
    {
    }

    public List<SubjectListModel> MapSubjectToListModels(ICollection<SubjectToUserEntity> entities)
    {
        List<SubjectListModel> list = new();
        SubjectModelMapper subjectModelMapper = new();
        foreach (var entity in entities)
        {
            if (entity.Subject != null) list.Add(subjectModelMapper.MapToListModel(entity.Subject));
        }

        return list;
    }

    public List<UserListModel> MapUserToListModels(ICollection<SubjectToUserEntity> entities)
    {
        List<UserListModel> list = new();
        UserModelMapper userModelMapper = new();
        foreach (var entity in entities)
        {
            if (entity.User != null) list.Add(userModelMapper.MapToListModel(entity.User));
        }

        return list;
    }

    public override SubjectToUserDetailModel MapToDetailModel(SubjectToUserEntity? entity)
    {
        return (entity is null)
            ? SubjectToUserDetailModel.Empty
            : new SubjectToUserDetailModel
            {

                Id = entity.Id,
                UserId = entity.UserId,
                SubjectId = entity.SubjectId,
            };
    }


    public override SubjectToUserEntity MapToEntity(SubjectToUserDetailModel detailModel, Guid userId)
        => new()
        {
            Id = Guid.NewGuid(),
            SubjectId = detailModel.SubjectId ?? Guid.Empty,
            UserId = userId
        };
}