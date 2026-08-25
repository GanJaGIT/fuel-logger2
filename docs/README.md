# Fuel Logger

Корпоративный инструмент для учёта транспортных средств, расчёта расхода топлива и формирования отчётов.

---

## Статус проекта

🚧 **В активной разработке**

### ✅ Реализовано

- Модели данных (Department, Vehicle, Task, etc.)
- Контекст базы данных (SQLite + Entity Framework Core)
- Главное окно с деревом подразделений и вкладками
- Окно выбора/создания проекта
- Автоматическая миграция БД

### ⬜ В плане

- CRUD для подразделений
- CRUD для марок ТС
- Учёт транспорта в подразделениях
- Создание и расчёт задач
- Генерация Excel-отчётов

---

## Структура проекта
FuelLogger/
├── FuelLogger.Core/ # Модели и интерфейсы
│ └── Models/
│ ├── Department.cs
│ ├── FuelTask.cs
│ ├── TemplateVehicle.cs
│ └── ...
├── FuelLogger.Data/ # Работа с БД
│ ├── AppDbContext.cs
│ └── DatabaseInitializer.cs
├── FuelLogger.Desktop/ # WPF-приложение
│ ├── Views/
│ │ ├── MainWindow.xaml
│ │ ├── ProjectSelectorWindow.xaml
│ │ └── CreateProjectDialog.xaml
│ ├── ViewModels/
│ │ ├── MainWindowViewModel.cs
│ │ └── RelayCommand.cs
│ └── Converters/
│ └── LevelToVisibilityConverter.cs
└── FuelLogger.sln

text

---

## Технологический стек

| Компонент | Технологии |
|-----------|------------|
| **Фронтенд** | .NET 10.0 WPF, C#, MVVM |
| **БД** | SQLite, Entity Framework Core |
| **Отчёты** | ClosedXML (Excel) |
| **Сериализация** | Newtonsoft.Json |

---

## Установка и запуск

1. Клонируй репозиторий
2. Открой `FuelLogger.sln` в Visual Studio
3. Выбери проект `FuelLogger.Desktop` как запускаемый
4. Нажми **F5**

---

## Разработчик

Артемий Ганжа

---