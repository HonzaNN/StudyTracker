using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL;
using StudyTracker.BL.Models;
using StudyTracker.BL.Tests2;
using StudyTracker.DAL;
using StudyTracker.DAL.Seeds;
using StudyTracker.Common.Tests;
using Xunit.Abstractions;

namespace StudyTracker.BL.Tests;

public class ActivityToUserFacadeTest : FacadeTestsBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IActivityToUserFacade _activityToUserFacadeSUT;
    private readonly IUserFacade _userFacadeSUT;
    public ActivityToUserFacadeTest(ITestOutputHelper output, ITestOutputHelper testOutputHelper) : base(output)
    {
        _testOutputHelper = testOutputHelper;
        _activityToUserFacadeSUT = new ActivityToUserFacade(UnitOfWorkFactory, ActivityToUserModelMapper);
        _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
    }

    [Fact]
    public async Task DeleteById_Deleted()
    {
        await _activityToUserFacadeSUT.DeleteAsync(ActivityToUserSeeds.Item1.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.ActivityToUser.AnyAsync(i => i.Id == ActivityToUserSeeds.Item1.Id));
        Assert.False(await dbxAssert.Users.AnyAsync(i => (i.Id == ActivityToUserSeeds.Item1.UserId) && i.Activities.Contains(ActivityToUserSeeds.Item1)));
        Console.WriteLine("ActivityToUser succesfully deleted");
    }

    [Fact]
    public async Task CreateEntity()
    {
        var newUser = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Surname = "Test User Surname",
            ImageUri = "asd"
        };
       
        var newUserModel = await _userFacadeSUT.SaveAsync(newUser); 
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.True(await dbxAssert.Users.AnyAsync(i=> i.Id == newUserModel.Id));
        Console.WriteLine("User succesfully created");
        var activityToUser = new ActivityToUserDetailModel()
        {
            Id = Guid.NewGuid(),
            ActivityId = ActivitySeeds.Activity1.Id,
            UserId = UserSeeds.User1.Id
        };
        var activityToUserModel = await _activityToUserFacadeSUT.SaveAsync(activityToUser, UserSeeds.User1.Id);
        Assert.NotNull(activityToUserModel);
        Assert.Equal(activityToUser.ActivityId, activityToUserModel.ActivityId);
        Assert.Equal(activityToUser.UserId, activityToUserModel.UserId);
        Console.WriteLine("ActivityToUser mapped model is the same as original model");
        dbxAssert.UpdateRange();
        Assert.True(await dbxAssert.ActivityToUser.AnyAsync(i => i.Id == activityToUserModel.Id));
        Assert.True(await dbxAssert.Users.AnyAsync(i => (i.Id == activityToUserModel.UserId) && i.Activities.Any(i => i.Id == activityToUserModel.Id)));
        Console.WriteLine("ActivityToUser succesfully created");
    }

    [Fact]
    public async Task HasFreeTimeFalse()
    {
       Assert.False(_activityToUserFacadeSUT.HasFreeTime(UserSeeds.User1.Id, DateTime.MinValue, DateTime.MaxValue).Result);
    }

    [Fact]
    public async Task HasFreeTimeTrue()
    {
        Assert.True(_activityToUserFacadeSUT.HasFreeTime(UserSeeds.User1.Id, new DateTime(1900, 2,3,12,0,0), new DateTime(1999, 2, 4) ).Result);
    }

    [Fact]
    public async Task HasFreeTime_WtihNewUser()
    {
        var newUser = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Surname = "Test User Surname",
            ImageUri = "asd"
        };

        var newUserModel = await _userFacadeSUT.SaveAsync(newUser);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.True(await dbxAssert.Users.AnyAsync(i => i.Id == newUserModel.Id));
        Console.WriteLine("User succesfully created");
        Assert.True(_activityToUserFacadeSUT.HasFreeTime(newUserModel.Id, DateTime.MinValue, DateTime.MaxValue).Result);
    }

}