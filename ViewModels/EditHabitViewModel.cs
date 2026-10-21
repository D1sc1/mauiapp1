using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Models;
using HabitTracker.Services;

namespace HabitTracker.ViewModels;

[QueryProperty(nameof(HabitId), "id")]
public partial class EditHabitViewModel(DatabaseService db) : ObservableObject
{
	public IReadOnlyList<string> Emojis { get; } =
		["💧", "📚", "🏃", "🧘", "💤", "🥗", "✍️", "🎸", "💊", "🧹", "🌱", "💻"];

	public IReadOnlyList<Color> Palette { get; } = new[]
	{
		"#512BD4", "#E5484D", "#F76B15", "#FFB224", "#30A46C", "#0091FF", "#8E4EC6", "#E93D82",
	}.Select(hex => Color.FromArgb(hex)).ToList();

	[ObservableProperty]
	private int habitId;

	[ObservableProperty]
	private string pageTitle = "Новая привычка";

	[ObservableProperty]
	private string name = "";

	[ObservableProperty]
	private string selectedEmoji = "💧";

	[ObservableProperty]
	private Color selectedColor = Color.FromArgb("#512BD4");

	partial void OnHabitIdChanged(int value) => _ = LoadAsync(value);

	private async Task LoadAsync(int id)
	{
		if (id == 0)
			return;

		var habit = await db.GetHabitAsync(id);
		if (habit is null)
			return;

		PageTitle = "Редактирование";
		Name = habit.Name;
		SelectedEmoji = habit.Emoji;
		SelectedColor = Color.FromArgb(habit.ColorHex);
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		var trimmed = Name.Trim();
		if (trimmed.Length == 0)
		{
			await Shell.Current.DisplayAlert("Нужно название", "Введите название привычки.", "OK");
			return;
		}

		var habit = (HabitId == 0 ? null : await db.GetHabitAsync(HabitId)) ?? new Habit();
		habit.Name = trimmed;
		habit.Emoji = SelectedEmoji;
		habit.ColorHex = SelectedColor.ToArgbHex();

		await db.SaveHabitAsync(habit);
		await Shell.Current.GoToAsync("..");
	}
}
