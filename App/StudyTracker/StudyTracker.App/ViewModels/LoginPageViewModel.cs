using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

public partial class LoginPageViewModel : ViewModelBase, IRecipient<UserAddMessage>, IRecipient<UserDeleteMessage>,
    IRecipient<UserEditedMessage>
{
    private readonly INavigationService navigationService;
    private readonly IUserFacade userFacade;

    public LoginPageViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.userFacade = userFacade;
        this.navigationService = navigationService;
    }

    public IEnumerable<UserListModel> Users { get; set; } = null!;

    public async void Receive(UserAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserEditedMessage message)
    {
        await LoadDataAsync();
    }


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Users = await userFacade.GetAsync();
    }

    public async void UserDelete()
    {
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("//LoginPage/AddUserPage");
    }

    [RelayCommand]
    private async Task GoToMainPageAsync(Guid Id)
    {
        await navigationService.GoToAsync<MainPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(MainPageViewModel.CurrentUserId), Id }
        });
    }
}