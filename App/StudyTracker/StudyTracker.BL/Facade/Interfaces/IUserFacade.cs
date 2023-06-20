using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Facade.Interfaces;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
}