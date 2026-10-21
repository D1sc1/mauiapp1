using HabitTracker.ViewModels;

namespace HabitTracker.Views;

public partial class EditHabitPage : ContentPage
{
	public EditHabitPage(EditHabitViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
