using StudyTracker.App.ViewModels;
using StudyTracker.DAL.Common;

namespace StudyTracker.App.Views;

public partial class SubjectPageView
{
    private readonly SubjectPageViewModel _viewModel;

    public SubjectPageView(SubjectPageViewModel viewModel)
        : base(viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
    }

    private void OnTypePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedType = (ActivityTypeEntity)TypePicker.SelectedItem;
        _viewModel.FilterActivitiesByType(selectedType);
    }

    private void OnStatePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedState = (ActivityStateEntity)StatePicker.SelectedItem;
        _viewModel.FilterActivitiesByState(selectedState);
    }

    private void OnDatePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedDateOption = (string)DatePicker.SelectedItem;
        _viewModel.FilterActivitiesByDate(selectedDateOption);
    }
}