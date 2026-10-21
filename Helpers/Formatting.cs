using System.Globalization;

namespace HabitTracker.Helpers;

public static class Formatting
{
	public static readonly CultureInfo Ru = new("ru-RU");

	public static string Capitalize(string text)
		=> string.IsNullOrEmpty(text) ? text : char.ToUpper(text[0], Ru) + text[1..];

	/// <summary>«Среда, 21 октября»</summary>
	public static string LongDate(DateTime date)
		=> Capitalize(date.ToString("dddd, d MMMM", Ru));

	/// <summary>«Октябрь 2026»</summary>
	public static string MonthTitle(DateTime date)
		=> Capitalize(date.ToString("MMMM yyyy", Ru));

	/// <summary>Склонение слова «день» для числа: 1 день, 2 дня, 5 дней.</summary>
	public static string Days(int n)
	{
		var mod100 = n % 100;
		if (mod100 is >= 11 and <= 14)
			return "дней";

		return (n % 10) switch
		{
			1 => "день",
			2 or 3 or 4 => "дня",
			_ => "дней",
		};
	}
}
