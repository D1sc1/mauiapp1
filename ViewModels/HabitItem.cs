using CommunityToolkit.Mvvm.ComponentModel;
using HabitTracker.Helpers;
using HabitTracker.Models;
using HabitTracker.Services;

namespace HabitTracker.ViewModels;

/// <summary>Строка списка привычек: сама привычка + её отметки и вычисленная серия.</summary>
public partial class HabitItem : ObservableObject
{
	public HabitItem(Habit habit, HashSet<DateTime> dates)
	{
		Habit = habit;
		Dates = dates;
		Refresh();
	}

	public Habit Habit { get; }

	public HashSet<DateTime> Dates { get; }

	public string Name => Habit.Name;

	public string Emoji => Habit.Emoji;

	public Color AccentColor => Color.FromArgb(Habit.ColorHex);

	[ObservableProperty]
	private bool isDoneToday;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(StreakText))]
	private int currentStreak;

	public string StreakText => CurrentStreak == 0
		? "Серия не начата"
		: $"🔥 {CurrentStreak} {Formatting.Days(CurrentStreak)} подряд";

	public void Refresh()
	{
		var today = DateTime.Today;
		IsDoneToday = Dates.Contains(today);
		CurrentStreak = StreakCalculator.Current(Dates, today);
	}
}
