using HabitTracker.ViewModels;

namespace HabitTracker.Views;

public partial class HabitDetailPage : ContentPage
{
	private readonly HabitDetailViewModel _viewModel;

	public HabitDetailPage(HabitDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		// Возврат с экрана редактирования — перечитываем данные.
		_viewModel.LoadCommand.Execute(null);
	}
}
