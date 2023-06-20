using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.Repositories;
using StudyTracker.DAL.UnitOfWork;

namespace StudyTracker.BL.Facade;

public class SubjectToUserFacade :
    FacadeBaseManyToMany<SubjectToUserEntity, SubjectToUserDetailModel, SubjectToUserMapper>, ISubjectToUserFacade
{
    private readonly ISubjectToUserModelMapper _subjectToUserMapper;

    public SubjectToUserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ISubjectToUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper) =>
        _subjectToUserMapper = modelMapper;

    public SubjectToUserFacade(IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapperManyToMany<SubjectToUserEntity, SubjectToUserDetailModel> modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }


    public async Task<SubjectToUserDetailModel> SaveAsync(SubjectToUserDetailModel model, Guid userId)
    {
        SubjectToUserEntity entity = _subjectToUserMapper.MapToEntity(model, userId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
            IRepository<SubjectToUserEntity> repository =
                uow.GetRepository<SubjectToUserEntity, SubjectToUserMapper>(); 
        await repository.AddAsync(entity);
        try
        {

            
              await uow.CommitAsync();
 



        }
        catch (Exception ex)
        {
            var innerException = ex.InnerException;
        }
     
        return ModelMapper.MapToDetailModel(entity);

    }

    public virtual async Task<List<SubjectToUserEntity>> GetByUserIdAsync(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<SubjectToUserEntity> query = uow.GetRepository<SubjectToUserEntity, SubjectToUserMapper>().Get();

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

        List<SubjectToUserEntity> entities = await query.Where(e => e.UserId == userId).ToListAsync();

        return entities;
    }
    public virtual async Task<List<SubjectToUserEntity>> GetBySubjectIdAsync(Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<SubjectToUserEntity> query = uow.GetRepository<SubjectToUserEntity, SubjectToUserMapper>().Get();

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

        List<SubjectToUserEntity> entities = await query.Where(e => e.SubjectId == subjectId).ToListAsync();

        return entities;
    }

    public virtual async Task<bool> IsJoined(Guid subjectId, Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<SubjectToUserEntity?> query = uow.GetRepository<SubjectToUserEntity, SubjectToUserMapper>().Get();
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
        SubjectToUserEntity? entity = await query.SingleOrDefaultAsync(e => e.UserId == userId && e.SubjectId == subjectId);
        return entity is not null;
    }

    public virtual async Task<Guid> GetId(Guid subjectId, Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<SubjectToUserEntity?> query = uow.GetRepository<SubjectToUserEntity, SubjectToUserMapper>().Get();
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
        SubjectToUserEntity? entity = await query.SingleOrDefaultAsync(e => e.UserId == userId && e.SubjectId == subjectId);
        return entity?.Id ?? Guid.Empty;
    }


}