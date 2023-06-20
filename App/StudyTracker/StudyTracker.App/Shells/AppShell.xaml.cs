using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Services;
using StudyTracker.App.ViewModels;

namespace StudyTracker.App.Shells;

public partial class AppShell
{
    private readonly INavigationService navigationService;

    public AppShell(INavigationService navigationService)
    {
        this.navigationService = navigationService;

        InitializeComponent();
    }

    [RelayCommand]
    private async Task NavigateToLoginPage()
    {
        await navigationService.GoToAsync<LoginPageViewModel>();
    }
}