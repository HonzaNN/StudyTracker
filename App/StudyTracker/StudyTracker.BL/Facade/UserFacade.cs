using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.UnitOfWork;

namespace StudyTracker.BL.Facade;

public class UserFacade : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    public UserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }

    protected override string IncludesNavigationPathDetail =>
        $"{nameof(UserEntity.Subjects)}.{nameof(SubjectToUserEntity.Subject)}";

    protected override string IncludesNavigationPathDetail2 =>
        $"{nameof(UserEntity.Activities)}.{nameof(ActivityToUserEntity.Activity)}";

    protected override string IncludesNavigationPathDetail3 =>
        $"{nameof(UserEntity.Subjects)}";

    protected override string IncludesNavigationPathDetail4 =>
        $"{nameof(UserEntity.Activities)}";
}