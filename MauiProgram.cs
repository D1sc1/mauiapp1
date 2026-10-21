using HabitTracker.Services;
using HabitTracker.ViewModels;
using HabitTracker.Views;

namespace HabitTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddSingleton<DatabaseService>();

		builder.Services.AddTransient<HabitsViewModel>();
		builder.Services.AddTransient<HabitDetailViewModel>();
		builder.Services.AddTransient<EditHabitViewModel>();

		builder.Services.AddTransient<HabitsPage>();
		builder.Services.AddTransient<HabitDetailPage>();
		builder.Services.AddTransient<EditHabitPage>();

		return builder.Build();
	}
}
