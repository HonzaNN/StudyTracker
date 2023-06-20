using System.Collections.ObjectModel;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers.Interface;

public interface ISubjectToUserModelMapper : IModelMapperManyToMany<SubjectToUserEntity, SubjectToUserDetailModel>
{
    List<SubjectListModel> MapSubjectToListModels(ICollection<SubjectToUserEntity> entities);

    List<UserListModel> MapUserToListModels(ICollection<SubjectToUserEntity> entities);
}