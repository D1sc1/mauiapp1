using SQLite;

namespace HabitTracker.Models;

public class Habit
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public string Name { get; set; } = "";

	public string Emoji { get; set; } = "💧";

	/// <summary>Акцентный цвет в формате #RRGGBB.</summary>
	public string ColorHex { get; set; } = "#512BD4";

	public DateTime CreatedAt { get; set; } = DateTime.Today;
}
