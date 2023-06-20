using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class EditUserPageViewModel : ViewModelBase
{
    private readonly IActivityToUserFacade activityToUserFacade;
    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;
    private readonly ISubjectToUserFacade subjectToUserFacade;
    private readonly IUserFacade userFacade;

    public EditUserPageViewModel(
        IUserFacade userFacade,
        ISubjectFacade subjectFacade,
        ISubjectToUserFacade subjectToUserFacade,
        IActivityToUserFacade activityToUserFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.userFacade = userFacade;
        this.subjectFacade = subjectFacade;
        this.subjectToUserFacade = subjectToUserFacade;
        this.navigationService = navigationService;
        this.activityToUserFacade = activityToUserFacade;
    }

    public Guid Id { get; set; }
    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        User = await userFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (User.Name == "" || User.Surname == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter your name and surname", "OK");
            return;
        }

        if (User.ImageUri == "")
            User.ImageUri =
                "https://static.vecteezy.com/system/resources/previews/005/544/718/original/profile-icon-design-free-vector.jpg";

        await userFacade.UpdateAsync(User with { Activities = default, Subjects = default });
        messengerService.Send(new UserEditedMessage { Id = User.Id });
        await navigationService.GoToAsync<MainPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(MainPageViewModel.CurrentUserId), Id }
        });
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await navigationService.GoToAsync<MainPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(MainPageViewModel.CurrentUserId), Id }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        var entities = await subjectToUserFacade.GetByUserIdAsync(User.Id);
        if (User != null && entities.Any())
            foreach (var entity in entities)
            {
                //get subject
                //delte by usertosubject id
                await subjectToUserFacade.DeleteAsync(entity.Id);
                // Do something with the subjectId
                Console.WriteLine(entity.Id);
            }

        var activities = await activityToUserFacade.GetByUserIdAsync(User.Id);
        if (User != null && activities.Any())
            foreach (var activity in activities)
                //get subject
                //delete by usertosubject id
                await activityToUserFacade.DeleteAsync(activity.Id);
        // Do something with the subjectId
        //delete user to activities
        await userFacade.DeleteAsync(Id);

        messengerService.Send(new UserDeleteMessage());
        await navigationService.GoToAsync("//LoginPage");
    }
}