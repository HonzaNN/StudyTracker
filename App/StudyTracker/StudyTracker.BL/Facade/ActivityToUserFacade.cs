using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.Repositories;
using StudyTracker.DAL.UnitOfWork;

namespace StudyTracker.BL.Facade;

public class ActivityToUserFacade :
    FacadeBaseManyToMany<ActivityToUserEntity, ActivityToUserDetailModel, ActivityToUserMapper>, IActivityToUserFacade
{
    private readonly IActivityToUserModelMapper _activityToUserMapper;

    public ActivityToUserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityToUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper) =>
        _activityToUserMapper = modelMapper;


    public async Task<ActivityToUserDetailModel> SaveAsync(ActivityToUserDetailModel model, Guid userid)
    {
        ActivityToUserEntity entity = _activityToUserMapper.MapToEntity(model, userid);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityToUserEntity> repository =
            uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>();


        await repository.AddAsync(entity);
        await uow.CommitAsync();


        return ModelMapper.MapToDetailModel(entity);
    }
    public virtual async Task<List<ActivityToUserEntity>> GetByUserIdAsync(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityToUserEntity> query = uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>().Get();

        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) && !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }

        List<ActivityToUserEntity> entities = await query.Where(e => e.UserId == userId).ToListAsync();

        return entities;
    }
    public virtual async Task<List<ActivityToUserEntity>> GetByActivityIdAsync(Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityToUserEntity> query = uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>().Get();

        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) && !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }

        List<ActivityToUserEntity> entities = await query.Where(e => e.ActivityId == activityId).ToListAsync();

        return entities;
    }

    public virtual async Task<bool> IsJoined(Guid activityId, Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<ActivityToUserEntity?> query = uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>().Get();
        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) &&
            !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }
        ActivityToUserEntity? entity = await query.SingleOrDefaultAsync(e => e.UserId == userId && e.ActivityId == activityId);
        return entity is not null;
    }

    public virtual async Task<Guid> GetID(Guid activityId, Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<ActivityToUserEntity> query = uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>().Get();
        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) &&
            !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }
        ActivityToUserEntity? entity = await query.SingleOrDefaultAsync(e => e.UserId == userId && e.ActivityId == activityId);
        return entity?.Id ?? Guid.Empty;
    }

    public virtual async Task<bool> HasFreeTime(Guid userId, DateTime start, DateTime end)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<ActivityToUserEntity> query = uow.GetRepository<ActivityToUserEntity, ActivityToUserMapper>().Get();
        IQueryable<ActivityEntity> query2 = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();
        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) &&
            !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }

        var entities =  query.Where(e => e.UserId == userId);
        if (!entities.Any())
            return true;
        var activities = query2.Where(e => entities.Any(e2 => e2.ActivityId == e.Id));
        if (!activities.Any())
            return true;
        foreach (var activity in activities)
        {
            if (activity.StartDate <= start && activity.EndDate >= start)
                return false;
            if (activity.StartDate <= end && activity.EndDate >= end)
                return false;
            if (activity.StartDate >= start && activity.EndDate <= end)
                return false;
        }
        return true;
    }   
}