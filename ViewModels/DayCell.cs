using CommunityToolkit.Mvvm.ComponentModel;

namespace HabitTracker.ViewModels;

/// <summary>Ячейка календарной сетки. Пустые ячейки (<see cref="IsInMonth"/> = false) выравнивают первую и последнюю неделю.</summary>
public partial class DayCell : ObservableObject
{
	public DayCell(DateTime date, bool isInMonth, bool isDone, Color accentColor)
	{
		Date = date;
		IsInMonth = isInMonth;
		IsDone = isDone;
		AccentColor = accentColor;
		IsToday = isInMonth && date == DateTime.Today;
		IsFuture = isInMonth && date > DateTime.Today;
	}

	public DateTime Date { get; }

	public bool IsInMonth { get; }

	public bool IsToday { get; }

	public bool IsFuture { get; }

	public Color AccentColor { get; }

	public string Text => IsInMonth ? Date.Day.ToString() : "";

	[ObservableProperty]
	private bool isDone;
}
