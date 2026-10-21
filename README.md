# Трекер привычек (.NET MAUI)

Учебное приложение на .NET 10 MAUI, по сложности близкое к «календарю с заметками».

## Что умеет

- Список привычек с отметкой «выполнено сегодня» одним нажатием и прогресс-баром за день
- Серия дней подряд (🔥) и рекорд
- Экран привычки: календарь-сетка по месяцам (с понедельника), переключение месяцев,
  отметка/снятие отметки любого прошедшего дня нажатием на ячейку
- Создание и редактирование привычки: название, иконка (emoji), цвет
- Удаление вместе с историей
- Хранение в SQLite, светлая и тёмная тема

## Стек и структура

| Что | Где |
|---|---|
| MVVM через `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`) | `ViewModels/` |
| SQLite через `sqlite-net-pcl` | `Services/DatabaseService.cs` |
| Подсчёт серий (чистая логика, легко покрыть тестами) | `Services/StreakCalculator.cs` |
| Shell-навигация, передача параметров через `[QueryProperty]` | `AppShell.xaml.cs`, `ViewModels/` |
| DI для сервисов, ViewModel и страниц | `MauiProgram.cs` |
| Календарная сетка на `FlexLayout` + `BindableLayout` | `Views/HabitDetailPage.xaml` |
| `DataTrigger`, `VisualState`, `AppThemeBinding` | `Views/`, `Resources/Styles/` |

## Запуск

Нужны Visual Studio 2026 (или 2022 17.14+) с workload «.NET Multi-platform App UI» (или `dotnet workload install maui`).

```
dotnet build -t:Run -f net10.0-android
dotnet build -t:Run -f net10.0-windows10.0.19041.0   # только на Windows
```

Или откройте `HabitTracker.csproj` в Visual Studio и выберите Android-эмулятор / Windows Machine.

## Идеи для развития

- Ежедневные напоминания (локальные уведомления)
- Целевые дни недели для привычки (например, только пн/ср/пт)
- Экран общей статистики с графиком
- iOS / Mac Catalyst (добавить `Platforms/iOS` и `Platforms/MacCatalyst` и целевые платформы в `.csproj`)
