using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Helpers;
using HabitTracker.Services;

namespace HabitTracker.ViewModels;

[QueryProperty(nameof(HabitId), "id")]
public partial class HabitDetailViewModel(DatabaseService db) : ObservableObject
{
	private HashSet<DateTime> _dates = [];
	private DateTime _month = FirstOfMonth(DateTime.Today);

	public ObservableCollection<DayCell> Days { get; } = [];

	[ObservableProperty]
	private int habitId;

	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string emoji = "";

	[ObservableProperty]
	private Color accentColor = Colors.Gray;

	[ObservableProperty]
	private string monthTitle = "";

	[ObservableProperty]
	private string monthSummary = "";

	[ObservableProperty]
	private bool canGoNext;

	[ObservableProperty]
	private int currentStreak;

	[ObservableProperty]
	private int bestStreak;

	[ObservableProperty]
	private int totalDone;

	partial void OnHabitIdChanged(int value) => LoadCommand.Execute(null);

	[RelayCommand]
	private async Task LoadAsync()
	{
		if (HabitId == 0)
			return;

		var habit = await db.GetHabitAsync(HabitId);
		if (habit is null)
			return;

		Title = habit.Name;
		Emoji = habit.Emoji;
		AccentColor = Color.FromArgb(habit.ColorHex);
		_dates = await db.GetDatesAsync(HabitId);

		BuildMonth();
		RefreshStats();
	}

	[RelayCommand]
	private void PreviousMonth()
	{
		_month = _month.AddMonths(-1);
		BuildMonth();
		RefreshStats();
	}

	[RelayCommand]
	private void NextMonth()
	{
		if (!CanGoNext)
			return;

		_month = _month.AddMonths(1);
		BuildMonth();
		RefreshStats();
	}

	[RelayCommand]
	private async Task ToggleDayAsync(DayCell cell)
	{
		if (!cell.IsInMonth || cell.IsFuture)
			return;

		var done = await db.ToggleAsync(HabitId, cell.Date);
		if (done)
			_dates.Add(cell.Date);
		else
			_dates.Remove(cell.Date);

		cell.IsDone = done;
		RefreshStats();
	}

	[RelayCommand]
	private Task EditAsync()
		=> Shell.Current.GoToAsync("edit", new Dictionary<string, object> { ["id"] = HabitId });

	[RelayCommand]
	private async Task DeleteAsync()
	{
		var confirmed = await Shell.Current.DisplayAlert(
			"Удалить привычку?",
			$"«{Title}» и вся история отметок будут удалены.",
			"Удалить",
			"Отмена");

		if (!confirmed)
			return;

		await db.DeleteHabitAsync(HabitId);
		await Shell.Current.GoToAsync("..");
	}

	private void BuildMonth()
	{
		// Неделя начинается с понедельника: DayOfWeek.Sunday (0) -> 6, Monday (1) -> 0.
		var leading = ((int)_month.DayOfWeek + 6) % 7;
		var daysInMonth = DateTime.DaysInMonth(_month.Year, _month.Month);

		var cells = new List<DayCell>();
		for (var i = 0; i < leading; i++)
			cells.Add(new DayCell(default, false, false, AccentColor));

		for (var day = 1; day <= daysInMonth; day++)
		{
			var date = new DateTime(_month.Year, _month.Month, day);
			cells.Add(new DayCell(date, true, _dates.Contains(date), AccentColor));
		}

		while (cells.Count % 7 != 0)
			cells.Add(new DayCell(default, false, false, AccentColor));

		Days.Clear();
		foreach (var cell in cells)
			Days.Add(cell);

		MonthTitle = Formatting.MonthTitle(_month);
		CanGoNext = _month < FirstOfMonth(DateTime.Today);
	}

	private void RefreshStats()
	{
		var today = DateTime.Today;
		CurrentStreak = StreakCalculator.Current(_dates, today);
		BestStreak = StreakCalculator.Best(_dates);
		TotalDone = _dates.Count;

		var doneInMonth = _dates.Count(d => d.Year == _month.Year && d.Month == _month.Month);
		var daysCounted = _month == FirstOfMonth(today)
			? today.Day
			: DateTime.DaysInMonth(_month.Year, _month.Month);
		MonthSummary = $"За месяц: {doneInMonth} из {daysCounted}";
	}

	private static DateTime FirstOfMonth(DateTime date) => new(date.Year, date.Month, 1);
}
