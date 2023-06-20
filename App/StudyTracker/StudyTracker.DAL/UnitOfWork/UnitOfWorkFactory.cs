using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<StudyTrackerDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<StudyTrackerDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}