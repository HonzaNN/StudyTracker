using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

public partial class AddUserPage : ViewModelBase
{
    private readonly INavigationService navigationService;
    private readonly IUserFacade userFacade;


    public AddUserPage(IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.userFacade = userFacade;
        this.navigationService = navigationService;
    }

    public UserDetailModel User { get; init; } = UserDetailModel.Empty;

    [RelayCommand]
    public async Task AddUserAsync(UserDetailModel user)
    {
        if (user.Name == "" || user.Surname == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter your name and surname", "OK");
            return;
        }

        if (user.ImageUri == "")
            user.ImageUri =
                "https://static.vecteezy.com/system/resources/previews/005/544/718/original/profile-icon-design-free-vector.jpg";

        await userFacade.SaveAsync(user);
        messengerService.Send(new UserAddMessage { UserID = user.Id });
        await navigationService.GoToAsync("//LoginPage");
    }

    [RelayCommand]
    public async Task DiscardAsync(UserDetailModel user)
    {
        user = UserDetailModel.Empty;
        await navigationService.GoToAsync("//LoginPage");
    }
}