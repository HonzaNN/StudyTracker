using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Facade;
using StudyTracker.BL.Models;
using StudyTracker.Common.Tests;
using Xunit.Abstractions;
using System.Collections.ObjectModel;
using StudyTracker.BL.Tests2;
using StudyTracker.DAL.Common;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Seeds;


namespace StudyTracker.BL.Tests;

public sealed class UserFacadeTests : FacadeTestsBase
{
    private readonly IUserFacade _userFacadeSUT;
    private readonly ISubjectToUserFacade _subjectToUserFacadeSUT;
    private readonly IActivityToUserFacade _activityToUserFacadeSUT;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        _subjectToUserFacadeSUT = new SubjectToUserFacade(UnitOfWorkFactory, SubjectToUserModelMapper);
        _activityToUserFacadeSUT = new ActivityToUserFacade(UnitOfWorkFactory, ActivityToUserModelMapper);
    }

    [Fact]
    public async Task Create_WithoutSubjectorActivity_DoesntThrowsandEquals()
    {
        var ingredient = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "User 2",
            Surname = "Mineral water",
            ImageUri = "WTF",
        };

        //Act
        ingredient = await _userFacadeSUT.SaveAsync(ingredient);

        //Assert
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var ingredientFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == ingredient.Id);
        DeepAssert.Equal(ingredient, UserModelMapper.MapToDetailModel(ingredientFromDb));
    }

    [Fact]
    public async Task Create_WithNonExistingActivity_Throws()
    {
        var model = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "User 3",
            Surname = "Mineral water",
            ImageUri = "WTF",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Type = default,
                    EndDate = default,
                    Name = "activity 1",
                    StartDate = default,
                    State = default,
                    SubjectId = default,
                    ActivityCreatorId = default
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithNonExistingSubject_Throws()
    {
        var model = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "User 4",
            Surname = "Mineral water",
            ImageUri = "WTF",
            Subjects = new ObservableCollection<SubjectListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = "subject 1",
                    Shortcut = "Sbj1"
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithSubject_Throws()
    {
        //but shouldnt i guess

        var model = new UserDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "User 5",
            Surname = "Mineral water",
            ImageUri = "WTF",
            Subjects = new ObservableCollection<SubjectListModel>()
            {
                new()
                {
                    Id = SubjectSeeds.Subject1.Id,
                    Name = SubjectSeeds.Subject1.Name,
                    Shortcut = SubjectSeeds.Subject1.Shortcut
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task GetById()
    {
        var user = await _userFacadeSUT.GetAsync(UserSeeds.User2.Id);

        DeepAssert.Equal(UserModelMapper.MapToDetailModel(UserSeeds.User2), user);
    }

    public async Task Create_With_DoesNotThrow()
    {
        var model = new UserDetailModel()
        {
            Id = Guid.Empty,
            Name = "Lukasz ",
            Surname = "Pycz",
            ImageUri = "www.google.com",
        };

        var _ = await _userFacadeSUT.SaveAsync(model);
    }


    [Fact]
    public async Task GetById_NonExistent()
    {
        var user = await _userFacadeSUT.GetAsync(UserSeeds.EmptyUserEntity.Id);

        Assert.Null(user);
    }

    [Fact]
    public async Task DeleteById_Deleted()
    {
        UserEntity User = UserSeeds.User1;

        List<SubjectToUserEntity> entities = await _subjectToUserFacadeSUT.GetByUserIdAsync(User.Id);
        if (User != null && entities.Any())
        {
            foreach (var entity in entities)
            {
                //get subject

                //delte by usertosubject id
                await _subjectToUserFacadeSUT.DeleteAsync(entity.Id);
                // Do something with the subjectId
                Console.WriteLine(entity.Id);
            }
        }
        List<ActivityToUserEntity> activities = await _activityToUserFacadeSUT.GetByUserIdAsync(User.Id);
        if (User != null && activities.Any())
        {
            foreach (var activity in activities)
            {
                //get subject

                //delete by usertosubject id
                await _activityToUserFacadeSUT.DeleteAsync(activity.Id);
                // Do something with the subjectId

            }
        }
        await _userFacadeSUT.DeleteAsync(UserSeeds.User1.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserSeeds.User1.Id));
    }

    [Fact]
    public async Task Delete_IngredientUsedInRecipe_Throws()
    {
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _userFacadeSUT.DeleteAsync(UserSeeds.User1.Id));
    }

    [Fact]
    public async Task NewIngredient_InsertOrUpdate_IngredientAdded()
    {
        //Arrange
        var user = new UserDetailModel()
        {
            Id = Guid.Empty,
            Name = "Peter",
            Surname = "Skok",
        };

        //Act
        user = await _userFacadeSUT.SaveAsync(user);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var ingredientFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
        DeepAssert.Equal(user, UserModelMapper.MapToDetailModel(ingredientFromDb));
    }





    [Fact]
    public async Task SeededWater_InsertOrUpdate_IngredientUpdated()
    {
        //Arrange
        var ingredient = new UserDetailModel()
        {
            Id = UserSeeds.User2.Id,
            Name = UserSeeds.User2.Name,
            Surname = UserSeeds.User2.Surname,
        };
        ingredient.Name += "updated";
        ingredient.Surname += "updated";

        //Act
        await _userFacadeSUT.SaveAsync(ingredient);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var ingredientFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == ingredient.Id);
        DeepAssert.Equal(ingredient, UserModelMapper.MapToDetailModel(ingredientFromDb));
    }

    private static void FixIds(UserDetailModel expectedModel, UserDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var ingredientAmountModel in returnedModel.Subjects)
        {
            var ingredientAmountDetailModel = expectedModel.Subjects.FirstOrDefault(i =>
                i.Shortcut == ingredientAmountModel.Shortcut
                && i.Name == ingredientAmountModel.Name);


            if (ingredientAmountDetailModel != null)
            {
                ingredientAmountModel.Id = ingredientAmountDetailModel.Id;
                ingredientAmountModel.Name = ingredientAmountDetailModel.Name;
                ingredientAmountModel.Shortcut = ingredientAmountDetailModel.Shortcut;
            }
        }
    }
}