namespace HabitTracker.Services;

public static class StreakCalculator
{
	/// <summary>
	/// Текущая серия подряд идущих дней. Если сегодня ещё не отмечено, серия не обрывается —
	/// считаем от вчерашнего дня, пока он выполнен.
	/// </summary>
	public static int Current(IReadOnlySet<DateTime> dates, DateTime today)
	{
		var day = dates.Contains(today) ? today : today.AddDays(-1);
		var count = 0;
		while (dates.Contains(day))
		{
			count++;
			day = day.AddDays(-1);
		}
		return count;
	}

	/// <summary>Самая длинная серия за всё время.</summary>
	public static int Best(IReadOnlySet<DateTime> dates)
	{
		var best = 0;
		var run = 0;
		DateTime? previous = null;

		foreach (var date in dates.OrderBy(d => d))
		{
			run = previous is not null && date == previous.Value.AddDays(1) ? run + 1 : 1;
			best = Math.Max(best, run);
			previous = date;
		}
		return best;
	}
}
