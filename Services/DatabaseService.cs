using HabitTracker.Models;
using SQLite;

namespace HabitTracker.Services;

public class DatabaseService
{
	private readonly SemaphoreSlim _initLock = new(1, 1);
	private SQLiteAsyncConnection? _db;

	private async Task<SQLiteAsyncConnection> GetConnectionAsync()
	{
		if (_db is not null)
			return _db;

		await _initLock.WaitAsync();
		try
		{
			if (_db is null)
			{
				var db = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "habits.db3"));
				await db.CreateTableAsync<Habit>();
				await db.CreateTableAsync<HabitEntry>();
				_db = db;
			}
			return _db;
		}
		finally
		{
			_initLock.Release();
		}
	}

	public async Task<List<Habit>> GetHabitsAsync()
	{
		var db = await GetConnectionAsync();
		return await db.Table<Habit>().OrderBy(h => h.Id).ToListAsync();
	}

	public async Task<Habit?> GetHabitAsync(int id)
	{
		var db = await GetConnectionAsync();
		return await db.Table<Habit>().Where(h => h.Id == id).FirstOrDefaultAsync();
	}

	public async Task SaveHabitAsync(Habit habit)
	{
		var db = await GetConnectionAsync();
		if (habit.Id == 0)
			await db.InsertAsync(habit);
		else
			await db.UpdateAsync(habit);
	}

	public async Task DeleteHabitAsync(int id)
	{
		var db = await GetConnectionAsync();
		await db.Table<HabitEntry>().DeleteAsync(e => e.HabitId == id);
		await db.Table<Habit>().DeleteAsync(h => h.Id == id);
	}

	public async Task<HashSet<DateTime>> GetDatesAsync(int habitId)
	{
		var db = await GetConnectionAsync();
		var entries = await db.Table<HabitEntry>().Where(e => e.HabitId == habitId).ToListAsync();
		return entries.Select(e => e.Date.Date).ToHashSet();
	}

	/// <summary>Все отметки сразу, сгруппированные по привычкам — чтобы не делать N запросов для списка.</summary>
	public async Task<Dictionary<int, HashSet<DateTime>>> GetAllDatesAsync()
	{
		var db = await GetConnectionAsync();
		var entries = await db.Table<HabitEntry>().ToListAsync();
		return entries
			.GroupBy(e => e.HabitId)
			.ToDictionary(g => g.Key, g => g.Select(e => e.Date.Date).ToHashSet());
	}

	/// <summary>Переключает отметку за день. Возвращает новое состояние: true — выполнено.</summary>
	public async Task<bool> ToggleAsync(int habitId, DateTime date)
	{
		var db = await GetConnectionAsync();
		date = date.Date;

		var existing = await db.Table<HabitEntry>()
			.Where(e => e.HabitId == habitId && e.Date == date)
			.FirstOrDefaultAsync();

		if (existing is not null)
		{
			await db.DeleteAsync(existing);
			return false;
		}

		await db.InsertAsync(new HabitEntry { HabitId = habitId, Date = date });
		return true;
	}
}
