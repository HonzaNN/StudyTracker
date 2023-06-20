using CommunityToolkit.Mvvm.ComponentModel;
using StudyTracker.App.Services;

namespace StudyTracker.App.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, IViewModel
{
    protected readonly IMessengerService messengerService;
    private bool isRefreshRequired = true;

    protected ViewModelBase(IMessengerService messengerService)
        : base(messengerService.Messenger)
    {
        this.messengerService = messengerService;
        IsActive = true;
    }

    public async Task OnAppearingAsync()
    {
        if (isRefreshRequired)
        {
            await LoadDataAsync();

            isRefreshRequired = false;
        }
    }

    protected virtual Task LoadDataAsync()
    {
        return Task.CompletedTask;
    }
}