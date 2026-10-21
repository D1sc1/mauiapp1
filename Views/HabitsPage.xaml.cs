using HabitTracker.ViewModels;

namespace HabitTracker.Views;

public partial class HabitsPage : ContentPage
{
	private readonly HabitsViewModel _viewModel;

	public HabitsPage(HabitsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadCommand.Execute(null);
	}
}
