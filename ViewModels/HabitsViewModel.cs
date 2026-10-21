using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Helpers;
using HabitTracker.Services;

namespace HabitTracker.ViewModels;

public partial class HabitsViewModel(DatabaseService db) : ObservableObject
{
	public ObservableCollection<HabitItem> Habits { get; } = [];

	[ObservableProperty]
	private string todayText = Formatting.LongDate(DateTime.Today);

	[ObservableProperty]
	private string summary = "";

	[ObservableProperty]
	private double progress;

	[ObservableProperty]
	private bool isEmpty = true;

	[RelayCommand]
	private async Task LoadAsync()
	{
		var habits = await db.GetHabitsAsync();
		var dates = await db.GetAllDatesAsync();

		TodayText = Formatting.LongDate(DateTime.Today);

		Habits.Clear();
		foreach (var habit in habits)
		{
			var set = dates.TryGetValue(habit.Id, out var found) ? found : new HashSet<DateTime>();
			Habits.Add(new HabitItem(habit, set));
		}

		UpdateSummary();
	}

	[RelayCommand]
	private async Task ToggleAsync(HabitItem item)
	{
		var today = DateTime.Today;
		var done = await db.ToggleAsync(item.Habit.Id, today);

		if (done)
			item.Dates.Add(today);
		else
			item.Dates.Remove(today);

		item.Refresh();
		UpdateSummary();
	}

	[RelayCommand]
	private Task AddAsync() => Shell.Current.GoToAsync("edit");

	[RelayCommand]
	private Task OpenAsync(HabitItem item)
		=> Shell.Current.GoToAsync("detail", new Dictionary<string, object> { ["id"] = item.Habit.Id });

	private void UpdateSummary()
	{
		var total = Habits.Count;
		var done = Habits.Count(h => h.IsDoneToday);

		IsEmpty = total == 0;
		Progress = total == 0 ? 0 : (double)done / total;
		Summary = total == 0
			? "Добавьте первую привычку"
			: done == total
				? $"Все {total} выполнены — отличный день! 🎉"
				: $"Выполнено {done} из {total}";
	}
}
