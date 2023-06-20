using StudyTracker.App.ViewModels;

namespace StudyTracker.App.Views;

public partial class ContentPageBase
{
    public ContentPageBase(IViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = this.viewModel = viewModel;
    }

    protected IViewModel viewModel { get; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.OnAppearingAsync();
    }
}