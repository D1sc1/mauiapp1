using SQLite;

namespace HabitTracker.Models;

/// <summary>Запись «привычка выполнена в этот день». Нет записи — день не выполнен.</summary>
public class HabitEntry
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[Indexed]
	public int HabitId { get; set; }

	/// <summary>Всегда дата без времени (<see cref="DateTime.Date"/>).</summary>
	public DateTime Date { get; set; }
}
