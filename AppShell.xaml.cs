using HabitTracker.Views;

namespace HabitTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("detail", typeof(HabitDetailPage));
		Routing.RegisterRoute("edit", typeof(EditHabitPage));
	}
}
