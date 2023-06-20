using StudyTracker.App.ViewModels;

namespace StudyTracker.App.Views;

public partial class LoginPageView
{
    public LoginPageView(LoginPageViewModel viewModel) :
        base(viewModel)
    {
        InitializeComponent();
    }
}